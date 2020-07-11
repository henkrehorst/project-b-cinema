class chromelyResponse {
    constructor(status, statusText, data) {
        this.status = status;
        this.statusText = statusText;

        try {
            this.data = JSON.parse(data)
        } catch (e) {
            this.data = "";
        }
    }

    getStatusCode() {
        return this.status;
    }

    getData() {
        return this.data;
    }

    getStatusText() {
        return this.statusText;
    }
}

async function chromelyRequest(route, method = 'GET', postData = {}) {
    return new Promise((resolve, reject) => {
        var request = {
            'method': method,
            'url': route,
            'parameters': null,
            'postData': postData
        };
        window.cefQuery({
            request: JSON.stringify(request),
            onSuccess: function (response) {
                let result = JSON.parse(JSON.parse(response).Data);

                resolve(new chromelyResponse(result.status, result.statusText, result.data));
            }, onFailure: function (err, msg) {
                reject(err);
            }
        });
    }).then(data => data);
}