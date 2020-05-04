/**
 * function with all javascript running on the product overview page
 * @returns {Promise<void>}
 */
async function productOverviewPage() {
    //get all products from backend
    const productResponse = await chromelyRequest('/products');
    const products = productResponse.getData();
    
    //display products in table
    let productTable = "";
    for(product in products){
        productTable += `<tr>
                <td>${products[product].name}</td>
                <td>&euro; ${products[product].price.toFixed(2).replace('.',',')}</td>
                <td><a href="/admin/product_edit.html?id=${products[product].Id}">Edit</a></td>
            </tr>`;
    }
    document.querySelector("body > div > div > div > table > tbody").innerHTML = productTable;
    
    // get default price ticket from backend
    const ticketPriceResponse = await chromelyRequest('/product#ticketprice');
    // show default ticket price
    document.querySelector("body > div:nth-child(3) > div > div > table > tbody > tr > td:nth-child(2)").innerHTML =
        `&euro; ${ticketPriceResponse.getData().toFixed(2).replace('.', ',')}`

    /**
     * show ticket price edit form
     */
    function showPriceEditForm() {
        document.querySelector("body > div:nth-child(3) > div > div > table > tbody > tr").innerHTML = 
            `<td>
                <div class="form-group">
                    <div class="input-group mb-2">
                        <div class="input-group-prepend">
                            <div class="input-group-text">&euro;</div>
                        </div>
                        <input name="price" value="${ticketPriceResponse.getData().toFixed(2)}" type="number" step="0.01" class="form-control" id="price_field">
                    </div>
            </td><td><button onclick="updateTicketPrice()" class="btn-primary btn">Pas aan</button></td>`
    }

    // click event edit price button
    document.querySelector("body > div:nth-child(3) > div > div > table > tbody > tr > td:nth-child(3) > button")
        .addEventListener('click', showPriceEditForm);
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
        
        // post product to backend
        const response = await chromelyRequest('/products#add', 'POST', {
            'name': productForm.get('name'),
            'price': productForm.get('price'),
            'type': productForm.get('product-type')
        })
        
        // redirect to overview page if product is created
        if(response.getStatusCode() === 204){
            window.location.href = '/admin/product.html';
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
    document.getElementById('name_field').value = product.name;
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

        // update product in backend
        const response = await chromelyRequest('/products#update', 'POST', {
            'id': getIdFromUrl(),
            'name': productForm.get('name'),
            'price': productForm.get('price'),
            'type': productForm.get('product-type')
        })

        // redirect to overview page if product is created
        if(response.getStatusCode() === 204){
            window.location.href = '/admin/product.html';
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