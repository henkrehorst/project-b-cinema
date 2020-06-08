/*
 * Update the active item in the navbar.
 */
async function setActiveNav(value = null) {
    document.querySelector("nav").setAttribute("data-active-item", (value === null) ? ((getReservationCookieValue().newOrder) ? "Films" : "Reserveringen") : value);
    markActive();
}

/**
 * Function for showing the details of the movie
 */
async function showMovieDetail() {
    //get movie by id
    const movieResponse = await chromelyRequest('/movies#id', 'POST', {'id': getReservationCookieValue().screentime.movie});
    let movie = movieResponse.getData();

    //display thumbnail
    document.getElementById("movieThumbnail").src = `local://frontend/uploads/${movie.thumbnailImage}`;
    //display title and time
    document.getElementById("movieTitle").innerText = movie.title;
    //change time format to nl
    moment.locale('nl-nl');
    document.getElementById("movieTime").innerText =
        moment(new Date(getReservationCookieValue().screentime.startTime)).format('dddd d MMMM yyyy') +
        " | " + moment(new Date(getReservationCookieValue().screentime.startTime)).format('LT') +
        " - " + moment(new Date(getReservationCookieValue().screentime.endTime)).format('LT');
}


/**
 * function with all javascript running on step one of the reservation flow (ticket selection step)
 */
async function stepOne() {
    // get all products with type ticket
    const ticketProductResponse = await chromelyRequest('/product#type', 'POST', {'type': 'ticket'});
    let ticketProducts = ticketProductResponse.getData();
    //get upsells
    const upsellResponse = await chromelyRequest('/product#type', 'POST', {'type': 'upsell'});
    let upsells = upsellResponse.getData();
    //get seat costs
    const seatCostResponse = await chromelyRequest('/product#type', 'POST', {'type': 'seatcost'});
    let seatCosts = seatCostResponse.getData();
    //merge seatCosts in upsells
    Object.assign(upsells, seatCosts);

    //display products
    displayProducts(ticketProducts, 'product_view', 'order')
    displayProducts(upsells, 'upsell_view', 'upsell')

    // create reservation cookie if it is a new order
    if (getParameterFromUrl("change") == null) {
        //prepare reservation cookie (shopping cart)
        await prepareReservationCookie(ticketProducts, upsells);
    } else {
        // fill product controls with order dataA
        fillProductControls()
    }


    showOrUpdateReservationCart();
    showMovieDetail();
    setActiveNav();

    //For the next step, check tickets have been selected
    document.getElementById("nextStep").addEventListener("click", () => {
        if (Object.size(getReservationCookieValue().order) <= 0) {
            document.getElementById("ticketError").style.display = "block";
        } else {
            window.location.href = "/reservation_step_two.html";
        }
    });
}

/**
 * function for displaying products
 */
function displayProducts(products, location, productType) {

    //show ticket products on page
    let productView = "";
    for (product in products) {
        if (products[product].type !== "seatcost") {
            productView +=
                `<div class="product_item">
                <div class="product_details">
                    <p>${products[product].name}</p>
                    <p>&euro; ${products[product].price.toFixed(2).replace('.', ',')}</p>
                </div>
                <div class="price_control">
                    <button onclick="productControl(${products[product].Id},-1, '${productType}')" type="button">-</button>
                    <input onchange="productControl(${products[product].Id},this.value, '${productType}')" 
                    type="number" step="1" value="0" name="amount" id="ticket-field-${products[product].Id}" >
                    <button onclick="productControl(${products[product].Id}, 1, '${productType}')" type="button">+</button>
                </div>
            </div>`;
        }
    }
    document.getElementById(location).innerHTML = productView;
}

/**
 * function with all javascript running on step two of the reservation flow (seat selection step)
 */
async function stepTwo() {
    setActiveNav();
    //show reservation
    showOrUpdateReservationCart();
    //display movie thumbnail
    showMovieDetail();
}

/**
 * function with all javascript running on step three of the reservation flow (confirm)
 */
