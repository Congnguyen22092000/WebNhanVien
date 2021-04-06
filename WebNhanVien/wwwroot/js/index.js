$(document).ready(function () {
    $("input[name='key']").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#StaffTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $(".delBtn").click(function (event) {
        event.preventDefault();
        var maNhanVien = $(this).attr("id");

        $.ajax({
            type: "Post",
            url: "/staff/delete",
            data: { maNhanVien: maNhanVien },
            dataType: "json",
            success: function (json) {
                $("#Huy-" + maNhanVien).trigger("click");
                $("#" + maNhanVien).closest("tr").remove();
            },
            error: function (req, status, error) {
                console.log(error);

            }
        });

    });
});