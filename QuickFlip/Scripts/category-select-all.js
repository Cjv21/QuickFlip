$(document).ready(function () {

    $('#category-select-all').click(function (event) {
        if (this.checked) {
            $('#categories').find(':checkbox').each(function () {
                this.checked = true;
            });
        }
        else {
            $('#categories').find(':checkbox').each(function () {
                this.checked = false;
            });
        }
    });

});


