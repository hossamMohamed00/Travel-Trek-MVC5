$(document).ready(function () {
    /* Like Post Logic*/
    $("#posts").on("click",
        ".js-like",
        function () {
            // Get reference for the button
            var button = $(this);

            //* First check if the user logged in or not
            var isAuth = button.attr("data-isAuth"); // get the isAuth data attribute - I added it to button

            //* First check if the user logged in or not
            if (!IsUserLoggedIn(isAuth)) {
                // Suggest to login first
                bootbox.alert("Please login first to be able to like the trip post and do alot of amazing things 💃🏻🕺🏻");
                return; // to stop the function
            }

            //* if there is a user already logged in, so continue...

            // toglle the class 
            post.classList.toggle("likedBtn");

            // Get post id
            var postId = button.attr("data-post-id");

            //* Get number of likes div
            var cardBody = button.parents(".card-body");
            console.log(cardBody);

            var numOfLikes = cardBody.children("num-of-likes");

            //var numOfLikes = cardBody.childs("#num-of-likes");

            $.ajax({
                url: '@Url.Action("LikePost", "Wall")',
                dataType: "json",
                type: "POST",
                data: {
                    id: postId
                },
                success: function (data) {
                    if (data.success) {
                        console.log("Liked post!");

                        //* Increase the number of likes
                        numOfLikes.innerHTML = data.likes + " likes."
                    } else {
                        bootbox.alert('Cannot like this post right now 😭😭!');
                    }
                },
                error: function (xhr) {
                    console.log(xhr);
                    bootbox.alert('Cannot like this post right now 😭😭!');
                }
            })
        });

    /* Save Post Logic*/
    $("#posts").on("click",
        ".js-save",
        function () {
            // Get reference for the button
            var button = $(this);

            //* First check if the user logged in or not
            var isAuth = button.attr("data-isAuth"); // get the isAuth data attribute - I added it to buttons 

            if (!IsUserLoggedIn(isAuth)) {
                // Suggest to login first
                bootbox.alert("Please login first to be able to save the trip post and do alot of amazing things 💃🏻🕺🏻");
                return; // to stop the function
            }

            //* if there is a user already logged in, so continue...


            // Get post id from the data attribute of the button
            var PostId = button.attr("data-post-id");

            $.ajax({
                url: "/Wall/posts/save",
                dataType: "json",
                type: "POST",
                data: {
                    postId: PostId
                },
                success: function (data) {
                    if (data.success) {
                        //* Show the returned message to the user
                        bootbox.alert(data.message);
                    } else {
                        bootbox.alert(data.message);
                    }

                    //* Get save btn and change its value
                    var cardBody = button.parents(".card-body");
                    var saveButton = cardBody[0].children.namedItem("saveBtn");
                    saveButton.innerText = "Saved ✔";
                },
                error: function (xhr) {
                    console.log(xhr);
                    bootbox.alert('Cannot save this post right now 😭😭!');
                }
            });
        });
});

/* This function for check if the user logged in or not */
function IsUserLoggedIn(isAuth) {
    /* isAuth -- is 'True' or 'False' */ 
    if (isAuth === 'True') {
        return true;
    } else {
        return false;

    }
}