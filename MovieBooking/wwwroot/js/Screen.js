$(document).ready(function () {
    showScreen();
    $('#state').attr('disable', true);
    LoadMovie();
    LoadTheatre();
})

function showScreen() {
    $.ajax({
        url: 'Screen/ScreenList',
        type: 'GET',
        dataT: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result, statu, xhr) {
            var obj = "";
            $.each(result, function (index, item) {
                obj += '<tr>';
                obj += '<td>' + item.screenId + '</td>';
                obj += '<td>' + item.movie.movieName + '</td>';
                obj += '<td>' + item.theatre.theatreName + '</td>';
                obj += '<td>' + item.totalSeats + '</td>';
                obj += '<td>' + item.showTime + '</td>';
                obj += '<td><a href="#" class="btn btn-primary" onclick="Edit(' + item.id + ')">Edit</a> ||  <a href="#" class="btn btn-danger" onclick="Delete(' + item.id + ')">Delete</a></td >';
                obj += '</tr>';
            })
            $('#tblData').html(obj);
        },
        error: function () {
            alert("Data can't get");
        }
    });
};

function LoadMovie() {
    $('#movie').empty();

    $.ajax({
        url: '/Screen/MovieList',
        success: function (response) {

            if (response != null && response != undefined && response.length > 0) {
                $('#movie').attr('disable', true);
                $('#movie').append('<option disabled selected>--- Select Movie ---</option>');

                $.each(response, function (i, data) {

                    $('#movie').append('<option value= ' + data.movieId + '>' + data.movieName + '</option>');
                });
            }
            else {
                $('#movie').attr('disable', true);
                $('#movie').append('<option>---  Movie not Available ---</option>');
            }
        }
    });
}

function LoadTheatre() {
    $('#theatre').empty();

    $.ajax({
        url: '/Screen/TheatreList',
        success: function (response) {

            if (response != null && response != undefined && response.length > 0) {
                $('#theatre').attr('disable', true);
                $('#theatre').append('<option disabled selected>--- Select Theatre ---</option>');

                $.each(response, function (i, data) {

                    $('#theatre').append('<option value= ' + data.theatreId + '>' + data.theatreName + '</option>');
                });
            }
            else {
                $('#theatre').attr('disable', true);
                $('#theatre').append('<option>---  States not Available ---</option>');
            }
        }
    });
}
$('#btnAdd').click(function () {
    ClearTextBox();
    $('span').remove();
    $('#userModal').modal('show');
    $('#cId').hide();
    $('#AddData').show();
    $('#UpdateData').hide();
    $('#addScreen').text('Add Screen');
});

function giveErrorScreen() {
    var ScreenName = $('#ScreenName').val();
    if (!ScreenName) {
        if ($('#ScreenNameError').length === 0) {
            $('#ScreenName').keyup(function () {
                if ($(this).val()) {
                    $('#ScreenNameError').remove();
                }
            });
            $('#ScreenName').after('<span id="ScreenNameError" class="text-danger">Screen Name is required.</span>');
        }
        
    }
    var StateName = $('#state').val();
    if (!StateName) {
        if ($('#StateNameError').length === 0) {
            $('#state').change(function () {
                if ($(this).val()) {
                    $('#StateNameError').remove();
                }
            });
            $('#state').after('<span id="StateNameError" class="text-danger">Select State is required.</span>');
        }
        
    }
    return;
}
function AddScreen() {
    giveErrorScreen();
    var objData = {
        ScreenId: $('#screenId').val(),
        TheatreId: $('#theatre').val(),
        MovieId: $('#movie').val(),
        TotalSeats: $('#totalseats').val(),
        ShowTime: $('#showtimeInput').val(),


    }
    $.ajax({
        url: '/Screen/AddScreen',
        type: 'POST',
        data: objData,
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                LoadMovie();
                LoadTheatre();
                alert('Screen Created Successfully');
                ClearTextBox();
                showScreen();
                HidePop();
            }
            else {
                alert('Screen not created:' + response.message);
            }
            
        },
        error: function () {
            alert("Screen not Created Successfully");
        }
    });
};

function HidePop() {
    $('#userModal').modal('hide');
}

function ClearTextBox() {
    $('#screenId').val('');
    $('#theatre').val('');
    $('#movie').val('');
    $('#totalseats').val('');
    $('#showtimeInput').val('');

}

function Delete(screenId) {
    if (confirm('Are You Sure, You want to delete the Screen? ')) {
        $.ajax({
            url: '/Screen/Delete?id=' + screenId,
            success: function (response) {
                if (response.success) {
                    alert('Screen Deleted SuccessFully!');
                    showScreen();
                }
                else {
                    alert(response.message);
                }
                
            },
            error: function () {
                alert("Screen not Deleted");
            }
        });
    }
}

function Edit(id) {
    $('span').remove();
    $.ajax({
        url: '/Screen/EditScreen?id=' + id,
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded;charset=utf-8',
        dataType: 'json',
        success: function (response) {
            $('#userModal').modal('show');
            $('#screenId').val(response.screenId);
            $('#theatre').val(response.theatreId);
            $('#movie').val(response.movieId);
            $('#totalseats').val(response.totalSeats);
            $('#showtimeInput').val(response.showtime);
            $('#Id').val(response.id);
            $('#AddData').hide();
            $('#UpdateData').show();
        },
        error: function () {
            alert("Screen not Found");
        }
    });
}

function UpdateScreen() {
    giveErrorScreen();
    var objData = {
        ScreenId: $('#screenId').val(),
        TheatreId: $('#theatre').val(),
        MovieId: $('#movie').val(),
        TotalSeats: $('#totalseats').val(),
        ShowTime: $('#showtimeInput').val(),
        Id: $('#Id').val()
        
    }
    $.ajax({
        url: '/Screen/EditScreen',
        type: 'POST',
        data: objData,
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                alert('Screen Updated Successfully');
                ClearTextBox();
                showScreen();
                HidePop();
            }
            else {
                alert('Screen not updated');
            }
            
        },
        error: function () {
            alert("Screen can't Updated");
        }
    });

    function HidePop() {
        $('#userModal').modal('hide');
    }

    function ClearTextBox() {
        $('#ScreenName').val('');
    }
}