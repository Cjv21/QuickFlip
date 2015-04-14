$(function () {
    $("#dialog").dialog({
        modal: true,
        resizable: false,
        closeOnEscape: false,
        width: 400,
        height: 250,
        autoOpen: true,
        open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); }
    });

});