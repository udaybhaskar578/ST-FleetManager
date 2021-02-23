var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    try {
        dataTable = $('#tblData').DataTable({
            "ajax": {
                "url": "/Admin/GarageAssignment/GetAll"
            },
            "columns": [
                { "data": "bus.registrationNumber", "width": "30%" },
                { "data": "garage.name", "width": "30%" },
                { "data": "checkIn", "width": "30%" },
                {
                    "data": "id",
                    "orderable": false,
                    "render": function (data) {
                        return `
                    <div class="text-center">
                        <a onclick=Checkout('${data}') class="btn btn-success text-white" style="cursor:pointer">
                          <i class="fas fa-sign-out-alt"></i>
                        </a>
                    </div>
                    `;

                    }, "width": "10%"
                }

            ],
            order: [[1, 'asc']],
            rowGroup: {
                dataSrc: ["garage.name"]
            },
            columnDefs: [{
                targets: [1],
                visible: false
            }]
        });
    } catch (error) {
        alert("Unable to load the data tables at this point \n Error:" + error.message);
        console.log(error);
    }


}

function Checkout(id) {
    try {
        swal({
            title: "Are you sure you would like to checkout the bus?",
            text: "Please check with service advisor before leaving the garage.",
            icon: "warning",
            buttons: true,
            dangerMode: true
        }).then((willCheckout) => {
            if (willCheckout) {
                $.ajax({
                    type: "POST",
                    url: '/Admin/GarageAssignment/Checkout',
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
        });
    }
    catch (error) {
        alert("Unable to checkout the bus at this moment \n Error:" + error.message);
        console.log(error);
    }

}