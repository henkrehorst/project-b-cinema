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
            resolve(JSON.parse(response))
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
        "title": $('[name="movie-title"]'),
        "duration": parseInt($('[name=".movie-duration"]')),
        "genre": $('[name=".movie-genre"]'),
        "rating": parseFloat($('[name=".movie-rating"]')),
    };

    postMovie(data);
});