"use strict";
/**
 * Author: TUNGTD
 * Created: 09/08/2023
 * Description: Login page javascript
 */
const accessKey = "Authorization";
/**
 * Author: TUNGTD
 * Created: 09/08/2023
 * Description: submit login
 */
$("#loginForm").on("submit", function (e) {
    e.preventDefault();
    signIn();
});
$(document).ready(function () {
    localStorage.removeItem("token");
})

async function signIn() {
    try {
        let data = {
            'userName': $("#username").val(),
            "password": $("#password").val()
        };
        let errors = [];
        let swalSubTitle = "<p class='swal__admin__subtitle'>Đăng nhập không thành công!</p>";
        if (data.userName.trim().length == 0) {
            errors.push("Tài khoản không được để trống");
        }
        if (data.password.trim().length == 0) {
            errors.push("Mật khẩu không được để trống");
        }
        if (errors.length > 0) {
            let contentError = "<ul>";
            errors.forEach(function (item, index) {
                contentError += "<li class='text-start'>" + item + "</li>";
            })
            contentError += "</ul>";
            Swal.fire(
                'Đăng nhập' + swalSubTitle,
                contentError,
                'warning'
            );
            return;
        }

        let result = await httpService.postAsync("/account/api/login", data);
        if (result.status == "200") {
            let token = result.resources.accessToken;
            let profile = result.resources.profile;
            localStorage.setItem("token", token);
            localStorage.setItem("profile", JSON.stringify(profile));
            document.cookie = `${accessKey}=${token}`;
            Swal.fire("Đăng nhập thành công", "Chào mừng <b>" + data.userName + "</b> trở lại.", "success").then(function () {
                location.href = "/admin";
            });
        } else if (result.status == "404") {
            Swal.fire("Đăng nhập không thành công", result.message, "warning");
        } else {
            if (result.errors.length > 1) {
                let contentError = "<ul>";
                result.errors.forEach(function (item, index) {
                    contentError += "<li class='text-start'>" + item + "</li>";
                })
                contentError += "</ul>";
                Swal.fire(
                    'Đăng nhập' + swalSubTitle,
                    contentError,
                    'warning'
                );
            } else {
                Swal.fire(
                    "Đăng nhập",
                    result.errors[0],
                    "error");
            }
        }
    } catch (e) {
        Swal.fire(
            "Đăng nhập",
            "Đã có lỗi xảy ra xin vui lòng thử lại sau!",
            "error");
        console.error(e);
    }
}
$("#btnLogin").on("click", function (e) {
    e.preventDefault();
    signIn();
})

$("#loginForm").on("input change keypress keydown", "input", function (e) {
    let text = $(this).val().trim();
    $(this).val(text);
    if (e.which == 13) {
        signIn();
    }
})
$(".none-space").on("change input blur", function () {
    let e = $(this);
    let text = e.val().trim();
    e.val(text);
})
$(".btn_show_pass").on("click", function (e) {
    var target = $($(this).attr("data-target"));
    if (target.attr("type") == "password") {
        target.attr("type", "text");
        $(this).html(`<i class="ki-duotone ki-eye-slash fs-3">
                                            <span class="path1 ki-uniEC07"></span>
                                            <span class="path2 ki-uniEC08"></span>
                                            <span class="path3 ki-uniEC09"></span>
                                            <span class="path4 ki-uniEC0A"></span>
                                        </i>`);
    } else {
        target.attr("type", "password");
        $(this).html(`<i class="ki-duotone ki-eye fs-3">
                                            <span class="path1 ki-uniEC0B"></span>
                                            <span class="path2 ki-uniEC0B"></span>
                                            <span class="path3 ki-uniEC0D"></span>
                                        </i>`);
    }
});

//Đăng nhập facebook

// Load SDK Facebook
window.fbAsyncInit = function () {
    FB.init({
        appId: '3028324723993554',
        cookie: true,
        xfbml: true,
        version: 'v22.0'
    });
    FB.AppEvents.logPageView();
    // Check login status khi SDK đã sẵn sàng
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) {
        return;
    }
    js = d.createElement(s);
    js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));


function statusChangeCallback(response) {
    console.log('Trạng thái đăng nhập:', response);
    if (response.status === 'connected') {
        console.log('Đã đăng nhập Facebook và ứng dụng.');
        fetchUserProfile();
    } else {
        console.log('Chưa đăng nhập ứng dụng hoặc Facebook.');
    }
}

function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}

function loginWithFacebook(event) {
    if (event) event.preventDefault();
    if (typeof FB === "undefined") {
        Swal.fire("Thông báo", "Facebook SDK đang tải, vui lòng đợi 1-2 giây!", "info");
        return;
    }

    FB.login(function (response) {
        if (response.authResponse) {
            console.log('Đăng nhập Facebook thành công!');
            fetchUserProfile();
        } else {
            console.log('Người dùng hủy đăng nhập.');
        }
    }, {scope: 'public_profile,email'});
}

function fetchUserProfile() {
    FB.api('/me', {fields: 'name,email'}, async function (userInfo) {
        console.log('Thông tin người dùng:', userInfo);

        try {
            let data = {
                email: userInfo.email,
                facebookId: userInfo.id,
                name: userInfo.name
            };

            // Gửi thông tin đến API Server để xử lý đăng nhập
            let result = await httpService.postAsync("/account/api/loginFacebook", data);

            if (result.status == "200") {
                let token = result.resources.accessToken;
                let profile = result.resources.profile;

                localStorage.setItem("token", token);
                localStorage.setItem("profile", JSON.stringify(profile));
                document.cookie = `${accessKey}=${token}`;

                Swal.fire("Đăng nhập thành công", "Chào mừng <b>" + userInfo.name + "</b> trở lại.", "success").then(function () {
                    location.href = "/admin";
                });

            } else {
                Swal.fire("Đăng nhập không thành công", result.message, "warning");
            }
        } catch (e) {
            Swal.fire("Đăng nhập", "Đã có lỗi xảy ra xin vui lòng thử lại sau!", "error");
            console.error(e);
        }
    });
}
