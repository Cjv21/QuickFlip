$(function () {
    $("#dialog").dialog({
        modal: true,
        resizable: false,
        width: 400,
        height: 180,
        autoOpen: false
    });
    $("#changeProfilePicture").on("click", function () {
        $("#dialog").dialog("open");
    });
});
