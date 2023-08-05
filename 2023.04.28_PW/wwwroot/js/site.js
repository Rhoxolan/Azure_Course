document.getElementById("pushBlobBtn").addEventListener("click", function () {
    var imageInput = document.getElementById("Image");
    if (imageInput.files.length === 0) {
        sendPostRequest(null);
    } else {
        var formData = new FormData();
        formData.append("Blob", imageInput.files[0]);
        sendPostRequest(formData);
    }
});

function sendPostRequest(formData) {
    fetch("/blob", {
        method: "POST",
        body: formData
    })
        .then(response => {
            if (response.ok) {
                console.log("Image uploaded successfully!");
            }
            else {
                console.error("Error uploading image!");
            }
        })
        .catch(error => {
            console.error("Error:", error);
        });
}