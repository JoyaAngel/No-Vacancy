| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-01 Registro de Usuario | Date: 2025-03-25 |

# Sistema de Venta de Ropa
# Use Case: UC-01 Registro de Usuario

## 1. Descripción
    El sistema permite a los usuarios registrarse para crear una cuenta.

## 2. Actores
    2.1 Usuario: Persona que desea registrarse en el sistema.

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe tener una dirección de correo electrónico válida.

## 4. Flujo básico de eventos
    1. El usuario accede a la página de registro.
    2. El sistema muestra el formulario de registro.
    3. El usuario completa el formulario con su información personal (nombre, correo electrónico, y una contraseña que solo incluya letras, números y guiones).
    4. El usuario envía el formulario.
    5. El sistema valida la información ingresada.
    6. Si la información es válida, el sistema crea una nueva cuenta de usuario y envía un correo de confirmación al usuario.
    7. El sistema muestra un mensaje de éxito al usuario.

## 5. Flujos alternativos
    5.1 Información inválida:
        1. Si el usuario ingresa información inválida (por ejemplo, un correo electrónico no válido o una contraseña que no cumple con los requisitos), el sistema muestra un mensaje de error y solicita al usuario que corrija la información.
        2. El usuario corrige la información y vuelve al paso 4 del flujo básico.

    5.2 Correo electrónico ya registrado:
        1. Si el correo electrónico ingresado ya está registrado, el sistema muestra un mensaje de error indicando que el correo ya está en uso.
        2. El usuario puede optar por recuperar su contraseña o ingresar un nuevo correo electrónico.
        3. El usuario corrige la información y vuelve al paso 4 del flujo básico.

## 6. Postcondiciones
    6.1 El sistema crea una nueva cuenta de usuario con la información proporcionada.

## 7. Excepciones
