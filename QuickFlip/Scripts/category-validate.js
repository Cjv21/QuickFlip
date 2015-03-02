$(document).ready(function () {

    var checkboxes = $("input[type='checkbox']");
    var submitButt = $("input[type='submit']");

    checkboxes.click(function () {
        var error = $('label[for="checkbox-validate"]');

        if (checkboxes.filter(':checked').length > 0) {
            submitButt.prop("disabled", false);
            error.hide();
        } else {
            submitButt.prop("disabled", true);
            error.show();
        }
    });

});


