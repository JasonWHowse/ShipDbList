var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/ShipList/GetShipsDb/",
            "type": "Get",
            "datatype": "json"
        },
        "columns": [
            { "data": "shipName", "width": "15%" },
            { "data": "weight", "width": "15%" },
            { "data": "hardPoints", "width": "15%" },
            { "data": "typeString", "width": "5%" },
            { "data": "classString", "width": "5%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/ShipList/Updater?Id=${data}" class='btn btn-success text-while' style='cursor:pointer; width:30%'>
                                Updater
                            </a>
                            &nbsp;
                            <a class='btn btn-danger text-white' style='cursor:pointer; width:30%;' onclick=Delete('/ShipList/DeleteShip?Id='+${data})>
                                Deleter
                            </a>
                            &nbsp;
                            <a class='btn btn-warning text-white' style='cursor:pointer; width:30%;' onclick=Clone('/ShipList/CloneShip?Id='+${data})>
                                Cloner
                            </a>
                        </div>`;
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "No ships"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Delete?",
        text: "Are you sure?",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                    dataTable.ajax.reload();
                }
            });
        }
    });
}

function Clone(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function(data) {
            if (data.success) {
                toastr.success(data.message);
            }
            else {
                toastr.error(data.message);
            }
            dataTable.ajax.reload();
        }
    });
}

//function Clone(url) {

//    $.ajax({
//        type: "DELETE",
//        url: url,
//        success: function (data) {
//            if (data.success) {
//                toastr.success(data.message);
//            }
//            else {
//                toastr.error(data.message);
//            }
//            dataTable.ajax.reload();
//        }
//    });
//}