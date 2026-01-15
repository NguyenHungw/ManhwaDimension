document.getElementById("kt_sign_up_submit").addEventListener("click", function (e) {
    e.preventDefault(); // Ngăn submit trước khi check

    let hasError = false;

    const username = document.getElementById("user_username").value.trim();
    const password = document.getElementById("user_password").value.trim();
    const rePassword = document.getElementById("user_rePassword").value.trim();
    const firstName = document.getElementById("user_firstName").value.trim();
    const lastName = document.getElementById("user_lastName").value.trim();
    const email = document.getElementById("user_email").value.trim();

    // Ẩn tất cả thông báo cũ
    document.getElementById("error_user_username").style.display = "none";
    document.getElementById("error_user_password").style.display = "none";
    document.getElementById("error_user_rePassword").style.display = "none";
    document.getElementById("error_user_firstName").style.display = "none";
    document.getElementById("error_user_lastName").style.display = "none";
    document.getElementById("error_user_email").style.display = "none";

    // Kiểm tra từng trường
    if (username === "") {
        document.getElementById("error_user_username").innerText = "Vui lòng nhập tên đăng nhập";
        document.getElementById("error_user_username").style.display = "block";
        hasError = true;
    }
    if (password === "") {
        document.getElementById("error_user_password").innerText = "Vui lòng nhập mật khẩu";
        document.getElementById("error_user_password").style.display = "block";
        hasError = true;
    }
    if (rePassword === "") {
        document.getElementById("error_user_rePassword").innerText = "Vui lòng nhập lại mật khẩu";
        document.getElementById("error_user_rePassword").style.display = "block";
        hasError = true;
    }
    if (password !== "" && rePassword !== "" && password !== rePassword) {
        document.getElementById("error_user_rePassword").innerText = "Mật khẩu nhập lại không trùng";
        document.getElementById("error_user_rePassword").style.display = "block";
        hasError = true;
    }
    if (firstName === "") {
        document.getElementById("error_user_firstName").innerText = "Vui lòng nhập họ và tên đệm";
        document.getElementById("error_user_firstName").style.display = "block";
        hasError = true;
    }
    if (lastName === "") {
        document.getElementById("error_user_lastName").innerText = "Vui lòng nhập tên";
        document.getElementById("error_user_lastName").style.display = "block";
        hasError = true;
    }
    if (email === "") {
        document.getElementById("error_user_email").innerText = "Vui lòng nhập email";
        document.getElementById("error_user_email").style.display = "block";
        hasError = true;
    }

    if (!hasError) {
        document.getElementById("kt_sign_up_form").submit();
    }
});
