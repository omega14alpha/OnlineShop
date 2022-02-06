$(document).ready(function () {
    $('#charItems a').on('click', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'get',
            url: $(this).attr('href'),
            success: function (data) {
                $('#insertDiv').html(data);
            },
            error(req, status, error) {
                console.log(error);
            }
        });
    });
});
