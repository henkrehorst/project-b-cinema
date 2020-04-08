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
            let movieOverview = "";

            for (let key in movies) {
                let movie = movies[key];
                let movieItem = "<div class=\"col-md-3 col-sm-4\">\n" +
                    "            <a class='box_wrapper_a' href='./movie_detail.html?id="+ movie.id +"'><div class=\"box_wrapper\">\n" +
                    "                <img src=\"local://frontend/uploads/" + movie.coverImage + "\" alt=\"" + movie.title + "\">\n" +
                    "                <p>" + movie.title + "</p>\n" +
                    "                <button>Meer info</button>\n" +
                    "            </div></a>\n" +
                    "        </div>";
                
                movieOverview += movieItem;
            }
            document.querySelector("body > main > div").innerHTML = movieOverview;
        }, onFailure: function (err, msg) {
            document.querySelector("body > main > div > p").innerHTML = "Geen films gevonden!";
        }
    });
}

loadOverview();