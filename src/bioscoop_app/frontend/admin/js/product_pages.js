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
    console.log(products);
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
            'price': productForm.get('price')
        })
        
        // redirect to overview page if product is created
        if(response.getStatusCode() === 204){
            window.location.href = '/admin/product.html';
        }
    }

    document.querySelector("body > div > div > div > form > div:nth-child(3) > button").addEventListener('click', addProduct);
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
            'price': productForm.get('price')
        })

        // redirect to overview page if product is created
        if(response.getStatusCode() === 204){
            window.location.href = '/admin/product.html';
        }
    }
    
    document.querySelector("body > div > div > div > form > div:nth-child(3) > button").addEventListener('click', updateProduct);
}