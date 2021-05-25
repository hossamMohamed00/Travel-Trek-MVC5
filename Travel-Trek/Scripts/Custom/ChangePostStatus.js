$(document).ready(function () {
    //* Handle approve button
    $("#posts").on("click", ".js-approve",
        function () {
            // Get reference for the button
            var button = $(this);

            bootbox.confirm({
                message: "Are you sure you want to approve this post ? No undo for this. 🙄",
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
                        // Send the ajax request
                        $.ajax({
                            url: "/Dashboard/posts/approve",
                            dataType: "json",
                            type: "POST",
                            data: {
                                id: button.attr("data-post-id")
                            },
                            success: function (data) {
                                if (data.success) {

                                    var dialog = bootbox.dialog({
                                        title: 'Approving Trip Post 🐱‍🏍',
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
                                    bootbox.alert('Cannot approve this post right now 😭😭!');
                                }
                            },
                            error: function (xhr) {
                                console.log(xhr);
                                bootbox.alert('Cannot approve this post right now 😭😭!');
                            }
                        });
                    }
                }
            });
        });

    //* Handle refuse button
    $("#posts").on("click", ".js-refuse",
        function() {
            // Get reference for the button
            var button = $(this);

            bootbox.prompt({
                title: "Please select the reason for refusing this trip post. 🙄",
                centerVertical: true,
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
                inputType: 'select',
                inputOptions: [
                    {
                        text: 'Violate our rules.',
                        value: 'Violate our rules.'
                    },
                    {
                        text: 'False Information',
                        value: 'False Information'
                    },
                    {
                        text: 'Hate Speech',
                        value: 'Hate Speech'
                    },
                    {
                        text: 'Spam',
                        value: 'Spam Post'
                    },
                    {
                        text: 'Others',
                        value: 'No reason specified'
                    }
                ],
                callback: function(result) {

                    if (result != null) {
                        var refuseMessage = result;
                        // Send the ajax request
                        $.ajax({
                            url: '/Dashboard/posts/refuse',
                            dataType: "json",
                            type: "POST",
                            data: {
                                id: button.attr("data-post-id"),
                                refuseMessage: refuseMessage
                            },
                            success: function(data) {
                                if (data.success) {

                                    var dialog = bootbox.dialog({
                                        title: 'Refusing Trip Post 🐱‍🏍',
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
                                    bootbox.alert('Cannot refuse this post right now 😭😭!');
                                }
                            },
                            error: function(xhr) {
                                console.log(xhr);
                                bootbox.alert('Cannot refuse this post right now 😭😭!');
                            }
                        });
                    }
                }
            });
        });
});
