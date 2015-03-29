$(document).ready(function () {

    $('#post-type').click(function (event) {

        if ($('#post-type').val() == 'ForSale') {
            $('#required-price-label').text('Max Price:');
            $('#required-price-div').show();
        }
        else if ($('#post-type').val() == 'WantToBuy') {
            $('#required-price-label').text('Willing to Pay:');
            $('#required-price-div').show();
        } else {
            $('#required-price-div').hide();
        }

    });

});


