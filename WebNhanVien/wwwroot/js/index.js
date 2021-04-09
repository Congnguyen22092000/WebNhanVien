



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

    $("#hoTen").blur(function () {
        console.log($("#maNhanVien").val());
        var maNhanVien = $("#maNhanVien").val();
        var hoTen = $("#hoTen").val();
        var ngaySinh = $("#ngaySinh").val();
        var soDT = $("#soDT").val();

        var chucVu = $("#chucVu").val();

        $.ajax({
            type: "Post",
            url: "/staff/IsDuplicatedStaff",
            data: { maNhanVien: maNhanVien, hoTen: hoTen, ngaySinh: ngaySinh, soDT: soDT, chucVu: chucVu },
            dataType: "json",
            success: function (json) {
                var a = json;
                if (a == true) {
                    $("#SubmitBtn").prop("disabled", true);
                    $("#ErrorMgs").text("Nhân Viên Đã Tồn Tại");
                    $("#hoTen").css("border-color", "red");
                    $("#ngaySinh").css("border-color", "red");
                }
                if (a == false) {
                    $("#SubmitBtn").prop('disabled', false);
                    $("#ErrorMgs").text("");
                    $("#hoTen").css("border-color", "#ced4da");
                    $("#ngaySinh").css("border-color", "#eff1f3");
                }
            },
            error: function (req, status, error) {
                console.log(error);

            }
        });
    });
    $("#ngaySinh").blur(function () {
        var maNhanVien = $("#maNhanVien").val();
        var hoTen = $("#hoTen").val();
        var ngaySinh = $("#ngaySinh").val();
        var soDT = $("#soDT").val();

        var chucVu = $("#chucVu").val();

        $.ajax({
            type: "Post",
            url: "/staff/IsDuplicatedStaff",
            data: { maNhanVien: maNhanVien, hoTen: hoTen, ngaySinh: ngaySinh, soDT: soDT, chucVu: chucVu },
            dataType: "json",
            success: function (json) {
                var a = json;
                if (a == true) {
                    $("#SubmitBtn").prop("disabled", true);
                    $("#ErrorMgs").text("Nhân Viên Đã Tồn Tại");
                    $("#hoTen").css("border-color", "red");
                    $("#ngaySinh").css("border-color", "red");
                }
                if (a == false) {
                    $("#SubmitBtn").prop('disabled', false);
                    $("#CreateForm").unbind(event)
                    $("#ErrorMgs").text("");
                    $("#hoTen").css("border-color", "ced4da");
                    $("#ngaySinh").css("border-color", "ced4da");
                }
            },
            error: function (req, status, error) {
                console.log(error);

            }
        });
    });
});




