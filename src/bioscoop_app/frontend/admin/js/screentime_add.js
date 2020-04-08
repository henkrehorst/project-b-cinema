function fillSelect() {
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
                document.querySelector("#movies_field").innerHTML += '<option value="' + movies[key].id + '">' + movies[key].title + '</option>';
            }
        }, onFailure: function (err, msg) {
            console.log(err, msg);
        }
    });
}

fillSelect();

function add_screen_time() {
    let screenTimeForm = new FormData(document.querySelector("body > div > div > div > form"));
    let request = {
        'method': 'POST',
        'url': '/screentime/add',
        'parameters': null,
        'postData': {
            'movie_id': screenTimeForm.get('movie'),
            'start_time': screenTimeForm.get('start_time'),
            'end_time': screenTimeForm.get('end_time')
        }
    };
    window.cefQuery({
        request: JSON.stringify(request),
        onSuccess: function (response) {
            window.location.href = "/admin/screentime.html";
        },
        onFailure: function (err, msg) {
            console.log(err, msg);
        }
    })

}

document.querySelector("body > div > div > div > form > div:nth-child(4) > button").addEventListener('click', add_screen_time);