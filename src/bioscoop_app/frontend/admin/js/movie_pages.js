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
    for (movie in movies) {
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
    const movieResponse = await chromelyRequest('/movies#id', 'POST', {'id': getIdFromUrl()});
    let movie = movieResponse.getData();

    //fill form with movie data
    document.getElementById('title').value = movie.title;
    document.getElementById('genre').value = movie.genre;
    document.getElementById('duration').value = movie.duration;
    document.getElementById('rating').value = movie.rating;
    document.getElementById('samenvatting').value = movie.samenvatting;
    //show preview cover image
    document.getElementById('cover_preview').innerHTML +=
        `<img src="local://frontend/uploads/${movie.coverImage}" alt="cover image" />`
    //show preview thumbnail image
    document.getElementById('thumbnail_preview').innerHTML +=
        `<img src="local://frontend/uploads/${movie.thumbnailImage}" alt="thumbnail image" />`

    /**
     * function for posting movie to backend
     */
    async function updateMovie() {
        // get movie form data
        const movieForm = new FormData(document.querySelector("body > div > div > div > form"));
        //convert cover image to string
        let cover_image = await getBase64String(movieForm.get('cover_image'))
        //convert thumbnail image to string
        let thumbnail = await getBase64String(movieForm.get('thumbnail_image'));
        //get values of checked kijkwijzers
        let kijkwijzers = Object.values(document.querySelectorAll("#kijkwijzerFields > li > input:checked"))
            .map(item => {return parseInt(item.value)})

        // post movie to backend
        const response = await chromelyRequest('/movies#update', 'POST', {
            'id': getIdFromUrl(),
            'title': movieForm.get('title'),
            'duration': movieForm.get('duration'),
            'genre': movieForm.get('genre'),
            'rating': movieForm.get('rating'),
            'samenvatting': movieForm.get('samenvatting'),
            'cover_image': cover_image,
            'kijkwijzers': kijkwijzers,
            'thumbnail_image': thumbnail
        })

        // if response = 200: redirect to movie overview page
        if (response.getStatusCode() === 200) {
            window.location.href = "/admin/movie.html"
        }
    }

    document.querySelector("body > div > div > div > form > div > button").addEventListener('click', updateMovie);
    
    if(movie.kijkwijzer === null) movie.kijkwijzer = [];

    //fill kijkwijzers fields
    let kijkwijzersFields = "";
    Object.values(await getKijkwijzers()).map((item, key) => {
        kijkwijzersFields +=
            `<li>
               <input type="checkbox" id="kijkwijzer${key}" value="${item.Id}"
               ${movie.kijkwijzer.includes(item.Id) ? 'checked': ''} />
               <label for="kijkwijzer${key}"><img src="./../assets/kijkwijzers/${item.symbool}" />
               </label>
            </li>`;
    })
    //display kijkwijzers
    document.getElementById('kijkwijzerFields').innerHTML = kijkwijzersFields;
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
        //convert cover image to string
        let cover_image = await getBase64String(movieForm.get('cover_image'));
        //convert thumbnail image to string
        let thumbnail = await getBase64String(movieForm.get('thumbnail_image'));
        //get values of checked kijkwijzers
        let kijkwijzers = Object.values(document.querySelectorAll("#kijkwijzerFields > li > input:checked"))
            .map(item => {return parseInt(item.value)})

        // post movie to backend
        const response = await chromelyRequest('/movies/add', 'POST', {
            'title': movieForm.get('title'),
            'duration': movieForm.get('duration'),
            'genre': movieForm.get('genre'),
            'rating': movieForm.get('rating'),
            'samenvatting': movieForm.get('samenvatting'),
            'cover_image': cover_image,
            'kijkwijzers': kijkwijzers,
            'thumbnail_image': thumbnail
        })

        // if response = 204: redirect to movie overview page
        if (response.getStatusCode() === 204) {
            window.location.href = "/admin/movie.html"
        }
    }

    document.querySelector("body > div > div > div > form > div > button").addEventListener('click', addMovie);
    
    
    //fill kijkwijzers fields
    let kijkwijzersFields = "";
    Object.values(await getKijkwijzers()).map((item, key) => {
        kijkwijzersFields += 
            `<li>
               <input type="checkbox" id="kijkwijzer${key}" value="${item.Id}"/>
               <label for="kijkwijzer${key}"><img src="./../assets/kijkwijzers/${item.symbool}" /></label>
            </li>`;
    })
    //display kijkwijzers
    document.getElementById('kijkwijzerFields').innerHTML = kijkwijzersFields;
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
            if (reader.result === "data:") {
                resolve("")
            } else {
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

    document.getElementById('cover_preview').innerHTML =
        `<p>Cover image preview</p>
         <img src="${cover_image}" alt="cover image" />`;
}


/**
 * show a preview of the uploaded thumbnail image
 * @returns {Promise<void>}
 */
async function thumbnailImagePreview() {
    // get movie form data
    const movieForm = new FormData(document.querySelector("body > div > div > div > form"));
    //convert thumbnail image to string
    let thumbnail = await getBase64String(movieForm.get('thumbnail_image'));

    document.getElementById('thumbnail_preview').innerHTML =
        `<p>Cover image preview</p>
         <img src="${thumbnail}" alt="cover image" />`;
}

/**
 * get array of kijkwijzers from the backend
 */
async function getKijkwijzers() {
    const KijkwijzerResponse = await chromelyRequest('/kijkwijzer');
    return KijkwijzerResponse.getData();
}