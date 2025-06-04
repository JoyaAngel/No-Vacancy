| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-05 Filtrar Productos | Date: 2025-03-25 |

# Sistema de Venta de Ropa
# Use Case: UC-05 Filtrar Productos

## 1. Descripción
    El sistema permite a los usuarios filtrar productos según diferentes criterios (categoría, precio, marca, color y talla).

## 2. Actores
    2.1 Usuario: Persona que desea buscar productos en el sistema.

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe estar registrado e iniciar sesión en el sistema.

## 4. Flujo básico de eventos
    1. El usuario accede a la página de productos.
    2. El sistema muestra una lista de productos disponibles.
    3. El usuario selecciona los criterios de filtrado (categoría, precio, marca, color y talla).
    4. El usuario aplica los filtros seleccionados.
    5. El sistema actualiza la lista de productos según los criterios seleccionados.

## 5. Flujos alternativos
    5.1 Si no se selecciona ningún criterio de filtrado:
        1. El sistema muestra todos los productos disponibles.
        2. El usuario puede optar por aplicar filtros posteriormente.
        3. El flujo regresa al paso 4 del flujo básico.

## 6. Postcondiciones
    6.1 El sistema muestra una lista de productos filtrados según los criterios seleccionados por el usuario.

## 7. Excepciones
    E1. Si no hay productos que coincidan con los criterios de filtrado:
        1. El sistema muestra un mensaje indicando que no se encontraron productos que coincidan con los criterios seleccionados.
        2. El usuario puede optar por modificar los criterios de filtrado o ver todos los productos disponibles.