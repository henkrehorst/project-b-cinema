document.getElementById("getorder").addEventListener("click", () => {
    let code = document.getElementById("code-field").value;
    let res = await chromelyRequest("/order#fetch", "POST", { "code": code });
    if (res.getStatusCode == 200) {
        displayOrderState(res.getData());
    } else if (res.getStatusCode == 204) {
        //Display error message
        Console.log(res.getStatusText());
    } else {
        Console.log(res.getStatusCode(), res.getStatusText());
    }
})

function displayOrderState(data) {
    //display order
    console.log(JSON.stringify(data));
}