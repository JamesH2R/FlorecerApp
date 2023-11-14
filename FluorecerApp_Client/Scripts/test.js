
// Función botón método AssignEvaluation
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


// Función botón método DownloadEvaluation
function downloadEvaluation(event) {
    event.preventDefault();

    // Lógica para descargar la evaluación
    fetch('/Test/DownloadEvaluation', {
        method: 'GET'
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al descargar la evaluación');
            }
            return response.blob();
        })
        .then(blob => {
            // Crear un enlace para descargar el archivo
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = 'Evaluations.zip';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
        })
        .catch(error => console.error('Error:', error));
}


// Función botón método SendResult
function sendResult(event) {
    event.preventDefault();

    var form = document.getElementById('sendTestForm');
    var formData = new FormData(form);

    fetch('/Test/SendResult', {
        method: 'POST',
        body: formData
    })
        .then(response => response.text())
        .then(data => {
            console.log(data);
            // Aquí puedes manejar la respuesta del servidor, por ejemplo, mostrar un mensaje al usuario
            alert("Evaluación enviada con éxito");
        })
        .catch(error => console.error('Error:', error));
}

