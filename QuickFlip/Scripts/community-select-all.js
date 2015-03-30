$(document).ready(function () {

    $('#community-select-all').click(function (event) {
        if (this.checked) {
            $('#communities').find(':checkbox').each(function () {
                this.checked = true;
            });
        }
        else {
            $('#communities').find(':checkbox').each(function () {
                this.checked = false;
            });
        }
    });

});


