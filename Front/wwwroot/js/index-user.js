document.addEventListener("DOMContentLoaded", function () {
    const tableBody = document.getElementById('usersTableBody');
    tableBody.addEventListener("click", function (event) {
        let target = event.target; // Obtener el elemento que fue clickeado

        // Subir por los nodos del DOM hasta encontrar el botón de eliminar
        while (target != null && !target.classList.contains('delete-button')) {
            target = target.parentNode;
        }

        if (target != null && target.classList.contains('delete-button')) { // Verificar si es un botón de eliminar
            const userId = target.getAttribute('data-id');
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
                    // Redirige al método de eliminación con el ID
                    window.location.href = `/Usuarios/Eliminar/${userId}`;
                }
            });
        }
    });
});

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveUserCreation", function (message) {
    updateUsersTable();
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

function updateUsersTable() {
    fetch('/Usuarios/getUsuariosJson')
        .then(response => response.json())
        .then(data => {
            const tableBody = document.getElementById('usersTableBody');
            tableBody.innerHTML = '';  // Limpiar la tabla existente
            data.forEach(user => {
                const row = `<tr>
                                <td>${user.id}</td>
                                <td>${user.nombre}</td>
                                <td>${user.email}</td>
                                <td>${user.password}</td>
                                <td>
                                    <a href="/Usuarios/Editar/${user.id}" class="btn btn-warning btn-sm">
                                        <i class="fa-solid fa-pencil"></i>
                                    </a>
                                    <a href="/Usuarios/Detalles/${user.id}" class="btn btn-info btn-sm">
                                        <i class="fa-solid fa-book-open"></i>
                                    </a>
                                    <button class="btn btn-danger btn-sm delete-button" data-id="${user.id}">
                                        <i class="fa-solid fa-trash"></i>
                                    </button>
                                </td>
                             </tr>`;
                tableBody.innerHTML += row;  // Agregar fila a la tabla
            });
        })
        .catch(error => {
            console.error('Error fetching users:', error);
            toastr.error("Error al cargar la lista de usuarios.");
        });
}

connection.onclose(start);

start();