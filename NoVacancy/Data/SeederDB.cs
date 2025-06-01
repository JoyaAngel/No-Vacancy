using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoVacancy.Models;
using System;
using System.Linq;

namespace NoVacancy.Data
{
    public static class SeederDB
    {
        public static void SeedAll(IServiceProvider serviceProvider)
        {
            using (var context = new NoVacancyDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<NoVacancyDbContext>>() ))
            {
                SeedTallas(context);
                SeedCategorias(context);
                SeedColores(context);
                SeedProductosBase(context);
                SeedUsuarios(context);
                SeedCarritosYLineas(context);
                SeedImagenesYReseniasBase(context);
                SeedProductosExtra(context);
                SeedLineasYPedidosExtra(context);
                SeedImagenesYReseniasParaTodos(context);
                SeedProductosAleatorios(context);
                SeedPedidosYDetallesAleatorios(context);
            }
        }

        // Métodos privados para cada sección
        private static void SeedTallas(NoVacancyDbContext context)
        {
            var tallasSeed = new[]
            {
                new Talla { idTalla = 1, nombre = "XS" },
                new Talla { idTalla = 2, nombre = "S" },
                new Talla { idTalla = 3, nombre = "M" },
                new Talla { idTalla = 4, nombre = "L" },
                new Talla { idTalla = 5, nombre = "XL" },
                new Talla { idTalla = 6, nombre = "XXL" }
            };
            foreach (var talla in tallasSeed)
            {
                if (!context.Tallas.Any(t => t.nombre == talla.nombre))
                    context.Tallas.Add(talla);
            }
            context.SaveChanges();
        }

        private static void SeedCategorias(NoVacancyDbContext context)
        {
            var categoriasSeed = new[]
            {
                new Categoria { idCategoria = 1, nombre = "Camisetas" },
                new Categoria { idCategoria = 2, nombre = "Pantalones" },
                new Categoria { idCategoria = 3, nombre = "Vestidos" },
                new Categoria { idCategoria = 4, nombre = "Faldas" },
                new Categoria { idCategoria = 5, nombre = "Chaquetas" },
                new Categoria { idCategoria = 6, nombre = "Ropa interior" },
                new Categoria { idCategoria = 7, nombre = "Zapatos" },
                new Categoria { idCategoria = 8, nombre = "Accesorios" }
            };
            foreach (var categoria in categoriasSeed)
            {
                if (!context.Categorias.Any(c => c.nombre == categoria.nombre))
                    context.Categorias.Add(categoria);
            }
            context.SaveChanges();
        }

        private static void SeedColores(NoVacancyDbContext context)
        {
            var coloresSeed = new[]
            {
                new Color { nombre = "Negro", codigo = "#000000" },
                new Color { nombre = "Azul", codigo = "#0000FF" },
                new Color { nombre = "Rojo", codigo = "#FF0000" }
            };
            foreach (var color in coloresSeed)
            {
                if (!context.Colores.Any(c => c.nombre == color.nombre))
                    context.Colores.Add(color);
            }
            context.SaveChanges();
        }

        private static void SeedProductosBase(NoVacancyDbContext context)
        {
            var productosSeed = new[]
            {
                new Producto
                {
                    nombre = "Playera básica negra",
                    descripcion = "Playera de algodón color negro, cómoda y versátil.",
                    precio = 200.00,
                    cantidad = 50,
                    limite = 2,
                    idTalla = 4,
                    idCategoria = 1,
                    idColor = 1
                },
                new Producto
                {
                    nombre = "Pantalón mezclilla azul",
                    descripcion = "Pantalón de mezclilla azul clásico.",
                    precio = 450.00,
                    cantidad = 30,
                    limite = 1,
                    idTalla = 3,
                    idCategoria = 2,
                    idColor = 2
                },
                new Producto
                {
                    nombre = "Vestido rojo verano",
                    descripcion = "Vestido fresco ideal para verano.",
                    precio = 600.00,
                    cantidad = 20,
                    limite = 3,
                    idTalla = 2,
                    idCategoria = 3,
                    idColor = 3
                }
            };
            foreach (var producto in productosSeed)
            {
                if (!context.Productos.Any(p => p.nombre == producto.nombre))
                    context.Productos.Add(producto);
            }
            context.SaveChanges();
        }

