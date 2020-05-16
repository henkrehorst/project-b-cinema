/**
 * function with all javascript running on the movie overview admin page
 * @returns {Promise<void>}
 */
async function moviesOverviewPage() {
    // get all movies
    const moviesResponse = await chromelyRequest('/movies');
    console.log(moviesResponse);
    let movies = moviesResponse.getData();
    let movieTable = "";

    // create a table with screentimes
    for(movie in movies){
        movieTable += `<tr>
                <td>${movies[movie].naam}</td>
                <td>${movies[movie].email}</td>
                <td>${movies[movie].producten}</td>
                <td><a href='/medewerkers/reservering_edit.html?id=${movies[movie].Id}'>Edit</a></td>
            </tr>`;
    }

    //display table
    document.querySelector("body > div > div > div > table > tbody").innerHTML += movieTable;
}

/**
 * function with all javascript running on the movie edit page
 * @returns {Promise<void>}
 */
async function movieEditPage() {
    //get movie by id
    const movieResponse = await chromelyRequest('/movies#id','POST',{'id': getIdFromUrl()});
    let movie = movieResponse.getData();
    
    //fill form with movie data
    document.getElementById('naam').value = movie.naam;
    document.getElementById('email').value = movie.email;
    document.getElementById('producten').value = movie.producten;
    
    /**
     * function for posting movie to backend
     */
    async function updateMovie() {
        // get movie form data
        const movieForm = new FormData(document.querySelector("body > div > div > div > form"));
       

        // post movie to backend
        const response = await chromelyRequest('/movies#update', 'POST', {
            'id': getIdFromUrl(),
            'naam': movieForm.get('naam'),
            'email': movieForm.get('email'),
            'producten': movieForm.get('producten'),
            
        })

        // if response = 200: redirect to movie overview page
        if(response.getStatusCode() === 200){
            window.location.href = "/medewerkers/reservering.html"
        }
    }

    document.querySelector("body > div > div > div > form > div > button").addEventListener('click', updateMovie);
}

/**
 * function with all javascript running on the movie add page
 * @returns {Promise<void>}
 */
async function reserveringAddPage() {
    /**
     * function for posting movie to backend
     */
    async function addMovie() {
        // get movie form data
        const movieForm = new FormData(document.querySelector("body > div > div > div > form"));
        
            
        // post movie to backend
        const response = await chromelyRequest('/movies/add', 'POST', {
                'naam': movieForm.get('naam'),
                'email': movieForm.get('email'),
                'producten': movieForm.get('producten'),
               
            })
            
            // if response = 204: redirect to movie overview page
            if(response.getStatusCode() === 204){
                window.location.href = "/medewerkers/reservering.html"
            }
    }

    document.querySelector("body > div > div > div > form > div > button").addEventListener('click', addMovie);
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
    const movieForm = new FormData(document.querySelector("body > div > div > div > form"));
   
   
}