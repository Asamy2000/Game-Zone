//$(document).ready(function () {
//    $('#Cover').on('change', function () {
//        $('.cover-preview').attr('src', window.URL.createObjectURL(this.files[0]));
//    });
//});


$(document).ready(function () {
    $('#Cover').on('change', function () {
        var coverPreview = $('.cover-preview');
        var fileInput = this;

        if (fileInput.files.length > 0) {
            // If a file is selected, display it in the image preview
            coverPreview.attr('src', window.URL.createObjectURL(fileInput.files[0]));
            coverPreview.removeClass('d-none'); // Remove the "d-none" class to show the image
        } else {
            // If no file is selected, hide the image preview
            coverPreview.attr('src', ''); // Clear the image source
            coverPreview.addClass('d-none'); // Add the "d-none" class to hide the image
        }
    });
});




