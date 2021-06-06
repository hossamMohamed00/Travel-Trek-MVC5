$(document).ready(function () {
    //* Handle delete post operation
    $("#posts").on("click", ".js-delete",
        function () {
            // Get reference for the button
            var button = $(this);

            bootbox.confirm({
                message: "Are you sure you want to delete this post ? 🙄",
                buttons: {
                    cancel: {
                        label: "Cancel",
                        className: "btn-danger"
                    },
                    confirm: {
                        label: "Confirm",
                        className: "btn-success"
                    }
                },
                callback: function (result) {

                    if (result) {
                        console.log("Deleting Post...✌");
                        // Send the ajax request
                        $.ajax({
                            url: '/Dashboard/posts/delete',
                            dataType: "json",
                            type: "POST",
                            data: {
                                id: button.attr("data-post-id")
                            }, // here you can pass arguments to your request if you need
                            success: function (data) {
                                if (data.success) {

                                    var dialog = bootbox.dialog({
                                        title: 'Deleting Trip Post 🐱‍🏍',
                                        message: '<p><i class="fa fa-spin fa-spinner"></i> Processing 👀...</p>'
                                    });

                                    dialog.init(function () {
                                        setTimeout(function () {
                                            button.parents("tr").remove();
                                            dialog.find('.bootbox-body').html(data.message + " 🦾");
                                            dialog.closeButton = true;
                                            dialog.backdrop = true;
                                        }, 1000);
                                    });
                                } else {
                                    bootbox.alert('Cannot delete this post right now 😭!');
                                }
                            },
                            error: function (xhr) {
                                console.log(xhr);
                                bootbox.alert('Cannot delete this post right now 😭!');
                            }
                        });
                    }
                }
            });
        });

    //* Show post rejection message 
    $("#posts").on('click', '.js-refused', function() {
        // Get reference for the button
        var link = $(this);
        var message = link.attr("data-post-refuse-message");

        console.log(message);

        bootbox.alert({
            title: "Rejection Message for the Trip Post 🙂",
            backdrop: true,
            message: "This trip post refused because it is considered as <text class='text-danger'>" + message + "</text> 😐",
            buttons: {
                ok: {
                    label: "Ok",
                    className: "btn-primary"
                }
            }
            });
        });
    });
