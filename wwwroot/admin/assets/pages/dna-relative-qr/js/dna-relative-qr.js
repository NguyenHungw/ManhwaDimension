'use strict';

(function () {
    /**
     * Tải QR dưới dạng file PNG.
     */
    function downloadQr() {
        var qrEl = document.getElementById('qrSvg');
        if (!qrEl) {
            console.warn('Không tìm thấy phần tử #qrSvg');
            return;
        }

        // Nếu là <img>, tải ảnh QR về dạng file PNG
        if (qrEl.tagName && qrEl.tagName.toLowerCase() === 'img') {
            var imgUrl = qrEl.src;
            if (!imgUrl) {
                console.warn('Ảnh QR không có src');
                alert('Ảnh QR chưa được tải. Vui lòng thử lại sau.');
                return;
            }

            // Dùng fetch để lấy blob rồi tải về, tránh một số hạn chế với download attribute
            fetch(imgUrl)
                .then(function (res) { return res.blob(); })
                .then(function (blob) {
                    var url = URL.createObjectURL(blob);
                    var aImg = document.createElement('a');
                    aImg.href = url;
                    aImg.download = 'qr-code.png';
                    document.body.appendChild(aImg);
                    aImg.click();
                    document.body.removeChild(aImg);
                    URL.revokeObjectURL(url);
                })
                .catch(function (err) {
                    console.error('Lỗi khi tải ảnh QR:', err);
                    alert('Không tải được ảnh QR. Vui lòng thử lại sau.');
                });
            return;
        }

        // Trường hợp còn lại: giả sử là <svg> giống code cũ
        var serializer = new XMLSerializer();
        var source = serializer.serializeToString(qrEl);

        if (!source.match(/^<\?xml/)) {
            source = '<?xml version="1.0" standalone="no"?>\r\n' + source;
        }

        var blob = new Blob([source], { type: 'image/svg+xml;charset=utf-8' });
        var url = URL.createObjectURL(blob);

        var a = document.createElement('a');
        a.href = url;
        a.download = 'qr-code.svg';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }

    /**
     * Bộ đếm ngược thời gian từ 60 giây về 0.
     */
    function initCountdown() {
        var countdownElement = document.getElementById('countdownTimer');
        var countdownTextElement = document.getElementById('qrCountdownText');
        if (!countdownElement || !countdownTextElement) return;

        var timeLeft = 60; // 60 giây = 1 phút

        function updateCountdown() {
            if (timeLeft <= 0) {
                // Thay đổi text khi hết thời gian
                clearInterval(countdownInterval);
                countdownTextElement.textContent = 'QR đã hết hạn';
            } else {
                countdownElement.textContent = timeLeft;
                timeLeft--;
            }
        }

        // Cập nhật ngay lập tức
        updateCountdown();
        
        // Cập nhật mỗi giây
        var countdownInterval = setInterval(updateCountdown, 1000);
    }

    /**
     * Chia sẻ đường dẫn hoặc mã QR (sử dụng Web Share API nếu có).
     * Ở đây tạm thời chia sẻ URL hiện tại của trang.
     */
    function shareQr() {
        var shareData = {
            title: document.title || 'QR Code',
            text: 'Liên kết chứa trong mã QR của tôi.',
            url: window.location.href
        };

        if (navigator.share) {
            navigator.share(shareData).catch(function (err) {
                console.warn('Share bị huỷ hoặc lỗi:', err);
            });
        } else {
            // Fallback: copy URL vào clipboard
            if (navigator.clipboard && navigator.clipboard.writeText) {
                navigator.clipboard.writeText(shareData.url)
                    .then(function () {
                        alert('Đã sao chép liên kết để chia sẻ.');
                    })
                    .catch(function () {
                        alert('Không sao chép được liên kết. Vui lòng tự copy trên thanh địa chỉ.');
                    });
            } else {
                alert('Trình duyệt không hỗ trợ chức năng chia sẻ. Vui lòng tự copy liên kết trên thanh địa chỉ.');
            }
        }
    }

    document.addEventListener('DOMContentLoaded', function () {
        // Xử lý nút tải xuống
        var btnDownload = document.getElementById('btnDownloadQr');
        if (btnDownload) {
            btnDownload.addEventListener('click', downloadQr);
        }

        // Xử lý nút chia sẻ
        var btnShare = document.getElementById('btnShareQr');
        if (btnShare) {
            btnShare.addEventListener('click', shareQr);
        }

        // Xử lý sidebar icon click
        var sidebarIcon = document.getElementById('dnaQrSidebarShortcut') || document.getElementById('dnaQrSidebarIcon');
        if (sidebarIcon) {
            sidebarIcon.style.cursor = 'pointer';
            sidebarIcon.addEventListener('click', function () {
                window.location.href = '/qr-code';
            });
        }

        // Khởi tạo bộ đếm ngược
        initCountdown();
    });
})();


