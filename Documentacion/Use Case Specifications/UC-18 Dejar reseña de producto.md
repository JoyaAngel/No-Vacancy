| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-18 Dejar reseña de producto | Date: 2025-03-26 |

# Sistema de Venta de Ropa
# Use Case: UC-18 Dejar reseña de producto

## 1. Descripción
    El sistema permite a los usuarios dejar reseñas sobre los productos que han comprado.

## 2. Actores
    2.1 Usuario: Persona que desea dejar una reseña sobre un producto.

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe haber iniciado sesión en el sistema.
    3.3 El usuario debe haber comprado el producto sobre el cual desea dejar una reseña.

## 4. Flujo básico de eventos
    1. El usuario selecciona la opción "Dejar reseña" en la página del producto.
    2. El sistema muestra un formulario para dejar una reseña (calificación, comentario y opción de subir imágenes).
    3. El usuario completa el formulario con su calificación y comentario.
    4. El usuario puede subir imágenes relacionadas con el producto (opcional).
    5. El usuario envía la reseña.
    6. El sistema valida la información ingresada y guarda la reseña en la base de datos.
    7. El sistema muestra un mensaje de confirmación de que la reseña ha sido enviada.

## 5. Flujo alternativo
    5.1 Si el usuario no completa todos los campos obligatorios del formulario:
        1. El sistema muestra un mensaje de error indicando que debe completar todos los campos obligatorios.
        2. El usuario completa los campos obligatorios y vuelve al paso 5 del flujo básico de eventos.

## 6. Postcondiciones
    6.1 La reseña del usuario se guarda en la base de datos y se asocia al producto correspondiente.
    6.2 El sistema muestra la reseña en la página del producto para que otros usuarios la vean.

## 7. Excepciones
