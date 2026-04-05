# Contexto del Proyecto: Tienda Urbana API

## Descripción General
Este es un proyecto de una API para una tienda de ropa básica urbana, desarrollado para la materia de Programación Web 2.

## Tecnologías Utilizadas
- **Backend:** ASP.NET Core 8 (MVC y Web API), C#
- **Base de Datos:** SQL Server, Entity Framework Core
- **Frontend (A futuro/Relacionado):** HTML puro, Bootstrap 5 y Vanilla JavaScript con `fetch()`.

## Reglas de Arquitectura (Nivel de Código)
❌ **PROHIBIDO** usar arquitecturas complejas.
❌ **NO** usar DTOs.
❌ **NO** usar Unit of Work.
❌ **NO** usar CQRS.
❌ **NO** usar records.
✅ **SÍ** Inyectar el `DbContext` directamente en los controladores.
✅ **SÍ** Los Modelos solo deben llevar propiedades básicas y Data Annotations (como `[Required]`, `[ValidateNever]`, `[JsonIgnore]`).

## Problemas Resueltos y Decisiones Tomadas

### 🎯 Explicación para Ti: ¿Por qué no te dejaba crear Ropa? (El problema de la Categoría 1)
Imagina que las "Categorías" son los **estantes** de tu tienda, y la "Ropa" son los **productos**. 
El error que tenías ocurría porque le estabas diciendo a la base de datos: *"Ey, pon esta zapatilla Jordan en el estante 1"*, pero la base de datos te respondía: *"¡Bro, el estante 1 no existe!"*.

En bases de datos relacionales (como SQL Server), esto se llama **"Integridad Referencial"**. Si en el modelo `Ropa` dices que pertenece a un `CategoriaId`, la base de datos **te va a obligar sí o sí** a que haya un registro de Categoría con ese ID *antes* de que guardes la Ropa. Tu docente resolvía esto poblando (o "sembrando") las categorías básicas mediante código automáticamente desde el arranque, técnica conocida como **Data Seeding**.

1. **Error HTTP 500 al insertar Ropa con CategoriaId inexistente:** 
   Se producía una excepción `DbUpdateException` en SQL Server por conflictos de Foreign Key (`FK_Ropas_Categorias_CategoriaId`). En vez de dejar que la base de datos tire el error 500, implementamos una validación manual en el `RopaController` (`_context.Categorias.AnyAsync()`) para retornar un agradable `400 Bad Request` si la categoría no existe.
   
2. **Ciclos de Referencia y Validaciones Implícitas (Model Binding):** 
   Se usó el signo de interrogación `?` (Nullable), junto con `[ValidateNever]` y `[JsonIgnore]` en propiedades de navegación (`Categoria` en `Ropa`, `Usuario` y `Detalles` en `Pedido`, etc.). Esto evita que ASP.NET Core MVC exija esos objetos enteros en los JSON (evitando el error HTTP 400 por validación de modelo), dándonos flexibilidad para mandar solo los `Id` (`CategoriaId`, `UsuarioId`).

3. **Endpoints de "Cheat Code":**
   El controlador `CategoriaController` tiene un endpoint `GET /api/Categoria/crear-basicas` que permite popular la tabla Categorias para evitar crearlas manualmente 1 a 1 y poder empezar a realizar pruebas rápidamente en el estatus "Sneakers" (CategoriaId = 1).

*Nota para el equipo o próximos agentes: Mantener siempre el código simple, directo y validando las cosas en el mismo controlador antes de tocar la BD.*
