$(document).ready(function () {

    var checkboxes = $("input[type='checkbox']");
    var submitButt = $("input[type='submit']");

    var error = $('label[for="checkbox-validate"]');
    error.hide();

    checkboxes.click(function () {
        if (checkboxes.filter(':checked').length > 0) {
            submitButt.prop("disabled", false);
            error.hide();
        } else {
            submitButt.prop("disabled", true);
            error.show();
        }
    });

});


