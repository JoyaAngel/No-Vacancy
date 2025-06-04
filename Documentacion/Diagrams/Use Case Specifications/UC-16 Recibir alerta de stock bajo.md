| Sistema de Venta de Ropa |  |
| :--------- | --------- |
| Use Case Specification: UC-16 Recibir alerta de stock bajo | Date: 2025-03-26 |

# Sistema de Venta de Ropa
# Use Case: UC-16 Recibir alerta de stock bajo

## 1. Descripción
    El sistema envía alertas al administrador cuando el stock de un producto cae por debajo de un umbral predefinido.

## 2. Actores
    2.1 Administrador: Persona encargada de gestionar el stock de productos en el sistema.

## 3. Precondiciones
    3.1 El administrador debe tener acceso a Internet.
    3.2 El administrador debe haber iniciado sesión en el sistema con privilegios de administrador.

## 4. Flujo básico de eventos
    1. El sistema monitorea el stock de productos en tiempo real.
    2. Cuando el stock de un producto cae por debajo del umbral predefinido, el sistema genera una alerta.
    3. El sistema envía un correo electrónico al administrador con los detalles del producto y la cantidad de stock restante.

## 5. Flujo alternativo

## 6. Postcondiciones
    6.1 El administrador recibe la alerta de stock bajo en su correo electrónico.
    6.2 El sistema registra la alerta en el historial de alertas del administrador.

## 7. Excepciones
    E1. Si el sistema no puede enviar el correo electrónico al administrador:
        1. El sistema registra un error en el log de errores.
        2. El flujo básico de eventos se detiene.