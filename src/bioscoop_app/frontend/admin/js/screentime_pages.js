/**
 * function with all javascript running on the screentime overview page
 * @returns {Promise<void>}
 */
async function screentimeOverviewPage() {
    // get all screentimes
    const screenTimes = await chromelyRequest('/screentime');
    let screenTimeTable = "";
    
    // create a table with screentimes
    for(key in screenTimes){
        //get movie by screentime
        let movie = await chromelyRequest('/movies#id','POST',{'id': screenTimes[key].movie});
        
        screenTimeTable += `<tr>
                <td>${movie.title}</td>
                <td>${new Date(screenTimes[key].startTime).toLocaleString()}</td>
                <td>${new Date(screenTimes[key].endTime).toLocaleString()}</td>
                <td><a href='/admin/screentime_edit.html?id=${screenTimes[key].id}'>Edit</a></td>
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
    function addScreenTime() {
        // get screentime form data
        const screenTimeForm = new FormData(document.querySelector("body > div > div > div > form"));
        
        // post screentime to backend
        chromelyRequest('/screentime/add', 'POST', {
            'movie_id': screenTimeForm.get('movie'),
            'start_time': screenTimeForm.get('start_time'),
            'end_time': screenTimeForm.get('end_time')
        }).then(value => {
            return window.location.href = "/admin/screentime.html";
        })
    }
    
    document.querySelector("body > div > div > div > form > div:nth-child(4) > button").addEventListener('click', addScreenTime);
}


/**
 * fill movie dropdown with movies
 * @returns {Promise<void>}
 */
async function fillMovieDropdown() {
    let movies = await chromelyRequest('/movies');
    for(let movie in movies){
        document.querySelector("#movies_field").innerHTML += 
            `<option value="${movies[movie].id}">${movies[movie].title}</option>`;
    }
}