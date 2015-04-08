$(function () {
    $("#phone-dialog").dialog({
        modal: true,
        resizable: false,
        width: 400,
        height: 180,
        autoOpen: false
    });
    $("#changePhone").on("click", function () {
        $("#phone-dialog").dialog("open");
    });
});
