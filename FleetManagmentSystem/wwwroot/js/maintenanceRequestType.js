var dataTable;
var isActive = true;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/MaintenanceRequestType/GetAll"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "listPrice", "width": "20%" },
            {
                "data": "isActive",
                "render": function (data) {
                    if (data) {
                        isActive = true;
                        return `<input type="checkbox" disabled checked/>`;
                    }
                    else {
                        isActive = false;
                        return `<input type="checkbox" disabled/>`;
                    }
                }, "width": "20%"
            },
            {
                "data": "id",
                "orderable": false,
                "render": function (data) {
                    var iconTemplate = isActive ? "fas fa-toggle-on" : "fas fa-toggle-off"
                    return  `
                    <div class="text-center">
                        <a href="/Admin/MaintenanceRequestType/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                            <i class="fas fa-edit"></i>
                        </a>
                       <a onclick=ToggleActiveStatus('${data}') class="btn btn-danger text-white" style="cursor:pointer">
                            <i class="${iconTemplate}"></i> 
                        </a>
                    </div>
                    `;

                }, "width": "40%"
            }

        ]
    });

}

//function ToggleActiveStatus(url) {
//    swal({
//        title: "Are you sure you would like to change the status the maintenance entry?",
//        text: "You will not be able to resotre the data",
//        icon: "warning",
//        buttons: true,
//        dangerMode: true
//    }).then((willDelete) => {
//        if (willDelete) {
//            $.ajax({
//                type: "POST",
//                url: url,
//                success: function (data) {
//                    if (data.success == true) {
//                        toastr.success(data.message);
//                        dataTable.ajax.reload();
//                    }
//                    else {
//                        toastr.error(data.message);
//                    }
//                }
//            });
//        }
//    });
//}


function ToggleActiveStatus(id) {

    $.ajax({
        type: "POST",
        url: '/Admin/MaintenanceRequestType/ToggleActiveStatus',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });

}