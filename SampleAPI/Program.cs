using DotAuth.Presentation.EndPoints;
using DotAuth.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDotAuth(options =>
{
    options.UsePostgreSql(
        builder.Configuration.GetConnectionString("DotAuth")!);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
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
app.MapDotAuth();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
