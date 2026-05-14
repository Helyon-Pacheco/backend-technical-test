using OficinaMecanica.API.Repositories;
using OficinaMecanica.API.Services;
using OficinaMecanica.API.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<OrcamentoRepository>();
builder.Services.AddScoped<OrcamentoValidator>();
builder.Services.AddScoped<OrcamentoService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Oficina Mecânica API",
        Version = "v1",
        Description = "API para gerenciamento de orçamentos de oficina mecânica."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Oficina Mecânica API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
