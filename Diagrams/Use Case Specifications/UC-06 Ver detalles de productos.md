| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-06 Ver detalles de productos | Date: 2025-03-25 |

# Sistema de Venta de Ropa
# Use Case: UC-06 Ver detalles de productos

## 1. Descripción
    El sistema permite a los usuarios ver los detalles de un producto específico.

## 2. Actores
    2.1 Usuario: Persona que desea ver los detalles de un producto.

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe haber iniciado sesión en el sistema.

## 4. Flujo básico de eventos
    1. El sistema muestra una lista de productos disponibles.
    2. El usuario selecciona un producto de la lista.
    3. El sistema muestra los detalles del producto seleccionado (nombre, descripción, precio, imágenes, disponibilidad y talla).
    4. El usuario puede agregar el producto al carrito (continua en el UC-07) de compras o volver a la lista de productos.
 
## 5. Flujo alternativo

## 6. Postcondiciones
    6.1 El usuario puede ver los detalles del producto seleccionado.
    6.2 El sistema actualiza la información del producto en la base de datos si es necesario.

## 7. Excepciones
    E1. Si el usuario decide cancelar la operación:
        1. El sistema regresa a la lista de productos disponibles.
        2. El flujo básico de eventos se detiene.