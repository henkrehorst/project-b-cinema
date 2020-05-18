let orderId;

/**
 * Eventlistener to fetch the order by the input code.
 */
document.getElementById("getorder").addEventListener("click", () => {
    let code = document.getElementById("code-field").value;
    let res = await chromelyRequest("/order#fetch", "POST", { "code": code });
    if (res.getStatusCode == 200) {
        orderId = res.getData().Id;
        displayOrderState(res.getData());
    } else if (res.getStatusCode == 204) {
        //Display error message
        Console.log(res.getStatusText());
    } else {
        Console.log(res.getStatusCode(), res.getStatusText());
    }
});

function displayOrderState(data) {
    //display order
    console.log(JSON.stringify(data));
    createCancelOrderButton();
}

/*
 * Function that creates a cancel button for the order.
 */
function createCancelOrderButton() {
    let btn = document.createElement("button");
    btn.innerHTML = "Cancel order";
    btn.className = "cancel-button";
    btn.addEventListener("click", () => {
        let res = await chromelyRequest("/order#cancel", "POST", orderId);
        if (res.getStatusCode() == 200) {
            Console.log("succes");
        } else if (res.getStatusCode() == 400) {
            Console.log(res.getStatusText());
        } else {
            Console.log(res.getStatusCode());
        }
    });
}