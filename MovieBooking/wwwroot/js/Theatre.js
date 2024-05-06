$(document).ready(function () {
    showTheatre();
})

function showTheatre() {
    $.ajax({
        url: 'Theatre/TheatreList',
        type: 'GET',
        dataT: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result, statu, xhr) {
            var obj = "";
            $.each(result, function (index, item) {
                obj += '<tr>';
                obj += '<td>' + item.theatreId + '</td>';
                obj += '<td>' + item.theatreName + '</td>';
                obj += '<td>' + item.address + '</td>';
                obj += '<td><a href="#" class="btn btn-primary" onclick="Edit(' + item.theatreId + ')">Edit</a> ||  <a href="#" class="btn btn-danger" onclick="Delete(' + item.theatreId + ')">Delete</a></td >';
                obj += '</tr>';
            })
            $('#tblData').html(obj);
        },
        error: function () {
            alert("Data can't get");
        }
    });
};

$('#btnAdd').click(function () {
    ClearTextBox();
    $('span').remove();
    $('#userModal').modal('show');
    $('#cId').hide();
    $('#AddData').show();
    $('#UpdateData').hide();
    $('#addTheatre').text('Add Theatre');
});

function giveErrorTheatre() {
    var theatreName = $('#TheatreName').val();
    if (!theatreName) {
        if ($('#TheatreNameError').length === 0) {
            $('#TheatreName').keyup(function () {
                if ($(this).val()) {
                    $('#TheatreNameError').remove();
                }
            });
            $('#TheatreName').after('<span id="TheatreNameError" class="text-danger">Theatre Name is required.</span>');
        }
        return;
    }
}

function AddTheatre() {
    giveErrorTheatre();
    var objData = {
        TheatreName: $('#TheatreName').val(),
        Address: $('#Address').val()
    }
    $.ajax({
        url: '/Theatre/AddTheatre',
        type: 'POST',
        data: objData,
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                alert('Theatre Created Successfully');
                ClearTextBox();
                showTheatre();
                HidePop();
            }
            else {
                alert('Theatre not created: ' + response.message);
            }
            
        },
        error: function () {
            alert("Theatre not Created Successfully");
        }
    });
};

function HidePop() {
    $('#userModal').modal('hide');
}

function ClearTextBox() {
    $('#TheatreName').val('');
}

function Delete(theatreId) {
    if (confirm('Are You Sure, You want to delete the Theatre? ')) {
        $.ajax({
            url: '/Theatre/Delete?id=' + theatreId,
            success: function (response) {
                if (response.success) {

                    alert('Theatre Deleted SuccessFully!');
                    showTheatre();
                }
                else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("Theatre not Deleted");
            }
        });
    }
}

function Edit(theatreId) {
    $('span').remove();
    $.ajax({
        url: '/Theatre/Edit?id=' + theatreId,
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded;charset=utf-8',
        dataType: 'json',
        success: function (response) {
            $('#userModal').modal('show');
            $('#TheatreId').val(response.theatreId);
            $('#TheatreName').val(response.theatreName);
            $('#Address').val(response.address);

            $('#AddData').hide();
            $('#UpdateData').show();
        },
        error: function () {
            alert("Theatre not Found");
        }
    });
}

function UpdateTheatre() {
    giveErrorTheatre();
    var objData = {
        TheatreId: $('#TheatreId').val(),
        TheatreName: $('#TheatreName').val(),
        Address: $('#Address').val()
    }
    $.ajax({
        url: '/Theatre/EditTheatre',
        type: 'POST',
        data: objData,
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                alert('Theatre Updated Successfully');
                ClearTextBox();
                showTheatre();
                HidePop();
            }
            else {
                alert('Theatre not updated');
            }
            
        },
        error: function () {
            alert("Theatre can't Updated");
        }
    });

    function HidePop() {
        $('#userModal').modal('hide');
    }

    function ClearTextBox() {
        $('#TheatreName').val('');
    }


}