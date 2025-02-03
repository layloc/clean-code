using AspNetSemester.Contexts;
using AspNetSemester.Repositories;
using AspNetSemester.Repositories.Abstractions;
using AspNetSemester.Services;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UserContext>();
builder.Services.AddDbContext<DocumentContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";  
        options.AccessDeniedPath = "/account/accessdenied"; 
    });
builder.Services.AddSingleton<MinioService>(sp =>
{
    var endpoint = builder.Configuration["Minio:Endpoint"];
    var accessKey = builder.Configuration["Minio:AccessKey"];
    var secretKey = builder.Configuration["Minio:SecretKey"];
    var bucketName = builder.Configuration["Minio:BucketName"];

    return new MinioService(endpoint, accessKey, secretKey, bucketName);
});
var app = builder.Build();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(); 

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
});
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.Run();

