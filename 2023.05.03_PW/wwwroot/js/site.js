document.getElementById("addLotBtn").addEventListener("click", async function () {
    var formData = new FormData();
    formData.append("CurrencyType", document.getElementById("CurrencyType").value);
    formData.append("SellerLastName", document.getElementById("Seller").value);
    formData.append("Amount", document.getElementById("Amount").value);
    await sendPostRequest(formData);
});

async function sendPostRequest(formData) {
    try {
        let resp = await fetch("/Home/AddLot", {
            method: "POST",
            body: formData
        });

        if (resp.ok === true) {
            let messageId = await resp.text();
            console.log("Lot added successfully! Message ID:", messageId);
        } else {
            console.error("Error adding lot!");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}
