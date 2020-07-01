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

function submitForm() {
    let formData = new FormData(document.getElementById('gift-form'));
    console.log(formData.get('name'), formData.get('email'))

    res = await chromelyRequest('/gift#create', 'POST', order);
    console.log(res.getData());
}

document.querySelector('#submit-form').addEventListener('click', () => {
    submitForm();
});