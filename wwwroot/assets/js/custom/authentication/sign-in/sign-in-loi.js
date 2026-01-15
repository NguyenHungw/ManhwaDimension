document.getElementById("kt_sign_in_submit").addEventListener("click", function (e) {
    e.preventDefault(); // Ngăn form submit trước khi check

    const userName = document.getElementById("user_name").value.trim();
    const password = document.getElementById("password").value.trim();
    let hasError = false;

    // Ẩn thông báo cũ
    document.getElementById("error_user_name").style.display = "none";
    document.getElementById("error_password").style.display = "none";

    // Kiểm tra tên đăng nhập
    if (userName === "") {
        document.getElementById("error_user_name").innerText = "Vui lòng điền tên đăng nhập";
        document.getElementById("error_user_name").style.display = "block";
        hasError = true;
    }

    // Kiểm tra mật khẩu
    if (password === "") {
        document.getElementById("error_password").innerText = "Vui lòng điền mật khẩu";
        document.getElementById("error_password").style.display = "block";
        hasError = true;
    }

    // Submit form nếu không lỗi
    if (!hasError) {
        document.getElementById("kt_sign_in_form").submit();
    }
});
