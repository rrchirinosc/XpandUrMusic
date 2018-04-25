// Write your JavaScript code.
var Index = function () {    

    $('#artist-find').click(function () {

        var url = '/Home/ArtistLookup';//.format(jsContext.rootURL);
        var data = { artistName: $('#artist-input').val() };

        $.ajax(
        {
            type: 'GET',
            url: url,
            data: data,
            dataType: 'text',
            cache: false,
            //contentType: "application/json; charset=utf-8"
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert("Find Artist failed");
        }).done(function (result, textStatus, jqXHR) {
            $('#artists-display').empty();
            $('#artists-display').append("<span>{0}</span>".format(result));
        });
        
    });
}

$(document).ready(function () {
    new Index();
});
