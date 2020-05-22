let orderId;

/**
 * Eventlistener to fetch the order by the input code.
 */
document.getElementById("getorder").addEventListener("click", async () => {
    let code = document.getElementById("code-field").value;
    let res = await chromelyRequest("/order#fetch", "POST", { "code": code });
    if (res.getStatusCode() === 200) {
        console.log("getorder 200");
        orderId = res.getData().Id;
        displayOrderState(res.getData());
    } else if (res.getStatusCode() === 204) {
        //Display error message
        console.log(res.getStatusText());
    } else {
        console.log(res.getStatusCode(), res.getStatusText());
    }
});

function displayOrderState(data) {
    //display order
    console.log(data);
    createCancelOrderButton();
}

/*
 * Function that creates a cancel button for the order.
 */
function createCancelOrderButton() {
    let btn = document.createElement("button");
    btn.innerHTML = "Cancel order";
    btn.className = "cancel-button";
    btn.addEventListener("click", async () => {
        let res = await chromelyRequest("/order#cancel", "POST", { "id": orderId });
        if (res.getStatusCode() === 200) {
            window.location.href = "./index.html"
        } else if (res.getStatusCode() === 400) {
            console.log(res.getStatusText());
        } else {
            console.log(res.getStatusCode());
        }
    });
    document.getElementById("cancel_container").appendChild(btn);
}