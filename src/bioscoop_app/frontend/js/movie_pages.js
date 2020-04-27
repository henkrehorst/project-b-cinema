/**
 * function with all javascript running on the movie overview page
 * @returns {Promise<void>}
 */
async function movieOverviewPage() {
    //get all movies
    const moviesResponse = await chromelyRequest('/movies');
    let movies = moviesResponse.getData();
    let movieOverview = "";

    //convert movies array to html
    for (let movie in movies) {
        movieOverview +=
            `<div class=\"col-md-3 col-sm-4\">
                <a class='box_wrapper_a' href="./movie_detail.html?id=${movies[movie].Id}"><div class="box_wrapper">
                    <img src="local://frontend/uploads/${movies[movie].coverImage}"  alt="${movies[movie].title}">
                    <p>${movies[movie].title}</p>
                    <button>Meer info</button>
                    </div>
                </a>
            </div>`;
    }
    //display movie overview html
    document.querySelector("body > main > div").innerHTML = movieOverview;
}


/**
 * function with all javascript running on the movie detail page
 * @returns {Promise<void>}
 */
async function movieDetailPage() {
    //get movie by id
    const movieResponse = await chromelyRequest('/movies#id', 'POST', {'id': getIdFromUrl()});
    let movie = movieResponse.getData();

    //display cover image of movie
    document.querySelector("body > div > div > div.col-md-3").innerHTML =
        `<img class='cover_image' src='local://frontend/uploads/${movie.coverImage}' alt='${movie.title}'/>`;

    //display movie information
    document.querySelector("body > div > div > div.col-md-9").innerHTML = `<h1>${movie.title}</h1>
        <table class='table'>
                <tr><th>Genre</th><td>${movie.genre}</td></tr>
                <tr><th>Duur</th><td>${movie.duration}</td></tr>
                <tr><th>Rating</th><td>${movie.rating}</td></tr>
        </table>`;

    //display movie screentimes
    const screenTimesResponse = await chromelyRequest('/screentime#movie', 'POST', {id: getIdFromUrl()});
    let screenTimes = screenTimesResponse.getData();

    if (Object.keys(screenTimes).length === 0) {
        document.querySelector("body > div > div:nth-child(2) > div").innerHTML += "<p>Geen tijden gevonden</p>";
    } else {
        //create screentime table
        let screentimeTable = "<table class='table'><tr><th>Start tijd</th><th>Eind tijd</th><th></th></tr>";
        
        for(time in screenTimes){
            screentimeTable += `<tr>
                <td>${new Date(screenTimes[time].startTime).toLocaleString()}</td>
                <td>${new Date(screenTimes[time].startTime).toLocaleString()}</td>
                <td><a href="#">Reserveren</a></td>
            </tr>`;
        }
        //close table
        screentimeTable += "</table>"
        
        //display screentime table
        document.querySelector("body > div > div:nth-child(2) > div").innerHTML = screentimeTable;
    }
}