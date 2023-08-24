document.getElementById("addLotBtn").addEventListener("click", async function () {
    var formData = new FormData();
    formData.append("CurrencyType", document.getElementById("CurrencyType").value);
    formData.append("SellerLastName", document.getElementById("Seller").value);
    formData.append("Amount", document.getElementById("Amount").value);
    await sendPostRequest(formData);
});

document.getElementById("CurrencyTypeShow").addEventListener("change", async function () {
    await updateLotsTable(this.value);
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
            debugger;
            const showLotsDiv = document.getElementById("showLotsDiv");
            showLotsDiv.innerHTML = "";
            showLotsDiv.appendChild(generateTableElement(lots));
        } else {
            console.error("Error fetching lots!");
        }
    } catch (error) {
        console.error("Error:", error);
    }
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
    thead.appendChild(headerRow);
    table.appendChild(thead);
    const tbody = document.createElement("tbody");
    for (let lot of lots) {
        const row = document.createElement("tr");
        const tdCurrencyType = document.createElement("td");
        tdCurrencyType.textContent = lot.currencyType;
        row.appendChild(tdCurrencyType);
        const tdAmount = document.createElement("td");
        tdAmount.textContent = lot.amount;
        row.appendChild(tdAmount);
        const tdSeller = document.createElement("td");
        tdSeller.textContent = lot.sellerLastName;
        row.appendChild(tdSeller);
        tbody.appendChild(row);
    }
    table.appendChild(tbody);
    return table;
}

//setInterval(async function () {
//    const selectedCurrency = document.getElementById("CurrencyTypeShow").value;
//    if (selectedCurrency) {
//        await updateLotsTable(selectedCurrency);
//    }
//}, 10000);