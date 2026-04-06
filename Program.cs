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
                Nombre = "Nike Dunk Low Retro", Precio = 150.00m, Talla = "42", Color = "Blanco/Negro", Stock = 20, 
                ImagenUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 1
            },
            new Ropa { 
                Nombre = "Campera The North Face Nuptse", Precio = 280.50m, Talla = "L", Color = "Negro", Stock = 12, 
                ImagenUrl = "https://images.unsplash.com/photo-1551028719-00167b16eac5?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 2
            },
            new Ropa { 
                Nombre = "Air Jordan 1 High Chicago", Precio = 450.00m, Talla = "43", Color = "Rojo/Blanco/Negro", Stock = 8, 
                ImagenUrl = "https://images.unsplash.com/photo-1597045566677-8cf032ed6634?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 1
            },
            new Ropa { 
                Nombre = "Remera Oversize Essential", Precio = 35.00m, Talla = "XL", Color = "Gris Jaspeado", Stock = 30, 
                ImagenUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 3
            },
            new Ropa { 
                Nombre = "Pantalón Cargo Ancho Y2K", Precio = 85.00m, Talla = "M", Color = "Verde Oliva", Stock = 15, 
                ImagenUrl = "https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 5
            },
            new Ropa { 
                Nombre = "Gorra New Era NY Yankees", Precio = 40.00m, Talla = "Única", Color = "Azul Marino/Blanco", Stock = 50, 
                ImagenUrl = "https://images.unsplash.com/photo-1588850561407-ed78c282e89b?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 4
            },
            new Ropa { 
                Nombre = "Buzo Hoodie Heavyweight Cactus Jack", Precio = 120.00m, Talla = "XXL", Color = "Marrón", Stock = 10, 
                ImagenUrl = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 2
            },
            new Ropa { 
                Nombre = "Sneakers Yeezy Boost 350 V2", Precio = 300.00m, Talla = "41", Color = "Zebra", Stock = 5, 
                ImagenUrl = "https://images.unsplash.com/photo-1584735174965-acaa00bfe45c?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 1
            },
            new Ropa { 
                Nombre = "Jeans Baggy Skate Wash", Precio = 90.00m, Talla = "32", Color = "Azul Claro", Stock = 18, 
                ImagenUrl = "https://images.unsplash.com/photo-1604176354204-9268737828e4?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 5
            },
            new Ropa { 
                Nombre = "Lentes de Sol Retro Futuristas", Precio = 55.00m, Talla = "Única", Color = "Negro Mate", Stock = 25, 
                ImagenUrl = "https://images.unsplash.com/photo-1577803645773-f96470509666?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 4
            },
            new Ropa {
                Nombre = "Remera Gráfica Vintage Bootleg", Precio = 45.00m, Talla = "L", Color = "Negro Desgastado", Stock = 22,
                ImagenUrl = "https://images.unsplash.com/photo-1529374255404-311a2a4f1fd9?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 3
            },
            new Ropa {
                Nombre = "Rompevientos Techwear", Precio = 110.00m, Talla = "XL", Color = "Gris Reflectante", Stock = 14,
                ImagenUrl = "https://images.unsplash.com/photo-1563223019-38b47c0507cd?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 2
            },
            new Ropa {
                Nombre = "Sneakers Vans Old Skool Pro", Precio = 85.00m, Talla = "40", Color = "Negro/Blanco", Stock = 30,
                ImagenUrl = "https://images.unsplash.com/photo-1525966222134-fcfa99b8ae77?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 1
            },
            new Ropa {
                Nombre = "Morral Tactical Crossbody", Precio = 65.00m, Talla = "Única", Color = "Camuflaje", Stock = 18,
                ImagenUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 4
            },
            new Ropa {
                Nombre = "Bermuda de Jean Carpenter", Precio = 75.00m, Talla = "34", Color = "Azul Medio", Stock = 20,
                ImagenUrl = "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 5
            },
            new Ropa {
                Nombre = "Polera Cuello Alto Mock Neck", Precio = 50.00m, Talla = "M", Color = "Crema", Stock = 12,
                ImagenUrl = "https://images.unsplash.com/photo-1620799140408-edc6dcb6d633?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 3
            },
            new Ropa {
                Nombre = "Cadena Cubana Acero Inoxidable", Precio = 30.00m, Talla = "Única", Color = "Plata", Stock = 40,
                ImagenUrl = "https://images.unsplash.com/photo-1535632066927-ab7c9ab60908?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 4
            },
            new Ropa {
                Nombre = "Sneakers New Balance 550", Precio = 140.00m, Talla = "44", Color = "Blanco/Verde", Stock = 10,
                ImagenUrl = "https://images.unsplash.com/photo-1539185441755-769473a23570?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", CategoriaId = 1
            }
        );
        context.SaveChanges();
    }

    // 3. Crear usuario por defecto Juan Perez con un pedido de prueba
    if (!context.Usuarios.Any())
    {
        var usuarioPrueba = new Usuario { 
            Nombre = "Juan Perez", 
            Email = "juan@urban.com", 
            DireccionEnvio = "Av. Siempre Viva 123", 
            Telefono = "12345678" 
        };
        context.Usuarios.Add(usuarioPrueba);
        context.SaveChanges();

        var pedidoNuevo = new Pedido { 
            Fecha = DateTime.Now, 
            Total = 430.50m, 
            Estado = "Completado", 
            UsuarioId = usuarioPrueba.Id 
        };
        context.Pedidos.Add(pedidoNuevo);
        context.SaveChanges();

        // Creamos los detalles del pedido asumiendo que RopaId 1 y 2 existen en la base de datos recién sembrada.
        context.DetallesPedido.AddRange(
            new DetallePedido { Cantidad = 1, PrecioUnitario = 150.00m, PedidoId = pedidoNuevo.Id, RopaId = 1 },
            new DetallePedido { Cantidad = 1, PrecioUnitario = 280.50m, PedidoId = pedidoNuevo.Id, RopaId = 2 }
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
