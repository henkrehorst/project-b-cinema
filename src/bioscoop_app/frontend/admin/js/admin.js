function togglepage(selector) {
    switch (document.querySelector(selector).style.display) {
        case "block":
            document.querySelector(selector).style.display = "none";
            break;
        case "none":
            document.querySelector(selector).style.display = "block";
            break;
    }
}

document.querySelector("li > a[id='movies']").addEventListener('click', () => {
    togglepage("div.page[id='movies']");
});
document.querySelector("li > a[id='add_movie']").addEventListener('click', () => {
    togglepage("div.page[id='add_movie']");
});