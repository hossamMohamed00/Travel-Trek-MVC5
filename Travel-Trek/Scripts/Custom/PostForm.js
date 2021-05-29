/* This method will be called after success of the ajax request */
function InformUser(data) {

    console.log(data);

    if (data.success) {
        console.log("Success!");

        var dialog = bootbox.dialog({
            title: "Trip Post🐱‍🏍",
            message: '<p><i class="fa fa-spin fa-spinner"></i> Processing 👀...</p>',
            backdrop: true,
            buttons: {
                Ok: {
                    label: "Ok",
                    className: 'btn-success',
                    callback: function() {
                        window.location.replace("/Agency/");
                    }
                }
            }
        });

        dialog.init(function () {
            setTimeout(function () {
                    //* Target the dialog body and put the success msg
                    dialog.find('.bootbox-body').html(data.message);
                    dialog.closeButton = true;
                    //* Target the form and empty it
                    document.getElementById("formData").reset();
                },
                1000);
        });

    } else {
        bootbox.alert(data.message);
    }

};