async function stepThree() {
    setActiveNav();
    //show shopping cart
    showOrUpdateReservationCart();
    //display movie thumbnail
    showMovieDetail();
    //fill customer information in change order flow
    if (getReservationCookieValue().newOrder === false) {
        document.getElementById("name-field").value = getReservationCookieValue().name;
        document.getElementById("email-field").value = getReservationCookieValue().email;
    }

    /**
     * function for finish reservation
     */
    async function finishReservation() {
        //read form information
        let confirmForm = new FormData(document.getElementById('checkout-form'));
        console.log(confirmForm.get('name'), confirmForm.get('email'))
        //clear error messages
        clearFieldErrorMessage("name-field");
        clearFieldErrorMessage("email-field");

        let cookieval = getReservationCookieValue();
        let res;
        if (cookieval.newOrder) {
            console.log("create route for order");
            let order = {
                'items': generateProductOrder(),
                'tickets': cookieval['tickets'],
                'cust_name': confirmForm.get('name'),
                'cust_email': confirmForm.get('email')
            };
            res = await chromelyRequest('/order#create', 'POST', order);
            console.log(res.getData(), res.getStatusCode())
        } else {
            console.log("update route for order " + localStorage.getItem("ordercode"));
            let order = {
                'id': localStorage.getItem("ordercode"),
                'items': generateProductOrder(),
                'tickets': cookieval['tickets'],
                'cust_name': confirmForm.get('name'),
                'cust_email': confirmForm.get('email')
            };
            res = await chromelyRequest('/order#update', 'POST', order);
            console.log("done waiting, " + res.getStatusCode());
            console.log(res.getData());
        }
        if(res.getStatusCode() === 200) {
            let reservationCode = (res.getStatusCode() === 200) ? res.getData() : -1;
            //display reservation code after success
            document.querySelector("body > div > div > div.col-md-8.reservation_boxes > div.reservation_confirm_form").innerHTML =
                `<p>Hieronder staat je reserveringscode om je tickets mee op te halen,
            deze is ook terug te vinden in je email.</p>
        <div class="mt-5 reservation_code_box"><p>${reservationCode}</p></div>
         <a href="/index.html" class="btn btn-success confirm_button">GA TERUG NAAR HET OVERZICHT</a>`;

            //change title
            document.querySelector("body > div > div > div.col-md-8.reservation_boxes > div.reservation_box_header > h3").innerHTML =
                "We hebben je reservering succesvol ontvangen";
            //make last step completed
            document.getElementById("lastStep").classList.replace("current", "completed")
        }else if(res.getStatusCode() === 422){
            //display error messages
            for(let item in res.getData()){
                displayFieldErrorMessage(item, res.getData()[item])
            }
        }
    }

    //add finish function on confirm button
    document.getElementById('confirm_button').addEventListener('click', finishReservation);
}

/**
 * function for controlling the amount of price control
 * @param id
 * @param amount
 * @param type
 */
function productControl(id, amount, type = "order") {
    if (typeof amount === 'string') {
        //parse int of amount
        if (amount.match(/^-{0,1}\d+$/)) {
            amount = parseInt(amount);
        } else {
            amount = 0;
        }
    } else {
        //read field value
        amount = parseInt(document.getElementById(`ticket-field-${id}`).value) + amount;
    }

    // amount cannot lower than zero
    if (amount < 0) {
        amount = 0;
    }

    //update amount field
    document.getElementById(`ticket-field-${id}`).value = amount;

    //update order information
    let orderInformation = getReservationCookieValue();

    if (amount === 0) {
        //remove ticket from order
        delete orderInformation[type][id];
        //check if key is not deleted
        if (orderInformation[type] === undefined) {
            orderInformation[type] = {};
        }
    } else {
        //update order
        orderInformation[type][id] = amount;
    }
    //update reservation cookie
    updateCreateReservationCookie(orderInformation);
    showOrUpdateReservationCart();
}


/**
 * function update product in shopping cart
 * @param id
 * @param amount
 * @param type
 */
function updateProductInShoppingCart(id, amount, type = "order") {
    //update order information
    let orderInformation = getReservationCookieValue();

    if (amount === 0 && orderInformation[type][id] !== undefined) {
        //remove ticket from order
        delete orderInformation[type][id];
        //check if key is not deleted
        if (orderInformation[type] === undefined) {
            orderInformation[type] = {};
        }
    } else if (amount !== 0) {
        //update order
        orderInformation[type][id] = amount;
    }
    //update reservation cookie
    updateCreateReservationCookie(orderInformation);
    showOrUpdateReservationCart();
}

/**
 * function for preparing reservation cookie
 * @param ticketProducts
 * @param upsells
 */
