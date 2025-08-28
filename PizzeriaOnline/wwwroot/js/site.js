// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    // --- LÓGICA PARA QUITAR ITEMS DEL CARRITO CON AJAX ---

    function attachQuitarEvents() {
        // Botones para quitar Pizzas
        document.querySelectorAll('.btn-quitar-pizza').forEach(button => {
            button.addEventListener('click', function () {
                const itemId = this.dataset.id;
                quitarItem('/Home/QuitarPizza', { id: itemId });
            });
        });

        // Botones para quitar Extras
        document.querySelectorAll('.btn-quitar-extra').forEach(button => {
            button.addEventListener('click', function () {
                const itemId = this.dataset.id;
                quitarItem('/Home/QuitarExtra', { productoExtraId: parseInt(itemId) });
            });
        });
    }

    function quitarItem(url, data) {
        // Necesitamos el token de seguridad para peticiones POST
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        if (!tokenInput) {
            // Si estamos en una página sin formulario principal, no podemos continuar
            // En este caso, simplemente recargamos como fallback.
            window.location.reload();
            return;
        }

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': tokenInput.value
            },
            body: JSON.stringify(data)
        }).then(response => {
            if (response.ok) {
                // Si la eliminación fue exitosa, recargamos la página para ver el cambio
                // Esta es la forma más simple de actualizar la vista.
                window.location.reload();
            } else {
                alert("Error al quitar el producto del carrito.");
            }
        });
    }

    // Ejecutamos la función para que los botones de la página actual funcionen
    attachQuitarEvents();
});