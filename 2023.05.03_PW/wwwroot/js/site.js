document.getElementById("addLotBtn").addEventListener("click", async function () {
    var currencyTypeSelect = document.getElementById("CurrencyType");
    var sellerInput = document.getElementById("Seller");
    var amountInput = document.getElementById("Amount");

    var formData = new FormData();
    formData.append("CurrencyType", currencyTypeSelect.value);
    formData.append("SellerLastName", sellerInput.value);
    formData.append("Amount", amountInput.value);

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
