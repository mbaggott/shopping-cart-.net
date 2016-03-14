$(document).ready(function () {
   /* Load the page with default values selected*/
   $('#ccNumber').focus();
   $('#month option:eq(0)').prop('selected', true);
   $('#year option:eq(0)').prop('selected', true);
});

/* Carry out credit card number validation*/
$(function () {
   /* If the required c/card length is reached, do a Luhn check - notify the user if it passes
      or not - if the length is not reached, also notify the user*/
   $('#ccNumber').validateCreditCard(function (result) {
      var ccCheck = $(result.length_valid ? (result.luhn_valid ? $('#ccTick').show() + $('#ccCross')
         .hide() : $('#ccTick').hide() + $('#ccCross').show()) : $('#ccTick').hide() + $('#ccCross').show());
   });
});

/* Carry out card expiry validation based on the month selected*/
$('#month').change(function () {
   /* Local current date variables for comparing to selected values*/
   var d = new Date();
   var month = d.getMonth() + 1;
   var year = d.getFullYear();

   /* If the month and year selected are greater than the reference values, OR if the month
      selected is less than the current month BUT the year is greater than current year, display
      a valid indication to the user, otherwise display an invalid indication*/
   if ($('#month option:selected').text() >= month && $('#year option:selected').text() >= year ||
      $('#month option:selected').text() <= month && $('#year option:selected').text() > year) {
      //$('.expiry').html('Valid');
      $('#expCross').hide();
      $('#expTick').show();
   }
   else {
      //$('.expiry').html('Invalid');
      $('#expTick').hide();
      $('#expCross').show();
   }
});

/* Carry out card expiry validation based on the year selected*/
$('#year').change(function () {
   /* Local current date variables for comparing to selected values*/
   var d = new Date();
   var month = d.getMonth() + 1;
   var year = d.getFullYear();

   /* If the month and year selected are greater than the reference values, OR if the month
      selected is less than the current month BUT the year is greater than current year, display
      a valid indication to the user, otherwise display an invalid indication*/
   if ($('#month option:selected').text() >= month && $('#year option:selected').text() >= year ||
      $('#month option:selected').text() <= month && $('#year option:selected').text() > year) {
      $('#expCross').hide();
      $('#expTick').show();
   }
   else {
      $('#expTick').hide();
      $('#expCross').show();
   }
});

/* If the CVV is not the correct length, dislpay an invalid message, 
   conversely, if it is the correct length, display a valid message*/
$('#cvv').keyup(function () {
   var correctLength = this.value.length == 3;
   var incorrectLength = this.value.length != 3;
   $('#cvvCross')[incorrectLength ? 'show' : 'hide']();
   $('#cvvTick')[correctLength ? 'show' : 'hide']();
});

/* If the name is not the correct length, dislpay an invalid message, 
   conversely, if it is the correct length, display a valid message*/
$('#name').keyup(function () {
   var correctLength = this.value.length > 1;
   var incorrectLength = this.value.length < 2;
   $('#nameCross')[incorrectLength ? 'show' : 'hide']();
   $('#nameTick')[correctLength ? 'show' : 'hide']();
});

/* If all the fields are valid, as denoted by the visible green ticks, continue
   to the Summary page, otherwise igoner the button press*/
$('#submitBtn').click(function () {
    if ($('#ccTick').is(':visible') && $('#expTick').is(':visible') &&
      $('#cvvTick').is(':visible') && $('#nameTick').is(':visible')) {
      window.location.href = "Summary";
   }
});