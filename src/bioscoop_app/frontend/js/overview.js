function loadOverview() {
    var request = {
        'method': 'GET',
        'url': '/movies',
        'parameters': null,
        'postData': null
    };
    window.cefQuery({
        request: JSON.stringify(request),
        onSuccess: function (response) {
            let movies = JSON.parse(JSON.parse(response).Data);

            for (let key in movies) {
                let movie = movies[key];
                let title = '<td data-id="title">' + movie.title + '</td>';
                let genre = '<td data-id="genre">' + movie.genre + '</td>';
                let rating = '<td data-id="rating">' + movie.rating + '</td>';

                document.querySelector('tbody').innerHTML += '<tr>' + title + genre + rating + '</tr>';
            }
        }, onFailure: function (err, msg) {
            console.log(err, msg);
        }
    });
}

loadOverview();