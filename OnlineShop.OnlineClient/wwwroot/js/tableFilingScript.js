$(document).ready(function () {
    interceptPaging('#menuItems a');
});

function interceptPaging(element) {
    $(element).click(function (e) {
        e.preventDefault();
        $.ajax({
            type: 'post',
            url: $(this).attr('href'),
            success: function (data) {
                $('#dataDiv').html(data);
                interceptPaging('#paginationBlock a');
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    });
}
