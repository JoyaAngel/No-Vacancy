| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-03: Recuperar contraseña | Date: 2025-03-25 |

# Sistema de Venta de Ropa
# Use Case: UC-03: Recuperar contraseña

## 1. Descripción
    El sistema permite a los usuarios recuperar su contraseña en caso de haberla olvidado.

## 2. Actores
    2.1 Usuario: Persona que desea recuperar su contraseña.

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe tener una cuenta registrada en el sistema.

## 4. Flujo básico de eventos
    1. El usuario accede a la página de inicio de sesión.
    2. El sistema muestra el formulario de inicio de sesión.
    3. El usuario hace clic en "Olvidé mi contraseña".
    4. El sistema redirige al usuario a la página de recuperación de contraseña.
    5. El usuario ingresa su correo electrónico registrado.
    6. El usuario envía el formulario.
    7. El sistema valida la información ingresada.
    8. Si la información es válida, el sistema envía un correo electrónico con un enlace para restablecer la contraseña.
    9. El sistema muestra un mensaje de éxito al usuario.

## 5. Flujos alternativos
    5.1 Correo electrónico no válido:
        1. Si el usuario ingresa un correo electrónico no válido, el sistema muestra un mensaje de error y solicita al usuario que corrija la información.
        2. El usuario corrige la información y vuelve al paso 5 del flujo básico.
    5.2 Información inválida:
        1. Si el usuario ingresa un correo electrónico no registrado, el sistema muestra un mensaje de error y solicita al usuario que corrija la información.
        2. El usuario corrige la información y vuelve al paso 5 del flujo básico.

## 6. Postcondiciones
    6.1 El sistema envía un correo electrónico al usuario con un enlace para restablecer la contraseña.

## 7. Excepciones