using Microsoft.EntityFrameworkCore;
using TiendaUrbanaAPI.Data;
using System.Text.Json.Serialization;
using TiendaUrbanaAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. EL TRADUCTOR JSON: Ignorar ciclos infinitos (Vital para EF Core)
builder.Services.AddControllers().AddJsonOptions(opciones =>
{
    opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// 2. CONEXIÓN A SQL SERVER
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. EL PORTERO (CORS): Para que tu JS se pueda conectar sin que lo bloqueen
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger para probar fácil la API

var app = builder.Build();

// ============================================
// DATA SEEDER: Insertar datos de prueba iniciales
// ============================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // 1. Verificar e insertar 5 Categorías de ropa urbana
    if (!context.Categorias.Any())
    {
        context.Categorias.AddRange(
            new Categoria { NombreCategoria = "Sneakers", Descripcion = "Zapatillas de colección y uso diario" },
            new Categoria { NombreCategoria = "Camperas", Descripcion = "Chaquetas, puffers y rompevientos" },
            new Categoria { NombreCategoria = "Oversize", Descripcion = "Poleras y remeras de corte holgado" },
            new Categoria { NombreCategoria = "Accesorios", Descripcion = "Gorras, cadenas, lentes y morrales" },
            new Categoria { NombreCategoria = "Pantalones", Descripcion = "Jeans anchos, cargos y joggers" }
        );
        context.SaveChanges();
    }

    // 2. Verificar e insertar 3 Ropas asignadas obligatoriamente a las categorías 1 o 2
    if (!context.Ropas.Any())
    {
        context.Ropas.AddRange(
            new Ropa { 
                Nombre = "Nike Dunk Low Retro", 
                Precio = 150.00m, 
                Talla = "42", 
                Color = "Blanco/Negro", 
                Stock = 20, 
                ImagenUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", 
                CategoriaId = 1 // Id 1: Sneakers
            },
            new Ropa { 
                Nombre = "Campera The North Face Nuptse", 
                Precio = 280.50m, 
                Talla = "L", 
                Color = "Negro", 
                Stock = 12, 
                ImagenUrl = "https://images.unsplash.com/photo-1551028719-00167b16eac5?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", 
                CategoriaId = 2 // Id 2: Camperas
            },
            new Ropa { 
                Nombre = "Air Jordan 1 High Chicago", 
                Precio = 450.00m, 
                Talla = "43", 
                Color = "Rojo/Blanco/Negro", 
                Stock = 8, 
                ImagenUrl = "https://images.unsplash.com/photo-1597045566677-8cf032ed6634?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", 
                CategoriaId = 1 // Id 1: Sneakers
            }
        );
        context.SaveChanges();
    }
}
// ============================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ACTIVAR EL PORTERO
app.UseCors("PermitirFrontend");

app.UseAuthorization();
app.MapControllers();
app.Run();
