$(document).ready(function () {
    modalPopUp();
});

function modalPopUp() {
    try {
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
    } catch (error) {
        alert("Unable to load modal view at this point \n Error:" + error.message);
        console.log(error);
    }

}


function attachGetResaleValue() {

    $(".form-control").change(function () {

        calculateResaleValue();
    });

}

function getResaleValue() {
    try {
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
    } catch (error) {
        alert("Unable to fetch resale value at this moment \n Error:" + error.message);
        console.log(error);
    }

}

//Post Request  for Get Resale Value
function calculateResaleValue() {
    try {
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
            "AirConditioning": ac == "True" ? true : false,
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
    } catch (error) {
        alert("Unable to calculate the resale value at this moment \n Error:" + error.message);
        console.log(error);
    }

}