<!--
  Proyecto: No-Vacancy
  Autor: [Tu nombre y equipo]
  Fecha: 3 de junio de 2025
  Licencia: MIT
-->

# No-Vacancy

![MIT License](https://img.shields.io/badge/license-MIT-green)

> **Resumen Ejecutivo:**
>
> No-Vacancy es una aplicación web para la gestión de productos, pedidos y usuarios, orientada a la administración de un catálogo y la experiencia de compra en línea. Este manual describe la funcionalidad de los principales módulos (controladores), con énfasis en el carrito de compras.

---

## Organización del equipo

El equipo de desarrollo de No-Vacancy está conformado por miembros con roles definidos para asegurar una gestión eficiente del proyecto. Cada integrante asume responsabilidades específicas en áreas como desarrollo backend, frontend, documentación, pruebas y gestión de tareas.

Para la coordinación y seguimiento de actividades, utilizamos la plataforma Trello. Puedes consultar el tablero del proyecto en el siguiente enlace:

[Trello - No-Vacancy](https://trello.com/invite/b/67df53a8bf41c50c9c3b7200/ATTI3eb1f6769e73e755e746e254f6308fb01C32AD1A/no-vacancy)

---

## Índice
1. [Requisitos e Instalación](#requisitos-e-instalacion)
2. [Estructura del Proyecto](#estructura-del-proyecto)
3. [Manual de Usuario por Módulo](#manual-de-usuario-por-modulo)
    - [UsuarioController](#usuariocontroller)
    - [ColorController](#colorcontroller)
    - [CarritoLineaController](#carritolineacontroller)
    - [CarritoCabeceraController](#carritocabeceracontroller)
    - [AdminController](#admincontroller)
4. [Recomendaciones Generales](#recomendaciones-generales)
5. [Contacto y Soporte](#contacto-y-soporte)
6. [Diagramas del Sistema](#diagramas-del-sistema)
7. [Especificaciones de Casos de Uso](#especificaciones-casos-uso)

---

## 1. Requisitos e Instalación <a name="requisitos-e-instalacion"></a>

- **.NET 6.0 SDK** o superior
- **SQL Server** (o el motor configurado en `appsettings.json`)
- **Visual Studio 2022** o **Visual Studio Code**

### Instalación Rápida
```powershell
dotnet restore
dotnet ef database update
dotnet run
```
La aplicación estará disponible en la URL indicada en la consola (por defecto, `https://localhost:5001`).

---

## 2. Estructura del Proyecto <a name="estructura-del-proyecto"></a>

- `Controllers/`: Controladores MVC
- `Models/`: Modelos de datos
- `Views/`: Vistas Razor
- `Data/`: Contexto de base de datos
- `wwwroot/`: Archivos estáticos
- `Migrations/`: Migraciones de Entity Framework
- `Diagrams/`: Diagramas y especificaciones

---

## 3. Manual de Usuario por Módulo <a name="manual-de-usuario-por-modulo"></a>

### UsuarioController
> Gestiona todas las operaciones relacionadas con los usuarios.

**Funcionalidades principales:**
- Registro de usuario
- Inicio de sesión
- Edición de perfil
- Recuperación de contraseña
- Historial de pedidos
- Administración de usuarios (solo administradores)

**Ejemplo de uso:**
- Para registrarse, acceda a "Registrarse" y complete el formulario.
- Si olvida su contraseña, use "¿Olvidó su contraseña?" para recibir instrucciones.

**Mensajes de error comunes:**
- `Usuario o contraseña incorrectos.`
- `El correo ya está registrado.`
- `No se pudo actualizar el perfil.`

---

### ColorController
> Permite la gestión de colores disponibles para los productos (solo administradores).

**Funcionalidades principales:**
- Ver colores
- Agregar color
- Editar color
- Eliminar color

**Ejemplo de uso:**
- Desde el panel de administración, seleccione "Colores" para ver, agregar o editar colores.

**Mensajes de error comunes:**
- `No se pudo guardar el color.`
- `Color no encontrado.`

---

### CarritoLineaController
> Gestiona las líneas (productos) dentro del carrito de compras.

**Funcionalidades principales:**
- Agregar producto al carrito
- Modificar cantidad
- Eliminar producto
- Vaciar carrito
- Ver carrito
- Facturación

**Ejemplo de uso:**
1. Navegue por el catálogo y seleccione un producto.
2. Haga clic en "Agregar al carrito".
3. Acceda al carrito para modificar cantidades o eliminar productos.
4. Proceda a la compra y solicite factura si lo desea.

**Mensajes de error comunes:**
- `No hay stock suficiente para el producto.`
- `El producto ya está en el carrito.`
- `No se pudo eliminar el producto.`

---

### CarritoCabeceraController
> Gestiona la cabecera del carrito y el proceso de confirmación de pedido.

**Funcionalidades principales:**
- Confirmar pedido
- Validación de stock
- Cálculo de totales
- Creación de pedido
- Envío de confirmación por correo

**Ejemplo de uso:**
1. Desde el carrito, haga clic en "Confirmar compra".
2. Revise el resumen del pedido y confirme.
3. Recibirá un correo con los detalles de la compra.

**Mensajes de error comunes:**
- `No hay stock suficiente para uno o más productos.`
- `Error al procesar el pedido.`

---

### AdminController
> Permite a los administradores gestionar productos y usuarios.

**Funcionalidades principales:**
- Ver productos
- Agregar producto
- Editar producto
- Eliminar producto
- Gestión de usuarios

**Ejemplo de uso:**
- Acceda al panel de administración para gestionar productos y usuarios.

**Mensajes de error comunes:**
- `No se pudo guardar el producto.`
- `Producto no encontrado.`

---

## 4. Recomendaciones Generales <a name="recomendaciones-generales"></a>

> **Tip:** Revise siempre el stock antes de confirmar una compra. Mantenga actualizados sus datos de perfil para recibir notificaciones.

- Si experimenta problemas, contacte al soporte técnico.
- Consulte los diagramas y casos de uso en la carpeta `Diagrams/` para mayor detalle.

---

## 5. Contacto y Soporte <a name="contacto-y-soporte"></a>

Para dudas o soporte, contacte al equipo de desarrollo.

---

## Diagramas del Sistema <a name="diagramas-del-sistema"></a>

A continuación se presentan los principales diagramas del sistema. Puedes encontrar los archivos fuente y versiones editables en la carpeta `Documentacion/Diagrams/`.

### Diagrama de Componentes

![Diagrama de Componentes](Documentacion/Diagrams/Components%20Diagram/Diagrama%20de%20componentes.png)

> **Descripción:**
> Este diagrama muestra la arquitectura general del sistema, incluyendo la interacción entre la interfaz de usuario, el backend y la base de datos.

---

### Diagramas de Casos de Uso

#### 1. Registro e Inicio de Sesión

![Registro e Inicio de Sesión](Documentacion/Diagrams/Use%20Case%20Diagrams/sing%20in%20and%20log%20in.png)

#### 2. Navegación y Búsqueda de Productos

![Navegación y Búsqueda de Productos](Documentacion/Diagrams/Use%20Case%20Diagrams/product%20navigation%20and%20search.png)

#### 3. Carrito de Compras y Pedidos

![Carrito de Compras y Pedidos](Documentacion/Diagrams/Use%20Case%20Diagrams/shopping%20cart%20and%20orders.png)

#### 4. Gestión de Productos (Administrador)

![Gestión de Productos](Documentacion/Diagrams/Use%20Case%20Diagrams/product%20management.png)

#### 5. Pedidos e Interacción con Reseñas

![Pedidos e Interacción con Reseñas](Documentacion/Diagrams/Use%20Case%20Diagrams/order%20and%20review%20management.png)

> **Nota:** Puedes consultar los archivos fuente y versiones editables de estos diagramas en la carpeta `Documentacion/Diagrams/Use Case Diagrams/`.

---

### Diagramas de Secuencia

A continuación se muestran los principales diagramas de secuencia del sistema. Puedes encontrar los archivos fuente y versiones editables en la carpeta `Documentacion/Diagrams/Sequence diagrams/`.

#### 1. Autenticación

![Diagrama de Secuencia - Autenticación](Documentacion/Diagrams/Sequence%20diagrams/Authenticate.png)

> **Descripción:**
> Este diagrama ilustra el proceso de autenticación de usuarios, desde la introducción de credenciales hasta la validación y respuesta del sistema.

#### 2. Registro de Usuario

![Diagrama de Secuencia - Registro](Documentacion/Diagrams/Sequence%20diagrams/Register.png)

> **Descripción:**
> Representa el flujo de registro de un nuevo usuario, incluyendo la validación de datos y la creación de la cuenta.

#### 3. Comprar Producto

![Diagrama de Secuencia - Comprar producto](Documentacion/Diagrams/Sequence%20diagrams/Buy%20product.png)

> **Descripción:**
> Muestra el proceso de compra de un producto, desde la selección en el catálogo hasta la confirmación del pedido.

---

### Diagrama de Clases

![Diagrama de Clases](Documentacion/Diagrams/Class%20Diagram/class%20diagram.png)

> **Descripción:**
> Este diagrama muestra la estructura de clases principales del sistema, sus atributos y relaciones.

---

### Diagrama Entidad-Relación

![Diagrama Entidad-Relación](Documentacion/Diagrams/Entity-Relationship%20Diagram/Entity-Relationship%20Diagram.png)

> **Descripción:**
> Este diagrama representa la estructura lógica de la base de datos, mostrando entidades, atributos y relaciones clave.

---

## Especificaciones de Casos de Uso <a name="especificaciones-casos-uso"></a>

A continuación se incluyen las especificaciones detalladas de los principales casos de uso del sistema. Puedes consultar todos los archivos en la carpeta `Diagrams/Use Case Specifications/`.

### UC-01 Registro de usuario
[Ver especificación completa](Diagrams/Use%20Case%20Specifications/UC-01%20Registro%20de%20usuario.md)
```
El sistema permite a los usuarios registrarse para crear una cuenta.

Actores: Usuario
Precondiciones: Acceso a Internet, correo electrónico válido.
Flujo básico:
  1. El usuario accede a la página de registro.
  2. El sistema muestra el formulario de registro.
  3. El usuario completa el formulario (nombre, correo, contraseña).
  4. El usuario envía el formulario.
  5. El sistema valida la información.
  6. Si es válida, crea la cuenta y envía correo de confirmación.
  7. Muestra mensaje de éxito.
Flujos alternativos:
  - Información inválida: muestra error y solicita corrección.
  - Correo ya registrado: muestra error y permite recuperar contraseña o ingresar otro correo.
Postcondición: Cuenta creada.
```

### UC-02 Inicio de sesión
[Ver especificación completa](Diagrams/Use%20Case%20Specifications/UC-02%20Inicio%20de%20sesión.md)
```
El sistema permite a los usuarios iniciar sesión en su cuenta existente.

Actores: Usuario, Administrador
Precondiciones: Acceso a Internet, cuenta registrada.
Flujo básico:
  1. El usuario accede a la página de inicio de sesión.
  2. El sistema muestra el formulario.
  3. El usuario ingresa correo y contraseña.
  4. El usuario envía el formulario.
  5. El sistema valida la información.
  6. Si es válida, inicia sesión y redirige al perfil.
  7. Muestra mensaje de éxito.
Flujos alternativos:
  - Información inválida: muestra error y solicita corrección.
  - Contraseña olvidada: permite recuperar contraseña (UC-03).
Postcondición: Usuario autenticado.
```

### UC-07 Agregar producto al carrito
[Ver especificación completa](Diagrams/Use%20Case%20Specifications/UC-07%20Agregar%20producto%20al%20carrito.md)
```
El sistema permite a los usuarios agregar un producto al carrito de compras.

Actores: Usuario
Precondiciones: Acceso a Internet, sesión iniciada.
Flujo básico:
  1. El sistema muestra productos disponibles.
  2. El usuario selecciona un producto.
  3. El sistema muestra detalles del producto.
  4. El usuario selecciona cantidad.
  5. El usuario hace clic en "Agregar al carrito".
  6. El sistema agrega el producto y muestra confirmación.
  7. El usuario puede seguir agregando productos o comprar.
Flujos alternativos:
  - Sin stock suficiente: muestra error y solicita nueva cantidad.
  - Cancelar operación: muestra confirmación y regresa a la lista.
Postcondición: Producto agregado al carrito.
```

### UC-10 Registrar pedido
[Ver especificación completa](Diagrams/Use%20Case%20Specifications/UC-10%20Registrar%20pedido.md)
```
El sistema permite a los usuarios registrar un pedido después de agregar productos al carrito.

Actores: Usuario
Precondiciones: Acceso a Internet, sesión iniciada, al menos un producto en el carrito.
Flujo básico:
  1. El usuario selecciona el carrito.
  2. El sistema muestra los productos en el carrito.
  3. El usuario selecciona "Registrar pedido".
  4. El sistema solicita dirección de envío y método de pago.
  5. El usuario ingresa la información.
  6. El sistema verifica la información.
  7. El sistema registra el pedido y muestra confirmación.
  8. El usuario puede ver el estado del pedido en su perfil.
Flujos alternativos:
  - Carrito vacío: muestra error y solicita agregar productos.
  - Cancelar operación: muestra confirmación y detiene el flujo.
Postcondición: Pedido registrado y correo de confirmación enviado.
```

### UC-17 Ver historial de pedidos
[Ver especificación completa](Diagrams/Use%20Case%20Specifications/UC-17%20Ver%20historial%20de%20pedidos.md)
```
El sistema permite a los usuarios ver su historial de pedidos realizados.

Actores: Usuario, Administrador
Precondiciones: Acceso a Internet, sesión iniciada.
Flujo básico:
  1. El usuario selecciona "Historial de pedidos" en su perfil.
  2. El sistema muestra la lista de pedidos realizados.
  3. El usuario puede seleccionar un pedido para ver detalles.
  4. El sistema muestra los detalles del pedido.
  5. El usuario puede regresar a la lista o salir.
Flujos alternativos:
  - Sin pedidos: muestra mensaje indicando que no hay pedidos.
Postcondición: El usuario visualiza su historial de pedidos.
```

### UC-18 Dejar reseña de producto
[Ver especificación completa](Diagrams/Use%20Case%20Specifications/UC-18%20Dejar%20reseña%20de%20producto.md)
```
El sistema permite a los usuarios dejar reseñas sobre los productos comprados.

Actores: Usuario
Precondiciones: Acceso a Internet, sesión iniciada, haber comprado el producto.
Flujo básico:
  1. El usuario selecciona "Dejar reseña" en la página del producto.
  2. El sistema muestra el formulario de reseña.
  3. El usuario completa el formulario (calificación, comentario, imágenes).
  4. El usuario envía la reseña.
  5. El sistema valida y guarda la reseña.
  6. El sistema muestra confirmación.
Flujos alternativos:
  - Campos obligatorios incompletos: muestra error y solicita completar.
Postcondición: Reseña guardada y visible en la página del producto.
```

> **Nota:** Puedes consultar todas las especificaciones completas en la carpeta `Diagrams/Use Case Specifications/`.

---

**Fin del manual**
