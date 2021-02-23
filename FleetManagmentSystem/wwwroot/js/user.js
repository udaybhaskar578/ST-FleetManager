var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    try {
        dataTable = $('#tblData').DataTable({
            "ajax": {
                "url": "/Admin/User/GetAll"
            },
            "columns": [
                { "data": "name", "width": "15%" },
                { "data": "email", "width": "15%" },
                { "data": "phoneNumber", "width": "15%" },
                { "data": "role", "width": "15%" },
                {
                    "data": {
                        id: "id", lockoutEnd: "lockoutEnd"
                    },
                    "render": function (data) {
                        var today = new Date().getTime();
                        var lockout = new Date(data.lockoutEnd).getTime();
                        if (lockout > today) {
                            //user is currently locked
                            return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="fas fa-lock-open"></i>  Unlock
                                </a>
                            </div>
                           `;
                        }
                        else {
                            return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="fas fa-lock"></i>  Lock
                                </a>
                            </div>
                           `;
                        }

                    }, "width": "25%"
                }
            ],
            order: [[3, 'asc']],
            rowGroup: {
                dataSrc: ["role"]
            },
            columnDefs: [{
                targets: [3],
                visible: false
            }]
        });
    } catch (error) {
        alert("Unable to load the data tables at this point \n Error:" + error.message);
        console.log(error);
    }

}

function LockUnlock(id) {
    try {
        $.ajax({
            type: "POST",
            url: '/Admin/User/LockUnlock',
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
        alert("Unable to lock or unlock the user at this moment \n Error:" + error.message);
        console.log(error);
    }


}