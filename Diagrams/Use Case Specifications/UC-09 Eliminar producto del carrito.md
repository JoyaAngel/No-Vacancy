| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-09 Eliminar producto del carrito | Date: 2025-03-26 |

# Sistema de Venta de Ropa
# Use Case: UC-09 Eliminar producto del carrito

## 1. Descripción
    El sistema permite a los usuarios eliminar un producto del carrito de compras.

## 2. Actores
    2.1 Usuario: Persona que desea eliminar un producto del carrito de compras.

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe haber iniciado sesión en el sistema.
    3.3 El usuario debe tener al menos un producto en el carrito de compras.

## 4. Flujo básico de eventos
    1. El usuario selecciona el carrito de compras.
    2. El sistema muestra los productos en el carrito de compras con sus detalles (nombre, descripción, precio, cantidad y subtotal).
    3. El usuario selecciona el producto que desea eliminar del carrito.
    4. El sistema muestra un mensaje de confirmación para eliminar el producto seleccionado.
    5. El usuario confirma la eliminación del producto.
    6. El sistema elimina el producto del carrito de compras y actualiza el subtotal.
    7. El usuario puede continuar eliminando otros productos o proceder a la compra.

## 5. Flujo alternativo
    5.1 Si el usuario decide cancelar la operación de eliminación:
        1. El sistema regresa al carrito de compras sin realizar cambios.
        2. El flujo básico de eventos se detiene.

## 6. Postcondiciones
    6.1 El producto seleccionado se elimina del carrito de compras del usuario.
    6.2 El sistema actualiza el subtotal del carrito de compras.

## 7. Excepciones