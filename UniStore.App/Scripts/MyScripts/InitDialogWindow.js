function InitDialogWindow() {

    $(function () {
        $("#result").dialog({
            autoOpen: false,
            width: "auto",
            height: "auto",
            modal: true
        });
    });

    function openPopup() {
        $("#result").dialog("open");
    }
}
