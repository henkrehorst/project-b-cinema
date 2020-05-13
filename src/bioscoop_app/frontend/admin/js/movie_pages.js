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
                <td>${movies[movie].title}</td>
                <td>${movies[movie].genre}</td>
                <td>${movies[movie].duration}</td>
                <td>${movies[movie].rating}</td>
                 <td>${movies[movie].samenvatting}</td>
                <td><a href='/admin/movie_edit.html?id=${movies[movie].Id}'>Edit</a></td>
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
    document.getElementById('title').value = movie.title;
    document.getElementById('genre').value = movie.genre;
    document.getElementById('duration').value = movie.duration;
    document.getElementById('rating').value = movie.rating;
     document.getElementById('samenvatting').value = movie.samenvatting;
    //show preview cover image
    document.querySelector("body > div > div > .cover-image-preview").innerHTML +=
        `<img src="local://frontend/uploads/${movie.coverImage}" alt="cover image" />`
    
    /**
     * function for posting movie to backend
     */
    async function updateMovie() {
        // get movie form data
        const movieForm = new FormData(document.querySelector("body > div > div > div > form"));
        //covert cover image to string
        let cover_image = await getBase64String(movieForm.get('cover_image'))

        // post movie to backend
        const response = await chromelyRequest('/movies#update', 'POST', {
            'id': getIdFromUrl(),
            'title': movieForm.get('title'),
            'duration': movieForm.get('duration'),
            'genre': movieForm.get('genre'),
            'rating': movieForm.get('rating'),
            'samenvatting': movieForm.get('samenvatting'),
            'cover_image': cover_image
        })

        // if response = 200: redirect to movie overview page
        if(response.getStatusCode() === 200){
            window.location.href = "/admin/movie.html"
        }
    }

    document.querySelector("body > div > div > div > form > div > button").addEventListener('click', updateMovie);
}

/**
 * function with all javascript running on the movie add page
 * @returns {Promise<void>}
 */
async function movieAddPage() {
    /**
     * function for posting movie to backend
     */
    async function addMovie() {
        // get movie form data
        const movieForm = new FormData(document.querySelector("body > div > div > div > form"));
        //covert cover image to string
        let cover_image = await getBase64String(movieForm.get('cover_image'))
            
        // post movie to backend
        const response = await chromelyRequest('/movies/add', 'POST', {
                'title': movieForm.get('title'),
                'duration': movieForm.get('duration'),
                'genre': movieForm.get('genre'),
                'rating': movieForm.get('rating'),
                'samenvatting': movieForm.get('samenvatting'),

                'cover_image': cover_image
            })
            
            // if response = 204: redirect to movie overview page
            if(response.getStatusCode() === 204){
                window.location.href = "/admin/movie.html"
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
    // convert cover image to base64
    let cover_image = await getBase64String(movieForm.get('cover_image'))
    
    document.querySelector("body > div > div > .cover-image-preview").innerHTML =
        `<p>Cover image preview</p>
         <img src="${cover_image}" alt="cover image" />`;
}