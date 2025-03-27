| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-07 Agregar producto al carrito | Date: 2025-03-25 |

# Sistema de Venta de Ropa
# Use Case: UC-07 Agregar producto al carrito

## 1. Descripción
    El sistema permite a los usuarios agregar un producto al carrito de compras.

## 2. Actores
    2.1 Usuario: Persona que desea agregar un producto al carrito de compras.

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe haber iniciado sesión en el sistema.

## 4. Flujo básico de eventos
    1. El sistema muestra una lista de productos disponibles.
    2. El usuario selecciona un producto de la lista.
    3. El sistema muestra los detalles del producto seleccionado (nombre, descripción, precio, imágenes, disponibilidad y talla).
    4. El usuario selecciona la cantidad deseada del producto.
    5. El usuario hace clic en el botón "Agregar al carrito".
    6. El sistema agrega el producto al carrito de compras y muestra un mensaje de confirmación.
    7. El usuario puede continuar agregando productos al carrito o proceder completar la compra.

## 5. Flujo alternativo
    5.1 Si el usuario intenta agregar un producto que no está disponible en la cantidad seleccionada:
        1. El sistema muestra un mensaje de error y solicita al usuario que seleccione una cantidad diferente.
        2. El usuario selecciona una cantidad diferente.
        3. El Flujo básico de eventos continúa desde el paso 5.
    5.2 Si el usuario decide cancelar la operación:
        1. El sistema muestra un mensaje de confirmación de cancelación.
        2. El usuario confirma la cancelación.
        3. El sistema regresa a la lista de productos disponibles.

## 6. Postcondiciones
    6.1 El producto seleccionado se agrega al carrito de compras del usuario.
    6.2 El sistema actualiza la cantidad total de productos en el carrito.
    6.3 El sistema envia un correo electrónico de confirmación al usuario con los detalles del producto agregado al carrito.

## 7. Excepciones
