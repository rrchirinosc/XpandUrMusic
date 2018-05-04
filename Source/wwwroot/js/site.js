// Write your JavaScript code.
var Index = function () {

    // Find button event
    $('#artist-find').click(function () {
        artistLookup();
    });

    //Check for enter key
    $('#artist-input').on('keydown', function (event) {
        var artistName = $('#artist-input').val();
        if (event.which === 13 && artistName !== "") {
            artistLookup();
        }
    });

    // Seach for given artist
    function artistLookup() {
        var url = '/Home/ArtistLookup';
        var artistName = $('#artist-input').val();
        var data = { artistName: artistName };

        //disengage previous click event on artists
        $('#artists-display').off('click', '.artist-container', artistClicked);

        $.ajax(
            {
                type: 'GET',
                url: url,
                data: data,
                dataType: 'json',
                cache: false
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert("artistLookup() failed");
            }).done(function (artistsList, textStatus, jqXHR) {
                $('#artists-area').css("display", "block");
                $('#artists-display').empty();
                if (artistsList === undefined || artistsList.length === 0)
                    $('#artists-display').append("<span>No Artists matching <b>{0}</b> found</span>".format(artistName));
                else {
                    var image =
                        '<div class="artist-container"> ' +
                        '   <img class="artist-image" src={0} alt="{1}" data-id={2}>' +
                        '   <span class="artist-name">{3}</span>' +
                        '</div>';

                    for (index in artistsList) {
                        //check if artist has no image and replace it with a default one
                        var imageUrl = artistsList[index].image === null ? "./images/default_artist_image.jpg" : artistsList[index].image.url;
                        $('#artists-display').append(image.format(imageUrl, artistsList[index].name, artistsList[index].id, artistsList[index].name));
                    }
                    //setup click event for artist container
                    $('#artists-display').on('click', '.artist-container', artistClicked);

                    //enable 'Show Recommendations'
                    $('#get-recommendations').removeAttr('disabled');
                }
            });
    }

    // Artist container clicked on
    function artistClicked(e) {
        var element = $(this).find(":first");
        if ($(this).hasClass("artist-selected") === true)
            $(this).removeClass("artist-selected");
        else {
            $(this).addClass("artist-selected");
        }
    }

    // Genre container clicked on
    function genreClicked(e) {
        var element = $(this).find(":first");
        if ($(this).hasClass("genre-selected") === true) {
            $(this).removeClass("genre-selected");
        }
        else {
            $(this).addClass("genre-selected");
        }
    }

    

    // Build artist comma separated IDs as required
    function getArtistsIDs() {

        var artistsIDs = [];
        //get all selected artists
        var elements = $("#artists-display").children(".artist-selected");
        for (var i = 0; i < elements.length; i++) {
            var image = $(elements[i]).find(":first");
            var id = image.attr("data-id");
            artistsIDs.push(id);
        }

        return artistsIDs;
    }

    // Build artist comma separated IDs as required
    function getGenresIDs() {

        var genresIDs = [];
        //get all selected artists
        var elements = $("#musical-genres").children(".genre-selected");
        for (var i = 0; i < elements.length; i++) {
            var id = $(elements[i]).text();
            genresIDs.push(id);
        }

        return genresIDs;
    }

    function setGenres() {
        var url = '/Home/GetGenres';

        //disengage previous click event on artists
        $('#musical-genres').off('click', '.genre-box', genreClicked);

        $.ajax(
            {
                type: 'GET',
                url: url,
                cache: false
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert("setGenres() failed");
            }).done(function (genres, textStatus, jqXHR) {
                $('#musical-genres').empty();
                if (genres === undefined || genres.length === 0)
                    $('#musical-genres').append("<span>Failed to fetch genres</span>".format(artistName));
                else {
                    var genre =
                        '<span class="genre-box">{0}</span>';

                    for (index in genres) {
                        $('#musical-genres').append(genre.format(genres[index]));
                    }
                    //setup click event for artist container
                    $('#musical-genres').on('click', '.genre-box', genreClicked);

                    //enable 'Show Recommendations'
                    $('#get-recommendations').removeAttr('disabled');
                }
            });
    }

    // Request recommendations and show them
    $('#get-recommendations').click(function () {
        $('#artists').val(getArtistsIDs());
        $('#genres').val(getGenresIDs());
        $('#show-recommendations').submit();
    });

    //load genres
    setGenres();
};

$(document).ready(function () {
    new Index();
});
