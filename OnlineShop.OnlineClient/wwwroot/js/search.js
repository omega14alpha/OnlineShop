var flag = true;
var pattern = /^[0-9a-zA-Z]+$/;

$(document).ready(function () {
    interceptDropdown('#filtrationDiv .search-panel .dropdown-menu');
});

function interceptDropdown(element) {
    $(element).find('a').click(function (e) {
        e.preventDefault();
        var param = $(this).attr("href");
        var concept = $(this).text();
        $('.search-panel span#search_concept').text(concept);
        $('.input-group #search_param').val(param);
        $('#fill_box').prop('disabled', false);
    });
}

$('#fill_box').bind("change paste keyup", function (e) {
    if (flag) {
        setTimeout(sendData, 1000);
        flag = false;
    }
});

function sendData() {
    $.ajax({
        type: 'post',
        url: $('.input-group #search_param').val(),
        data: {
            "Field": $('.search-panel span#search_concept').text(),
            "Data": $('#fill_box').val()
        },
        success: function (data) {
            $('#dataDiv').html(data);
            flag = true;
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
}