        private static void SeedUsuarios(NoVacancyDbContext context)
        {
            var usuariosSeed = new[]
            {
                new Usuario { UserName = "juan@example.com", Email = "juan@example.com", Nombre = "Juan Pérez", Rol = "Cliente" },
                new Usuario { UserName = "ana@example.com", Email = "ana@example.com", Nombre = "Ana López", Rol = "Cliente" },
                new Usuario { UserName = "admin@example.com", Email = "admin@example.com", Nombre = "Admin", Rol = "Administrador" }
            };
            foreach (var usuario in usuariosSeed)
            {
                if (!context.Users.Any(u => u.Email == usuario.Email))
                    context.Users.Add(usuario);
            }
            context.SaveChanges();
        }

        private static void SeedCarritosYLineas(NoVacancyDbContext context)
        {
            var usuario1 = context.Users.FirstOrDefault(u => u.Email == "juan@example.com");
            var usuario2 = context.Users.FirstOrDefault(u => u.Email == "ana@example.com");
            if (usuario1 != null && usuario2 != null)
            {
                var carritosSeed = new[]
                {
                    new CarritoCabecera { Id = usuario1.Id },
                    new CarritoCabecera { Id = usuario2.Id }
                };
                foreach (var carrito in carritosSeed)
                {
                    if (!context.CarritosCabecera.Any(c => c.Id == carrito.Id))
                        context.CarritosCabecera.Add(carrito);
                }
                context.SaveChanges();

                var carrito1 = context.CarritosCabecera.FirstOrDefault(c => c.Id == usuario1.Id);
                var carrito2 = context.CarritosCabecera.FirstOrDefault(c => c.Id == usuario2.Id);
                var producto1 = context.Productos.FirstOrDefault(p => p.nombre == "Playera básica negra");
                var producto2 = context.Productos.FirstOrDefault(p => p.nombre == "Pantalón mezclilla azul");
                var producto3 = context.Productos.FirstOrDefault(p => p.nombre == "Vestido rojo verano");
                if (carrito1 != null && carrito2 != null && producto1 != null && producto2 != null && producto3 != null)
                {
                    var lineasSeed = new[]
                    {
                        new CarritoLinea { idCarrito = carrito1.idCarrito, idProducto = producto1.idProducto, cantidad = 2 },
                        new CarritoLinea { idCarrito = carrito1.idCarrito, idProducto = producto2.idProducto, cantidad = 1 },
                        new CarritoLinea { idCarrito = carrito2.idCarrito, idProducto = producto3.idProducto, cantidad = 1 }
                    };
                    foreach (var linea in lineasSeed)
                    {
                        if (!context.CarritosLineas.Any(l => l.idCarrito == linea.idCarrito && l.idProducto == linea.idProducto))
                            context.CarritosLineas.Add(linea);
                    }
                    context.SaveChanges();

                    // Pedidos y detalles
                    var pedidosSeed = new[]
                    {
                        new Pedido { idCarrito = carrito1.idCarrito, rfc = "JUAP800101XXX", regimen = "General", codigoPostal = "12345" },
                        new Pedido { idCarrito = carrito2.idCarrito, rfc = "ANAL900202YYY", regimen = "General", codigoPostal = "54321" }
                    };
                    foreach (var pedido in pedidosSeed)
                    {
                        if (!context.Pedidos.Any(p => p.idCarrito == pedido.idCarrito))
                            context.Pedidos.Add(pedido);
                    }
                    context.SaveChanges();

                    var pedido1 = context.Pedidos.FirstOrDefault(p => p.idCarrito == carrito1.idCarrito);
                    var pedido2 = context.Pedidos.FirstOrDefault(p => p.idCarrito == carrito2.idCarrito);
                    if (pedido1 != null && pedido2 != null)
                    {
                        var detallesSeed = new[]
                        {
                            new Detalle { idDetalle = 1, idPedido = pedido1.idPedido, monto = 850.00 },
                            new Detalle { idDetalle = 2, idPedido = pedido2.idPedido, monto = 600.00 }
                        };
                        foreach (var detalle in detallesSeed)
                        {
                            if (!context.Set<Detalle>().Any(d => d.idDetalle == detalle.idDetalle && d.idPedido == detalle.idPedido))
                                context.Set<Detalle>().Add(detalle);
                        }
                        context.SaveChanges();
                    }
                }
            }
        }

