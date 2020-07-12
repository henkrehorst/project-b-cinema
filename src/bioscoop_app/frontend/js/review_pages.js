/**
 * function with all javascript running on the review add page
 * @returns {Promise<void>}
 */
async function reviewAddPage() {
    //prepare movie dropdown
    await fillMovieDropdown();
        console.log("test1");

    /**
     * function for posting review to backend
     */
        async function addReview() {

        // get review form data
            console.log("test2");
        const reviewForm = new FormData(document.querySelector("body > div > div > div > form"));
            console.log("test3");
            console.log(reviewForm.get('movie') + reviewForm.get('rating') + reviewForm.get('mening'));
        // post review to backend
        const response = await chromelyRequest('/review/add', 'POST', {
            'movie_id': reviewForm.get('movie'),
            'rating': reviewForm.get('rating'),
            'mening': reviewForm.get('mening')
        })
            console.log("test4");

        if(response.getStatusCode() === 204){
            window.location.href = "/index.html";
        }
        console.log(response);
        // display error message by 400
        if(response.getStatusCode() === 400){
            response.data.map(error => {
                displayFieldErrorMessage(error.PropertyName, error.ErrorMessage);
            })
        }
    }
    console.log("test5");
    document.querySelector("body > div > div > div > form > div:nth-child(4) > button").addEventListener('click', addReview);
        console.log("test6");

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