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
    //prepare reservation cookie (shopping cart)
    prepareReservationCookie(ticketProducts, upsells);
    
    //display products
    displayProducts(ticketProducts, 'product_view','order')
    displayProducts(upsells, 'upsell_view', 'upsell')
    
    showOrUpdateReservationCart();
}

/**
 * function for displaying products
 */
function displayProducts(products, location, productType){
    
    //show ticket products on page
    let productView = "";
    for (product in products) {
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
    document.getElementById(location).innerHTML = productView;
}

/**
 * function with all javascript running on step two of the reservation flow (seat selection step)
 */
async function stepTwo() {
    //show reservation
    showOrUpdateReservationCart();
}

/**
 * function with all javascript running on step three of the reservation flow (confirm)
 */
async function stepThree() {
    //show shopping cart
    showOrUpdateReservationCart();

    /**
     * function for finish reservation
     */
    async function finishReservation() {
        //read form information
        let confirmForm = new FormData(document.getElementById('checkout-form'));
        console.log(confirmForm.get('name'),confirmForm.get('email'))
        
        //display reservation code after success
        document.querySelector("body > div > div > div.col-md-8.reservation_boxes > div.reservation_confirm_form").innerHTML =
        `<p>Hieronder staat je reserveringscode om je tickets mee op te halen,
            deze is ook terug te vinden in je email.</p>
        <div class="mt-5 reservation_code_box"><p>DGWE1123FGEWW</p></div>
         <a href="/index.html" class="btn btn-success confirm_button">GA TERUG NAAR HET OVERZICHT</a>`;
        
        //change title
        document.querySelector("body > div > div > div.col-md-8.reservation_boxes > div.reservation_box_header > h3").innerHTML =
            "We hebben je reservering succesvol ontvangen";
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
function productControl(id, amount, type= "order") {
    if (typeof amount === 'string') {
        //parse int of amount
        if(amount.match(/^-{0,1}\d+$/)){
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
    
    if(amount === 0){
        //remove ticket from order
        delete orderInformation[type][id];
    }else {
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
function prepareReservationCookie(ticketProducts, upsells) {
    //create json array for cookie
    let cookieValue = {
        'order':{},
        'products': ticketProducts,
        'upsellProducts': upsells,
        'upsell':{}
    };
    
    updateCreateReservationCookie(cookieValue);
}

/**
 * function for update and create reservation cookie
 * @param cookieValue
 */
function updateCreateReservationCookie(cookieValue) {
    //create new cookie or update
    document.cookie = "reservation=" + JSON.stringify(cookieValue) + ";" + 24*60*60*1000 + ";path=/";
}

/**
 * Get value of reservation cookie
 */
function getReservationCookieValue() {
    //get array of cookies
    let cookieArray = decodeURIComponent(document.cookie).split(';');
    //remove space
    for(cookie in cookieArray){
        cookieArray[cookie] = cookieArray[cookie].trim();
    }
    
    //search cookie
    for(cookie in cookieArray){
        //split key from value
        let currentCookie = cookieArray[cookie].split('=');
        if(currentCookie[0] === 'reservation'){
            return JSON.parse(currentCookie[1]);
        }
    }
    
    return {};
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
    
    for(order in reservation['order']){
        reservationView += 
            `<tr>
                <td>${reservation['order'][order]}</td>
                <td>${reservation['products'][order].name}</td>
                <td>&euro; ${(reservation['order'][order] * reservation['products'][order].price).toFixed(2).replace('.',',')}</td>
            </tr>`
        totalCost += reservation['order'][order] * reservation['products'][order].price
    }
    
    if(reservationView === ""){
        reservationView = '<tr><th colspan="3" style="text-align: center">Nog geen tickets geselecteerd.</th></tr>'
    }

    document.getElementById('reservation_view').innerHTML = reservationView;
    
    let upsellView = "";
    //show upsells
    for(upsell in reservation['upsell']){
        upsellView +=
            `<tr>
                <td>${reservation['upsell'][upsell]}</td>
                <td>${reservation['upsellProducts'][upsell].name}</td>
                <td>&euro; ${(reservation['upsell'][upsell] * reservation['upsellProducts'][upsell].price).toFixed(2).replace('.',',')}</td>
            </tr>`
        totalCost += reservation['upsell'][upsell] * reservation['upsellProducts'][upsell].price;
    }

    if(upsellView === ""){
        upsellView = '<tr><th colspan="3" style="text-align: center">Geen extra producten geselecteerd.</th></tr>'
    }

    document.getElementById('upsell_cart').innerHTML = upsellView;
    //display total price
    document.getElementById('total_cost').innerHTML = `&euro; ${totalCost.toFixed(2).replace('.',',')}`;
}

/**
 * Function for getting the total count of tickets
 */
function getTotalTicketCount() {
    // get orders
    let orders = getReservationCookieValue().order;
    // calculate ticket count
    let ticketCount = 0;
    
    for(order in orders){
           ticketCount += orders[order];
    }
    
    return ticketCount;
}