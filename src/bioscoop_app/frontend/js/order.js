let orderId;

/**
 * Eventlistener to fetch the order by the input code.
 */
document.getElementById("getorder").addEventListener("click", async () => {
    document.getElementById('codeError').style.display = "none";
    let code = document.getElementById("code-field").value;
    let res = await chromelyRequest("/order#fetch", "POST", { "code": code });
    if (res.getStatusCode() === 200) {
        //Order find, redirect to reservation overview page
        localStorage.setItem("ordercode", res.getData().Id);
        window.location.href = "./reservation_overview.html?orderCode=" + code;
    } else if (res.getStatusCode() === 204) {
        //Display error message
        console.log(res.getStatusText());
        document.getElementById('codeError').style.display = "block";
    } else {
        console.log(res.getStatusCode(), res.getStatusText());
        document.getElementById('codeError').style.display = "block";
    }
});