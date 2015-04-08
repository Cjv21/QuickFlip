$(function () {
    $("#comm-dialog").dialog({
        modal: true,
        resizable: false,
        width: 400,
        height: 180,
        autoOpen: false
    });
    $("#changeCommunity").on("click", function () {
        $("#comm-dialog").dialog("open");
    });
});
