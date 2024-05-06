$('.showtime-link').click(function () {


    var remainingSeats = $(this).attr('data-remainingSeats');
    var bookingId = $(this).data('id');

    $('#BookingId').val(bookingId);
    $('#RemainingSeats').text('Remaining Seats: ' + remainingSeats);

    if (remainingSeats > 0) {
        $('#detailModal').modal('show'); 
        $('#Booking').show(); 
    } else {
        alert('No seats Available');
    }


});

$('#increment-btn').click(function () {
    var quantityInput = $('#ticketQuantity');
    var quantity = parseInt(quantityInput.val());
    var remainingSeats = parseInt($('#RemainingSeats').text().split(': ')[1]);
    if (quantity < remainingSeats) {
        quantity++;
    }
    quantityInput.val(quantity);
});


$('#decrement-btn').click(function () {
    var quantityInput = $('#ticketQuantity');
    var quantity = parseInt(quantityInput.val());
    if (quantity > 1) {
        quantity--;
        quantityInput.val(quantity);
    }
});

$('#bookTickets').click(function () {
    var quantity = $('#ticketQuantity').val();
    bookTickets(quantity);
});
 

function bookTickets(quantity) {
    var bookingId = $('#BookingId').val();

    $.ajax({
        url: '/Home/Booking',
        type: 'POST',
        data: { id: bookingId, Quantity: quantity },
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        success: function (response) {
            if (response.success) {
           
                displayBookingConfirmation(response.data);
            } else {
                alert('Error: ' + response.error);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error booking tickets:', error);
            alert('An error occurred while booking tickets. Please try again later.');
        }
    });


    event.preventDefault();
}


function displayBookingConfirmation(confirmationDetails) {
   
    var html = '<p><strong>Screen ID:</strong> ' + confirmationDetails.screen.screenId + '</p>' +
        '<p><strong>Total Seats Selected:</strong> ' + confirmationDetails.totalSeatSelected + '</p>' +
        '<p><strong>Amount:</strong> ' + confirmationDetails.amount + '</p>' +
        '<p><strong>Show Time:</strong> ' + confirmationDetails.screen.showtime + '</p>' +
        '<p><strong>Movie Name:</strong> ' + confirmationDetails.movie.movieName + '</p>' +
        '<p><strong>Theatre Name:</strong> ' + confirmationDetails.theatre.theatreName + '</p>';

    $('#bookingModal .modal-body').html(html);
  
    $('#bookingModal').modal('show');
}

$('#closeBookingModal').click(function () {

    location.reload();
});