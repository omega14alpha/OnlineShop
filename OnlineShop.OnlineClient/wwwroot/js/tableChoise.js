$(document).ready(function () {
    $('#menuItems a').on('click', function (e) {
        e.preventDefault();
        $('#insertDiv').html('<div id="filtrationDiv"></div><div id = "dataDiv" class= "pb-5" ></div >');
        loadFiltrationBar($(this).attr('href'));
        loadTableData($(this).attr('href'));
    });
});

function loadFiltrationBar(url) {
    $.ajax({
        type: 'get',
        url: url + "/Filtration",
        success: function (data) {
            $('#filtrationDiv').html(data);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
}

function loadTableData(url) {
    $.ajax({
        type: 'post',
        url: url,
        success: function (data) {
            $('#dataDiv').html(data);
            editTable();
            deleteRecord();
            loadPagination();
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
}

function editTable() {
    $('#table a#editOption').on('click', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'post',
            url: $(this).attr('href'),
            success: function (data) {
                $('#insertDiv').html(data);
                saveRecord();
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    });
}

function deleteRecord() {
    $('#table a#deleteOption').on('click', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'post',
            url: $(this).attr('href'),
            success: function (data) {
                $('#insertDiv').html('<div id="filtrationDiv"></div><div id = "dataDiv" class= "pb-5" ></div >');
                loadFiltrationBar(controller);
                $('#dataDiv').html(data);
                editTable();
                deleteRecord();
                loadPagination();
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    });
}

function loadPagination() {
    $('#paginationBlock a').on('click', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'post',
            url: $(this).attr('href'),
            success: function (data) {
                $('#dataDiv').html(data);
                loadPagination();
                editTable();
                deleteRecord();
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    });
}

function saveRecord() {
    $('#insertDiv').on('submit', 'form', function (e) {
        e.preventDefault();
        var controller = $(this).attr('action').split('/')[1];
        $.ajax({
            type: $(this).attr('method'),
            url: $(this).attr('action'),
            data: $(this).serialize(),
            success: function (data) {
               // console.log(data);
                $('#insertDiv').html('<div id="filtrationDiv"></div><div id = "dataDiv" class= "pb-5" ></div >');
                loadFiltrationBar(controller);
                $('#dataDiv').html(data);
                editTable();
                deleteRecord();
                loadPagination();
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    });
}