﻿/**
 * function with all javascript running on the screentime overview page
 * @returns {Promise<void>}
 */
async function screentimeOverviewPage() {
    // get all screentimes
    const screenTimesResponse = await chromelyRequest('/screentime');
    let screenTimes = screenTimesResponse.getData();
    let screenTimeTable = "";
    
    // create a table with screentimes
    for(key in screenTimes){
        //get movie by screentime
        let movieResponse = await chromelyRequest('/movies#id','POST',{'id': screenTimes[key].movie});
        let movie = movieResponse.getData();
            
        screenTimeTable += `<tr>
                <td>${movie.title}</td>
                <td>${new Date(screenTimes[key].startTime).toLocaleString()}</td>
                <td>${new Date(screenTimes[key].endTime).toLocaleString()}</td>
                <td><a href='/admin/screentime_edit.html?id=${screenTimes[key].Id}'>Edit</a></td>
            </tr>`;       
    }
    
    //display table
    document.querySelector("body > div > div > div > table > tbody").innerHTML += screenTimeTable;
}

/**
 * function with all javascript running on the screentime edit page
 * @returns {Promise<void>}
 */
async function screentimeEditPage() {
    //prepare movie dropdown
    await fillMovieDropdown();
    //get screen time by id
    let screenTimeResponse = await chromelyRequest('/screentime#id','POST',{'id': getIdFromUrl()});
    let screenTime = screenTimeResponse.getData();
    
    //make current item active in movie dropdown
    let movieDropdown = document.querySelector("#movies_field");
    for(key in movieDropdown.options){
        if(movieDropdown.options[key].value === screenTime.movie.toString()){
            movieDropdown.options[key].selected = true;
        }
    }
    
    document.querySelector("#start_time").value = screenTime.startTime;
    document.querySelector("#end_time").value = screenTime.endTime;

    /**
     * function for updating screentime in backend
     */
    async function updateScreenTime() {
        // get screentime form data
        const screenTimeForm = new FormData(document.querySelector("body > div > div > div > form"));

        // post screentime to backend
        const response = await chromelyRequest('/screentime#update', 'POST', {
            'id': getIdFromUrl(),
            'movie_id': screenTimeForm.get('movie'),
            'start_time': screenTimeForm.get('start_time'),
            'end_time': screenTimeForm.get('end_time')
        })
        
        if(response.getStatusCode() === 204){
            window.location.href = "/admin/screentime.html";
        }
    }

    document.querySelector("body > div > div > div > form > div:nth-child(4) > button").addEventListener('click', updateScreenTime);
}

/**
 * function with all javascript running on the screentime add page
 * @returns {Promise<void>}
 */
async function screentimeAddPage() {
    //prepare movie dropdown
    await fillMovieDropdown();

    /**
     * function for posting screentime to backend
     */
    async function addScreenTime() {
        // get screentime form data
        const screenTimeForm = new FormData(document.querySelector("body > div > div > div > form"));
        
        // post screentime to backend
        const response = await chromelyRequest('/screentime/add', 'POST', {
            'movie_id': screenTimeForm.get('movie'),
            'start_time': screenTimeForm.get('start_time'),
            'end_time': screenTimeForm.get('end_time')
        })
        
        if(response.getStatusCode() === 204){
            window.location.href = "/admin/screentime.html";
        }
    }
    
    document.querySelector("body > div > div > div > form > div:nth-child(4) > button").addEventListener('click', addScreenTime);
}


/**
 * fill movie dropdown with movies
 * @returns {Promise<void>}
 */
async function fillMovieDropdown() {
    let moviesResponse = await chromelyRequest('/movies');
    let movies = moviesResponse.getData();
    for(let movie in movies){
        document.querySelector("#movies_field").innerHTML += 
            `<option value="${movies[movie].Id}">${movies[movie].title}</option>`;
    }
}