$(function () {
    $("#dialog-fav-offer").dialog({
        modal: true,
        resizable: false,
        width: 600,
        height: 400,
        autoOpen: false
    });
    $("#offerButton").on("click", function () {
        $("#dialog-fav-offer").dialog("open");
    });
});
