/**
 * function with all javascript running on the movie overview admin page
 * @returns {Promise<void>}
 */
async function moviesOverviewPage() {
    // get all movies
    const movies = await chromelyRequest('/movies');
    let movieTable = "";

    // create a table with screentimes
    for(movie in movies){
        movieTable += `<tr>
                <td>${movies[movie].title}</td>
                <td>${movies[movie].genre}</td>
                <td>${movies[movie].duration}</td>
                <td>${movies[movie].rating}</td>
                <td><a href='/admin/movie_edit.html?id=${movies[movie].Id}'>Edit</a></td>
            </tr>`;
    }

    //display table
    document.querySelector("body > div > div > div > table > tbody").innerHTML += movieTable;
}

/**
 * function with all javascript running on the screentime edit page
 * @returns {Promise<void>}
 */
async function screentimeEditPage() {
    //prepare movie dropdown
    await fillMovieDropdown();
    //get screen time by id
    let screenTime = await chromelyRequest('/screentime#id','POST',{'id': getIdFromUrl()})

    //file movie form
    let movieDropdown = document.querySelector("#movies_field");
    movieDropdown.options[screenTime.movie].selected = true;
    document.querySelector("#start_time").value = screenTime.startTime;
    document.querySelector("#end_time").value = screenTime.endTime;

    /**
     * function for updating screentime in backend
     */
    function updateScreenTime() {
        // get screentime form data
        const screenTimeForm = new FormData(document.querySelector("body > div > div > div > form"));

        // post screentime to backend
        chromelyRequest('/screentime#update', 'POST', {
            'id': getIdFromUrl(),
            'movie_id': screenTimeForm.get('movie'),
            'start_time': screenTimeForm.get('start_time'),
            'end_time': screenTimeForm.get('end_time')
        }).then(value => {
            return window.location.href = "/admin/screentime.html";
        })
    }

    document.querySelector("body > div > div > div > form > div:nth-child(4) > button").addEventListener('click', updateScreenTime);
}

/**
 * function with all javascript running on the movie add page
 * @returns {Promise<void>}
 */
async function movieAddPage() {
    /**
     * function for posting movie to backend
     */
    function addMovie() {
        // get movie form data
        const movieForm = new FormData(document.querySelector("body > div > div > div > form"));
        //covert cover image to string
        getBase64String(movieForm.get('cover_image')).then(cover_image => {
            
            // post movie to backend
            chromelyRequest('/movies/add', 'POST', {
                'title': movieForm.get('title'),
                'duration': movieForm.get('duration'),
                'genre': movieForm.get('genre'),
                'rating': movieForm.get('rating'),
                'cover_image': cover_image
            }).then(value => {
                return window.location.href = "/admin/movie.html";
            })
        });
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
            resolve(reader.result);
        };
        reader.error = function (error) {
            reject(error);
        };
    })
}