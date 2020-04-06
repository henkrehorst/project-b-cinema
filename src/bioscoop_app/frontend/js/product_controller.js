function retrieve() {
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
    throw new Error("Function 'edit' was not implemented.");
}

function display(response) {
    let products = JSON.parse(JSON.parse(response).Data);
    let td = (id, data) => "<td data-id=" + id + '>' + data + "</td>";

    for (let key in products) {
        let product = products[key]
        let name = td("name", product.name);
        let price = td("price", product.price);
        let edit = "<td><button name='edit' id='" + key + "'>Edit</button></td>";
        let tr = "<tr>" + name + price + edit + "</tr>";

        document.querySelector("#overview > tbody").innerHTML += tr;
        document.querySelector("#overview > tbody > tr > td > button[name='edit'][id='" + key + "']").addEventListener('click', editClick.bind(key)); //alternatively use lambda to bind
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

document.querySelector("button[id='add-product']").addEventListener('click', () => {
    toggle_hide("button[id='add-product']");
    toggle_hide("div.form[name='add']");
});

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
        onSucces: reslog,
        onFailure: log
    });
});

window.onload = retrieve();