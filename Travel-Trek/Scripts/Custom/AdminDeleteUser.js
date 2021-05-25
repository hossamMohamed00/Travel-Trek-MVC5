$(document).ready(function () {
    $("#users").on("click", ".js-delete",
        function () {
            // Get reference for the button
            var button = $(this);

            bootbox.confirm({
                message: "Are you sure you want to delete this user ? 🙄",
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
                        console.log("Deleting User...✌");
                        // Send the ajax request
                        $.ajax({
                            url: "/Dashboard/users/delete",
                            dataType: "json",
                            type: "POST",
                            data: {
                                id: button.attr("data-user-id")
                            }, // here you can pass arguments to your request if you need
                            success: function (data) {
                                if (data.success) {

                                    var dialog = bootbox.dialog({
                                        title: 'Deleting User 🐱‍🏍',
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
                                    bootbox.alert('Cannot delete this user right now 😭😭!');
                                }
                            },
                            error: function (xhr) {
                                console.log(xhr);
                                bootbox.alert('Cannot delete this user right now 😭😭!');
                            }
                        });
                    }
                }
            });
        });
    });
