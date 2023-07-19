using MagicVillageAPI;
using MagicVillageAPI.Datos;
using MagicVillageAPI.Repositorio;
using MagicVillageAPI.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();// Se agregan los servicio del NuGet
builder.Services.AddAutoMapper(typeof(MappingConfig)); //Se agrega la clase donde se hace el mapeo con typeof
builder.Services.AddScoped<IVillaRepositorio, VillaRepositorio>();//Scoped se crean una vez, y se destruyen, cuando son solicitados.

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