async function prepareReservationCookie(ticketProducts, upsells) {
    //get screentime from url id
    const screentimeResponse = await chromelyRequest('/screentime#id', 'POST', {'id': getIdFromUrl()})
    //create json array for cookie
    let cookieValue = {
        'order': {},
        'products': ticketProducts,
        'upsellProducts': upsells,
        'upsell': {},
        'screentime': screentimeResponse.getData(),
        'newOrder': true,
        'tickets': []
    };

    updateCreateReservationCookie(cookieValue);
}

/**
 * function for update and create reservation cookie
 * @param cookieValue
 */
function updateCreateReservationCookie(cookieValue) {
    //create new cookie or update
    localStorage.setItem("reservation", JSON.stringify(cookieValue));
}

/**
 * Get value of reservation cookie
 */
function getReservationCookieValue() {
    //read data from local storage
    return JSON.parse(localStorage.getItem("reservation"));
}

/**
 * display reservation information
 */
function showOrUpdateReservationCart() {
    //get order information
    let reservation = getReservationCookieValue();
    //display reservation in table
    let reservationView = "";
    let totalCost = 0;

    for (order in reservation['order']) {
        reservationView +=
            `<tr>
                <td>${reservation['order'][order]}</td>
                <td>${reservation['products'][order].name}</td>
                <td>&euro; ${(reservation['order'][order] * reservation['products'][order].price).toFixed(2).replace('.', ',')}</td>
            </tr>`
        totalCost += reservation['order'][order] * reservation['products'][order].price
    }

    if (reservationView === "") {
        reservationView = '<tr><th colspan="3" style="text-align: center">Nog geen tickets geselecteerd.</th></tr>'
    }

    document.getElementById('reservation_view').innerHTML = reservationView;

    let upsellView = "";
    //show upsells
    for (upsell in reservation['upsell']) {
        upsellView +=
            `<tr>
                <td>${reservation['upsell'][upsell]}</td>
                <td>${reservation['upsellProducts'][upsell].name}</td>
                <td>&euro; ${(reservation['upsell'][upsell] * reservation['upsellProducts'][upsell].price).toFixed(2).replace('.', ',')}</td>
            </tr>`
        totalCost += reservation['upsell'][upsell] * reservation['upsellProducts'][upsell].price;
    }

    if (upsellView === "") {
        upsellView = '<tr><th colspan="3" style="text-align: center">Geen extra producten geselecteerd.</th></tr>'
    }

    document.getElementById('upsell_cart').innerHTML = upsellView;
    //display total price
    document.getElementById('total_cost').innerHTML = `&euro; ${totalCost.toFixed(2).replace('.', ',')}`;
}

/**
 * Function for getting the total count of tickets
 */
function getTotalTicketCount() {
    // get orders
    let orders = getReservationCookieValue().order;
    // calculate ticket count
    let ticketCount = 0;

    for (order in orders) {
        ticketCount += orders[order];
    }

    return ticketCount;
}

/**
 * Generate tickets by selected seats
 * @param selectedSeats
 */
function generateTickets(selectedSeats) {
    console.log(selectedSeats);
    //get reservation cookie value
    let reservationCookie = getReservationCookieValue();
    if (selectedSeats.length === 0) {
        reservationCookie['tickets'] = [];
        updateProductInShoppingCart(9, 0, "upsell");
        updateProductInShoppingCart(10, 0, "upsell");
        updateCreateReservationCookie(reservationCookie);
    } else {
        //calculate extra seat cost
        let deluxeCount = 0;
        let premiumCount = 0;
        let seatPos = 0;
        let ticketArray = [];
        for (order in reservationCookie['order']) {
            let ticketCount = reservationCookie['order'][order];
            for (let i = 0; i < ticketCount; i++) {
                ticketArray[seatPos] = {
                    'Id': order,
                    'price': parseFloat(reservationCookie['products'][order].price),
                    'name': reservationCookie['products'][order].name,
                    'row': selectedSeats[seatPos] === undefined ? 0 : selectedSeats[seatPos].row,
                    'seatnr': selectedSeats[seatPos] === undefined ? 0 : selectedSeats[seatPos].seat,
                    'screenTime': reservationCookie['screentime'].Id,
                    'visitorAge': 12
                };

                reservationCookie['tickets'] = ticketArray;
                if (selectedSeats[seatPos] !== undefined) {
                    //check seats has extra seat costs
                    if (selectedSeats[seatPos].type === "luxe") {
                        premiumCount++;
                    } else if (selectedSeats[seatPos].type === "VIP") {
                        deluxeCount++;
                    }
                }
                seatPos++;
                updateCreateReservationCookie(reservationCookie);
            }
        }
        updateProductInShoppingCart(9, deluxeCount, "upsell");
        updateProductInShoppingCart(10, premiumCount, "upsell");
    }
}

