﻿$(function () {
    $("#prof-dialog").dialog({
        modal: true,
        resizable: false,
        width: 400,
        height: 180,
        autoOpen: false
    });
    $("#changeProfilePicture").on("click", function () {
        $("#prof-dialog").dialog("open");
    });
});
