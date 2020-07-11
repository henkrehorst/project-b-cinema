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

function setCookie(value) {
    localStorage.setItem('voucher-code', JSON.stringify(value));
}

function getCookie() {
    return JSON.parse(localStorage.getItem('voucher-code'));
}

async function submitForm() {
    let formData = new FormData(document.getElementById('gift-form'));

    let res = await chromelyRequest('/gift#create', 'POST', {
        'gift-email': formData.get('gift-email'),
        'gift-type': formData.get('gift-type')
    });

    if (res.getStatusCode() == 200) {
        setCookie(res.data);
        window.location.href = './voucher.html';
    } else {
        console.log(res.getStatusCode(), res.getStatusText());
        document.querySelector('.error-message').style.display = 'block';
    }
}

document.querySelector('#submit-form').addEventListener('click', () => {
    submitForm();
});