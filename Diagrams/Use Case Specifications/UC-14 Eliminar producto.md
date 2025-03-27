| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-14 Eliminar producto | Date: 2025-03-26 |

# Sistema de Venta de Ropa
# Use Case: UC-14 Eliminar producto

## 1. Descripción
    El sistema permite a los administradores eliminar un producto del catálogo de productos.

## 2. Actores
    2.1 Administrador: Persona encargada de gestionar los productos en el sistema.

## 3. Precondiciones
    3.1 El administrador debe tener acceso a Internet.
    3.2 El administrador debe haber iniciado sesión en el sistema con privilegios de administrador.

## 4. Flujo básico de eventos
    1. El administrador selecciona la opción "Eliminar producto" en el menú de administración.
    2. El sistema muestra una lista de productos existentes.
    3. El administrador selecciona el producto que desea eliminar.
    4. El sistema muestra un mensaje de confirmación de eliminación.
    5. El administrador confirma la eliminación del producto.
    6. El sistema elimina el producto de la base de datos y muestra un mensaje de confirmación de que el producto ha sido eliminado exitosamente.
    7. El administrador puede continuar eliminando más productos o regresar al menú principal.

## 5. Flujo alternativo
    5.1 Si el administrador decide cancelar la operación:
        1. El sistema muestra un mensaje de confirmación de cancelación.
        2. El administrador confirma la cancelación.
        3. El sistema regresa al menú de administración.
        4. El flujo básico de eventos se detiene.

## 6. Postcondiciones
    6.1 El producto seleccionado se elimina del catálogo de productos del sistema.
    6.2 El sistema actualiza la base de datos para reflejar la eliminación del producto.
    6.3 El sistema envía un correo electrónico de confirmación al administrador con los detalles del producto eliminado.

## 7. Excepciones