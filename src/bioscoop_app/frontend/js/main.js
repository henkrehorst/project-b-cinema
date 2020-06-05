/**
 * enable security model
 */
function addSecurityModel() {


    //add security model in body
    document.querySelector("body").innerHTML +=
        `<div id="security-model" class="modal">
            <div class="modal-content">
                <span id="clossButton" class="close">&times;</span>
                <h1>Inloggen medewerker/beheerder</h1>
                <p>Voer hieronder je gegevens in, om in te loggen.</p>
                <div class="container">
                    <div class="row justify-content-center">
                        <div class="col-md-6">
                        <form id="securityForm">
                            <div class="form-group mt-2">
                                <label for="password-field text-left">Wachtwoord:</label>
                                <input name="password" type="password" placeholder="Wachtwoord" class="form-control" id="password-field">
                            </div>
                            <div id="error-message"></div>
                            <div class="form-group">
                                <button type="button" onclick="userLogin()" id="signInButton" class="btn btn-primary btn-block">INLOGGEN</button>
                            </div>
                        </form>
                        </div>
                    </div>
                </div>
                </div>
            </div>
        </div>`;

    const securityModel = document.getElementById('security-model');
    const closeButton = document.getElementById('clossButton');

    //if user click outside the security model, close the model
    window.onclick = function (event) {
        if (event.target === securityModel) {
            securityModel.style.display = "none";
            document.querySelector('body').style.overflowY = "auto"
        }
    }

    //close model by onclick close button
    closeButton.onclick = () => {
        securityModel.style.display = "none";
        document.querySelector('body').style.overflowY = "auto"
    }

    function showSecurityModel() {
        document.getElementById("security-model").style.display = "block";
        document.querySelector('body').style.overflowY = "hidden"
    }

    //show security model when short cut is pressed
    document.onkeydown = (event) => {
        if (event.ctrlKey && event.shiftKey) {
            //show model
            showSecurityModel();
        }
    }
}

addSecurityModel()

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
          <li class=\"menu-item\"><a href="./order.html">Reserveringen</a></li>
          <li class=\"menu-item\"><a href="./medewerkers/medewerker.html">Medewerkers</a></li>
          <li class=\"menu-item\"><a href="./admin/admin.html">Beheerder</a></li>
        </ul>`;

    markActive();
}

/*
 * Marks the active item in the navbar.
 */
function markActive() {
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
 * @returns {chromelyResponse}
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
                let result = JSON.parse(JSON.parse(response).Data);
                resolve(new chromelyResponse(result.status, result.statusText, result.data))
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
    return getParameterFromUrl('id');
}

/**
 * function for reading get parameter from url
 * @returns {string}
 */
function getParameterFromUrl(key) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(key);
}

/**
 * chromely response format class
 */
class chromelyResponse {
    /**
     * @param status
     * @param statusText
     * @param data
     */
    constructor(status, statusText, data) {
        this.status = status;
        this.statusText = statusText;
        //convert json data string to object
        try {
            this.data = JSON.parse(data)
        } catch (e) {
            this.data = "";
        }
    }

    /**
     * @returns number
     */
    getStatusCode() {
        return this.status;
    }

    /**
     * @returns {*}
     */
    getData() {
        return this.data;
    }

    /**
     * @returns string
     */
    getStatusText() {
        return this.statusText;
    }
}

/**
 * check user login in backend
 */
async function userLogin() {
    //get password from security form
    let password = new FormData(document.getElementById('securityForm')).get('password');

    //check password in backend
    const response = await chromelyRequest('/security/login', 'POST', {password: password})
    
    //if status code  === 200, password is correct redirect user to correct screen
    if (response.getStatusCode() === 200) {
        if (response.getData().role === "cashier") {
            //redirect to cashier screen
            window.location.href = "./medewerkers/medewerker.html";
        } else if (response.getData().role === "admin") {
            //redirect to admin screen
            window.location.href = "./admin/admin.html";
        } else {
            //show error message
            document.getElementById('error-message').innerHTML =
            `<div class="alert alert-primary" role="alert">
                  Er is iets fout gegaan probeer het nog een keer!
            </div>`;
        }
    } else {
        //show error message if password is incorrect
        document.getElementById('error-message').innerHTML =
            `<div class="alert alert-danger" role="alert">
                  Incorrect wachtwoord, probeer het nog een keer!
            </div>`;
    }
}