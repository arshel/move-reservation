// limit amount of seats that are allowed 
var limit = 8;
$('input.limit-seat').on('change', function(evt) {
   if($(this).siblings(':checked').length >= limit) {
       this.checked = false;
       alert("only 8 seats allowed per reservation")
   }
});