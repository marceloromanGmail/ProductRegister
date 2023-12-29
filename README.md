# Registro de Productos - Proyecto .NET 8

## Introducci�n

Este repositorio contiene el c�digo fuente de un proyecto de Registro de Productos desarrollado en .NET 8. El dise�o sigue una arquitectura de cuatro capas: Presentation, Core, Infraestructura y Test. Se ha implementado un enfoque de Desarrollo Guiado por Pruebas (TDD) para garantizar la calidad del c�digo. Adem�s, se ha adoptado el patr�n Mediator junto con CQRS (Command Query Responsibility Segregation) para mejorar la separaci�n de responsabilidades. Esta elecci�n nos proporciona ventajas significativas, como la capacidad de separar funciones espec�ficas y la eliminaci�n de la necesidad de interfaces o inyecciones de dependencias directas entre la capa WebApi y la capa Application.

## Arquitectura y Patrones

El proyecto sigue una arquitectura escalonada en cuatro capas, cada una con un prop�sito espec�fico:

1. **Capa de Presentaci�n (`Presentation`):** Contiene los controladores y la l�gica relacionada con la interfaz de usuario.

2. **Capa Core (`Core`):** Alberga las entidades y la l�gica del dominio central. Aqu� se definen los objetos de negocio y las operaciones fundamentales.

3. **Capa de Infraestructura (`Infraestructure`):** Encargada de las operaciones relacionadas con la persistencia de datos, acceso a servicios externos y otros aspectos de bajo nivel.

4. **Capa de Pruebas (`Test`):** Contiene pruebas unitarias que aseguran el correcto funcionamiento de las funcionalidades.

## Patr�n Mediator con CQRS

El patr�n Mediator se ha implementado en conjunto con CQRS para mejorar la separaci�n de responsabilidades. CQRS divide las operaciones de lectura y escritura, permitiendo que las clases se centren en una tarea espec�fica. El Mediator facilita la comunicaci�n entre estas clases, eliminando acoplamientos directos.

## Capa de Presentaci�n (Presentation)

En la capa de presentaci�n (`Presentation`), la WebApi cuenta principalmente con controladores y 2 Middlewares esenciales:

- **Controladores:** Manejan las solicitudes HTTP y orquestan las interacciones entre la capa de presentaci�n y la capa de aplicaci�n.

- **Middlewares:**
  - **Error Handling Middleware:** Captura errores en un solo lugar, permitiendo un manejo centralizado de excepciones. Los errores o logs de warning son configurables y se registran en un archivo de texto, cuya ruta se puede configurar en `appsettings.json` bajo la clave `Serilog:WriteTo:Args:path`.
  
  - **Response Time Middleware:** Controla el tiempo de respuesta de todas las peticiones realizadas. Los tiempos de respuesta se registran en un archivo de texto configurable desde `appsettings.json` bajo la clave `AppSettings:RequestLogPathFile`.

Ambos middlewares se han inyectado desde el programa utilizando `Microsoft.AspNetCore.Builder.UseMiddlewareExtensions`.

## Capa Core (Core)

### Proyecto Application

En la capa `Core`, espec�ficamente en el proyecto `Application`, se encuentran las siguientes caracter�sticas:

- **Comportamientos:** Dos clases, `RequestPerformanceBehaviour` y `RequestValidationBehavior`, que interceptan las solicitudes para realizar las siguientes tareas:
  - Registrar un log en caso de que el tiempo de espera de las llamadas demore m�s de 5000 milisegundos (transformados a segundos).
  - Validar la solicitud antes de ser ejecutada por los comandos.

- **Validadores con FluentValidation:** Se utilizan validadores basados en `FluentValidation.AbstractValidator` para validar tanto la creaci�n como la actualizaci�n de productos. Este enfoque proporciona una forma declarativa y centrada en el dominio de especificar reglas de validaci�n.

- **Automapper:** Se utiliza AutoMapper para facilitar el mapeo de datos entre DTOs (Data Transfer Objects) y Entidades del dominio.

