using AspNetSemester.Models;
using MarkdownProccesor;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSemester.Controllers;
[ApiController]
public class MarkdownController : Controller
{
    private readonly MarkdownToHtmlProcessor _markdownToHtmlProcessor = new MarkdownToHtmlProcessor();
    [HttpPost("/api/convert")]
    public async Task<IActionResult> Convert([FromBody] Markdown? markdown)
    {
        if (markdown != null)
        {
            string html = await Task.Run(() => _markdownToHtmlProcessor.ConvertToHtml(markdown.Text));
            return Ok(new { html });
        }
        return BadRequest("Text is required.");
    }
}