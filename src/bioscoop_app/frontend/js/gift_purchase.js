class chromelyResponse {
    constructor(status, statusText, data) {
        this.status = status;
        this.statusText = statusText;

        try {
            this.data = JSON.parse(data)
        } catch (e) {
            this.data = "";
        }
    }

    getStatusCode() {
        return this.status;
    }

    getData() {
        return this.data;
    }

    getStatusText() {
        return this.statusText;
    }
}

async function chromelyRequest(route, method = 'GET', postData = {}) {
    return new Promise((resolve, reject) => {
        var request = {
            'method': method,
            'url': route,
            'parameters': null,
            'postData': postData
        };
        window.cefQuery({
            request: JSON.stringify(request),
            onSuccess: function (response) {
                let result = JSON.parse(JSON.parse(response).Data);
                console.log(result.data);
                resolve(new chromelyResponse(result.status, result.statusText, result.data))
            }, onFailure: function (err, msg) {
                reject(err)
            }
        });
    }).then(data => data);
}

function loadNav() {
    document.querySelector('nav > div').innerHTML =
        `<ul>
           <li><a href=\"/index.html\">De kijkdoos</a></li>
        </ul>
        <ul>
          <li class=\"menu-item active\"><a href="./gift_purchase.html">Kado Kopen</a></li>
          <li class=\"menu-item\"><a href="./index.html">Films</a></li>
          <li class=\"menu-item\"><a href="./order.html">Reserveringen</a></li>
        </ul>`;
}

loadNav();

async function submitForm() {
    let formData = new FormData(document.getElementById('gift-form'));
    console.log(formData.get('name'), formData.get('email'));

    res = await chromelyRequest('/gift#create', 'POST', formData);
    console.log(res.getData());
}

document.querySelector('#submit-form').addEventListener('click', () => {
    submitForm();
});