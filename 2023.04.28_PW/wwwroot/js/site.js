document.getElementById("pushBlobBtn").addEventListener("click", async function () {
    var imageInput = document.getElementById("Image");
    if (imageInput.files.length === 0) {
        await sendPostRequest(null);
    } else {
        var formData = new FormData();
        formData.append("Blob", imageInput.files[0]);
        await sendPostRequest(formData);
    }
});

document.getElementById("searchBlobBtn").addEventListener("click", async function () {
    await sendGetRequest(document.getElementById("ImageName").value.trim());
});

async function sendPostRequest(formData) {
    try {
        let resp = await fetch("/blob", {
            method: "POST",
            body: formData
        });
        if (resp.ok === true) {
            console.log("Image uploaded successfully!");
        }
        else {
            console.error("Error uploading image!");
        }
    }
    catch (error) {
        console.error("Error:", error);
    }
}

async function sendGetRequest(imageName) {
    try {
        let resp = await fetch("/blob/" + encodeURIComponent(imageName), {
            method: "GET"
        });
        if (resp.ok === true) {
            let blobUrl = await resp.text();
            var imageDiv = document.createElement("div");
            imageDiv.innerHTML = `<img src='${blobUrl}' style='max-width: 100%; max-height: 300px;' />`;
            document.getElementById("imageContainer").innerHTML = "";
            document.getElementById("imageContainer").appendChild(imageDiv);
        }
        else {
            console.error("Image not found!");
        }
    }
    catch (error) {
        console.error("Error:", error);
    }
}