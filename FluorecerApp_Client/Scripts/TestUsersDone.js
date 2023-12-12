
// TestUsersDone.js


function downloadFile(url) {
    // Crear un enlace invisible para la descarga
    var link = document.createElement('a');
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

/*

// JavaScript para emular la solicitud DELETE
document.querySelectorAll('.btn-danger').forEach(function (button) {
    button.addEventListener('click', function (event) {
        event.preventDefault();
        var resultId = this.getAttribute('data-result-id');
        var form = document.getElementById('deleteForm_' + resultId);

        if (confirm('¿Estás seguro de que deseas eliminar esta evaluación?')) {
            form.submit();
        }
    });
});
*/