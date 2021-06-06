$(document).ready(function () {
    /* UnSave Post Logic*/
    $("#posts").on("click",
        ".js-remove",
        function () {
            // Get reference for the button
            var button = $(this);

            // Get post id
            var PostId = button.attr("data-post-id");

            $.ajax({
                url: '/wall/posts/removeSaved',
                dataType: "json",
                type: "POST",
                data: {
                    postId: PostId
                },
                success: function (data) {
                    if (data.success) {
                        bootbox.alert(data.message);
                        //* Get the card and remove
                        button.parents(".js-card").remove();
                        $("#posts").load(location.href + " #posts");
                    } else {
                        bootbox.alert(data.message);
                    }
                },
                error: function (xhr) {
                    console.log(xhr);
                    bootbox.alert('Cannot un-save this post right now 😭!');
                }
            });
        });
});