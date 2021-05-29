$(document).ready(function() {
    /* This function to compare values */
    function Contains(text_one, text_two) {
        if (text_one.indexOf(text_two) != -1)
            return true;
    }

    /* Get the search term and apply the filter */
    $("#Search").keyup(function() {
        var searchTerm = $("#Search").val().toLowerCase();
        /* Loop  all the posts and filter it */
        $(".posts").each(function() {
            if (!Contains($(this).text().toLowerCase(), searchTerm)) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });
})