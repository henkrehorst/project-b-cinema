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

document.querySelector('#gift-form select[name="gift-type"]').addEventListener('change', (event) => {
    let description;
    let el = document.querySelector('#gift-form select[name="gift-type"]');

    //for (let i = 0; i < el.options.length; i++) {
    //    if (el.options[i].selected) {
    //        console.log(el.options[i]);
    //    }
    //}

    if (el.value == 'bronze') {
        description = `De BRONS kortingsbon heeft een waarde van 8 euro.\nEen perfecte gift voor iemand die je graag wilt verassen!`;
    }
    else if (el.value == 'silver') {
        description = `De ZILVER kortingsbon heeft een waarde van 15 euro.\nMet z'n flinke korting kan je iedereen blij mee maken!`;
    }
    else if (el.value == 'gold') {
        description = `De GOUD kortingsbon heeft een waarde van 25 euro.\nDit is voor de echte film-fanaat, vooral handig als je met een groep wilt gaan!`;
    }

    document.querySelector('#gift-description').innerText = description;
});

document.querySelector('#gift-description').innerText = `De BRONS kortingsbon heeft een waarde van 8 euro.\nEen perfecte gift voor iemand die je graag wilt verassen!`;