/**
 * load dynamic items in navbar
 */
function fillNavbar() {
    document.querySelector("body > nav > div").innerHTML =
        `<ul>
           <li><a href=\"/\">De kijkdoos</a></li>
        </ul>
        <ul>
          <li class=\"menu-item\"><a href="./index.html">Films</a></li>
          <li class=\"menu-item\"><a href="#">Reserveringen</a></li>
          <li class=\"menu-item\"><a href="./admin/admin.html">Beheerder</a></li>
        </ul>`;

    //set active item
    let nav = document.querySelector("body > nav");
    if (nav.dataset.activeItem !== undefined) {
        //get nav items
        let navItems = document.getElementsByClassName("menu-item");
        for (let i = 0; i < navItems.length; i++) {
            if (navItems[i].innerText === nav.dataset.activeItem) {
                navItems[i].classList.add("active");
            }
        }
    }
}

//trigger fillNavbar function
fillNavbar();

/**
 * function for getting data and posting data to backend
 * @param route
 * @param method
 * @param postData
 * @returns {Promise<{}>}
 */
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
                resolve(JSON.parse(JSON.parse(response).Data));
            }, onFailure: function (err, msg) {
                reject(err)
            }
        });
    }).then(data => data);
}

/**
 * function for reading id parameter from url
 * @returns {string}
 */
function getIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('id');
}