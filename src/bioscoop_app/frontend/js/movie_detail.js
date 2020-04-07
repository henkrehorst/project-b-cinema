function getIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('id');
}

function displayMovie() {
    var request = {
        'method': 'POST',
        'url': '/movies#id',
        'parameters': null,
        'postData': {'id': getIdFromUrl()}
    };
    window.cefQuery({
        request: JSON.stringify(request),
        onSuccess: function (response) {
            let movie = JSON.parse(JSON.parse(response).Data);
            console.log(movie);
            document.querySelector("body > div > div > div.col-md-3").innerHTML = 
                "<img class='cover_image' src='local://frontend/uploads/"+ movie.coverImage +"' alt='"+ movie.title +"'/>";
            document.querySelector("body > div > div > div.col-md-9").innerHTML = 
                "<h1>"+ movie.title +"</h1>" +
                "<table>" +
                "<tr><th>Genre</th><td>"+ movie.genre +"</td></tr>" +
                "<tr><th>Duur</th><td>"+ movie.duration +"</td></tr>" +
                "<tr><th>Rating</th><td>"+ movie.rating +"</td></tr>" +
                "</table>";
        }, onFailure: function (err, msg) {
            console.log(err);
        }
    });
}

displayMovie();

console.log(getIdFromUrl());