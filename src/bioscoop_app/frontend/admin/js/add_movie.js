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
    // convert image file to base 64 image
    getBase64String($('input[name="movie-cover-image"]').files[0]).then(function (fileString) {
        let data = {
            'title': $('input[name="movie-title"]').value,
            'duration': parseInt($('input[name="movie-duration"]').value),
            'genre': $('input[name="movie-genre"]').value,
            'rating': parseFloat($('input[name="movie-rating"]').value),
            'cover_image': fileString
        };

        console.log(data);

        postMovie(data);
    });
});


function getBase64String(file) {
    return new Promise((resolve, reject) => {
        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            resolve(reader.result);
        };
        reader.error = function (error) {
            reject(error);
        };
    });
}

