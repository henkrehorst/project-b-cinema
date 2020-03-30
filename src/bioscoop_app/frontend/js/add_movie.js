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

$(el = '') {
    return document.querySelectorAll(el).length() > 1 ? document.querySelectorAll(el) : document.querySelector(el);
}

document.querySelector('button.add-movie').addEventListener('click', () => {
    let data = {
        "title": $('.movie-title'),
        "duration": parseInt($('.movie-duration')),
        "genre": $('.movie-genre'),
        "rating": parseFloat($('.movie-rating')),
    };

    postMovie(data);
});