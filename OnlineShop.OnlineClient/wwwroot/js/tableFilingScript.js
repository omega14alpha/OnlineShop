$(document).ready(function () {
    interceptPaging('#menuItems a');
    loadFiltrationBar('#menuItems a');
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

function loadFiltrationBar(element) {
    $(element).click(function (e) {
        e.preventDefault();
        $.ajax({
            type: 'get',
            url: $(this).attr('href') + "/Filtration",
            success: function (data) {
                $('#filtrationDiv').html(data);
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    });
}

$('#table a#editOption').on('click', function (e) {
    e.preventDefault();
    $.ajax({
        type: 'post',
        url: $(this).attr('href'),
        success: function (data) {
            //$('#insertDiv').html(data);
            $('#dataDiv').html(data);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
});

$('#insertDiv').on('submit', 'form', function (e) {
    e.preventDefault();
    $.ajax({
        type: $(this).attr('method'),
        url: $(this).attr('action'),
        data: $(this).serialize(),
        success: function (data) {
            $('insertDiv').html('<div id="filtrationDiv"></div>< div id = "dataDiv" class= "pb-5" ></div >');
            console.log(data);
            $('#dataDiv').html(data);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
});

/*
 $('#insertDiv button#edit_form').on('click', function (e) {
    e.preventDefault();
    var qwe = new FormData($('tableShamble'));
    var asd = $('tableShamble').attr('method');
    var xcv = $('tableShamble').attr('action');
    
    console.log(asd);
    console.log(xcv);

    $.ajax({
        type: $(this).attr('method'),
        url: $(this).attr('action'),
        data: new FormData(this),
        success: function (data) {
            console.log(data);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
});
 */