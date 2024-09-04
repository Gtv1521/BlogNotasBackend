# BlogNotas
Esta api esta hecha pera darle manejo a las rutas de usuarios y notas creadas por usuarios a modo que cada usuario pueda ver y manejar las notas a su acomodo..


La api esta escrita en C# .Net Core donde se le da manejo a las rutas con su restapectiva seguridad (manejo de token)

## Herramientas
Esta aplicacion esta construida con las librerias y herramientas descritas a continuacion:
    - Esta construida en proncipio sobre .Net Core 8
    - Implementa la libreria jwt para en manejo del token y el acceso de los usuarios 
    - Implementa la libreria Bcrip que es la que encripta las contraseñas de los usuarios 
    - Implementa cors para la comunicacion con demas apliaciones de front o alguna otra api 

## Como usar
Esta app se puede usar para tener linea de comunucacion directa con la base de datos, para obtener un token debes de iniciar session o crear un usuario donde te dara acceso a las rutas de consultas de datos a la base de datos...
