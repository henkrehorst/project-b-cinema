function loadNav() {
    document.querySelector("nav > div").innerHTML =
        `<ul>
           <li><a href=\"/index.html\">De kijkdoos</a></li>
        </ul>
        <ul>
          <li class=\"menu-item\"><a href="./gift_purchase.html">Gift Kopen</a></li>
          <li class=\"menu-item\"><a href="./index.html">Films</a></li>
          <li class=\"menu-item\"><a href="./order.html">Reserveringen</a></li>
        </ul>`;

    let elNav = document.querySelector("body > nav");

    if (elNav.dataset.activeItem !== undefined) {
        let navItems = document.getElementsByClassName("menu-item");

        for (let i = 0; i < navItems.length; i++) {
            if (navItems[i].innerText == elNav.dataset.activeItem) {
                navItems[i].classList.add("active");
            }
        }
    }
}

loadNav();