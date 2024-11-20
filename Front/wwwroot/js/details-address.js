document.addEventListener("DOMContentLoaded", function () {
    const tableBody = document.getElementById('addressTableBody');

    tableBody.addEventListener("click", function (event) {
        let target = event.target;

        // Subir por los nodos del DOM hasta encontrar el botón de eliminar
        while (target !== this && !target.classList.contains('delete-button')) {
            target = target.parentNode;
        }

        if (target && target.classList.contains('delete-button')) {
            const direccionId = target.getAttribute('data-id');
            const usuarioId = target.getAttribute('data-usuario-id');

            if (!direccionId || !usuarioId) {
                return;  // Salir si los datos no están disponibles
            }

            Swal.fire({
                title: '¿Estás seguro?',
                text: "¡No podrás revertir esto!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = `/Usuarios/EliminarDireccion/?id=${direccionId}&usuarioId=${usuarioId}`;
                }
            });
        }
    });
});

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveNotification", function (id, message) {
    updateAddressTable(id);
    toastr.info(message);
});

function start() {
    connection.start().then(function () {
        console.log("conexión exitosa");
    }).catch(function (err) {
        console.error(err.toString());
        setTimeout(start, 5000);
    });
}

function updateAddressTable(id) {
    fetch(`/Usuarios/getAddressJson/${id}`)
        .then(response => response.json())
        .then(data => {
            const tableBody = document.getElementById('addressTableBody');
            tableBody.innerHTML = '';  // Limpiar la tabla existente
            data.forEach(address => {
                const row = `<tr>
                                 <td>${address.id}</td>
                                 <td>${address.direccionEspecifica}</td>
                                 <td>${address.ciudad}</td>
                                 <td>${address.codigoPostal}</td>
                                 <td>
                                     <a href="/Usuarios/EditarDireccion/${address.id}" class="btn btn-warning btn-sm">
                                         <i class="fa-solid fa-pencil"></i>
                                     </a>
                                     <button class="btn btn-danger btn-sm delete-button" title="EliminarDireccion" data-id="${address.id}" data-usuario-id="${id}" type="button">
                                         <i class="fa-solid fa-trash"></i>
                                     </button>
                                 </td>
                             </tr>`;
                tableBody.innerHTML += row;  // Agregar fila a la tabla
            });
        })
        .catch(error => {
            console.error('Error fetching address:', error);
            toastr.error("Error al cargar la lista de direcciones.");
        });
}

connection.onclose(start);

start();