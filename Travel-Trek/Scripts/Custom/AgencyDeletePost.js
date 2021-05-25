$(document).ready(function() {
    //* Handle delete post
    $("#posts").on("click", '#js-delete',
        function () {
            // Get reference for the button
            var button = $(this);

            bootbox.confirm({
                message: "Are you sure you want to delete this post ? 🙄",
                buttons: {
                    cancel: {
                        label: 'Cancel',
                        className: 'btn-danger'
                    },
                    confirm: {
                        label: 'Confirm',
                        className: 'btn-success'
                    }
                },
                callback: function (result) {

                    if (result) {
                        console.log("Deleting Post...✌");
                        // Send the ajax request
                        $.ajax({
                            url: '/Agency/Posts/delete',
                            dataType: "json",
                            type: "POST",
                            data: {
                                id: button.attr("data-post-id")
                            },
                            success: function (data) {
                                if (data.success) {
                                    var dialog = bootbox.dialog({
                                        title: 'Deleting Post 🐱‍🏍',
                                        message: '<p><i class="fa fa-spin fa-spinner"></i> Processing 👀...</p>', backdrop: true,
                                        buttons: {
                                            Ok: {
                                                label: "Ok",
                                                className: 'btn-primary',
                                                callback: function () {
                                                    $("#posts").load(location.href + " #posts");
                                                }
                                            }
                                        }
                                    });

                                    dialog.init(function () {
                                        setTimeout(function () {
                                            //* Get the card and remove 
                                            button.parents(".card").remove();

                                            //* add the message to the dialog
                                            dialog.find('.bootbox-body').html(data.message + " 🦾");
                                            dialog.closeButton = true;
                                            dialog.backdrop = true;
                                        },
                                            1000);
                                    });
                                } else {
                                    bootbox.alert('Cannot delete this post right now 😭😭!');
                                }
                            },
                            error: function (xhr) {
                                console.log(xhr);
                                bootbox.alert('Cannot delete this post right now 😭😭!');
                            }
                        });
                    }
                }
            });
        });
    })
