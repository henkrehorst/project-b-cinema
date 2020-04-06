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

document.querySelector("li > button[id='movies']").addEventListener('click', () => {
    console.log("movies_start");
    togglepage("div.page[id='movies']");
    console.log("movies_end");
});
document.querySelector("li > button[id='add_movie']").addEventListener('click', () => {
    console.log("add_movie_start");
    togglepage("div.page[id='add_movie']");
    console.log("add_movie_end");
});