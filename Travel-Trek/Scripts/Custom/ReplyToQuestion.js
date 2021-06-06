$(document).ready(function() {
    $("#questions").on("click",
        ".js-reply-btn",
        function() {
            // Get reference for the button
            let button = $(this);

            //* Get the input box for this question
            let { replyInput, replyForm } = GetTextArea(button);

            let replyMessage = replyInput.value;

            /* Ensure that the reply has value */
            if (replyMessage.length === 0) {
                return bootbox.alert("You must write a reply to send it to the traveler. 😡");
            }

            //* Get the value from the data attribute
            let PostId = button.attr("data-post-id");
            let UserId = button.attr("data-user-id");

            // Send the ajax request
            $.ajax({
                url: "/Agency/FAQ/reply",
                dataType: "json",
                type: "POST",
                data: {
                    postId: PostId,
                    userId: UserId,
                    reply: replyMessage
                },
                success: function(data) {
                    if (data.success) {

                        let dialog = bootbox.dialog({
                            title: 'Sending the reply to the traveler 🐱‍🏍',
                            message: '<p><i class="fa fa-spin fa-spinner"></i> Processing ⏳🕺🏻...</p>',
                            backdrop: true,
                            closeButton: true,
                            buttons: {
                                Ok: {
                                    label: "Ok",
                                    className: 'btn-success'
                                }
                            }
                        });

                        dialog.init(function() {
                            setTimeout(function() {
                                    //* if every thing is ok
                                    //* replace the text area
                                    replyInput.remove();

                                    //* Change the style of the button
                                    button.toggleClass("btn-danger");

                                    button.html("Closed 🛑");

                                    button.attr('disabled','disabled');

                                    //* Change the content of the div and add the reply
                                    replyForm.classList.remove("float-right");
                                    replyForm.innerHTML = '<p>' +
                                    '<span class="text-secondary font-weight-bold">' +
                                    'Replay by:' +
                                    '</span> ' +
                                    '<span class="text-primary font-weight-bold">' +
                                    'Agency' +
                                    '</span>' +
                                    '</p> ' +
                                    '<br/>' +
                                    '<h5>' +
                                    replyMessage +
                                    '</h5 >';

                                    //* Inform the user
                                    dialog.find('.bootbox-body').html(data.message + " 🦾");

                                },
                                1000);
                        });

                    } else {
                        bootbox.alert('Cannot reply to this question right now, please try again later 😐🔃!');
                    }
                },
                error: function(xhr) {
                    console.log(xhr);
                    bootbox.alert('Cannot reply to this question right now, please try again later 😐🔃!');
                }
            });

        });
});

/* function to get and return the textArea text */
function GetTextArea(button) {
    let card = button.parents(".card")[0]; // get the card parent
    let cardBody = card.children[0]; // get the card-body
    let replyForm = cardBody.children[5];// get the form-floating of the card body
    let replyInput = replyForm.children[0]; // get the text area

    return { replyInput, replyForm };
}