        private static void SeedImagenesYReseniasBase(NoVacancyDbContext context)
        {
            var producto1 = context.Productos.FirstOrDefault(p => p.nombre == "Playera básica negra");
            var producto2 = context.Productos.FirstOrDefault(p => p.nombre == "Pantalón mezclilla azul");
            var producto3 = context.Productos.FirstOrDefault(p => p.nombre == "Vestido rojo verano");
            if (producto1 != null && producto2 != null && producto3 != null)
            {
                var imagenesSeed = new[]
                {
                    new Imagen { nombre = "playera_negra.png", idProducto = producto1.idProducto },
                    new Imagen { nombre = "pantalon_azul.png", idProducto = producto2.idProducto },
                    new Imagen { nombre = "vestido_rojo.jpg", idProducto = producto3.idProducto }
                };
                foreach (var imagen in imagenesSeed)
                {
                    if (!context.Imagenes.Any(i => i.nombre == imagen.nombre && i.idProducto == imagen.idProducto))
                        context.Imagenes.Add(imagen);
                }
                context.SaveChanges();

                var reseniasSeed = new[]
                {
                    new Resenia { resenia = "Muy buena calidad", calificacion = 5, idProducto = producto1.idProducto },
                    new Resenia { resenia = "Cómodo y bonito", calificacion = 4, idProducto = producto2.idProducto },
                    new Resenia { resenia = "Perfecto para el verano", calificacion = 5, idProducto = producto3.idProducto }
                };
                foreach (var resenia in reseniasSeed)
                {
                    if (!context.Resenias.Any(r => r.resenia == resenia.resenia && r.idProducto == resenia.idProducto))
                        context.Resenias.Add(resenia);
                }
                context.SaveChanges();
            }
        }

        private static void SeedProductosExtra(NoVacancyDbContext context)
        {
            var productosExtra = new[]
            {
                new Producto { nombre = "Camisa blanca formal", descripcion = "Camisa de vestir blanca, manga larga.", precio = 350.00, cantidad = 40, limite = 2, idTalla = 3, idCategoria = 1, idColor = 1 },
                new Producto { nombre = "Falda azul plisada", descripcion = "Falda azul, ideal para primavera.", precio = 320.00, cantidad = 25, limite = 2, idTalla = 2, idCategoria = 4, idColor = 2 },
                new Producto { nombre = "Chaqueta de mezclilla", descripcion = "Chaqueta clásica de mezclilla.", precio = 700.00, cantidad = 15, limite = 1, idTalla = 4, idCategoria = 5, idColor = 2 },
                new Producto { nombre = "Zapatos deportivos rojos", descripcion = "Zapatos cómodos para correr.", precio = 500.00, cantidad = 35, limite = 2, idTalla = 5, idCategoria = 7, idColor = 3 },
                new Producto { nombre = "Blusa negra elegante", descripcion = "Blusa de seda negra para ocasiones especiales.", precio = 400.00, cantidad = 18, limite = 1, idTalla = 2, idCategoria = 1, idColor = 1 },
                new Producto { nombre = "Pantalón corto azul", descripcion = "Short de mezclilla azul.", precio = 250.00, cantidad = 22, limite = 2, idTalla = 3, idCategoria = 2, idColor = 2 },
                new Producto { nombre = "Vestido largo rojo", descripcion = "Vestido largo elegante color rojo.", precio = 900.00, cantidad = 10, limite = 1, idTalla = 4, idCategoria = 3, idColor = 3 },
                new Producto { nombre = "Cinturón negro", descripcion = "Cinturón de piel color negro.", precio = 150.00, cantidad = 50, limite = 3, idTalla = 1, idCategoria = 8, idColor = 1 },
                new Producto { nombre = "Calcetas deportivas", descripcion = "Pack de 3 pares de calcetas.", precio = 120.00, cantidad = 60, limite = 5, idTalla = 3, idCategoria = 6, idColor = 1 },
                new Producto { nombre = "Gorra azul", descripcion = "Gorra casual azul.", precio = 180.00, cantidad = 30, limite = 2, idTalla = 1, idCategoria = 8, idColor = 2 }
            };
            foreach (var producto in productosExtra)
            {
                if (!context.Productos.Any(p => p.nombre == producto.nombre))
                    context.Productos.Add(producto);
            }
            context.SaveChanges();
        }

