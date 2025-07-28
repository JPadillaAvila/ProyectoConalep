
function DownloadExcel(nombreArchivo, base64Archivo) {

    const link = document.createElement("a");
    link.download = nombreArchivo;

    /*link.href = "data:application/octet-stream;base64," + base64;*/
    link.href = "data:application/vnd.ms-excel;base64," + base64Archivo;
    /* link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64Archivo;*/

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
function DownloadTxt(nombre, contenidoBase64) {
    var link = document.createElement('a');
    link.download = nombre;
    link.href = 'data:text/plain;base64,' + contenidoBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

// Función para descargar un archivo ZIP desde una cadena base64
function DownloadZip(nombreArchivo, base64Zip) {
    // Decodificar la cadena base64 a un array de bytes
    const byteCharacters = atob(base64Zip);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);

    // Crear un blob a partir del array de bytes
    const blob = new Blob([byteArray], { type: 'application/zip' });

    // Crear una URL para el blob
    const url = URL.createObjectURL(blob);

    // Crear un enlace y desencadenar la descarga
    const a = document.createElement('a');
    a.href = url;
    a.download = nombreArchivo;
    document.body.appendChild(a);
    a.click();

    // Limpiar
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
}


window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName;
    anchorElement.click();
    URL.revokeObjectURL(url);
};


