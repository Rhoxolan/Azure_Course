document.getElementById("addLotBtn").addEventListener("click", async function () {
    var currencyTypeSelect = document.getElementById("CurrencyType");
    var sellerInput = document.getElementById("Seller");
    var amountInput = document.getElementById("Amount");

    var currencyType = currencyTypeSelect.value;
    var seller = sellerInput.value;
    var amount = parseInt(amountInput.value);

    var lotData = {
        CurrencyType: currencyType,
        SellerLastName: seller,
        Amount: amount
    };

    await sendPostRequest(lotData);
});

async function sendPostRequest(lotData) {
    try {
        let resp = await fetch("/Home/AddLot", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(lotData)
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
