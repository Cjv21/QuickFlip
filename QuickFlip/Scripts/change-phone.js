$(function () {
    $("#phone-dialog").dialog({
        modal: true,
        resizable: false,
        width: 400,
        height: 250,
        autoOpen: false
    });
    $("#changePhone").on("click", function () {
        $("#phone-dialog").dialog("open");
    });
});
