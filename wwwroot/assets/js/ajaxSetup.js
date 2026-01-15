"use strict";
// Cấu hình mặc định cho AJAX
$.ajaxSetup({
    beforeSend: function (xhr) {
        if (localStorage.token) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + localStorage.token);
        }
    },
    error: function (jqXHR) {
        // Bắt lỗi từ backend và hiển thị bằng SweetAlert
        if (jqXHR.responseJSON && jqXHR.responseJSON.message) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: jqXHR.responseJSON.message,
                confirmButtonText: 'OK'
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: 'Đã có lỗi xảy ra, vui lòng thử lại.',
                confirmButtonText: 'OK'
            });
        }
    }
});