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

document.getElementById("searchBlobBtn").addEventListener("click", function () {
    var imageNameInput = document.getElementById("ImageName");
    var imageName = imageNameInput.value.trim();

    sendGetRequest(imageName);
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

function sendGetRequest(imageName) {
    fetch("/blob/" + encodeURIComponent(imageName), {
        method: "GET"
    })
        .then(response => response.text())
        .then(data => {
            if (data) {
                var imageDiv = document.createElement("div");
                imageDiv.innerHTML = "<img src='" + data + "' style='max-width: 100%; max-height: 300px;' />";
                document.getElementById("imageContainer").innerHTML = "";
                document.getElementById("imageContainer").appendChild(imageDiv);
            } else {
                console.error("Image not found!");
            }
        })
        .catch(error => {
            console.error("Error:", error);
        });
}