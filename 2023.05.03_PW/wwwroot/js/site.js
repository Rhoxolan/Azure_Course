let updateTimer = null;

document.getElementById("addLotBtn").addEventListener("click", async function () {
    var formData = new FormData();
    formData.append("CurrencyType", document.getElementById("CurrencyType").value);
    formData.append("SellerLastName", document.getElementById("Seller").value);
    formData.append("Amount", document.getElementById("Amount").value);
    await sendPostRequest(formData);
});

document.getElementById("CurrencyTypeShow").addEventListener("change", async function () {
    switch (this.value) {
        case "None":
            cleanLotsTable();
            clearInterval(updateTimer);
            break;
        default:
            await updateLotsTable(this.value);
            clearInterval(updateTimer);
            updateTimer = setInterval(async () => {
                await updateLotsTable(this.value);
            }, 10000);
            break;
    }
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

async function updateLotsTable(currencyType) {
    try {
        let resp = await fetch(`/Home/GetLots?currencyType=${currencyType}`);
        if (resp.ok === true) {
            let lots = await resp.json();
            const showLotsDiv = document.getElementById("showLotsDiv");
            showLotsDiv.innerHTML = "";
            showLotsDiv.appendChild(generateTableElement(lots.map(l => {
                let parsedMessage = JSON.parse(l.messageText);
                parsedMessage.MessageId = l.messageId;
                return parsedMessage;
            })));
        } else {
            console.error("Error fetching lots!");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}

function cleanLotsTable() {
    document.getElementById("showLotsDiv").innerHTML = "";
}

function generateTableElement(lots) {
    const table = document.createElement("table");
    table.classList.add("table", "table-striped");
    const thead = document.createElement("thead");
    const headerRow = document.createElement("tr");
    const thCurrencyType = document.createElement("th");
    thCurrencyType.textContent = "Currency Type";
    headerRow.appendChild(thCurrencyType);
    const thAmount = document.createElement("th");
    thAmount.textContent = "Amount";
    headerRow.appendChild(thAmount);
    const thSeller = document.createElement("th");
    thSeller.textContent = "Seller";
    headerRow.appendChild(thSeller);
    const thActions = document.createElement("th"); // New column for actions
    thActions.textContent = "Actions";
    headerRow.appendChild(thActions);
    thead.appendChild(headerRow);
    table.appendChild(thead);
    const tbody = document.createElement("tbody");
    for (let lot of lots) {
        const row = document.createElement("tr");
        const tdCurrencyType = document.createElement("td");
        tdCurrencyType.textContent = lot.CurrencyType;
        row.appendChild(tdCurrencyType);
        const tdAmount = document.createElement("td");
        tdAmount.textContent = lot.Amount;
        row.appendChild(tdAmount);
        const tdSeller = document.createElement("td");
        tdSeller.textContent = lot.SellerLastName;
        row.appendChild(tdSeller);

        // Create the Buy Lot button
        const tdActions = document.createElement("td");
        const buyButton = document.createElement("button");
        buyButton.textContent = "Buy Lot";
        buyButton.classList.add("btn", "btn-outline-warning", "buy-lot-button");
        buyButton.dataset.messageId = lot.MessageId; // Attach MessageId to the button
        buyButton.addEventListener("click", buyLotHandler);
        tdActions.appendChild(buyButton);
        row.appendChild(tdActions);

        tbody.appendChild(row);
    }
    table.appendChild(tbody);
    return table;
}

async function buyLotHandler(event) {
    const messageId = event.target.dataset.messageId;
    try {
        let resp = await fetch(`/Home/BuyLot?messageId=${messageId}`, {
            method: "DELETE"
        });
        if (resp.ok === true) {
            console.log("Lot bought successfully!");
            // You might want to update the table here or handle the UI accordingly
        } else {
            console.error("Error buying lot!");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}