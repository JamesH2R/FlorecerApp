// Asigna la fecha actual al campo de fecha al cargar la página
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('date').valueAsDate = new Date();

    // Muestra el nombre del archivo seleccionado en el campo FilePath
    document.getElementById('file').addEventListener('change', function () {
        var fullPath = document.getElementById('file').value;
        if (fullPath) {
            var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
            var fileName = fullPath.substring(startIndex);
            if (fileName.indexOf('\\') === 0 || fileName.indexOf('/') === 0) {
                fileName = fileName.substring(1);
            }
            document.getElementById('fileName').value = fileName;
        }
    });
});

// Asegúrate de que el script se ejecute cuando se envíe el formulario
function submitForm(event) {
    event.preventDefault();
    var form = document.getElementById('assignForm');
    var formData = new FormData(form);

    fetch('/Test/AssignEvaluation', {
        method: 'POST',
        body: formData
    })
        .then(response => response.text())
        .then(data => console.log(data))
        .catch(error => console.error('Error:', error));
}
