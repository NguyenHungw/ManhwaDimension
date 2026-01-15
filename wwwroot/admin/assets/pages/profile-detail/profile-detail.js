'use strict';
(function () {
    const ProfileDetailPage = {
        profileInfo: null,
        profileAccount: null,
        infoBindings: [
            { id: 'code', key: 'tongQuan.maHoSo' },
            { id: 'fullname', key: 'fullName' },
            { id: 'age', key: 'age' },
            {
                id: 'dob', key: 'dateOfBirth', formatter: value => {
                    return value ? moment(new Date(value)).format('DD/MM/YYYY') : '--';
                }
            },
            {
                id: 'gender',
                key: 'sex',
                useHtml: true,
                formatter: value => {
                    if (!value) return '';
                    if (value.toLowerCase() === 'nam') return `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 16 16" fill="none">
                  <path opacity="0.4" d="M12.0003 9.16667C12.0003 12.02 9.68699 14.3333 6.83366 14.3333C3.98033 14.3333 1.66699 12.02 1.66699 9.16667C1.66699 6.31333 3.98033 4 6.83366 4C8.08033 4 9.22032 4.44 10.1137 5.18H10.1203C10.3803 5.39333 10.6137 5.62667 10.8203 5.88667C11.5603 6.78 12.0003 7.92 12.0003 9.16667Z" fill="url(#paint0_linear_1275_15614)"/>
                  <path d="M14.8333 1.66699V6.00033C14.8333 6.27366 14.6067 6.50033 14.3333 6.50033C14.06 6.50033 13.8333 6.27366 13.8333 6.00033V2.87366L10.82 5.88699C10.6133 5.62699 10.38 5.39366 10.12 5.18033L13.1267 2.16699H10C9.72667 2.16699 9.5 1.94033 9.5 1.66699C9.5 1.39366 9.72667 1.16699 10 1.16699H14.3333C14.4 1.16699 14.46 1.18033 14.5267 1.20699C14.6467 1.25366 14.7467 1.35366 14.7933 1.47366C14.82 1.54033 14.8333 1.60033 14.8333 1.66699Z" fill="url(#paint1_linear_1275_15614)"/>
                  <defs>
                    <linearGradient id="paint0_linear_1275_15614" x1="1.66699" y1="9.94167" x2="6.83366" y2="9.16667" gradientUnits="userSpaceOnUse">
                      <stop stop-color="#509FE3"/>
                      <stop offset="1" stop-color="#4174D1"/>
                    </linearGradient>
                    <linearGradient id="paint1_linear_1275_15614" x1="9.5" y1="4.23366" x2="12.1667" y2="3.83366" gradientUnits="userSpaceOnUse">
                      <stop stop-color="#509FE3"/>
                      <stop offset="1" stop-color="#4174D1"/>
                    </linearGradient>
                  </defs>
                </svg>`;
                    return `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 16 16" fill="none">
                  <path opacity="0.4" d="M12.6663 5.99967C12.6663 8.41301 10.8397 10.393 8.49967 10.6397C8.33301 10.6597 8.16634 10.6663 7.99967 10.6663C7.83301 10.6663 7.66634 10.6597 7.49967 10.6397C5.15967 10.393 3.33301 8.41301 3.33301 5.99967C3.33301 3.41967 5.41967 1.33301 7.99967 1.33301C10.5797 1.33301 12.6663 3.41967 12.6663 5.99967Z" fill="url(#paint0_linear_1275_15609)"/>
                  <path d="M10.5 12.6663C10.5 12.9396 10.2733 13.1663 10 13.1663H8.5V14.6663C8.5 14.9396 8.27333 15.1663 8 15.1663C7.72667 15.1663 7.5 14.9396 7.5 14.6663V13.1663H6C5.72667 13.1663 5.5 12.9396 5.5 12.6663C5.5 12.393 5.72667 12.1663 6 12.1663H7.5V10.6396C7.66667 10.6596 7.83333 10.6663 8 10.6663C8.16667 10.6663 8.33333 10.6596 8.5 10.6396V12.1663H10C10.2733 12.1663 10.5 12.393 10.5 12.6663Z" fill="url(#paint1_linear_1275_15609)"/>
                  <defs>
                    <linearGradient id="paint0_linear_1275_15609" x1="3.33301" y1="6.69967" x2="7.99967" y2="5.99967" gradientUnits="userSpaceOnUse">
                      <stop stop-color="#509FE3"/>
                      <stop offset="1" stop-color="#4174D1"/>
                    </linearGradient>
                    <linearGradient id="paint1_linear_1275_15609" x1="5.5" y1="13.2425" x2="7.98795" y2="12.8303" gradientUnits="userSpaceOnUse">
                      <stop stop-color="#509FE3"/>
                      <stop offset="1" stop-color="#4174D1"/>
                    </linearGradient>
                  </defs>
                </svg>`;
                }
            },
            { id: 'address', key: 'address' },
            { id: 'specialty', key: 'tongQuan.khoaChuyenKhoa' },
            { id: 'disease', key: 'tongQuan.benhNhomBenh' },
            {
                id: 'date_of_examination', key: 'tongQuan.ngayKhamGanNhat', formatter: value => {
                    return value ? moment(new Date(value)).format('DD/MM/YYYY') : '--';
                }
            },
            { id: 'doctor', key: 'tongQuan.bacSiDieuTriChinh' },
            { id: 'status', key: 'tongQuan.trangThaiHoSo' },
            {
                id: 'isSyncADN', key: 'hasSyncedShare', useHtml: true, formatter: value => {
                    return value ? `<input class="form-check-input " style="accent-color: green;" type="checkbox" checked="checked" disabled="true">`
                        : 'Chưa đồng bộ';
                }
            }
        ],
        familyDataset: [],
        familyTable: null,
        testResultDataset: [],
        testResultTable: null,
        adnDataSet: [],
        adnTable: null,
        adnIsSyncADN: false,
        listDiseaseDefinition: null,
        // QR Scanner properties
        cameraStream: null,
        scanning: false,
        scanCanvas: null,
        scanCtx: null,
        isSubmitting: false,
        init: async function () {
            await this.loadData();
            await this.bindEvent();
            await this.initQrScanner();
        },
        loadData: async function () {
            await ProfileDetailPage.loadDataDiseaseDefinition();
            await ProfileDetailPage.loadDataDetail();
            await ProfileDetailPage.renderDataDetail();
            await ProfileDetailPage.initFamilyHistoryTable();
            await ProfileDetailPage.initTestResultTable();
            await ProfileDetailPage.initAdnResultTable();
        },
        loadDataDetail: async function () {
            await $.ajax({
                url: '/profile/api/detail-by-account/' + accountId,
                method: 'GET',
                success: function (response) {
                    if (response.status === "200") {
                        var result = response.data[0];
                        ProfileDetailPage.profileInfo = {
                            age: new Date().getFullYear() - new Date(result.dateOfBirth).getFullYear(),
                            ...result
                        };
                        //Gán dữ liệu bảng kết quả khám sức khỏe
                        ProfileDetailPage.familyDataset = [{
                            ...result.ketQuaKhamSucKhoeTongQuat
                        }];
                        //Gán dữ liệu bảng kết quả xét nghiệm
                        ProfileDetailPage.testResultDataset = result.ketQuaXetNghiem;

                        ProfileDetailPage.adnIsSyncADN = result.hasSyncedShare;
                    }
                },
                error: function (err) {
                    ProfileDetailPage.profileInfo = null;
                }
            });
        },
        renderDataDetail: function () {
            const data = this.profileInfo;

            this.infoBindings.forEach(binding => {
                const el = document.getElementById(binding.id);
                if (!el) return;

                const rawValue = getValueByPath(data, binding.key);

                const value = typeof binding.formatter === 'function'
                    ? binding.formatter(rawValue)
                    : (rawValue ?? '--');

                if (binding.useHtml) {
                    el.innerHTML = value || '--';
                } else {
                    el.textContent = value || '--';
                }
            });
        },
        loadProfileInfo: async function () {
            var data = [];
            await $.ajax({
                url: `/profile/api/statistics/${accountId}`,
                method: 'GET',
                async: true,
                success: function (response) {
                    if (response.status === "200") {
                        data = response.resources;
                    }
                }
            });
            ProfileDetailPage.profileAccount = data;
            return data;
        },
        initFamilyHistoryTable: function () {
            const tableElement = $('#familyHistoryTable');
            if (!tableElement.length) return;
            const self = this;
            this.familyTable = tableElement.DataTable({
                processing: true,
                serverSide: false,
                paging: true,
                searching: false,
                ordering: true,
                data: self.familyDataset,
                columns: [
                    {
                        data: 'chieuCaoCm', render: function (data) {
                            return data ? data + 'cm' : '--';
                        }
                    },
                    {
                        data: 'canNangKg', render: function (data) {
                            return data ? data + 'kg' : '--';
                        }
                    },
                    {
                        data: 'chiSoBMI', render: function (data) {
                            return data ? data + 'kg/m2' : '--';
                        }
                    },
                    {
                        data: 'vongNgucCm', render: function (data) {
                            return data ? data + 'cm' : '--';
                        }
                    },
                    {
                        data: 'machLanPhut', render: function (data) {
                            return data ? data + 'lần/phút' : '--';
                        }
                    },
                    {
                        data: 'huyetApMmHg', render: function (data) {
                            return data ? data + 'mmHg' : '--';
                        }
                    },
                    {
                        data: 'vongBungCm', render: function (data) {
                            return data ? data + 'cm' : '--';
                        }
                    },
                ],
                order: [[0, 'asc']],
                pageLength: 5,
                lengthMenu: [
                    [5, 10, 20, 100],
                    [5, 10, 20, 100]
                ],
                language: AppSettings.dataTableLanguage.vi,
                drawCallback: function () {

                }
            });
        },
        initTestResultTable: function () {
            const tableElement = $('#test_result_table');
            if (!tableElement.length) return;
            const self = this;
            this.testResultTable = tableElement.DataTable({
                processing: true,
                serverSide: false,
                paging: true,
                searching: false,
                ordering: true,
                data: self.testResultDataset,
                columns: [
                    {
                        data: 'dichVu', render: function (data) {
                            return data ? data : '--';
                        }
                    },
                    {
                        data: 'ketQua', render: function (data) {
                            return data ? data : '--';
                        }
                    },
                    {
                        data: 'thamChieu', render: function (data) {
                            return data ? data : '--';
                        }
                    },
                    {
                        data: 'donVi', render: function (data) {
                            return data ? data : '--';
                        }
                    },
                ],
                order: [[0, 'asc']],
                pageLength: 5,
                lengthMenu: [
                    [5, 10, 20, 100],
                    [5, 10, 20, 100]
                ],
                language: AppSettings.dataTableLanguage.vi,
                drawCallback: function () {

                }
            });
        },
        initAdnResultTable: async function () {
            const tableElement = $('#adn_table');
            const syncElemetn = $("#list-disease-definition-status");
            const tabElement = $('.left-buttons-wrapper');
            if (!tableElement.length) return;
            const self = this;
            if (!ProfileDetailPage.adnIsSyncADN) {
                tableElement.hide();
                syncElemetn.removeClass('d-none').addClass('d-flex');
                tabElement.hide();
                return;
            }
            syncElemetn.removeClass('d-flex').addClass('d-none');

            //Load dữ liệu
            var profileInfoData = await ProfileDetailPage.loadProfileInfo();
            self.adnDataSet = profileInfoData.scores;
            console.log(self.adnDataSet);
            this.adnTable = tableElement.DataTable({
                processing: true,
                serverSide: false,
                paging: true,
                searching: false,
                ordering: true,
                data: self.adnDataSet,
                columns: [
                    {
                        data: 'stt',
                        className: 'mw-50px text-center',
                        render: function (data, type, row, meta) {
                            var stt = meta.row + 1;
                            return stt; // This contains the row index
                        }
                    },
                    {
                        data: 'diseaseKey', render: function (data) {
                            return data ? getDiseaseInfo(data) : '--';
                        }
                    },
                    {
                        data: 'riskLevel', 
                        className: 'text-center mw-100px',
                        render: function (data) {
                            if (!data) return '--';
                            const riskText = getRiskText(data);
                            const badgeColor = getRiskBadgeColor(data);
                            return `<span class="badge" style="background: ${badgeColor}; color: #fff; padding: 4px 12px; border-radius: 4px;">
                                ${riskText}
                            </span>`;
                        }
                    },
                    {
                        data: 'diseaseKey',
                        className: 'text-center mw-100px',
                        render: function (data) {
                            return `<a href="javascript:void(0)" class="btn btn-sm btn-primary bg-primary-custom p-2" data-id="${accountId}" data-d='${data}' title="Chi tiết" name="viewdetail">
                                    <div class="d-flex align-items-center bg">
                                       <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 14 14" fill="none">
                                          <path opacity="0.4" d="M9.44425 1.16699H4.55591C2.43258 1.16699 1.16675 2.43283 1.16675 4.55616V9.43866C1.16675 11.5678 2.43258 12.8337 4.55591 12.8337H9.43841C11.5617 12.8337 12.8276 11.5678 12.8276 9.44449V4.55616C12.8334 2.43283 11.5676 1.16699 9.44425 1.16699Z" fill="white"></path>
                                          <path d="M10.8092 6.6909L8.30667 4.1884C8.1375 4.01923 7.8575 4.01923 7.68833 4.1884C7.51917 4.35757 7.51917 4.63757 7.68833 4.80673L9.44417 6.56257H3.5C3.26083 6.56257 3.0625 6.7609 3.0625 7.00007C3.0625 7.23923 3.26083 7.43757 3.5 7.43757H9.44417L7.68833 9.1934C7.51917 9.36257 7.51917 9.64256 7.68833 9.81173C7.77583 9.89923 7.88667 9.94007 7.9975 9.94007C8.10833 9.94007 8.21917 9.89923 8.30667 9.81173L10.8092 7.30923C10.8908 7.22757 10.9375 7.11673 10.9375 7.00007C10.9375 6.8834 10.8908 6.77257 10.8092 6.6909Z" fill="white"></path>
                                        </svg>
                                    </div>
                                </a>`;
                        }
                    },
                ],
                order: [[0, 'asc']],
                pageLength: 5,
                lengthMenu: [
                    [5, 10, 20, 100],
                    [5, 10, 20, 100]
                ],
                language: AppSettings.dataTableLanguage.vi,
                drawCallback: function () {

                }
            });
        },
        loadDataDiseaseDefinition: function () {
            return $.ajax({
                url: '/diseaseDefinition/api/list',
                method: 'GET',
                async: true,
                success: function (response) {
                    if (response.status === "200") {
                        ProfileDetailPage.listDiseaseDefinition = response.data;
                    }
                }
            });
        },
        bindEvent: function () {
            const self = this;
            // Bind event cho nút đồng bộ dữ liệu
            $('#btn-sync-dna-data').on('click', function () {
                const modalEl = document.getElementById('qrScanModal');
                if (modalEl && window.bootstrap) {
                    const bsModal = new bootstrap.Modal(modalEl);
                    bsModal.show();
                }
            });
            ProfileDetailPage.bindEventViewDetail();
        },
        bindEventViewDetail: function () {
            // Dùng event delegation để bắt click sau khi DataTable paginate
            $(document).on('click', 'a[name=viewdetail]', function () {
                ProfileDetailPage.detailResult($(this).data('d'), $(this).data('id'));
            });
        },
        initQrScanner: function () {
            const self = this;
            const modalEl = document.getElementById('qrScanModal');
            if (!modalEl) return;

            // Chỉ init camera khi modal mở
            modalEl.addEventListener('shown.bs.modal', function () {
                self.initCamera();
                const statusEl = document.querySelector('#qrScanModal .fs-12px.text-center');
                if (statusEl) statusEl.textContent = 'Đang chờ quét...';
                const input = document.getElementById('qrCodeInput');
                if (input) input.value = '';
            });
            modalEl.addEventListener('hidden.bs.modal', function () {
                self.stopCamera();
                self.isSubmitting = false;
                const statusEl = document.querySelector('#qrScanModal .fs-12px.text-center');
                if (statusEl) statusEl.textContent = 'Đang chờ quét...';
                const input = document.getElementById('qrCodeInput');
                if (input) input.value = '';
            });

            // Event listeners cho buttons
            $(document).ready(function () {
                const btnCopy = document.getElementById('btnCopyQrCode');
                if (btnCopy) {
                    btnCopy.addEventListener('click', function () {
                        const input = document.getElementById('qrCodeInput');
                        if (input && input.value) {
                            navigator.clipboard.writeText(input.value).then(function () {
                                if (typeof Swal !== 'undefined') {
                                    Swal.fire({
                                        icon: 'success',
                                        title: 'Đã sao chép',
                                        text: 'Đã sao chép mã QR.',
                                        timer: 2000,
                                        showConfirmButton: false
                                    });
                                } else {
                                    alert('Đã sao chép mã QR.');
                                }
                            });
                        }
                    });
                }

                const btnUpload = document.getElementById('btnUploadQrImage');
                const fileInput = document.getElementById('qrImageInput');
                if (btnUpload && fileInput) {
                    btnUpload.addEventListener('click', function () {
                        fileInput.click();
                    });

                    fileInput.addEventListener('change', function (e) {
                        const file = e.target.files[0];
                        if (file) {
                            self.processQrImage(file);
                        }
                    });
                }

                const input = document.getElementById('qrCodeInput');
                if (input) {
                    input.addEventListener('keyup', function (e) {
                        if (e.key === 'Enter') {
                            self.handleScannedValue(input.value);
                        }
                    });
                }
            });
        },
        initCamera: function () {
            const self = this;
            const video = document.getElementById('qrCamera');
            if (!video || !navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
                console.warn('Trình duyệt không hỗ trợ camera hoặc không tìm thấy #qrCamera');
                return;
            }

            navigator.mediaDevices.getUserMedia({
                video: { facingMode: 'environment' }
            }).then(function (stream) {
                self.cameraStream = stream;
                video.srcObject = stream;
                self.scanning = true;
                self.scanCanvas = document.createElement('canvas');
                self.scanCtx = self.scanCanvas.getContext('2d');
                self.scanLoop();
            }).catch(function (err) {
                console.error('Không truy cập được camera:', err);
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Không mở được camera. Vui lòng kiểm tra quyền truy cập.'
                    });
                } else {
                    alert('Không mở được camera. Vui lòng kiểm tra quyền truy cập.');
                }
            });
        },
        stopCamera: function () {
            const self = this;
            if (self.cameraStream) {
                self.cameraStream.getTracks().forEach(function (t) {
                    t.stop();
                });
                self.cameraStream = null;
            }
            self.scanning = false;
        },
        scanLoop: function () {
            const self = this;
            if (!self.scanning) return;
            const video = document.getElementById('qrCamera');
            if (!video || video.readyState !== video.HAVE_ENOUGH_DATA) {
                requestAnimationFrame(function () {
                    self.scanLoop();
                });
                return;
            }
            if (!self.scanCanvas || !self.scanCtx) {
                requestAnimationFrame(function () {
                    self.scanLoop();
                });
                return;
            }
            self.scanCanvas.width = video.videoWidth;
            self.scanCanvas.height = video.videoHeight;
            self.scanCtx.drawImage(video, 0, 0, self.scanCanvas.width, self.scanCanvas.height);
            try {
                const imageData = self.scanCtx.getImageData(0, 0, self.scanCanvas.width, self.scanCanvas.height);
                if (window.jsQR) {
                    const code = jsQR(imageData.data, imageData.width, imageData.height);
                    if (code && code.data) {
                        self.handleScannedValue(code.data);
                        return;
                    }
                }
            } catch (e) {
                console.warn('Lỗi khi xử lý khung hình QR:', e);
            }
            requestAnimationFrame(function () {
                self.scanLoop();
            });
        },
        handleScannedValue: function (rawValue) {
            const self = this;
            if (!rawValue) return;
            const input = document.getElementById('qrCodeInput');
            if (input) input.value = rawValue;
            const statusEl = document.querySelector('#qrScanModal .fs-12px.text-center');
            if (statusEl) statusEl.textContent = 'Đã đọc được mã QR. Đang gửi lên hệ thống...';
            self.stopCamera();
            self.submitScan(rawValue);
        },
        extractTokenOrProfile: function (value) {
            if (!value) return {};
            try {
                const url = new URL(value);
                const token = url.searchParams.get('token');
                if (token) return { token: token, accountId: accountId };
            } catch (e) {
                // Không phải URL hợp lệ
            }
            if (/^[a-fA-F0-9]{32}$/.test(value) || /^[a-fA-F0-9-]{36}$/.test(value)) {
                return { token: value, accountId: accountId };
            }
            return {};
        },
        submitScan: function (rawValue) {
            const self = this;
            if (self.isSubmitting) return;
            const statusEl = document.querySelector('#qrScanModal .fs-12px.text-center');
            const parsed = self.extractTokenOrProfile(rawValue);
            if (!parsed.token && !parsed.profileId) {
                if (statusEl) statusEl.textContent = 'Mã không hợp lệ. Vui lòng thử lại.';
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi!',
                        text: 'Mã QR không hợp lệ.'
                    });
                } else {
                    alert('Mã QR không hợp lệ.');
                }
                return;
            }
            self.isSubmitting = true;
            $.ajax({
                url: "/hospital/api/scan-qr",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(parsed),
                beforeSend: function () {
                    if (typeof Swal !== 'undefined') {
                        Swal.fire({
                            title: 'Đang xử lý...',
                            html: 'Vui lòng chờ trong giây lát',
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            didOpen: function () {
                                Swal.showLoading();
                            }
                        });
                    }
                },
                success: function (response) {
                    if (response.status == "200") {
                        if (response.data[0]) {
                            Swal.fire({
                                title: "Đồng bộ thành công",
                                text: "Đã cập nhật chia sẻ hồ sơ cho bệnh viện.",
                                icon: "success",
                                confirmButtonText: "OK"
                            }).then(function () {
                                location.reload();
                            });
                        }
                        else {
                            Swal.fire({
                                title: "Đồng bộ không thành công",
                                text: "Mã QR không hợp lệ hoặc đã hết hạn.",
                                icon: "info",
                                confirmButtonText: "OK"
                            }).then(function () {
                                return
                            });
                        }
                    }
                },
                error: function (e) {
                    if (typeof Swal !== 'undefined') {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi!',
                            text: 'Không gửi được dữ liệu quét. Vui lòng thử lại.'
                        });
                    } else {
                        alert('Không gửi được dữ liệu quét. Vui lòng thử lại.');
                    }
                    console.error(e);
                    self.isSubmitting = false;
                    self.initCamera();
                }
            });
        },
        processQrImage: function (file) {
            const self = this;
            const reader = new FileReader();
            reader.onload = function (e) {
                const img = new Image();
                img.onload = function () {
                    if (!self.scanCanvas) {
                        self.scanCanvas = document.createElement('canvas');
                        self.scanCtx = self.scanCanvas.getContext('2d');
                    }
                    self.scanCanvas.width = img.width;
                    self.scanCanvas.height = img.height;
                    self.scanCtx.drawImage(img, 0, 0);
                    try {
                        const imageData = self.scanCtx.getImageData(0, 0, self.scanCanvas.width, self.scanCanvas.height);
                        if (window.jsQR) {
                            const code = jsQR(imageData.data, imageData.width, imageData.height);
                            if (code && code.data) {
                                self.handleScannedValue(code.data);
                            } else {
                                if (typeof Swal !== 'undefined') {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Lỗi',
                                        text: 'Không tìm thấy mã QR trong ảnh.'
                                    });
                                } else {
                                    alert('Không tìm thấy mã QR trong ảnh.');
                                }
                            }
                        }
                    } catch (err) {
                        console.error('Lỗi khi xử lý ảnh QR:', err);
                        if (typeof Swal !== 'undefined') {
                            Swal.fire({
                                icon: 'error',
                                title: 'Lỗi',
                                text: 'Không thể xử lý ảnh.'
                            });
                        } else {
                            alert('Không thể xử lý ảnh.');
                        }
                    }
                };
                img.src = e.target.result;
            };
            reader.readAsDataURL(file);
        },
        detailResult(diseaseKey, accountId) {
            $("#statiscalResult").modal('show');

            const mappingCall = $.ajax({
                url: "/js/disease_metric_mapping.json",
                method: "GET",
                dataType: "json"
            });

            $.when(mappingCall).done((mappingRes) => {

                const resources = ProfileDetailPage.profileAccount || {};
                const mapping = mappingRes || {};

                const scores = resources.scores || [];
                const metricCards = resources.metricCards || [];
                const riskMarkers = resources.riskMarkers || [];

                //window.userImage.src = '/images/userdefault.jpg';
                if (window.userImage) {

                    const photoUrl = resources.profile?.avatar;
                    console.log("photo", photoUrl);
                    window.userImage.src = photoUrl || '/images/userdefault.jpg';

                }

                const chartData = resources;

                $(document).trigger('initRiskChart', chartData);


                // Nếu không truyền diseaseKey (trang tổng quan) -> chọn bệnh có nguy cơ cao nhất
                if (!diseaseKey) {
                    if (!scores.length) {
                        console.warn("Không có scores để xác định bệnh nguy cơ cao nhất.");
                        return;
                    }

                    const riskPriority = {
                        poor: 3,       // Nguy cơ cao
                        moderate: 2,   // Nguy cơ trung bình
                        good: 1        // Nguy cơ thấp
                    };

                    const topScore = scores.slice().sort((a, b) => {
                        const aLevel = (a.riskLevel || '').toLowerCase();
                        const bLevel = (b.riskLevel || '').toLowerCase();
                        const aPri = riskPriority[aLevel] || 0;
                        const bPri = riskPriority[bLevel] || 0;

                        // Ưu tiên riskLevel: poor > moderate > good
                        if (aPri !== bPri) return bPri - aPri;

                        // Nếu cùng level thì sort theo scoreValue giảm dần
                        const aScore = typeof a.scoreValue === 'number' ? a.scoreValue : 0;
                        const bScore = typeof b.scoreValue === 'number' ? b.scoreValue : 0;
                        return bScore - aScore;
                    })[0];

                    diseaseKey = topScore?.diseaseKey;
                }


                // mapping name giữ nguyên
                const diseaseInfo = mapping[diseaseKey];

                if (!diseaseInfo) {
                    console.warn("Không tìm thấy diseaseInfo", diseaseKey);
                    return;
                }

                // Set tên bệnh

                const scoreItem = scores.find(s => s.diseaseKey === diseaseKey);
                const riskScore = scoreItem ? Number(scoreItem.scoreValue) || 0 : 0;
                const riskPercent = convertScoreToPercent(riskScore);

                const labelRiskLevel = scoreItem.riskLevel;
                console.log("Risk level:", labelRiskLevel);
              
                // Set màu risk level dựa vào riskScore
                const riskLevel =
                    riskScore >= 7 ? "red" :
                        riskScore >= 4 ? "orange" :
                            "green";

                if (window.setRiskLevel) {
                    window.setRiskLevel(riskLevel);
                }

                let clinicalPercent = 0;

                if (scoreItem) {
                    const scoreName = (scoreItem.name || '').trim().toLowerCase();

                    //  match đúng clinical theo tên bệnh
                    let clinicalMarker = riskMarkers.find(m =>
                        (m.type || '').toLowerCase() === 'clinical' &&
                        (m.label || '').trim().toLowerCase() === scoreName
                    );

                    // nếu không tìm được mà chỉ có 1 clinical trong riskMarkers thì dùng luôn nó
                    if (!clinicalMarker) {
                        const clinicalList = riskMarkers.filter(m =>
                            (m.type || '').toLowerCase() === 'clinical'
                        );
                        if (clinicalList.length === 1) {
                            clinicalMarker = clinicalList[0];
                        }
                    }

                    if (clinicalMarker) {
                        const clinicalScore = Number(clinicalMarker.value) || 0;
                        clinicalPercent = convertScoreToPercent(clinicalScore);
                    }
                }

                // Nếu có phần trăm nguy cơ lâm sàng riêng muốn show:
                if (clinicalPercent > 0) {
                    $("#clinical-risk-percent").text(clinicalPercent + "%");
                }

                if (window.setRiskBar) {
                    const markers = [];

                    // marker di truyền
                    markers.push({
                        value: riskPercent,
                        label: riskPercent + '%',
                        type: 1002 // map trong setRiskBar để ra màu vàng
                    });

                    // marker lâm sàng
                    if (clinicalPercent > 0) {
                        markers.push({
                            value: clinicalPercent,
                            label: clinicalPercent + '%',
                            type: 1001 // map trong setRiskBar để ra màu xanh
                        });
                    }

                    console.log('setRiskBar markers = ', markers);

                    window.setRiskBar(
                        { greenEnd: 8, orangeEnd: 16 }, // ngưỡng đang dùng
                        markers
                    );
                }

            }).fail(err => {
                console.error("loadSidebarDetails AJAX error:", err);
            });


        }
    }

    function getDiseaseInfo(diseaseKey) {
        // 1. Map cố định cho Hình ảnh (vì API không có field image)
        const diseaseData = ProfileDetailPage.listDiseaseDefinition.find(item => item.diseaseKey === diseaseKey);
        if (!diseaseData) return null;

        const displayName = diseaseData.nameVi || diseaseData.name || diseaseKey.toUpperCase();

        return displayName;
    }
    function getValueByPath(obj, path) {
        if (!obj || !path) return undefined;
        return path.split('.').reduce((acc, key) => acc?.[key], obj);
    }
    function getRiskText(riskLevel) {
        switch (riskLevel) {
            case 'poor':
                return 'Nguy cơ cao';
            case 'moderate':
                return 'Nguy cơ trung bình';
            case 'good':
                return 'Nguy cơ thấp';
            default:
                return 'Nguy cơ thấp';
        }
    }

    function getRiskBadgeColor(riskLevel) {
        switch (riskLevel) {
            case 'poor':
                // Màu đỏ/cam cho nguy cơ cao
                return 'linear-gradient(135deg, #CE3853, #E45585)';
            case 'moderate':
                // Màu vàng/cam cho nguy cơ trung bình
                return 'linear-gradient(135deg, #FCC960, #FFA726)';
            case 'good':
                // Màu xanh cho nguy cơ thấp
                return 'linear-gradient(135deg, #3D9C6F, #69EC9F)';
            default:
                return 'linear-gradient(135deg, #3D9C6F, #69EC9F)';
        }
    }

    function convertScoreToPercent(score) {
        switch (score) {
            case 0:
                return 3;
            case 1:
                return 7;
            case 2:
                return 11;
            case 3:
                return 15;
            case 4:
                return 22;
            case 5:
                return 30;
            case 6:
                return 40;
            case 7:
                return 55;
            case 8:
                return 70;
            case 9:
                return 85;
            case 10:
                return 95;
            default:
                return 0;
        }
    }

    $(document).ready(function () {
        ProfileDetailPage.init();
    });
}());