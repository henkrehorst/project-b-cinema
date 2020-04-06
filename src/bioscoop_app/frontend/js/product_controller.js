﻿function retrieve() {
    let req = {
        'method': 'GET',
        'url': '/products',
        'parameters': null,
        'postData': null
    };
    console.log("retrieve");
    return window.cefQuery({
        request: JSON.stringify(req),
        onSuccess: display,
        onFailure: log
    })
}

function editClick(id) {
    //throw new Error("Function 'edit' was not implemented.");
    //console.log("#overview > tbody > tr[id='" + id + "']");
    //console.log(document.querySelector("#overview > tbody > tr[id='" + id + "']"));

    document.querySelector("button[name='update'][id='submit']").data_update_id = id;
    let data = {};

    data["name"] = document.querySelector("#overview > tbody > tr[id='" + id + "'] > td[data_id='name'").innerHTML;
    data["price"] = document.querySelector("#overview > tbody > tr[id='" + id + "'] > td[data_id='price'").innerHTML;
    //document.querySelectorAll("#overview > tbody > tr[id='" + id + "'] > td").forEach(() => {
    //    data[this.data_id] = this.innerHTML;
    //});

    //for (let td in document.querySelectorAll("#overview > tbody > tr[id='" + id + "'] > td")) {
    //    console.log(td);
    //    data[td.data_id] = td.innerHTML;
    //}
    console.log(JSON.stringify(data));
    //At this point, the selected product is converted to a dictionary called data
    document.querySelector("#content > div.form[name='update'] > input[id='name']").value = data["name"];
    document.querySelector("#content > div.form[name='update'] > input[id='price']").value = data["price"];
    toggle_hide("#content > div.form[name='update']");
}

function display(response) {
    let products = JSON.parse(JSON.parse(response).Data);
    let td = (id, data) => "<td data_id=" + id + '>' + data + "</td>";
    const edit = "<td><button name='edit'>Edit</button></td>";

    for (let key in products) {
        let product = products[key]
        let name = td("name", product.name);
        let price = td("price", product.price);
        let tr = "<tr id ='" + key + "'>" + name + price + edit + "</tr>";

        document.querySelector("#overview > tbody").innerHTML += tr;
        document.querySelector("#overview > tbody > tr[id='" + key + "'] > td > button[name='edit']").addEventListener('click', () => {
            editClick(key);
        }); //alternatively use lambda to bind
    }
}

function log(err, msg) {
    console.log(err, msg);
}

function reslog(res) {
    console.log(JSON.parse(JSON.parse(res).Data));
}

function toggle_hide(identifier) {
    let el = document.querySelector(identifier);
    if (!el.style.display) {
        el.style.display = "none";
    } else if (el.style.display === "none") {
        el.style.display = "block";
    } else if (el.style.display === "block") {
        el.style.display = "none";
    }
}

function displayTicketPrice() {
    let req = {
        'method': 'GET',
        'url': "/product#ticketprice",
        'parameters': null,
        'postData': null
    }
    console.log("displayTicketPrice");
    window.cefQuery({
        request: JSON.stringify(req),
        onSuccess: (res) => {
            reslog(res);
            console.log("displaying ticket price");
            let price = JSON.parse(JSON.parse(res).Data);
            document.querySelector("input[name='update'][id='ticket']").value = price;
            document.querySelector("p[id='ticket_price']").innerHTML = price;
        },
        onFailure: log
    });
}

document.querySelector("button[id='add-product']").addEventListener('click', () => {
    toggle_hide("button[id='add-product']");
    toggle_hide("div.form[name='add']");
});

document.querySelector("button[name='update'][id='ticket']").addEventListener('click', () => {
    let data = { 'price': document.querySelector("input[name='update'][id='ticket']").value };
    let req = {
        'method': 'POST',
        'url': "/product#ticketprice",
        'parameters': null,
        'postData': data
    }
    console.log("updating ticket price");
    window.cefQuery({
        request: JSON.stringify(req),
        onSuccess: reslog,
        onFailure: log
    });
    displayTicketPrice();
})

document.querySelector("button[name='add'][id='submit']").addEventListener('click', () => {
    toggle_hide("div.form[name='add']");
    toggle_hide("button[id='add-product']");

    let data = {
        'name': document.querySelector("input[name='add'][id='name']").value,
        'price': parseFloat(document.querySelector("input[name='add'][id='price']").value)
    };

    //post data
    let req = {
        'method': 'POST',
        'url': "/products#add",
        'parameters': null,
        'postData': data
    };

    window.cefQuery({
        request: JSON.stringify(req),
        onSuccess: reslog,
        onFailure: log
    });
});

document.querySelector("button[name='update'][id='submit']").addEventListener('click', () => {
    toggle_hide("div.form[name='update']");

    let data = {
        'id': document.querySelector("button[name='update'][id='submit']").data_update_id,
        'name': document.querySelector("input[name='update'][id='name']").value,
        'price': parseFloat(document.querySelector("input[name='update'][id='price']").value)
    };

    //post data
    let req = {
        'method': 'POST',
        'url': "/products#update",
        'parameters': null,
        'postData': data
    };

    window.cefQuery({
        request: JSON.stringify(req),
        onSuccess: reslog,
        onFailure: log
    });
});

window.onload = function () {
    retrieve();
    displayTicketPrice();
}();