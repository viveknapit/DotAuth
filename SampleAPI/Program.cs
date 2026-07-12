using DotAuth.Presentation.EndPoints;
using DotAuth.Presentation.Extensions;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDotAuth(options =>
{
    options.UsePostgreSql(
        builder.Configuration.GetConnectionString("DotAuth")!);
    options.Jwt.SecretKey = "my-super-secret-key-for-dotauth-development-2026";
    options.Jwt.Issuer = "DotAuth";
    options.Jwt.Audience = "DotAuth.Api";
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT Token"
        });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapDotAuth();

app.MapControllers();

app.Run();