        private static void SeedLineasYPedidosExtra(NoVacancyDbContext context)
        {
            var todosProductos = context.Productos.ToList();
            var todosCarritos = context.CarritosCabecera.ToList();
            var usuarios = context.Users.ToList();
            int pedidoIndex = 100;
            foreach (var usuario in usuarios)
            {
                var carrito = context.CarritosCabecera.FirstOrDefault(c => c.Id == usuario.Id);
                if (carrito == null) continue;
                foreach (var prod in todosProductos.OrderBy(x => Guid.NewGuid()).Take(4))
                {
                    if (!context.CarritosLineas.Any(l => l.idCarrito == carrito.idCarrito && l.idProducto == prod.idProducto))
                    {
                        context.CarritosLineas.Add(new CarritoLinea { idCarrito = carrito.idCarrito, idProducto = prod.idProducto, cantidad = new Random().Next(1, 4) });
                    }
                }
                context.SaveChanges();
                for (int i = 0; i < 3; i++)
                {
                    var pedido = new Pedido { idCarrito = carrito.idCarrito, rfc = $"RFC{usuario.Id.Substring(0,4)}{pedidoIndex}", regimen = "General", codigoPostal = $"{10000 + pedidoIndex}" };
                    if (!context.Pedidos.Any(p => p.idCarrito == carrito.idCarrito && p.codigoPostal == pedido.codigoPostal))
                    {
                        context.Pedidos.Add(pedido);
                        context.SaveChanges();
                        context.Set<Detalle>().Add(new Detalle { idDetalle = pedidoIndex, idPedido = pedido.idPedido, monto = todosProductos.OrderBy(x => Guid.NewGuid()).Take(2).Sum(x => x.precio) });
                        context.SaveChanges();
                    }
                    pedidoIndex++;
                }
            }
        }

        private static void SeedImagenesYReseniasParaTodos(NoVacancyDbContext context)
        {
            var imagenesDisponibles = new[] {
                "blusa_negra_elegante.webp",
                "camisa_blanca_formal.jpg",
                "camiseta_blanca.jpg",
                "camiseta_negra.jpg",
                "gator.jpg",
                "oso.png",
                "pantalon_911.webp",
                "pantalon_azul.png",
                "playera_blanca.webp",
                "playera_drippy_cock.webp",
                "playera_faguette.webp",
                "playera_mlaria.webp",
                "playera_negra.png",
                "raton.webp",
                "vestido_rojo.jpg"
            };
            var todosProductos = context.Productos.ToList();
            int imgIndex = 0;
            foreach (var prod in todosProductos)
            {
                if (prod?.nombre != null)
                {
                    string imgName = imagenesDisponibles[imgIndex % imagenesDisponibles.Length];
                    if (!context.Imagenes.Any(i => i.idProducto == prod.idProducto))
                    {
                        context.Imagenes.Add(new Imagen { nombre = imgName, idProducto = prod.idProducto });
                    }
                    if (!context.Resenias.Any(r => r.idProducto == prod.idProducto))
                    {
                        context.Resenias.Add(new Resenia { resenia = $"Reseña de ejemplo para {prod.nombre}", calificacion = new Random().Next(3, 6), idProducto = prod.idProducto });
                    }
                    imgIndex++;
                }
            }
            context.SaveChanges();
        }

