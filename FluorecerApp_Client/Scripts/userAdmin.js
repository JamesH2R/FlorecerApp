function inactivateUser(userId) {
    if (confirm("¿Está seguro de que desea inactivar este usuario?")) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", '/UserAdmin/InactivateUser', true);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var data = xhr.responseText;
                if (data === "Usuario inactivado con éxito") {
                    alert(data);
                    location.reload();
                } else {
                    alert("Error al inactivar el usuario");
                }
            } else if (xhr.readyState === 4 && xhr.status !== 200) {
                alert("Ocurrió un error al realizar la solicitud");
            }
        };
        var data = JSON.stringify({ userId: userId });
        xhr.send(data);
    }
}

function searchUsers() {
    var searchTerm = $('#searchTerm').val();
    window.location.href = '/UserAdmin/SearchUsers?searchTerm=' + searchTerm;
}


function goBack() {
    window.history.back();
}






