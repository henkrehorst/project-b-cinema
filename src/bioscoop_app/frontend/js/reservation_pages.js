/**
 * function with all javascript running on step one of the reservation flow (ticket selection step)
 */
async function stepOne() {
    // get all products with type ticket
    const ticketProductResponse = await chromelyRequest('/product#type', 'POST', {'type': 'ticket'});
    let ticketProducts = ticketProductResponse.getData();
    //prepare reservation cookie (shopping cart)
    prepareReservationCookie(ticketProducts);

    //show ticket products on page
    let ticketProductView = "";
    for (product in ticketProducts) {
        ticketProductView +=
            `<div class="product_item">
                <div class="product_details">
                    <p>${ticketProducts[product].name}</p>
                    <p>&euro; ${ticketProducts[product].price.toFixed(2).replace('.', ',')}</p>
                </div>
                <div class="price_control">
                    <button onclick="productControl(${ticketProducts[product].Id},-1)" type="button">-</button>
                    <input onchange="productControl(${ticketProducts[product].Id},this.value)" 
                    type="number" step="1" value="0" name="amount" id="ticket-field-${ticketProducts[product].Id}" >
                    <button onclick="productControl(${ticketProducts[product].Id}, 1)" type="button">+</button>
                </div>
            </div>`;
    }
    document.getElementById('product_view').innerHTML = ticketProductView;

    showOrUpdateReservationCart();
}


/**
 * function for controlling the amount of price control
 * @param id
 * @param amount
 */
function productControl(id, amount) {
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
        delete orderInformation['order'][id];
    }else {
        //update order
        orderInformation['order'][id] = amount;
    }
    //update reservation cookie
    updateCreateReservationCookie(orderInformation);
    showOrUpdateReservationCart();
}

/**
 * function for preparing reservation cookie
 * @param ticketProducts
 */
function prepareReservationCookie(ticketProducts) {
    //create json array for cookie
    let cookieValue = {
        'order':{},
        'products': ticketProducts
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
    
    for(order in reservation['order']){
        reservationView += 
            `<tr>
                <td>${reservation['order'][order]}</td>
                <td>${reservation['products'][order].name}</td>
                <td>&euro; ${(reservation['order'][order] * reservation['products'][order].price).toFixed(2).replace('.',',')}</td>
            </tr>`
    }
    
    if(reservationView === ""){
        reservationView = '<tr><th colspan="3" style="text-align: center">Nog geen tickets geselecteerd.</th></tr>'
    }
    
    document.getElementById('reservation_view').innerHTML = reservationView;
}