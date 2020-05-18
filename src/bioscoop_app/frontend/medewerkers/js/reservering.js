/**
 * function with all javascript running on the movie overview admin page
 * @returns {Promise<void>}
 */
async function reserveringenOverviewPage() {
    // get all movies
    const reserveringenResponse = await chromelyRequest('/reserveringen');
    console.log(reserveringenResponse);
    let reserveringen = reserveringenResponse.getData();
    let reserveringTable = "";

    // create a table with screentimes
    for(reservering in reserveringen){
        reserveringTable += `<tr>
                <td>${reserveringen[reservering].naam}</td>
                <td>${reserveringen[reservering].email}</td>
                <td>${reserveringen[reservering].producten}</td>
                <td><a href='/medewerkers/reservering_edit.html?id=${reserveringen[reservering].Id}'>Edit</a></td>
            </tr>`;
    }

    //display table
    document.querySelector("body > div > div > div > table > tbody").innerHTML += reserveringTable;
}

/**
 * function with all javascript running on the movie edit page
 * @returns {Promise<void>}
 */
async function reserveringEditPage() {
    //get movie by id
    const reserveringResponse = await chromelyRequest('/reserveringen#id','POST',{'id': getIdFromUrl()});
    let reservering = reserveringResponse.getData();
    
    //fill form with movie data
    document.getElementById('naam').value = reservering.naam;
    document.getElementById('email').value = reservering.email;
    document.getElementById('producten').value = reservering.producten;
    
    /**
     * function for posting movie to backend
     */
    async function updateReservering() {
        // get movie form data
        const reserveringForm = new FormData(document.querySelector("body > div > div > div > form"));
       

        // post movie to backend
        const response = await chromelyRequest('/reserveringen#update', 'POST', {
            'id': getIdFromUrl(),
            'naam': reserveringForm.get('naam'),
            'email': reserveringForm.get('email'),
            'producten': reserveringForm.get('producten'),
            
        })

        // if response = 200: redirect to movie overview page
        if(response.getStatusCode() === 200){
            window.location.href = "/medewerkers/reservering.html"
        }
    }

    document.querySelector("body > div > div > div > form > div > button").addEventListener('click', updateReservering);
}

/**
 * function with all javascript running on the movie add page
 * @returns {Promise<void>}
 */
async function reserveringAddPage() {
    /**
     * function for posting movie to backend
     */
    async function addReservering() {
        // get movie form data
        const reserveringForm = new FormData(document.querySelector("body > div > div > div > form"));
        
            
        // post movie to backend
        const response = await chromelyRequest('/reserveringen/add', 'POST', {
                'naam': reserveringForm.get('naam'),
                'email': reserveringForm.get('email'),
                'producten': reserveringForm.get('producten'),
               
            })
            
            // if response = 204: redirect to movie overview page
            if(response.getStatusCode() === 204){
                window.location.href = "/medewerkers/reservering.html"
            }
    }

    document.querySelector("body > div > div > div > form > div > button").addEventListener('click', addReservering);
}

/**
 * Convert file to base 64 string
 * @param file
 * @returns {Promise<string>}
 */
async function getBase64String(file) {
    return new Promise((resolve, reject) => {
        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            if(reader.result === "data:"){
                resolve("")
            }else {
                resolve(reader.result);
            }
        };
        reader.error = function (error) {
            reject(error);
        };
    }).then(value => value)
}

/**
 * show a preview of the uploaded cover image
 * @returns {Promise<void>}
 */
async function coverImagePreview() {
    // get movie form data
    const reserveringForm = new FormData(document.querySelector("body > div > div > div > form"));
   
   
}