# Pizzería La Querencia - Sistema de Pedidos Online

¡Bienvenido al repositorio del sistema de pedidos online para la Pizzería La Querencia! Esta es una aplicación web completa desarrollada con ASP.NET Core MVC que simula un entorno de e-commerce real para una pizzería, incluyendo un panel de administración avanzado.

<img width="1846" height="883" alt="image" src="https://github.com/user-attachments/assets/68b38712-f81e-4a93-8ee1-4891bafd7bde" />

## 🌟 Características Principales

Este proyecto fue construido desde cero, enfocándose en prácticas de desarrollo profesionales y una arquitectura robusta.

### Para Clientes:
* **Menú Interactivo:** Visualiza todas las pizzas y productos disponibles.
* **Constructor de Pizzas:** Una interfaz dinámica para personalizar tu pizza con diferentes tamaños y sabores.
* **Carrito de Compras:** Añade pizzas personalizadas y productos extra (refrescos, etc.) a tu pedido.
* **Checkout Inteligente:**
    * **Geolocalización:** Usa un mapa interactivo de Google Maps para seleccionar tu ubicación de entrega exacta.
    * **Validación de Zona:** El sistema verifica si la ubicación del cliente está dentro del área de reparto.
* **Sistema de Pagos:** Integración con **Stripe** para pagos con tarjeta y opción de pago en **efectivo**.
* **Seguimiento de Pedido en Tiempo Real:** Gracias a **SignalR**, el cliente puede ver el estado de su pedido cambiar en vivo.
* **Chat en Vivo:** Comunicación directa entre el cliente y el administrador en la página del pedido.
* **Asistente con IA:** Un chatbot entrenado con **Azure AI Language** para responder preguntas frecuentes sobre el horario, menú y zonas de reparto.

### Para Administradores:
* **Autenticación Segura:** Sistema de login y gestión de usuarios con **ASP.NET Core Identity** y roles.
* **Dashboard de Estadísticas:** Visualiza métricas clave del negocio como ventas diarias, pedidos del día, pizzas más populares y alertas de inventario bajo.
* **Gestión de Pedidos (CRUD):** Visualiza, detalla y actualiza el estado de los pedidos de los clientes.
* **Gestión de Menú (CRUD):** Control total para crear, editar y eliminar las pizzas del menú, incluyendo la subida de imágenes.
* **Gestión de Inventario (CRUD):** Administra los ingredientes y su stock.
* **Sistema de Recetas:** Asigna ingredientes y cantidades específicas a cada pizza y tamaño para un control preciso.
* **Descuento Automático de Inventario:** El stock se descuenta automáticamente cuando un pedido se marca como "En preparación".

<img width="1843" height="885" alt="image" src="https://github.com/user-attachments/assets/8863cc76-94ba-4b73-b646-21e61e7c6565" />

## 🛠️ Tecnologías Utilizadas

* **Backend:** C# con ASP.NET Core MVC (.NET 8)
* **Base de Datos:** Entity Framework Core con SQLite
* **Frontend:** HTML, CSS, JavaScript, Bootstrap 5
* **Seguridad:** ASP.NET Core Identity
* **Comunicación en Tiempo Real:** SignalR
* **Servicios Externos:**
    * **Google Maps API** (Geolocalización y Mapas)
    * **Stripe API** (Pagos Electrónicos)
    * **Azure AI Language - Custom Question Answering** (Chatbot)
* **Versionamiento:** Git y GitHub

## 🚀 Cómo Empezar

1.  Clona el repositorio.
2.  Configura tus claves de API (Stripe, Google Maps, Azure) en el archivo de Secretos de Usuario (`secrets.json`).
3.  Abre el proyecto en Visual Studio 2022.
4.  Ejecuta los comandos de migración de Entity Framework: `Update-Database`.
5.  ¡Ejecuta el proyecto

##  Capturas

### Constructor de Pizzas
<img width="1837" height="887" alt="image" src="https://github.com/user-attachments/assets/22040ae4-930f-4cfd-a447-5e5fc5a793bc" />

### Página de Checkout
<img width="1845" height="883" alt="image" src="https://github.com/user-attachments/assets/c908aca3-3fff-4d45-9515-ed2b31eccdb0" />
<img width="1837" height="876" alt="image" src="https://github.com/user-attachments/assets/1e962fc5-1c6d-4e44-b4db-3b213dbee72b" />

### Sistema de Registro
<img width="1860" height="883" alt="image" src="https://github.com/user-attachments/assets/84066035-b2d4-4a7d-96bf-e93b041e7322" />

### Sistema de login
<img width="1859" height="885" alt="image" src="https://github.com/user-attachments/assets/87edcb4c-fd27-4ed1-95cc-59775c489fed" />

### Área de Contacto con la Pizzería
<img width="1846" height="883" alt="image" src="https://github.com/user-attachments/assets/e518eb44-dbbf-4601-8420-c70972c179a9" />

### Detalle de los pedidos
<img width="1844" height="880" alt="image" src="https://github.com/user-attachments/assets/0ae7248a-b036-4eb3-8bde-07c2bb531a06" />
-Tiene su propio chat Personal con el cliente
<img width="1843" height="884" alt="image" src="https://github.com/user-attachments/assets/1fbc6798-38ca-41db-9363-02691d5e14e4" />

### Estadísticas de la pizzería, cada día
<img width="1647" height="267" alt="image" src="https://github.com/user-attachments/assets/4caeb0ac-a369-4920-bd48-24eaffa82d78" />

### Gestión de Inventarios
<img width="1857" height="888" alt="image" src="https://github.com/user-attachments/assets/57cb2217-3b09-4709-af8f-ea22f68f8942" />
<img width="1862" height="885" alt="image" src="https://github.com/user-attachments/assets/673e0239-a74b-429f-b735-5a34ee9b4c8a" />
<img width="1859" height="876" alt="image" src="https://github.com/user-attachments/assets/70bcd024-dd19-4f61-83a4-66a796ecd144" />

### Gestionar Productos Extra
<img width="1864" height="882" alt="image" src="https://github.com/user-attachments/assets/aaff6335-a7e7-4d5a-bf77-e22b97cf5713" />
<img width="1863" height="878" alt="image" src="https://github.com/user-attachments/assets/f9283817-b98f-4606-9336-0873d7459ea5" />
<img width="1860" height="884" alt="image" src="https://github.com/user-attachments/assets/6580efe2-860b-4b16-a581-6b6957b784bb" />

### Administración de Pizzas
<img width="1842" height="883" alt="image" src="https://github.com/user-attachments/assets/1a42500d-4736-4750-92ac-b294238c21ce" />
<img width="1858" height="881" alt="image" src="https://github.com/user-attachments/assets/3e4e422c-a0b5-4a72-8c2f-99b5c95e7069" />

### Colocar un Ingrediente a una Pizza
-Primero escoge un tamaño
<img width="1861" height="874" alt="image" src="https://github.com/user-attachments/assets/6d733d71-4b18-4568-9df1-cbdced7d42dd" />
-Escoges los ingredientes y cuanto gasta
<img width="1858" height="879" alt="image" src="https://github.com/user-attachments/assets/469076de-3b98-4d37-8bcf-77d7130e20de" />

### Asistente de IA para preguntas frecuentes
<img width="464" height="681" alt="image" src="https://github.com/user-attachments/assets/faf2682d-9dfe-42ba-ab6b-ff0b9276b2d8" />

## 🧑‍💻 Contacto

Desarrollado por DIEGO FLORES GONZÁLEZ - https://www.linkedin.com/in/diego-flores-gonzález-86a4a52b7
