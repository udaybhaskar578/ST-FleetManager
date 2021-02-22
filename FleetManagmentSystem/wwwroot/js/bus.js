var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Bus/GetAll"
        },
        "columns": [
            { "data": "registrationNumber", "width": "10%"},
            { "data": "vin", "width": "10%"},
            { "data": "make", "width": "10%" },
            { "data": "model", "width": "8%" },
            { "data": "maximumCapacity", "width": "10%", "className": "text-right"},
            {
                "data": "airConditioning",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" disabled checked/>`;
                    }
                    else {
                        return `<input type="checkbox" disabled/>`;
                    }
                }, "width": "5%", "className": "text-center"
            },
            { "data": "currentStatus", "width": "15%" },
            {
                "data": "id",
                "orderable": false,
                "render": function (data) {
                    return `
                    <div class="text-center">
                        <a href="/Admin/Bus/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                            <i class="fas fa-edit"></i>
                        </a>
                        <a onclick=Delete("/Admin/Bus/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                            <i class="fas fa-trash-alt"></i> 
                        </a>
                        <a href="/Admin/Bus/Assign/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                            <i class="fas fa-tools"></i>
                        </a>

                    </div>
                    `;

                }, "width": "10%"
            }

        ]
    });

}

//<i class="fas fa-tools"></i>

function Delete(url) {
    swal({
        title: "Are you sure you would like to delete the bus?",
        text: "You will not be able to resotre the data",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success == true) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}