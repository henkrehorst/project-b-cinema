async function getMovieTitle(movie_id) {
    var request = {
        'method': 'POST',
        'url': '/movies#id',
        'parameters': null,
        'postData': {'id': movie_id}
    };
    return new Promise(((resolve, reject) => {
        window.cefQuery({
            request: JSON.stringify(request),
            onSuccess: function (response) {
                let movie = JSON.parse(JSON.parse(response).Data);
                resolve(movie.title);
            }, onFailure: function (err, msg) {
                reject(err);
            }
        })
    })).then(result => result);
}


async function getScreenTimes() {
    var request = {
        'method': 'GET',
        'url': '/screentime',
        'parameters': null,
        'postData': null
    };
    window.cefQuery({
        request: JSON.stringify(request),
        onSuccess: async function (response) {
            let times = JSON.parse(JSON.parse(response).Data);
            console.log(times);
            for (key in times) {
                let title = await getMovieTitle(times[key].movie);
                let row = "<tr>" +
                    "<td>" + title +"</td>" +
                    "<td>" + new Date(times[key].startTime).toLocaleString() + "</td>" +
                    "<td>" + new Date(times[key].endTime).toLocaleString() + "</td>" +
                    "<td><a href='/admin/screentime_edit.html?id="+ times[key].id +"'>Edit</a></td>" +
                    "</tr>";

                document.querySelector("body > div > div > div > table > tbody").innerHTML += row;
            }

        }, onFailure: function (err, msg) {
            console.log(err, msg);
        }
    });
}

getScreenTimes();
