var flag = true;

$(document).ready(function () {
    interceptDropdown('#filtrationDiv .search-panel .dropdown-menu');

    $('#fill_box').bind("change paste keyup", function (e) {
        if (flag) {
            setTimeout(sendData, 1000);
            flag = false;
        }
    });

    $('input[name="dateFrom"]').datepicker({
        format: 'dd.mm.yyyy',
        container: $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body",
        todayHighlight: true,
        autoclose: true,
    });

    $('input[name="dateTo"]').datepicker({
        format: 'dd.mm.yyyy',
        container: $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body",
        todayHighlight: true,
        autoclose: true,
    });

    sendDates();
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

            editTable();
            saveRecord();
            deleteRecord();
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
}

function sendDates() {
    $('#filtrationDiv').on('submit', 'form', function (e) {
        e.preventDefault();
        $.ajax({
            type: $(this).attr('method'),
            url: $(this).attr('action'),
            data: {
                "Field": $(this).find('#hiddenField').val(),
                "Dates": {
                    "DateFrom": $(this).find('#dateFrom').val(),
                    "DateTo": $(this).find('#dateTo').val()
                }
            },
            success: function (data) {
                $('#dataDiv').html(data);
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    });
}
