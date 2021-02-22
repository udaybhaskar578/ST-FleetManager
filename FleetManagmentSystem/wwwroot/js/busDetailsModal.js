$(document).ready(function () {
    modalPopUp();
});

function modalPopUp() {
    var PlaceHolderElement = $('#PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
            attachGetResaleValue();
            getResaleValue();
        })
    });
    $('img[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
            attachGetResaleValue();
            getResaleValue();
        })
    });
    PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            window.location.reload();
            placeHolderElement.find('.modal').modal('hide');
        });

    });
}


function attachGetResaleValue() {
    console.log("In ResaleEvent On Page Load");
    $(".form-control").change(function () {

        calculateResaleValue();
    });

}

function getResaleValue() {
    var id = $("#busId").val();
    $.ajax({
        type: "GET",
        url: '/User/Bus/GetResaleValue/' + id,
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                $("#resaleValue").text(data.message);
            }
            else {
                $("#resaleValue").append($.parseHTML(data.message));
            }
        }
    });
}

//Post Request  for Get Resale Value
function calculateResaleValue() {
    var id = $("#busId").val();
    var year = $("#Year").val()
    var capacity = $("#MaximumCapacity").val();
    var reading = $("#OdometerReading").val();
    var ac = $("#AirConditioning").val();
    var status = $("#CurrentStatus").val();

    var busdata = JSON.stringify({
        "Id": id,
        "Year": year,
        "MaximumCapacity": capacity,
        "OdometerReading": reading,
        "AirConditioning": ac == "True"? true:false,
        "CurrentStatus": status
    });

    $.ajax({
        type: "POST",
        url: '/User/Bus/CalculateResaleValue/',
        data: busdata,
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                $("#resaleValue").text("");
                $("#resaleValue").text(data.message);
            }
            else {
                $("#resaleValue").text("");
                $("#resaleValue").append($.parseHTML(data.message));
            }
        }
    });
}