/**
 * Convert product order to products
 * @return []
 */
function generateProductOrder() {
    const reservationCookie = getReservationCookieValue();
    let productArray = [];
    for (product in reservationCookie.upsell) {
        let upsellCount = reservationCookie.upsell[product];
        for (let i = 0; i < upsellCount; i++) {
            productArray.push(reservationCookie.upsellProducts[product])
        }
    }
    return productArray;
}

/**
 * Function with all javascript of the reservation overview page
 */
async function reservationOverview() {
    setActiveNav("Reserveringen");
    let orderResponse = await chromelyRequest("/order#fetch", "POST", {"code": getParameterFromUrl("orderCode")});

    if (orderResponse.getStatusCode() === 200) {
        //set reservation data from order
        await setReservation(orderResponse.getData());
        let orderId = orderResponse.getData().Id;

        //activate cancel button
        document.getElementById("cancelButton").addEventListener("click", async () => {
            let res = await chromelyRequest("/order#cancel", "POST", {"code": getParameterFromUrl("orderCode")});
            if (res.getStatusCode() === 200) {
                window.location.href = "./index.html"
            } else if (res.getStatusCode() === 400) {
                console.log(res.getStatusText());
            } else {
                console.log(res.getStatusCode());
            }
        });
    }

    //display costumer and order information
    document.getElementById("name-field").innerText = getReservationCookieValue().name;
    document.getElementById("email-field").innerText = getReservationCookieValue().email;
    document.getElementById("order-code").innerText = getParameterFromUrl("orderCode")

    //show shopping cart
    showOrUpdateReservationCart();
    //display movie thumbnail
    showMovieDetail();
}


/**
 * Function for set reservation data
 */
async function setReservation(order) {
    console.log(order);
    // get all products with type ticket
    const ticketProductResponse = await chromelyRequest('/product#type', 'POST', {'type': 'ticket'});
    //get upsells
    const upsellResponse = await chromelyRequest('/product#type', 'POST', {'type': 'upsell'});
    let upsells = upsellResponse.getData()
    //get seat costs
    const seatCostResponse = await chromelyRequest('/product#type', 'POST', {'type': 'seatcost'});
    let seatCosts = seatCostResponse.getData();
    //merge seatCosts in upsells
    Object.assign(upsells, seatCosts);


    //get screentime from url id
    const screentimeResponse = await chromelyRequest('/screentime#id', 'POST', {'id': order.tickets[0].screenTime})
    //create json array for cookie
    let cookieValue = {
        'order': convertTicketsProducts(order.tickets.map(item => item.Id)),
        'products': ticketProductResponse.getData(),
        'upsellProducts': upsells,
        'upsell': convertTicketsProducts(order.items.map(item => item.Id)),
        'screentime': screentimeResponse.getData(),
        'tickets': [],
        'newOrder': false,
        'name': order.cust_name,
        'email': order.cust_email,
        'order_tickets': order.tickets
    };

    updateCreateReservationCookie(cookieValue);
}


/**
 * Covert ticket or product array to shopping cart format
 */
function convertTicketsProducts(idArray) {
    let output = {}
    for (let item in idArray) {
        if (output[idArray[item]] === undefined) {
            output[idArray[item]] = 1;
        } else {
            output[idArray[item]] += 1;
        }
    }
    return output;
}

/**
 * fill product controls with order data
 */
function fillProductControls() {
    // fill ticket products
    let ticketData = getReservationCookieValue().order;
    for (let item in ticketData) {
        productControl(item, ticketData[item])
    }

    // fill extra products
    let productData = getReservationCookieValue().upsell;
    for (let item in productData) {
        if (9 !== parseInt(item) && parseInt(item) !== 10) {
            productControl(item, productData[item], 'upsell')
        }
    }
}