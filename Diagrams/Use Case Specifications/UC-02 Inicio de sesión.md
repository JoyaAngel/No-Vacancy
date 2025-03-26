| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-02 Inicio de sesión | Date: 2025-03-25 |

# Sistema de Venta de Ropa
# Use Case: UC-02 Inicio de sesión

## 1. Descripción
    El sistema permite a los usuarios iniciar sesión en su cuenta existente.

## 2. Actores
    2.1 Usuario: Persona que desea iniciar sesión en el sistema.
    2.2 Administrador: Persona que gestiona el sistema

## 3. Precondiciones
    3.1 El usuario debe tener acceso a Internet.
    3.2 El usuario debe tener una cuenta registrada en el sistema.

## 4. Flujo básico de eventos
    1. El usuario accede a la página de inicio de sesión.
    2. El sistema muestra el formulario de inicio de sesión.
    3. El usuario ingresa su correo electrónico y contraseña.
    4. El usuario envía el formulario.
    5. El sistema valida la información ingresada.
    6. Si la información es válida, el sistema inicia sesión y redirige al usuario a su perfil.
    7. El sistema muestra un mensaje de éxito al usuario.

## 5. Flujos alternativos
    5.1 Información inválida:
        1. Si el usuario ingresa información inválida (por ejemplo, un correo electrónico no registrado o una contraseña incorrecta), el sistema muestra un mensaje de error y solicita al usuario que corrija la información.
        2. El usuario corrige la información y vuelve al paso 3 del flujo básico.

    5.2 Contraseña olvidada:
        1. Si el usuario no recuerda su contraseña, puede hacer clic en "Olvidé mi contraseña".
        2. El sistema continúa en el UC-03 Recuperar contraseña.

## 6. Postcondiciones
    6.1 El sistema inicia sesión en la cuenta del usuario y lo redirige a su perfil.

## 7. Excepciones