- **Interfaces para Cache y Escritura de Archivos:** Se encuentran solo las interfaces que definen las operaciones relacionadas con el cache de datos y la escritura de archivos. Esto permite cambiar la implementaci�n en la capa de Infraestructura sin afectar las firmas.

- **Interfaces para Consultar a Servicios Externos:** Tambi�n se encuentran solo las interfaces para consultar servicios externos. La implementaci�n concreta de estas interfaces se encuentra en la capa de Infraestructura.

### Proyecto Domain

En la capa `Core`, proyecto `Domain`, se encuentran las entidades del negocio. Estas entidades representan objetos fundamentales para el registro de productos y contienen la l�gica central del dominio.

## Capa de Infraestructura (Infraestructure)

### Proyecto DataAccess

En la capa `Infraestructura`, proyecto `DataAccess`, se gestiona el acceso a los datos de la base de datos. Aqu� se encuentran las migraciones autom�ticas que se ejecutan con el comando `update-database` para crear o actualizar la base de datos definida en la cadena de conexi�n del `appsettings.json` bajo la clave `ConnectionStrings:MainConnection`. Se utiliza Fluent API para definir configuraciones y definiciones de las tablas a crear en la base de datos.

### Proyecto Infraestructure

En la capa `Infraestructura`, proyecto `Infraestructure`, se encuentran las siguientes implementaciones:

- **Cache con Microsoft.Extensions.Caching.Memory:** Se implementa el cache utilizando `Microsoft.Extensions.Caching.Memory` y se inyecta como Singleton. Esto significa que se mantiene una �nica instancia durante toda la vida de la aplicaci�n. La raz�n de utilizar Singleton es mejorar el rendimiento y compartir el mismo objeto de cache entre todas las partes de la aplicaci�n.

- **Escritura de Tiempos de Respuesta y Logs:** Se implementa la escritura de tiempos de respuesta y logs en esta capa. La ruta de los archivos de log y la configuraci�n relacionada se encuentra en `appsettings.json`.

- **Obtenci�n de Descuentos de Productos:** Se implementa la obtenci�n de descuentos de productos. Se ha definido una ruta relativa `api/discount/{id}` para consultar externamente la URL definida en `appsettings.json` bajo la clave `ServicesClients:DiscountProduct:Url`. Se utiliza la plataforma `mockapi.io` como fuente de datos externa. Se proporciona tambi�n una implementaci�n Fake (`DiscountsProductFakeServiceClient.cs`) para obtener datos aleatorios. Esta implementaci�n se inyecta solo si la bandera en `ServicesClients:PricesProduct:Fake` del `appsettings.json` est� configurada como `true`.

## Capa de Pruebas (Test)

En la capa `Test`, se han creado pruebas unitarias focalizadas en las operaciones `Create`, `Update` y `GetById` de productos. Estas pruebas hacen uso de Mocks para simular integraciones o respuestas de servicios o implementaciones externas. Para facilitar la preparaci�n de datos, se ha utilizado `Microsoft.EntityFrameworkCore.InMemory`. Esto proporciona un entorno de base de datos en memoria para las pruebas unitarias, evitando la necesidad de una base de datos f�sica durante la ejecuci�n de las pruebas.

## Levantar el Proyecto Localmente

### Configuraci�n de la Cadena de Conexi�n

Aseg�rate de tener instalado el framework con la versi�n .net **8.0.1** y de proporcionar las siguientes configuraciones en el archivo `appsettings.json`:

```json
"ConnectionStrings": {
  "MainConnection": "AGREGAR CADENA DE CONEXION SQL SERVER"
}
...
"AppSettings": {
  "Culture": "en-US",
  "RequestLogPathFile": "AGREGAR PATH"
}
...
"Serilog": {
  ...
  "WriteTo": [
    ...
    {
      "Name": "File",
      "Args": {
        "path": "AGREGAR PATH",
        ...
      }
    }
  ],
  ...
}
...
"ServicesClients": {
  "DiscountProduct": {
    "Url": "URL MOCKAPI.IO", //Ejemplo: https://658a27a6ba789a962236c2ae.mockapi.io
    "Fake": true
  }
}