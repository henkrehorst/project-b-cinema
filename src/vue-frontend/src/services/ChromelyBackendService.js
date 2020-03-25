class ChromelyBackendService {
  // get data from chromely backend
  getData(route = "") {
    return new Promise((resolve => {
      let request = {
        "method": "GET",
        "url": route,
        "parameters": null,
        "postData": null
      };
      window.cefQuery({
        request: JSON.stringify(request),
        onSuccess: function (response) {
          console.log(response);
          resolve(JSON.parse(response))
        }, onFailure: function (err, msg) {
          console.log(err, msg);
        }
      });
    }));
  }

  // post data to chromely backend
  postData(route = "", data = {}) {
    console.log(data);
    return new Promise((resolve => {
      let request = {
        "method": "POST",
        "url": route,
        "parameters": null,
        "postData": data
      };
      window.cefQuery({
        request: JSON.stringify(request),
        onSuccess: function (response) {
          console.log(response);
          resolve(JSON.parse(response))
        }, onFailure: function (err, msg) {
          console.log(err, msg);
        }
      });
    }));
  }
}

export const {getData, postData} = new ChromelyBackendService();
