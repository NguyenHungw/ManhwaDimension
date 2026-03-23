'use strict';

(function () {
    var cameraStream = null;
    var scanning = false;
    var scanCanvas = null;
    var scanCtx = null;
    var redirectTimeout = null;
    var isSubmitting = false;

    /**
     * Sao chép nội dung ô nhập mã QR vào clipboard.
     */
    function copyQrCode() {
        var input = document.getElementById('qrCodeInput');
        if (!input) {
            console.warn('Không tìm thấy ô nhập mã QR (#qrCodeInput)');
            return;
        }

        var value = input.value || '';
        if (!value) {
            alert('Không có mã để sao chép.');
            return;
        }

        if (navigator.clipboard && navigator.clipboard.writeText) {
            navigator.clipboard.writeText(value)
                .then(function () {
                    alert('Đã sao chép mã QR vào clipboard.');
                })
                .catch(function () {
                    alert('Không sao chép được mã. Vui lòng thử lại.');
                });
        } else {
            // Fallback cho trình duyệt cũ
            input.select();
            try {
                var ok = document.execCommand('copy');
                alert(ok ? 'Đã sao chép mã QR vào clipboard.' : 'Không sao chép được mã. Vui lòng thử lại.');
            } catch (e) {
                alert('Trình duyệt không hỗ trợ sao chép tự động.');
            }
        }
    }

    /**
     * Mở hộp thoại chọn ảnh QR.
     */
    function openUploadDialog() {
        var fileInput = document.getElementById('qrImageInput');
        if (!fileInput) {
            console.warn('Không tìm thấy input file (#qrImageInput)');
            return;
        }
        fileInput.click();
    }

    /**
     * Xử lý sau khi người dùng chọn ảnh QR.
     * (Hiện tại vẫn là demo, chưa decode QR từ file ảnh.)
     */
    function handleQrImageSelected(event) {
        var files = event.target.files;
        if (!files || !files.length) return;

        var file = files[0];
        var statusEl = document.querySelector('.fs-12px.text-center.mt-3');
        if (statusEl) {
            statusEl.textContent = 'Đang xử lý ảnh: ' + file.name + ' ...';
        }

        // TODO: Gửi file lên API để nhận diện QR từ ảnh.
        // Tạm thời chỉ mô phỏng quá trình quét.
        setTimeout(function () {
            if (statusEl) {
                statusEl.textContent = 'Đang chờ quét... (demo: chưa gọi API thật)';
            }
        }, 1500);
    }

    /**
     * Dừng camera (khi đã quét được mã).
     */
    function stopCamera() {
        if (cameraStream) {
            cameraStream.getTracks().forEach(function (t) { t.stop(); });
            cameraStream = null;
        }
        scanning = false;
        // Hủy timeout chuyển hướng nếu có
        if (redirectTimeout) {
            clearTimeout(redirectTimeout);
            redirectTimeout = null;
        }
    }

    /**
     * Bật camera và hiển thị lên thẻ <video id="qrCamera">,
     * đồng thời chạy vòng lặp để nhận diện mã QR bằng jsQR.
     */
    function initCamera() {
        var video = document.getElementById('qrCamera');
        if (!video || !navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
            console.warn('Trình duyệt không hỗ trợ camera hoặc không tìm thấy #qrCamera');
            return;
        }

        navigator.mediaDevices.getUserMedia({
            video: { facingMode: 'environment' } // ưu tiên camera sau trên mobile
        }).then(function (stream) {
            cameraStream = stream;
            video.srcObject = stream;
            scanning = true;

            // Chuẩn bị canvas để đọc frame
            scanCanvas = document.createElement('canvas');
            scanCtx = scanCanvas.getContext('2d');

            requestAnimationFrame(scanLoop);

            //// Sau 5 giây, chuyển hướng đến /profile/admin/list
            //redirectTimeout = setTimeout(function () {


            //    Swal.fire({
            //        title: "Đồng bộ thành công",
            //        text: "Đã đồng bộ dữ liệu user từ QR code thành công",
            //        icon: "success",
            //        showCancelButton: true,
            //        confirmButtonText: "OK",
            //    }).then((result) => {
            //        window.location.href = '/profile/admin/list';
            //    });

            //}, 5000);
        }).catch(function (err) {
            console.error('Không truy cập được camera:', err);
            alert('Không mở được camera. Vui lòng kiểm tra quyền truy cập.');
        });
    }

    /**
     * Vòng lặp đọc khung hình từ camera và nhận diện QR bằng jsQR.
     */
    function scanLoop() {
        if (!scanning) return;

        var video = document.getElementById('qrCamera');
        if (!video || video.readyState !== video.HAVE_ENOUGH_DATA) {
            requestAnimationFrame(scanLoop);
            return;
        }

        if (!scanCanvas || !scanCtx) {
            requestAnimationFrame(scanLoop);
            return;
        }

        scanCanvas.width = video.videoWidth;
        scanCanvas.height = video.videoHeight;
        scanCtx.drawImage(video, 0, 0, scanCanvas.width, scanCanvas.height);

        try {
            var imageData = scanCtx.getImageData(0, 0, scanCanvas.width, scanCanvas.height);
            if (window.jsQR) {
                var code = jsQR(imageData.data, imageData.width, imageData.height);
                if (code && code.data) {
                    handleScannedValue(code.data);
                    return;
                }
            }
        } catch (e) {
            console.warn('Lỗi khi xử lý khung hình QR:', e);
        }

        requestAnimationFrame(scanLoop);
    }

    function handleScannedValue(rawValue) {
        if (!rawValue) return;

        var input = document.getElementById('qrCodeInput');
        if (input) {
            input.value = rawValue;
        }

        var statusEl = document.querySelector('.fs-12px.text-center.mt-3');
        if (statusEl) {
            statusEl.textContent = 'Đã đọc được mã QR. Đang gửi lên hệ thống...';
        }

        stopCamera();
        submitScan(rawValue);
    }

    function extractTokenOrProfile(value) {
        if (!value) return {};

        // Nếu là URL, lấy query params
        try {
            var url = new URL(value);
            var token = url.searchParams.get('token');
            var profileId = url.searchParams.get('profileId');
            if (token) return { token: token };
            if (profileId && !isNaN(profileId)) return { profileId: Number(profileId) };
        } catch (e) {
            // không phải URL, fallback bên dưới
        }

        // Nếu chuỗi dạng token (guid) hoặc chỉ chứa token
        if (/^[a-fA-F0-9]{32}$/.test(value) || /^[a-fA-F0-9-]{36}$/.test(value)) {
            return { token: value };
        }

        // Nếu là số, coi là profileId (luồng cũ)
        if (!isNaN(value)) {
            return { profileId: Number(value) };
        }

        return {};
    }

    function submitScan(rawValue) {
        if (isSubmitting) return;
        var statusEl = document.querySelector('.fs-12px.text-center.mt-3');

        var parsed = extractTokenOrProfile(rawValue);
        if (!parsed.token && !parsed.profileId) {
            if (statusEl) statusEl.textContent = 'Mã không hợp lệ. Vui lòng thử lại.';
            Swal.fire('Lỗi!', 'Mã QR không hợp lệ.', 'error');
            return;
        }

        isSubmitting = true;

        $.ajax({
            url: systemURL + "hospital/api/scan-qr",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(parsed),
            success: function () {
                if (statusEl) statusEl.textContent = 'Đã cập nhật chia sẻ hồ sơ cho bệnh viện.';
                Swal.fire({
                    title: "Đồng bộ thành công",
                    text: "Đã cập nhật chia sẻ hồ sơ cho bệnh viện.",
                    icon: "success",
                    confirmButtonText: "OK"
                }).then(function () {
                    window.location.href = '/profile/admin/list';
                });
            },
            error: function (e) {
                if (statusEl) statusEl.textContent = 'Không gửi được dữ liệu quét. Vui lòng thử lại.';
                Swal.fire(
                    'Lỗi!',
                    'Không gửi được dữ liệu quét. Vui lòng thử lại.',
                    'error'
                );
                console.error(e);
                // Cho phép quét lại
                isSubmitting = false;
                initCamera();
            }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        initCamera();
        var btnCopy = document.getElementById('btnCopyQrCode');
        if (btnCopy) {
            btnCopy.addEventListener('click', copyQrCode);
        }

        var btnUpload = document.getElementById('btnUploadQrImage');
        if (btnUpload) {
            btnUpload.addEventListener('click', openUploadDialog);
        }

        var fileInput = document.getElementById('qrImageInput');
        if (fileInput) {
            fileInput.addEventListener('change', handleQrImageSelected);
        }

        // Cho phép dán/nhập thủ công rồi Enter để gửi
        var input = document.getElementById('qrCodeInput');
        if (input) {
            input.addEventListener('keyup', function (e) {
                if (e.key === 'Enter') {
                    handleScannedValue(input.value);
                }
            });
        }
    });
})();


