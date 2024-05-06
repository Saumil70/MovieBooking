$(document).ready(function () {
    showMovie();
})

function showMovie() {
    $.ajax({
        url: 'Movie/MovieList',
        type: 'GET',
        dataT: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result, statu, xhr) {
            var obj = "";
            $.each(result, function (index, item) {
                obj += '<tr>';
                obj += '<td>' + item.movieId + '</td>';
                obj += '<td>' + item.movieName + '</td>';
                obj += '<td>' + item.genre + '</td>';
                obj += '<td>' + item.time + '</td>';
                obj += '<td><a href="#" class="btn btn-primary" onclick="Edit(' + item.movieId + ')">Edit</a> ||  <a href="#" class="btn btn-danger" onclick="Delete(' + item.movieId + ')">Delete</a></td >';
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
    $('#addMovie').text('Add Movie');
});

function giveErrorMovie() {
    var movieName = $('#MovieName').val();
    if (!movieName) {
        if ($('#MovieNameError').length === 0) {
            $('#MovieName').keyup(function () {
                if ($(this).val()) {
                    $('#MovieNameError').remove();
                }
            });
            $('#MovieName').after('<span id="MovieNameError" class="text-danger">Movie Name is required.</span>');
        }
        return;
    }
}

function AddMovie() {
    giveErrorMovie();
    var formData = new FormData();
    formData.append('MovieName', $('#MovieName').val());
    formData.append('Genre', $('#GenreName').val());
    formData.append('Time', $('#Time').val());
    formData.append('File', $('#Image').prop('files')[0]);

    $.ajax({
        url: '/Movie/AddMovie',
        type: 'POST',
        data: formData,
        contentType: false, 
        processData: false, 
        success: function (response) {
            if (response.success) {
                alert('Movie Created Successfully');
                ClearTextBox();
                showMovie();
                HidePop();
            }
            else {
                alert('Movie not created: ' + response.message);
            }
        },
        error: function () {
            alert("Movie not Created Successfully");
        }
    });
};


function HidePop() {
    $('#userModal').modal('hide');
}

function ClearTextBox() {
    $('#MovieName').val('');
}

function Delete(movieId) {
    if (confirm('Are You Sure, You want to delete the Movie? ')) {
        $.ajax({
            url: '/Movie/Delete?id=' + movieId,
            success: function (response) {
                if (response.success) {

                    alert('Movie Deleted SuccessFully!');
                    showMovie();
                }
                else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("Movie not Deleted");
            }
        });
    }
}

function Edit(movieId) {
    $('span').remove();
    $.ajax({
        url: '/Movie/EditMovie?id=' + movieId,
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded;charset=utf-8',
        dataType: 'json',
        success: function (response) {
            $('#userModal').modal('show');
            $('#MovieId').val(response.movieId);
            $('#MovieName').val(response.movieName);
            $('#GenreName').val(response.genre);
            $('#Time').val(response.time);
            var imageUrl = response.imageUrl;
            if (imageUrl) {
                var imgElement = $('<img id="preview" class="rounded">').attr('src', imageUrl).attr('alt', 'User Image').attr('height', '100').attr('width', '100');
                $('#userImage').empty().append(imgElement);

            }
            else {
                $('#userImage').empty(); // Clear the image element if no image URL is available
            }

            // Add event listener to update image preview when a new file is selected
            $('#Image').on('change', function () {
                var file = this.files[0];
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#preview').attr('src', e.target.result);
                }

                reader.readAsDataURL(file);
            });

            $('#AddData').hide();
            $('#UpdateData').show();
        },
        error: function () {
            alert("Movie not Found");
        }
    });
}

function UpdateMovie() {
    giveErrorMovie();
    var objData = new FormData();
    objData.append('MovieId', $('#MovieId').val());
    objData.append('MovieName', $('#MovieName').val());
    objData.append('Genre', $('#GenreName').val());
    objData.append('Time', $('#Time').val());
    objData.append('ImageUrl', $('#preview').attr('src'));

    // Check if a new file is selected for upload
    var fileInput = $('#Image')[0];
    if (fileInput.files.length > 0) {
        objData.append('File', fileInput.files[0]);
    }

    $.ajax({
        url: '/Movie/EditMovie',
        type: 'POST',
        data: objData,
        contentType: false, // Set contentType to false for FormData
        processData: false, // Set processData to false for FormData
        success: function (response) {
            if (response.success) {
                alert('Movie Updated Successfully');
                ClearTextBox();
                showMovie();
                HidePop();
            } else {
                alert('Movie not updated');
            }
        },
        error: function () {
            alert("Movie can't Updated");
        }
    });

    function HidePop() {
        $('#userModal').modal('hide');
    }

    function ClearTextBox() {
        $('#MovieName').val('');
    }
}
