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

function update_screen_time() {
    let screenTimeForm = new FormData(document.querySelector("body > div > div > div > form"));
    let request = {
        'method': 'POST',
        'url': '/screentime#update',
        'parameters': null,
        'postData': {
            'id': getIdFromUrl(),
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

document.querySelector("body > div > div > div > form > div:nth-child(4) > button").addEventListener('click', update_screen_time);

function getIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('id');
}

function fillEditForm() {
    var request = {
        'method': 'POST',
        'url': '/screentime#id',
        'parameters': null,
        'postData': {'id': getIdFromUrl()}
    };
    window.cefQuery({
        request: JSON.stringify(request),
        onSuccess: function (response) {
            let screenTime = JSON.parse(JSON.parse(response).Data);
            //fill field in form
            let movieDropdown = document.querySelector("#movies_field");
            movieDropdown.options[screenTime.movie].selected = true;
            document.querySelector("#start_time").value = screenTime.startTime;
            document.querySelector("#end_time").value = screenTime.endTime;
            
        }, onFailure: function (err, msg) {
            console.log(err);
        }
    });
}

fillEditForm();
