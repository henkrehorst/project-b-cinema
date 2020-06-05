/**
 * Eventlistener to cancel the order by the input code.
 */
document.getElementById("cancel").addEventListener("click", async () => {
    let code = document.getElementById("code-field").value;
    let res = await chromelyRequest("/order#cancel", "POST", { "code": code });
    if (res.getStatusCode() === 200) {
        console.log("cancel order 200");
    } else if (res.getStatusCode() === 204) {
        //Display error message
        console.log(res.getStatusText());
    } else {
        console.log(res.getStatusCode(), res.getStatusText());
    }
});