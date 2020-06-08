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

    //display movie information
    document.getElementById("movieTitle").innerHTML += `${movie.title}`;
    document.getElementById("movieDescription").innerHTML += `${movie.samenvatting}`;
    document.getElementById("coverImage").src = `local://frontend/uploads/${movie.coverImage}`;
    document.getElementById("movieThumbnail").src = `local://frontend/uploads/${movie.thumbnailImage}`;
    document.getElementById("movieDuration").innerHTML += `${movie.duration} minuten`;

    //display genre labels
    let labels = ""
    movie.genre.split(" ").map(item => {
        labels += `<span class="genre-label">${item}</span>`;
    });
    document.getElementById("genreView").innerHTML = labels;

    //display star rating
    let rating = movie.rating;
    let ratingContainer = document.getElementById("ratingView");
    for (let i = 5; i > 0; i--) {
        let img = document.createElement("img");
        let selector = 10;
        if (rating < 1 && rating > 0) {
            selector = (10 * rating).toFixed();
        }
        else if (rating <= 0) {
            selector = 0;
        }
        img.src = `/assets/img/rating/${selector}.png`;
        img.className = "star-rating";
        ratingContainer.appendChild(img);
        --rating;
    }

    //display movie screentimes
    const screenTimesResponse = await chromelyRequest('/screentime#movie', 'POST', {id: getIdFromUrl()});
    let screenTimes = screenTimesResponse.getData();

    if (Object.keys(screenTimes).length === 0) {
        document.querySelector("body > div > div:nth-child(2) > div").innerHTML += "<p>Geen tijden gevonden</p>";
    } else {
        //create screentime table
        let screentimeTable = "";

        //change time format to nl
        moment.locale('nl-nl');

        for (time in screenTimes) {
            screentimeTable += `<tr>
                <td>${moment(new Date(screenTimes[time].startTime)).format('dddd d MMMM yyyy')}</td>
                <td>${moment(new Date(screenTimes[time].startTime)).format('LT')} - ${moment(new Date(screenTimes[time].endTime)).format('LT')}</td>
                <td><a class="btn-block btn-success btn" href="/reservation_step_one.html?id=${screenTimes[time].Id}">Reserveren</a></td>
            </tr>`;
        }

        //display screentime table
        document.getElementById("screentimePlace").innerHTML = screentimeTable;
    }

    //display kijkwijzers
    let kijkwijzersDiv = "";
    if (movie.kijkwijzer === null) movie.kijkwijzer = [];

    //create kijkerwijzer items
    Object.values(await getKijkwijzers()).map(item => {
        if (movie.kijkwijzer.includes(item.Id)) {
            kijkwijzersDiv +=
                `<img src='/assets/kijkwijzers/${item.symbool}' alt='kijkwijzer'/>`;
        }
    })
    //show kijkwijzers on page
    document.getElementById('kijkwijzerView').innerHTML = kijkwijzersDiv;
}

/**
 * get array of kijkwijzers from the backend
 */
async function getKijkwijzers() {
    const KijkwijzerResponse = await chromelyRequest('/kijkwijzer');
    return KijkwijzerResponse.getData();
}