document.querySelector("button.submit[id='update_movie'").addEventListener('click', () => {
    let files = document.querySelector("div.form[name='update_movie'] > input[name='cover-image']").files;
    let data = {
        'id': document.querySelector("button.submit[id='update_movie']").data_update_id,
        'title': document.querySelector("div.form[name='update_movie'] > input[name='title']").value,
        'duration': document.querySelector("div.form[name='update_movie'] > input[name='duration']").value,
        'genre': document.querySelector("div.form[name='update_movie'] > input[name='genre']").value,
        'rating': document.querySelector("div.form[name='update_movie'] > input[name='rating']").value,
        'filestring': null,
        'filename': document.querySelector("div.form[name='update_movie'] > input[name='cover-image']").data_existing
    };
    console.log(files.length);
    if (files.length > 0) {
        getBase64String(files[0]).then(function (filestring) {
            console.log("B64 Promise");
            data['filestring'] = filestring;
            postUpdate(data);
        });
    } else {
        postUpdate(data);
    } 
});

function postUpdate(data) {
    let req = {
        'method': 'POST',
        'url': '/movies#update',
        'parameters': null,
        'postData': data
    }
    console.log("data assigned");
    window.cefQuery({
        request: JSON.stringify(req),
        onSuccess: function (res) {
            console.log("response from server:");
            console.log(JSON.parse(res).Data);
            document.querySelector("div.page[id='update_movie']").style.display = "none";
        },
        onFailure: function (err, msg) {
            console.log(err, msg);
        }
    });
    console.log("cefquery executed");
}