        private static void SeedProductosAleatorios(NoVacancyDbContext context)
        {
            var random = new Random();
            int totalProductos = 50;
            var tallas = context.Tallas.ToList();
            var categorias = context.Categorias.ToList();
            var colores = context.Colores.ToList();
            var imagenesDisponibles = new[] {
                "blusa_negra_elegante.webp",
                "camisa_blanca_formal.jpg",
                "camiseta_blanca.jpg",
                "camiseta_negra.jpg",
                "gator.jpg",
                "oso.png",
                "pantalon_911.webp",
                "pantalon_azul.png",
                "playera_blanca.webp",
                "playera_drippy_cock.webp",
                "playera_faguette.webp",
                "playera_mlaria.webp",
                "playera_negra.png",
                "raton.webp",
                "vestido_rojo.jpg"
            };
            for (int i = 1; i <= totalProductos; i++)
            {
                string nombre = $"Producto Auto {i}";
                if (!context.Productos.Any(p => p.nombre == nombre))
                {
                    int numColores = random.Next(1, Math.Min(4, colores.Count + 1));
                    var coloresProducto = colores.OrderBy(x => random.Next()).Take(numColores).ToList();
                    int numTallas = random.Next(1, Math.Min(4, tallas.Count + 1));
                    var tallasProducto = tallas.OrderBy(x => random.Next()).Take(numTallas).ToList();
                    int numImagenes = random.Next(1, 4);
                    var imagenesProducto = imagenesDisponibles.OrderBy(x => random.Next()).Take(numImagenes).ToList();
                    var categoria = categorias[random.Next(categorias.Count)];
                    // Crear un solo producto con múltiples colores y tallas asociadas (simulado por registros en tablas de relación si existieran)
                    var producto = new Producto
                    {
                        nombre = nombre,
                        descripcion = $"Descripción automática para {nombre}",
                        precio = random.Next(100, 1000) + random.NextDouble(),
                        cantidad = random.Next(10, 100),
                        limite = random.Next(1, 5),
                        idTalla = tallasProducto[0].idTalla, // Asignar una talla principal (puedes ajustar si tienes tabla de relación)
                        idCategoria = categoria.idCategoria,
                        idColor = coloresProducto[0].idColor // Asignar un color principal (puedes ajustar si tienes tabla de relación)
                    };
                    context.Productos.Add(producto);
                    context.SaveChanges(); // Para obtener el idProducto
                    // Agregar imágenes
                    foreach (var img in imagenesProducto)
                    {
                        if (!context.Imagenes.Any(im => im.idProducto == producto.idProducto && im.nombre == img))
                        {
                            context.Imagenes.Add(new Imagen { nombre = img, idProducto = producto.idProducto });
                        }
                    }
                    // Opcional: guardar info de colores y tallas extra en comentarios o logs si no hay tabla de relación
                }
            }
            context.SaveChanges();
        }

        private static void SeedPedidosYDetallesAleatorios(NoVacancyDbContext context)
        {
            var random = new Random();
            var todosUsuarios = context.Users.ToList();
            var todosCarritosAuto = context.CarritosCabecera.ToList();
            var todosProductosAuto = context.Productos.ToList();
            int pedidoAutoIndex = 1000;
            foreach (var usuario in todosUsuarios)
            {
                var carrito = context.CarritosCabecera.FirstOrDefault(c => c.Id == usuario.Id);
                if (carrito == null) continue;
                int pedidosPorUsuario = random.Next(3, 7);
                for (int j = 0; j < pedidosPorUsuario; j++)
                {
                    var pedido = new Pedido
                    {
                        idCarrito = carrito.idCarrito,
                        rfc = $"RFC{usuario.Id.Substring(0,4)}{pedidoAutoIndex}",
                        regimen = "General",
                        codigoPostal = $"{random.Next(10000, 99999)}"
                    };
                    context.Pedidos.Add(pedido);
                    context.SaveChanges();
                    var productosPedido = todosProductosAuto.OrderBy(x => random.Next()).Take(random.Next(1, 5)).ToList();
                    double monto = productosPedido.Sum(x => x.precio);
                    context.Set<Detalle>().Add(new Detalle { idDetalle = pedidoAutoIndex, idPedido = pedido.idPedido, monto = monto });
                    context.SaveChanges();
                    pedidoAutoIndex++;
                }
            }
        }
    }
}
