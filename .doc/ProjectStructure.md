### Modelado de Base de Datos

Este es el diagrama de pata de cuervo hecho para la aplicacion, este modelado considera la gestion de lo siguiente:
- Diferentes deportes y torneos para la actividad Copa Upsa
- Noticias, Eventos y Quizes para el feed de los estudiantes
- Ingresos y registro de asistencia de los QR
- Gestion de Personas
  - Esto incluye lo que es estudiantes, adultos responsables de los estudiantes y al personal administrativo que utilizara la apliacion o panel admin
- Atributos de Integracion, permitiendo crear una integracion con el sistema de eventos ya creado de la UPSA
  
 <img width="5600" height="4127" alt="Proyectos Oscar - Nibu-C# (1)" src="https://github.com/user-attachments/assets/615928da-7066-4cf1-8956-a0852f673d73" />

Queda pendiente algunas cosas como podria ser definir el modulo de seguirad y que sea con permisos para que ciertos usuarios solo puedan acceder a ciertas cosas,
se deberia de evaluar que tanta granulidad de permisos se necesita ya que quizas con un acceso de roles a menus sea suficiente permitiendo realizar
al usuario todo lo que tiene en el menu al cual tiene acceso.

> Nota Importante: Absolutamente todas las entidades heredan la clase de BaseEntity la cual tiene los atributos de auditoria de Creado, Actualizado y Eliminado por/en y el atributo de si esta o no
activo para manejar borrados logicos y no duros.

