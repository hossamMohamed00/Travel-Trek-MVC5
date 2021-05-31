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

            // Get post id
            var PostId = button.attr("data-post-id");

            /* Send the ajax request to like the post */
            $.ajax({
                url: '/Wall/posts/like',
                dataType: "json",
                type: "POST",
                data: {
                    postId: PostId
                },
                success: function (data) {
                    if (data.success) {
                        console.log(data.message);

                        // toggle liked the class 
                        button[0].classList.toggle("likedBtn");

                        //* Get number of likes div and change its value
                        var cardBody = button.parents(".card-body")[0];
                        var numOfLikedElement = cardBody.children.namedItem("num-of-likes");
                        numOfLikedElement.innerHTML = data.likes + " likes"; // get the post likes number from the data object (sent from the controller)
                        console.log(numOfLikedElement);

                    } else {
                        bootbox.alert(data.message);
                    }
                },
                error: function (xhr) {
                    console.log(xhr);
                    bootbox.alert("Cannot like this post right now 😭😭!");
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

    /*Ask new question */
    $("#posts").on("click",
        ".js-ask",
        function () {
            var question
            // Get reference for the button
            var button = $(this);

            //* First check if the user logged in or not
            var isAuth = button.attr("data-isAuth"); // get the isAuth data attribute - I added it to buttons 

            if (!IsUserLoggedIn(isAuth)) {
                // Suggest to login first
                bootbox.alert("Please login first to be able to ask a question for this trip post and do alot of amazing things 💃🏻🕺🏻");
                return; // to stop the function
            }

            //* if there is a user already logged in, so continue...

            //* Show bootbox to get the question
            bootbox.prompt({
                title: "Ask New Question 💡❓",
                message: "Please type your question for this trip post 🤔",
                inputType: "textarea",
                callback: function (result) {
                    if (result == null) return;
                    question = result;
                    // Get post id from the data attribute of the button
                    var PostId = button.attr("data-post-id");

                    $.ajax({
                        url: "/Wall/posts/ask",
                        dataType: "json",
                        type: "POST",
                        data: {
                            postId: PostId,
                            question: question
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
                            var askButton = cardBody[0].children.namedItem("ask");
                            // Disable the btn here
                            askButton.innerText = "Asked ✔";
                            askButton.disabled = "disabled";
                        },
                        error: function (xhr) {
                            console.log(xhr);
                            bootbox.alert('Cannot ask a question right now 😐💔!');
                        }
                    });
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