function postMovie(data) {
    let request = {
        'method': 'POST',
        'url': '/movies/add',
        'parameters': null,
        'postData': data
    };
    window.cefQuery({
        request: JSON.stringify(request),
        onSuccess: function (response) {
            console.log(response);
        }, onFailure: function (err, msg) {
            console.log(err, msg);
        }
    });
}

function $(el = '') {
    return document.querySelectorAll(el).length > 1 ? document.querySelectorAll(el) : document.querySelector(el);
}

document.querySelector('button.add-movie').addEventListener('click', () => {
    let data = {
        'title': $('input[name="movie-title"]').value,
        'duration': parseInt($('input[name="movie-duration"]').value),
        'genre': $('input[name="movie-genre"]').value,
        'rating': parseFloat($('input[name="movie-rating"]').value),
    };

    console.log(data);

    postMovie(data);
});