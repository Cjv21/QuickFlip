$(function () {
    $("#alert-mode-dialog").dialog({
        modal: true,
        resizable: false,
        width: 400,
        height: 180,
        autoOpen: false
    });
    $("#changeAlertMode").on("click", function () {
        $("#alert-mode-dialog").dialog("open");
    });
});
