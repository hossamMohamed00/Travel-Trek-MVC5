$(document).ready(function() {
    $("#questions").on("click",
        ".js-view-post",
        function() {
            // Get reference for the button
            let button = $(this);


            url = "/Content/images/posts/trip1202105291826531293.jpg";

            let html = '<div class="card" style="width: 18rem; margin-bottom: 12px">' +
                '<img class="card-img-top" src = " ' + url +'" alt = "@post.TripTitle" >' +
                '<div class="card-body">' +
                '<h5 class="card-title">From: Hossam </h5>' +
                '<p class="card-text">Details: El-Gouna</p>' +
                '<p class="card-text">Destination: El-Gouna</p>' +
                '<p class="card-text">Date: 1/1/2021</p>' +
                '<!--likes badge-- >' +
                '<div class="badge bg-success likes-badge" style="width: auto">' +
                '@post.Likes likes.' +
                '</div>' +
                '</div >' +
                '< !--Card footer-- >' +
                '<div class="card-footer">' +
                '<text class="text-muted">Posted at 1/1/2020 </text>' +
                '<div class="text-muted">' +
                '</div>' +
                '</div>' +
                '</div >';

            bootbox.alert(html, function () {
                console.log("It was awesome!");
            });
        });
});