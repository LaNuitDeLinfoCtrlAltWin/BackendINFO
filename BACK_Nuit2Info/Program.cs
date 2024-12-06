using BACK_Nuit2Info;
using Microsoft.Net.Http.Headers;

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

// Ajouter CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173", policy =>
    {
        policy.WithOrigins("https://frontendinfo.onrender.com")
              .SetIsOriginAllowed(origin => true) // allow any origin // Adresse de votre frontend
              .AllowAnyHeader()
              .AllowCredentials()
              .AllowAnyMethod();
    });
    options.AddPolicy("AllowHeaders", builder =>
    {
        builder.WithOrigins("https://frontendinfo.onrender.com")
              .SetIsOriginAllowed(origin => true) // allow any origin
                .WithHeaders(HeaderNames.ContentType, HeaderNames.Server, HeaderNames.AccessControlAllowHeaders, HeaderNames.AccessControlExposeHeaders, "x-custom-header", "x-path", "x-record-in-use", HeaderNames.ContentDisposition);
    });
});

// Ajouter les services au conteneur
builder.Services.AddControllers();
// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost5173");

app.UseAuthorization();

app.MapControllers();

app.Run();
