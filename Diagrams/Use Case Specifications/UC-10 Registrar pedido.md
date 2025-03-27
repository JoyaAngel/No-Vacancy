| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-10 Registrar pedido | Date: 2025-03-26 |

# Sistema de Venta de Ropa
# Use Case: UC-10 Registrar pedido

## 1. Descripción
    El sistema permite a los usuarios registrar un pedido después de haber agregado productos al carrito de compras.

## 2. Actores
    2.1 Usuario: Persona que desea registrar un pedido.

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe haber iniciado sesión en el sistema.
    3.3 El usuario debe tener al menos un producto en el carrito de compras.

## 4. Flujo básico de eventos
    1. El usuario selecciona el carrito de compras.
    2. El sistema muestra los productos en el carrito de compras con sus detalles (nombre, descripción, precio, cantidad y subtotal).
    3. El usuario selecciona la opción "Registrar pedido".
    4. El sistema solicita al usuario que ingrese la dirección de envío y el método de pago.
    5. El usuario ingresa la información requerida.
    6. El sistema verifica la validez de la información ingresada.
    7. El sistema registra el pedido y muestra un mensaje de confirmación.
    8. El usuario puede ver el estado del pedido en su perfil.

## 5. Flujo alternativo
    5.1 Si el usuario intenta registrar un pedido sin productos en el carrito:
        1. El sistema muestra un mensaje de error y solicita al usuario que agregue productos al carrito.
        2. El flujo básico de eventos se detiene.
    5.2 Si el usuario decide cancelar la operación:
        1. El sistema muestra un mensaje de confirmación de cancelación.
        2. El usuario confirma la cancelación.
        3. El flujo básico de eventos se detiene.

## 6. Postcondiciones
    6.1 El pedido se registra en el sistema.
    6.2 El sistema actualiza la información del carrito de compras del usuario.
    6.3 El sistema envía un correo electrónico de confirmación al usuario con los detalles del pedido.

## 7. Excepciones
    E1. Si la información de envío o pago es inválida:
        1. El sistema muestra un mensaje de error y solicita al usuario que ingrese información válida.
        2. El flujo básico de eventos se detiene.
    E2. Si el sistema no puede procesar el pedido por problemas técnicos:
        1. El sistema muestra un mensaje de error y sugiere al usuario intentar más tarde.
        2. El flujo básico de eventos se detiene.