using System.Text;
using AspNetSemester.Contexts;
using AspNetSemester.Models;
using AspNetSemester.Repositories.Abstractions;
using AspNetSemester.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetSemester.Controllers;


[ApiController]
public class DocumentsController : ControllerBase
{
    private DocumentContext _dbContext = new DocumentContext(new DbContextOptions<DocumentContext>());
    private readonly MinioService _minioService;
    private readonly IUserRepository _userRepository;

    public DocumentsController(MinioService minioService, IUserRepository userRepository)
    {
        _minioService = minioService;
        _userRepository = userRepository;
    }

    [HttpPost("/api/docum")]
    public async Task<IActionResult> UploadFile(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            string objectName = file.FileName;
            string url = await _minioService.UploadFileAsync(objectName, memoryStream);

            if (url == null)
            {
                return StatusCode(500, "Failed to upload file.");
            }

            return Ok(new { Url = url });
        }
    }
    [HttpPost("/api/documents/upload")]
    public async Task<IActionResult> SaveDocument([FromBody] SaveDocumentRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Text) || (string.IsNullOrEmpty(request.FileName)))
        {
            return BadRequest("Текст и имя файла (или documentId) обязательны.");
        }

        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim == null)
        {
            return Unauthorized("Пользователь не авторизован.");
        }

        int userId = int.Parse(userIdClaim.Value);
        Document document;

        if (request.DocumentId != null && request.DocumentId != 0) // Перезапись существующего документа
        {
            document = await _dbContext.Documents.FindAsync(request.DocumentId);
            if (document == null)
            {
                return NotFound("Документ не найден.");
            }

            var access = await _dbContext.DocumentAccesses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.DocumentId == document.Id);
            
            if (access == null)
            {
                return Forbid("Нет доступа к изменению этого документа.");
            }

            if (access.IsReadOnly)
            {
                return Forbid("Вы имеете только право на чтение этого документа.");
            }
        }
        else // Создание нового документа
        {
            document = new Document { FileName = request.FileName, Url = $"documents/{request.FileName}"};
            _dbContext.Documents.Add(document);
            await _dbContext.SaveChangesAsync();

            var documentAccess = new DocumentAccess { UserId = userId, DocumentId = document.Id, IsReadOnly = false };
            _dbContext.DocumentAccesses.Add(documentAccess);
            await _dbContext.SaveChangesAsync();
        }

        using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(request.Text)))
        {
            string objectName = document.FileName;
            string url = await _minioService.UploadFileAsync(objectName, memoryStream);

            if (url == null)
            {
                return StatusCode(500, "Ошибка сохранения документа.");
            }

            document.Url = url;
            await _dbContext.SaveChangesAsync();

            return Ok(new { Url = url });
        }
    }
    [HttpGet("/api/documents/content")]
    public async Task<IActionResult> GetDocumentContent([FromQuery] int documentId)
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim == null)
            return Unauthorized("Пользователь не авторизован");

        int userId = int.Parse(userIdClaim.Value);

        var documentAccess = await _dbContext.DocumentAccesses
            .FirstOrDefaultAsync(da => da.UserId == userId && da.DocumentId == documentId);

        if (documentAccess == null)
            return Forbid("Нет доступа к документу");

        var document = await _dbContext.Documents.FindAsync(documentId);
        if (document == null)
            return NotFound("Документ не найден");

        var stream = await _minioService.GetFileAsync(document.FileName);
        if (stream == null)
            return NotFound("Ошибка загрузки документа");

        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            string content = await reader.ReadToEndAsync();
            return Ok(new { text = content });
        }
    }

    


    [HttpGet("/download/{objectName}")]
    public async Task<IActionResult> DownloadFile(string objectName)
    {
        var stream = await _minioService.GetFileAsync(objectName);

        if (stream == null)
        {
            return NotFound("File not found.");
        }

        return File(stream, "application/octet-stream", objectName);
    }
    [HttpGet("/api/documents/list")]
    [Authorize]
    public async Task<IActionResult> GetUserDocuments()
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim == null)
        {
            return Unauthorized("User must be logged in.");
        }

        int userId = int.Parse(userIdClaim.Value);

        var documentIds = await _dbContext.DocumentAccesses
            .Where(da => da.UserId == userId)
            .Select(da => da.DocumentId)
            .ToListAsync();

        var documents = await _dbContext.Documents
            .Where(d => documentIds.Contains(d.Id))
            .ToListAsync();

        return Ok(documents);
    }
    [HttpPost("/api/documents/grant-access")]
    public async Task<IActionResult> GrantAccess([FromBody] GrantAccessRequest request)
    {
        if (string.IsNullOrEmpty(request.Username) || request.DocumentId == 0)
        {
            return BadRequest("Username и DocumentId обязательны.");
        }

        var user = _userRepository.GetUserByUsername(request.Username);
        if (user == null)
        {
            return NotFound("Пользователь не найден.");
        }

        var document = await _dbContext.Documents.FindAsync(request.DocumentId);
        if (document == null)
        {
            return NotFound("Документ не найден.");
        }

        var access = await _dbContext.DocumentAccesses
            .FirstOrDefaultAsync(a => a.UserId == user.Id && a.DocumentId == request.DocumentId);

        if (access != null)
        {
            access.IsReadOnly = request.IsReadOnly; 
        }
        else
        {
            _dbContext.DocumentAccesses.Add(new DocumentAccess
            {
                UserId = user.Id,
                DocumentId = request.DocumentId,
                IsReadOnly = request.IsReadOnly
            });
        }

        await _dbContext.SaveChangesAsync();
        return Ok("Доступ предоставлен.");
    }

    public class GrantAccessRequest
    {
        public int DocumentId { get; set; }
        public string Username { get; set; }
        public bool IsReadOnly { get; set; }
    }

    
}