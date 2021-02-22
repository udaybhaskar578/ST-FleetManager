var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Garage/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "zipCode", "width": "15%" },
            {
                "data": "closestGarage.name",
                "defaultContent": "Not set yet",
                "width": "15%"
            },
            {
                "data": "id",
                "orderable": false,
                "render": function (data) {
                    return `
                    <div class="text-center">
                        <a href="/Admin/Garage/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                            <i class="fas fa-edit"></i>
                        </a>

                    </div>
                    `;

                }, "width": "40%"
            }

        ]
    });

}
//<a onclick=Delete("/Admin/Garage/Delete/${data}") class="btn btn-danger text-white" style = "cursor:pointer" >
//    <i class="fas fa-trash-alt"></i> 
//</a >
//function Delete(url) {
//    swal({
//        title: "Are you sure you would like to delete the Garage entry?",
//        text: "You will not be able to resotre the data",
//        icon: "warning",
//        buttons: true,
//        dangerMode: true
//    }).then((willDelete) => {
//        if (willDelete) {
//            $.ajax({
//                type: "DELETE",
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