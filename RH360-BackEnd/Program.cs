using RH360_BackEnd.Repositorio.Interface;
using RH360_BackEnd.Repositorio;
using RH360_BackEnd.Servico.Interface;
using RH360_BackEnd.Servico;

var builder = WebApplication.CreateBuilder(args);

// Adicionar a configura��o do reposit�rio e servi�o
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>
(provider =>
{
    // Pegar a string de conex�o do appsettings.json
    var configuration = provider.GetService<IConfiguration>();
    return new UsuarioRepositorio(configuration.GetConnectionString("PostgresConnection"));
});
builder.Services.AddScoped<IUsuarioServico, UsuarioServico>();

// Adicionar servi�os ao cont�iner.
builder.Services.AddControllers();
// Configurar o Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline de requisi��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
