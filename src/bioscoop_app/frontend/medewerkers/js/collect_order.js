/**
 * Eventlistener to collect the order by the input code.
 */
document.getElementById("getorder").addEventListener("click", async () => {
    let code = document.getElementById("code-field").value;
    let res = await chromelyRequest("/order#collect", "POST", { "code": code });
    if (res.getStatusCode() === 200) {
        console.log("getorder 200");
        orderId = res.getData().Id;
        populateTables(res.getData());
    } else if (res.getStatusCode() === 204) {
        //Display error message
        console.log(res.getStatusText());
    } else {
        console.log(res.getStatusCode(), res.getStatusText());
    }
});

/**
 * Fills the tables on the order collect page with the contents of the order.
 * @param {any} orderData
 */
function populateTables(orderData) {
    console.log(orderData);

    //populate tickets table
    let subtotaal1 = 0.0;
    for (let i = 0; i < orderData.tickets.length; i++) {
        let ticket = orderData.tickets[i];
        subtotaal1 += ticket.price;
        let trow = document.createElement("tr");
        let contentList = [ticket.name, ticket.row, ticket.seatnr, ticket.visitorAge, ticket.price];
        for (let i = 0; i < contentList.length; i++) {
            trow.appendChild(colNode(contentList[i]));
        }
        document.querySelector("#ticket_container table tbody").lastElementChild.before(trow);
    }
    document.querySelector("#ticket_container td[name='subtotaal']").innerHTML = "€" + subtotaal1;

    //populate item table
    let subtotaal2 = 0.0;
    for (let i = 0; i < orderData.items.length; i++) {
        let item = orderData.items[i];
        subtotaal2 += item.price;
        let trow = document.createElement("tr");
        trow.appendChild(colNode(item.name));
        trow.appendChild(colNode(item.price));
        document.querySelector("#item_container table tbody").lastElementChild.before(trow);
    }
    document.querySelector("#item_container td[name='subtotaal']").innerHTML = "€" + subtotaal2;
    document.querySelector("#order_details h2").append('€' + (subtotaal1 + subtotaal2));

    document.getElementById("order_details").hidden = false;
}

function colNode(content) {
    let cell = document.createElement("td");
    cell.innerHTML = content;
    return cell;
}