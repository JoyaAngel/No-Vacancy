| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-12 Agregar producto | Date: 2025-03-26 |

# Sistema de Venta de Ropa
# Use Case: UC-12 Agregar producto

## 1. Descripción
    El sistema permite a los administradores agregar un nuevo producto al catálogo de productos.

## 2. Actores
    2.1 Administrador: Persona encargada de gestionar el catálogo de productos.

## 3. Precondiciones
    3.1 El administrador debe tener acceso a Internet.
    3.2 El administrador debe haber iniciado sesión en el sistema con privilegios de administrador.

## 4. Flujo básico de eventos
    1. El administrador selecciona la opción "Agregar producto" en el menú de administración.
    2. El sistema muestra un formulario para ingresar los detalles del nuevo producto (nombre, descripción, colores disponibles, precio, imágenes, disponibilidad, talla y opcionalmente, si hay un máximo de piezas que puede comprar el cliente).
    3. El administrador completa el formulario con la información del nuevo producto.
    4. El administrador hace clic en el botón "Guardar" para agregar el producto al catálogo.
    5. El sistema valida la información ingresada y guarda el nuevo producto en la base de datos.
    6. El sistema muestra un mensaje de confirmación de que el producto ha sido agregado exitosamente.
    7. El administrador puede continuar agregando más productos o regresar al menú principal.

## 5. Flujo alternativo
    5.1 Si el administrador intenta agregar un producto con información incompleta o inválida:
        1. El sistema muestra un mensaje de error indicando los campos que deben corregirse.
        2. El administrador corrige la información y vuelve al paso 3 del flujo básico de eventos.
    5.2 Si el administrador decide cancelar la operación:
        1. El sistema muestra un mensaje de confirmación de cancelación.
        2. El administrador confirma la cancelación.
        3. El sistema regresa al menú de administración.
        4. El flujo básico de eventos se detiene.

## 6. Postcondiciones
    6.1 El nuevo producto se agrega al catálogo de productos del sistema.
    6.2 El sistema actualiza la base de datos con la información del nuevo producto.
    6.3 El sistema envía un correo electrónico de confirmación al administrador con los detalles del producto agregado.

## 7. Excepciones
    E1. Si el administrador intenta agregar un producto que ya existe en el catálogo:
        1. El sistema muestra un mensaje de error indicando que el producto ya existe y solicita al administrador que ingrese un nuevo producto.
        2. El flujo básico de eventos se detiene.