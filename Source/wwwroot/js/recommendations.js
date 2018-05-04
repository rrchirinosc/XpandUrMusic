var Recommendations = function () {

    // Decade box clicked on
    $('.decade-box').click(function () {
        clickedDecade();
    });

    function clickedDecade() {
        if ($(this).hasClass("decade-selected") === true)
            $(this).removeClass("decade-selected");
        else {
            $(this).addClass("decade-selected");
        }
        displayAlbumsFromDecade($(this).attr("data-year"), $(this).hasClass("decade-selected"));
    }


    function displayAlbumsFromDecade(givenYear, show) {

        var year = parseInt(givenYear);
        var albums = $("#albums-display").children(".album-container");
        for (var i = 0; i < albums.length; i++) {
            var id = parseInt($(albums[i]).attr("data-year"));
            
            if (id >= year && id <= year + 9) {
                show === true ? $(albums[i]).show(): $(albums[i]).hide();
                continue;
            }
            
        }
    }

    // Make available decades stored as the LSB of a binary
    // number where bit 7 correspond to the 1950's and bit 0 the 2010's
    function getAvailableDecades() {

        var decades = 0;
        var albums = $("#albums-display").children(".album-container");

        for (var i = 0; i < albums.length; i++) {
            var id = $(albums[i]).attr("data-year");
            if (id < 1950) continue;

            if (id >= 1950 && id <= 1959)
                decades |= 64;
            else if (id >= 1960 && id <= 1969)
                decades |= 32;
            else if (id >= 1970 && id <= 1979)
                decades |= 16;
            else if (id >= 1980 && id <= 1989)
                decades |= 8;
            else if (id >= 1990 && id <= 1999)
                decades |= 4;
            else if (id >= 2000 && id <= 2009)
                decades |= 2;
            else
                decades |= 1;
        }

        return decades;
    }

    function setAvailableDecades() {

        var decades = getAvailableDecades();
        if (decades === 0)
            return;

        var decadeBoxes = $("#album-filter-items").children(".decade-box");

        // go through all decades boxes mark as selected the ones that have recommended albums 
        for (var i = 0; i < decadeBoxes.length; i++) {
            var id = $(decadeBoxes[i]).attr("data-year");
            if (id >= 1950 && id <= 1959 && !(decades & 64))
                continue;
            else if (id >= 1960 && id <= 1969 && !(decades & 32))
                continue;
            else if (id >= 1970 && id <= 1979 && !(decades & 16))
                continue;
            else if (id >= 1980 && id <= 1989 && !(decades & 8))
                continue;
            else if (id >= 1990 && id <= 1999 && !(decades & 4))
                continue;
            else if (id >= 2000 && id <= 2009 && !(decades & 2))
                continue;
            else if (!(decades & 1))
                continue;

            $(decadeBoxes[i]).addClass("decade-selected");
            $(decadeBoxes[i]).bind("click", clickedDecade);
        }
    }

    setAvailableDecades(); 
};


$(document).ready(function () {
    new Recommendations();
});