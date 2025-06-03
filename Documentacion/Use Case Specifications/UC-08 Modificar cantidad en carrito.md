| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-08 Modificar cantidad en carrito | Date: 2025-03-26 |

# Sistema de Venta de Ropa
# Use Case: UC-08 Modificar cantidad en carrito

## 1. Descripción
    El sistema permite a los usuarios modificar la cantidad de un producto en el carrito de compras.

## 2. Actores
    2.1 Usuario: Persona que desea modificar la cantidad de un producto en el carrito de compras.

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe haber iniciado sesión en el sistema.
    3.3 El usuario debe tener al menos un producto en el carrito de compras.

## 4. Flujo básico de eventos
    1. El usuario selecciona el carrito de compras.
    2. El sistema muestra los productos en el carrito de compras con sus detalles (nombre, descripción, precio, cantidad y subtotal).
    3. El usuario selecciona el producto cuya cantidad desea modificar.
    4. El sistema muestra un campo para ingresar la nueva cantidad del producto seleccionado.
    5. El usuario ingresa la nueva cantidad deseada del producto.
    6. El sistema verifica la disponibilidad del producto en la nueva cantidad ingresada. 
    7. El sistema muestra un mensaje de confirmación de la modificación y actualiza el carrito de compras.
    8. El usuario puede continuar modificando la cantidad de otros productos o proceder a la compra.

## 5. Flujo alternativo
    5.1 Si el usuario intenta modificar la cantidad a un número no válido (por ejemplo, menor que 1 o mayor que la disponibilidad):
        1. El sistema muestra un mensaje de error y solicita al usuario que ingrese una cantidad válida.
        2. El usuario ingresa una cantidad válida.
        3. El Flujo básico de eventos continúa desde el paso 6.
    5.2 Si el usuario decide cancelar la operación:
        1. El sistema muestra un mensaje de confirmación de cancelación.
        2. El usuario confirma la cancelación.
        3. El sistema regresa al carrito de compras.

## 6. Postcondiciones
    6.1 La cantidad del producto seleccionado se actualiza en el carrito de compras del usuario.
    6.2 El sistema actualiza el subtotal del carrito de compras.

## 7. Excepciones