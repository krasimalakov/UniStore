function InitImageFormGroup() {

    $(function() {
        $("#upload-file-info").click(function() {
            $("#Image").click();
        });
        $("#Image").bind("change",
            function() {
                previewImage(this);
                $("#upload-file-info").text("Change image for product");
            });
    });

    function previewImage(input) {
        if (input.files && input.files[0]) {
            const reader = new FileReader();

            reader.onload = function(e) {
                $("#product-image").attr("src", e.target.result);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }
}
