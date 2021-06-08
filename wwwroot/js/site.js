// limit amount of seats that are allowed
$("input[type=checkbox]").on("change", function (e) {
  if ($("input[type=checkbox]:checked").length >= 9) {
    $(this).prop("checked", false);
    alert("om meer dan 8 stoelen te reserveren neem contact op");
  }
});
