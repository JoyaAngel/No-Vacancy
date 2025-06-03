| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-13 Registro de Usuario | Date: 2025-03-26 |

# Sistema de Venta de Ropa
# Use Case: UC-13 Modificar producto

## 1. Descripción
    El sistema permite a los administradores modificar la información de un producto existente.

## 2. Actores
    2.1 Administrador: Persona encargada de gestionar los productos en el sistema.

## 3. Precondiciones
    3.1 El administrador debe tener acceso a Internet.
    3.2 El administrador debe haber iniciado sesión en el sistema con privilegios de administrador.

## 4. Flujo básico de eventos
    1. El administrador selecciona la opción "Modificar producto" en el menú de administración.
    2. El sistema muestra una lista de productos existentes.
    3. El administrador selecciona el producto que desea modificar.
    4. El sistema muestra un formulario con los detalles del producto seleccionado (nombre, descripción, colores disponibles, precio, imágenes, disponibilidad, talla y opcionalmente, si hay un máximo de piezas que puede comprar el cliente).
    5. El administrador modifica la información del producto según sea necesario.
    6. El administrador hace clic en el botón "Guardar" para guardar los cambios realizados.
    7. El sistema valida la información ingresada y actualiza el producto en la base de datos.
    8. El sistema muestra un mensaje de confirmación de que el producto ha sido modificado exitosamente.
    9. El administrador puede continuar modificando más productos o regresar al menú principal.

## 5. Flujo alternativo
    5.1 Si el administrador intenta modificar un producto con información incompleta o inválida:
        1. El sistema muestra un mensaje de error indicando los campos que deben corregirse.
        2. El administrador corrige la información y vuelve al paso 5 del flujo básico de eventos.
    5.2 Si el administrador decide cancelar la operación:
        1. El sistema muestra un mensaje de confirmación de cancelación.
        2. El administrador confirma la cancelación.
        3. El sistema regresa al menú de administración.
        4. El flujo básico de eventos se detiene.

## 6. Postcondiciones
    6.1 El producto seleccionado se actualiza en el catálogo de productos del sistema.
    6.2 El sistema actualiza la base de datos con la información del producto modificado.
    6.3 El sistema envía un correo electrónico de confirmación al administrador con los detalles del producto modificado.

## 7. Excepciones