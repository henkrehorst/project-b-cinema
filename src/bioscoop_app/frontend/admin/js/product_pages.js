/**
 * function with all javascript running on the product overview page
 * @returns {Promise<void>}
 */
async function productOverviewPage() {
    //get all upsells from backend
    const upsellResponse = await chromelyRequest('/product#type','POST',{'type':'upsell'});
    const upsells = upsellResponse.getData();
    
    //display upsell products in table
    let productTable = "";
    for(upsell in upsells){
        productTable += `<tr>
                <td>${upsells[upsell].name}</td>
                <td>&euro; ${upsells[upsell].price.toFixed(2).replace('.',',')}</td>
                <td><a href="/admin/product_edit.html?id=${upsells[upsell].Id}">Edit</a></td>
            </tr>`;
    }
    document.querySelector("body > div > div > div > table > tbody").innerHTML = productTable;

    //get all upsells from backend
    const ticketResponse = await chromelyRequest('/product#type','POST',{'type':'ticket'});
    const tickets = ticketResponse.getData();
    
    //display tickets in table
    productTable = "";
    for(ticket in tickets){
        productTable += `<tr>
                <td>${tickets[ticket].name}</td>
                <td>&euro; ${tickets[ticket].price.toFixed(2).replace('.',',')}</td>
                <td><a href="/admin/product_edit.html?id=${tickets[ticket].Id}">Edit</a></td>
            </tr>`;
    }
    document.querySelector("body > div > div > div:nth-child(2) > table > tbody").innerHTML = productTable;
}

/**
 * function with all javascript running on the product add page
 * @returns {Promise<void>}
 */
async function productAddPage() {

    /**
     * function for creating product in backend
     */
    async function addProduct() {
        // get data from product form
        const productForm = new FormData(document.querySelector("body > div > div > div > form"));

        // clear error messages if exists
        clearProductFormErrors();
        
        // post product to backend
        const response = await chromelyRequest('/products#add', 'POST', {
            'name': productForm.get('name'),
            'price': productForm.get('price').length > 0 ? productForm.get('price') : 0,
            'type': productForm.get('product-type')
        })
        
        // redirect to overview page if product is created
        if(response.getStatusCode() === 204){
            window.location.href = '/admin/product.html';
        }
        // display error message by 400
        if(response.getStatusCode() === 400){
            response.data.map(error => {
                displayFieldErrorMessage(error.PropertyName, error.ErrorMessage);
            })
        }
    }

    document.querySelector("body > div > div > div > form > div:nth-child(4) > button").addEventListener('click', addProduct);
}


/**
 * function with all javascript running on the product edit page
 * @returns {Promise<void>}
 */
async function productEditPage() {
    //fill product form with data
    const productResponse = await chromelyRequest('/product#id', 'POST', {'id': getIdFromUrl()});
    let product = productResponse.getData();
    document.getElementById('name').value = product.name;
    document.getElementById('price_field').value = product.price.toFixed(2);
    let typeDropdown = document.querySelector("#product-type-field");
    for(key in typeDropdown.options){
        if(typeDropdown.options[key].value === product.type){
            typeDropdown.options[key].selected = true;
        }
    }
    
    /**
     * function for updating product in backend
     */
    async function updateProduct() {
        // get data from product form
        const productForm = new FormData(document.querySelector("body > div > div > div > form"));

        // clear error messages if exists
        clearProductFormErrors();
        
        // update product in backend
        const response = await chromelyRequest('/products#update', 'POST', {
            'Id': getIdFromUrl(),
            'name': productForm.get('name'),
            'price': productForm.get('price').length > 0 ? productForm.get('price') : 0,
            'type': productForm.get('product-type')
        })

        // redirect to overview page if product is created
        if(response.getStatusCode() === 204){
            window.location.href = '/admin/product.html';
        }
        // display error message by 400
        if(response.getStatusCode() === 400){
            response.data.map(error => {
                displayFieldErrorMessage(error.PropertyName, error.ErrorMessage);
            })
        }
    }

    document.querySelector("body > div > div > div > form > div:nth-child(4) > button").addEventListener('click', updateProduct);
}


/**
 * Function for update ticket price in backend 
 */
async function updateTicketPrice() {
    // read ticket price from price field
    let ticketPrice = document.getElementById('price_field').value;
    
    // update ticket price in backend
    const response = await chromelyRequest('/product#ticketprice','POST',{'price':parseFloat(ticketPrice)});
    
    // if response is success: return to overview page
    if(response.getStatusCode() === 204){
        window.location.href = '/admin/product.html'
    }
}

/**
 * clear error messages in product forms
 */
function clearProductFormErrors() {
    clearFieldErrorMessage("name");
}