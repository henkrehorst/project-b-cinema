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
            const edit = "<td><button name='edit'>Edit</button></td>";

            for (let key in movies) {
                let movie = movies[key];
                let title = '<td data-id="title">' + movie.title + '</td>';
                let duration = '<td data-id="duration">' + movie.duration + '</td>';
                let genre = '<td data-id="genre">' + movie.genre + '</td>';
                let rating = '<td data-id="rating">' + movie.rating + '</td>';

                document.querySelector('#movies > tbody').innerHTML += '<tr id="' + key + '">' + title + duration + genre + rating + edit + '</tr>';
                console.log("#movies > tbody > tr[id='" + key + "'] > td > button[name='edit']");
                console.log(document.querySelector("#movies > tbody > tr[id='" + key + "'] > td > button[name='edit']"));
                document.querySelector("#movies > tbody > tr[id='" + key + "'] > td > button[name='edit']").addEventListener('click', () => {
                    editMovieButton(key);
                });
                console.log("finished " + key);
            }
        }, onFailure: function (err, msg) {
            console.log(err, msg);
        }
    });
}

function editMovieButton(id) {
    document.querySelector("button.submit[id='update_movie']").data_update_id = id;
    let data = getMovieById(id);
    document.querySelector("div.form[name='update_movie'] > input[name='title']").value = data.title;
    document.querySelector("div.form[name='update_movie'] > input[name='duration']").value = data.duration;
    document.querySelector("div.form[name='update_movie'] > input[name='genre']").value = data.genre;
    document.querySelector("div.form[name='update_movie'] > input[name='rating']").value = data.rating;
    document.querySelector("div.page[id='update_movie']").style.display = "block";
}

function getMovieById(id) {
    let data;

    let req = {
        'method': 'POST',
        'url': '/movies#id',
        'parameters': null,
        'postData': { 'id': id }
    }
    window.cefQuery({
        request: JSON.stringify(req),
        onSuccess: function (res) {
            console.log(JSON.parse(res));
            data = JSON.parse(JSON.parse(res).Data);
        },
        onFailure: function (err, msg) {
            console.log(err, msg);
        }
    });

    return data;
}

window.onload = loadOverview();