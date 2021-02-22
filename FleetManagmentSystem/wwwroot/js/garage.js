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
