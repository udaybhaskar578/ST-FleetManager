var dataTable;
var isActive = true;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    try {
        dataTable = $('#tblData').DataTable({
            "ajax": {
                "url": "/Admin/MaintenanceLineItem/GetAll"
            },
            "columns": [
                { "data": "bus.registrationNumber", "width": "20%" },
                { "data": "garage.name", "width": "20%" },
                { "data": "maintenanceRequestType.name", "width": "20%" },
                { "data": "status", "width": "20%" },
                {
                    "data": "id",
                    "orderable": false,
                    "render": function (data) {
                        return `
                    <div class="text-center">
                        <a href="/Admin/MaintenanceLineItem/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                            <i class="fas fa-edit"></i>
                        </a>
                       <a onclick=MarkAsCompleted('${data}') class="btn btn-primary text-white" style="cursor:pointer">
                            <i class="fas fa-check"></i>
                        </a>
                    </div>
                    `;

                    }, "width": "20%"
                }
            ],
            order: [[1, 'asc'], [0, 'asc']],
            rowGroup: {
                dataSrc: ["garage.name", "bus.registrationNumber"]
            },
            columnDefs: [{
                targets: [0, 1],
                visible: false
            }]
        });
    } catch (error) {
        alert("Unable to load the data tables at this point \n Error:" + error.message);
        console.log(error);
    }

}

function MarkAsCompleted(id) {
    try {
        $.ajax({
            type: "POST",
            url: '/Admin/MaintenanceLineItem/Complete',
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
    } catch (error) {
        alert("Unable to mark the maintenance line item as completed \n Error:" + error.message);
        console.log(error);
    }

}