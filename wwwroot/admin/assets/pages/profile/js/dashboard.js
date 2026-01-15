'use strict';
(function () {
    const profilePage = {
        profileInfo: null,
        infoBindings: [
            { id: 'code', key: 'code' },
            { id: 'fullname', key: 'fullname' },
            { id: 'age', key: 'age' },
            { id: 'dob', key: 'dob' },
            {
                id: 'gender',
                key: 'gender',
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
            { id: 'nation', key: 'nation' },
            { id: 'bmi', key: 'bmi' },
            { id: 'collection_area', key: 'collection_area' },
            { id: 'height', key: 'height' },
            { id: 'weight', key: 'weight' },
            { id: 'blood_type', key: 'blood_type' }
        ],
        familyDataset: [],
        familyTable: null,
        biologicalDataset: [],
        biologicalTable: null,
        variantDataset: [],
        variantTable: null,

        init: async function () {
            var data = await this.loadDataSampleProfile();
            await this.renderProfileInfo();
            await this.initFamilyHistoryTable();
            await this.initBiologicalSamplesTable();
            await this.initVariantTable();
            await this.bindEvents();
        },

        renderProfileInfo: function () {
            const data = this.profileInfo;
            this.infoBindings.forEach(binding => {
                const el = document.getElementById(binding.id);
                if (!el) return;
                const rawValue = data[binding.key];
                const value = typeof binding.formatter === 'function'
                    ? binding.formatter(rawValue)
                    : (rawValue ?? '');

                if (binding.useHtml) {
                    el.innerHTML = value || '';
                } else {
                    el.textContent = value || '';
                }
            });
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
                    { data: 'id' },
                    { data: 'senderId' },
                    { data: 'sidTopic' },
                    { data: 'relationType' },
                    { data: 'genderRelation' },
                    { data: 'ethnicityRelation' },
                    { data: 'projectId' }
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

        initBiologicalSamplesTable: function () {
            const tableElement = $('#biological_samples_table');
            if (!tableElement.length) return;

            this.biologicalTable = tableElement.DataTable({
                processing: true,
                serverSide: false,
                paging: true,
                searching: true,
                ordering: true,
                data: this.biologicalDataset,
                columns: [
                    { data: 'sampleId' },
                    { data: 'sid' },
                    { data: 'sampleType' },
                    { data: 'component' },
                    {
                        data: 'collectedDate',
                        render: (data) => data ? moment(data).format('DD/MM/YYYY') : '--'
                    },
                    {
                        data: 'dnaExtractDate',
                        render: (data) => data ? moment(data).format('DD/MM/YYYY') : '--'
                    },
                    { data: 'freezingMethod' },
                    { data: 'storageMethod' }
                ],
                order: [[0, 'asc']],
                pageLength: 5,
                lengthMenu: [
                    [5, 10, 20, 100],
                    [5, 10, 20, 100]
                ],
                language: AppSettings.dataTableLanguage.vi,
            });
        },

        initVariantTable: function () {
            const tableElement = $('#variant_table');
            if (!tableElement.length) return;
            const self = this;

            this.variantTable = tableElement.DataTable({
                processing: true,
                serverSide: false,
                paging: true,
                searching: true,
                ordering: false,
                data: self.variantDataset,
                columns: [
                    // STT
                    {
                        data: null,
                        className: 'text-center',
                        render: function (data, type, row, meta) {
                            if (type === 'export') {
                                return meta.row + 1;
                            }
                            return meta.row + 1;
                        }
                    },
                    // DNA Change
                    { 
                        data: 'dnaChange',
                        render: function (data, type, row) {
                            if (type === 'export') {
                                return data || '--';
                            }
                            return data || '--';
                        }
                    },
                    // Variant Class - badge with border
                    {
                        data: 'variantClass',
                        className: 'w-100px',
                        render: function (data, type, row) {
                            if (type === 'export') {
                                return data || '--';
                            }
                            return `<span class="variant-class-badge">${data || '--'}</span>`;
                        }
                    },
                    // Consequence
                    { 
                        data: 'consequence',
                        render: function (data, type, row) {
                            if (type === 'export') {
                                return data || '--';
                            }
                            return data || '--';
                        }
                    },
                    // # Subjects - progress bar + red text + gray percent
                    {
                        data: null,
                        className: 'w-150px',
                        render: function (data, type, row) {
                            const percent = row.subjectsTotal ? ((row.subjectsCurrent / row.subjectsTotal) * 100).toFixed(1) : '0';
                            if (type === 'export') {
                                return `${row.subjectsCurrent}/${row.subjectsTotal} (${percent}%)`;
                            }
                            return `
                                <div class="variant-subjects-cell">
                                    <div class="variant-subjects-info">
                                        <span class="variant-subjects-count">${row.subjectsCurrent}/${row.subjectsTotal}</span>
                                        <span class="variant-subjects-percent">${percent}%</span>
                                    </div>
                                    <div class="variant-progress-bar">
                                        <div class="variant-progress-fill" style="width: ${percent}%"></div>
                                    </div>
                                </div>`;
                        }
                    },
                    // # Genes
                    { 
                        data: 'genes', 
                        className: 'text-center',
                        render: function (data, type, row) {
                            if (type === 'export') {
                                return data || '--';
                            }
                            return data || '--';
                        }
                    },
                    // Impact - green pill
                    {
                        data: 'impact',
                        className: 'text-center',
                        render: function (data, type, row) {
                            if (type === 'export') {
                                return data || '--';
                            }
                            return `<span class="variant-impact-chip">${data || '--'}</span>`;
                        }
                    },
                    // Clinical Significance - blue info icon
                    {
                        data: 'clinicalSignificance',
                        className: 'text-center w-150px',
                        render: function (data, type, row) {
                            if (type === 'export') {
                                return data || '--';
                            }
                            return `<span class="variant-clinical-significance">${row.clinicalSignificance || '--'}</span>`;
                        }
                    },
                    // dbSNP - link with icon
                    {
                        data: 'dbsnp',
                        className: 'text-center w-150px',
                        render: function (data, type, row) {
                            if (type === 'export') {
                                return data || '--';
                            }
                            return `<a href="https://www.ncbi.nlm.nih.gov/snp/${data}" target="_blank" class="variant-dbsnp-link text-primary d-flex flex-row align-items-center gap-2"><i class="ki-duotone ki-chart-line text-primary fs-2">
                                         <span class="path1"></span>
                                         <span class="path2"></span>
                                        </i>${data || '--'}</a>`;
                        }
                    }
                ],
                pageLength: 5,
                lengthMenu: [
                    [5, 10, 20, 100],
                    [5, 10, 20, 100]
                ],
                language: AppSettings.dataTableLanguage.vi,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export Excel',
                        className: 'd-none',
                        title: function () {
                            const profileCode = self.profileInfo?.code || 'profile';
                            const now = new Date();
                            const timestamp = now.toISOString().slice(0, 19).replace(/:/g, '-').replace('T', '_');
                            return `Variant_Export_${profileCode}_${timestamp}`;
                        },
                        filename: function () {
                            const profileCode = self.profileInfo?.code || 'profile';
                            const now = new Date();
                            const timestamp = now.toISOString().slice(0, 19).replace(/:/g, '-').replace('T', '_');
                            return `variant_export_${profileCode}_${timestamp}`;
                        },
                        exportOptions: {
                            columns: ':visible',
                            format: {
                                body: function (data, row, column, node) {
                                    // Loại bỏ HTML tags khi export
                                    if (typeof data === 'string' && data.includes('<')) {
                                        const tmp = document.createElement('DIV');
                                        tmp.innerHTML = data;
                                        data = tmp.textContent || tmp.innerText || '';
                                    }
                                    return data || '--';
                                }
                            }
                        }
                    },
                    {
                        extend: 'pdfHtml5',
                        text: 'Export PDF',
                        className: 'd-none',
                        title: function () {
                            const profileCode = self.profileInfo?.code || 'profile';
                            const now = new Date();
                            const timestamp = now.toISOString().slice(0, 19).replace(/:/g, '-').replace('T', '_');
                            return `Variant_Export_${profileCode}_${timestamp}`;
                        },
                        filename: function () {
                            const profileCode = self.profileInfo?.code || 'profile';
                            const now = new Date();
                            const timestamp = now.toISOString().slice(0, 19).replace(/:/g, '-').replace('T', '_');
                            return `variant_export_${profileCode}_${timestamp}`;
                        },
                        exportOptions: {
                            columns: ':visible',
                            format: {
                                body: function (data, row, column, node) {
                                    // Loại bỏ HTML tags khi export
                                    if (typeof data === 'string' && data.includes('<')) {
                                        const tmp = document.createElement('DIV');
                                        tmp.innerHTML = data;
                                        data = tmp.textContent || tmp.innerText || '';
                                    }
                                    return data || '--';
                                }
                            }
                        },
                        orientation: 'landscape',
                        pageSize: 'A4',
                        customize: function (doc) {
                            doc.defaultStyle.fontSize = 8;
                            doc.styles.tableHeader.fontSize = 9;
                            doc.styles.tableHeader.fontStyle = 'bold';
                            doc.styles.title.fontSize = 12;
                            doc.styles.title.fontStyle = 'bold';
                            doc.styles.title.alignment = 'center';
                            // Điều chỉnh độ rộng cột
                            doc.content[1].table.widths = ['5%', '12%', '10%', '15%', '12%', '8%', '8%', '15%', '15%'];
                        }
                    }
                ]
            });
        },

        bindEvents: function () {
            const self = this;

            // Variant table search
            $('#variant_search').on('keyup', function () {
                if (self.variantTable) {
                    self.variantTable.search($(this).val()).draw();
                }
            });

            // Variant export Excel button
            $('.variant-export-excel-btn').on('click', function (e) {
                e.preventDefault();
                self.exportVariantToExcel();
            });

            // Variant export PDF button
            $('.variant-export-pdf-btn').on('click', function (e) {
                e.preventDefault();
                self.exportVariantToPDF();
            });
        },

        exportVariantToExcel: function () {
            if (!this.variantTable || !this.variantDataset || this.variantDataset.length === 0) {
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Thông báo',
                        text: 'Không có dữ liệu để xuất file.'
                    });
                } else {
                    alert('Không có dữ liệu để xuất file.');
                }
                return;
            }

            // Kiểm tra xem DataTables Buttons có sẵn không
            if (typeof $.fn.dataTable.Buttons !== 'undefined' && this.variantTable.buttons) {
                // Sử dụng DataTables Buttons để export Excel
                try {
                    // Trigger button Excel đầu tiên (button ẩn đã được cấu hình)
                    this.variantTable.button(0).trigger();
                } catch (e) {
                    console.error('Error exporting with DataTables Buttons:', e);
                    // Fallback: export thủ công
                    this.exportVariantToExcelManual();
                }
            } else {
                // Fallback: export thủ công nếu không có DataTables Buttons
                this.exportVariantToExcelManual();
            }
        },

        exportVariantToPDF: function () {
            if (!this.variantTable || !this.variantDataset || this.variantDataset.length === 0) {
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Thông báo',
                        text: 'Không có dữ liệu để xuất file.'
                    });
                } else {
                    alert('Không có dữ liệu để xuất file.');
                }
                return;
            }

            // Kiểm tra xem DataTables Buttons có sẵn không
            if (typeof $.fn.dataTable.Buttons !== 'undefined' && this.variantTable.buttons) {
                // Sử dụng DataTables Buttons để export PDF
                try {
                    // Trigger button PDF thứ hai (button ẩn đã được cấu hình)
                    this.variantTable.button(1).trigger();
                } catch (e) {
                    console.error('Error exporting PDF with DataTables Buttons:', e);
                    if (typeof Swal !== 'undefined') {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi',
                            text: 'Không thể xuất file PDF. Vui lòng thử lại.'
                        });
                    } else {
                        alert('Không thể xuất file PDF. Vui lòng thử lại.');
                    }
                }
            } else {
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Thư viện export PDF chưa được tải. Vui lòng tải lại trang.'
                    });
                } else {
                    alert('Thư viện export PDF chưa được tải. Vui lòng tải lại trang.');
                }
            }
        },

        exportVariantToExcelManual: function () {
            // Fallback method: Export Excel thủ công sử dụng SheetJS hoặc CSV
            // Tạm thời export CSV với extension .xlsx (Excel có thể mở được)
            if (!this.variantDataset || this.variantDataset.length === 0) {
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Thông báo',
                        text: 'Không có dữ liệu để xuất file.'
                    });
                } else {
                    alert('Không có dữ liệu để xuất file.');
                }
                return;
            }

            // Định nghĩa header
            const headers = [
                'STT',
                'DNA Change',
                'Variant Class',
                'Consequence',
                '# Subjects',
                '# Genes',
                'Impact',
                'Clinical Significance',
                'dbSNP'
            ];

            // Tạo dữ liệu CSV (Excel có thể mở CSV)
            let csvContent = '\uFEFF'; // BOM để hỗ trợ UTF-8
            csvContent += headers.join('\t') + '\n';

            // Thêm dữ liệu từ variantDataset
            this.variantDataset.forEach((row, index) => {
                const percent = row.subjectsTotal ? ((row.subjectsCurrent / row.subjectsTotal) * 100).toFixed(1) : '0';
                const subjectsValue = `${row.subjectsCurrent}/${row.subjectsTotal} (${percent}%)`;
                
                const csvRow = [
                    index + 1,
                    (row.dnaChange || '--').toString().replace(/\t/g, ' '),
                    (row.variantClass || '--').toString().replace(/\t/g, ' '),
                    (row.consequence || '--').toString().replace(/\t/g, ' '),
                    subjectsValue.replace(/\t/g, ' '),
                    (row.genes || '--').toString().replace(/\t/g, ' '),
                    (row.impact || '--').toString().replace(/\t/g, ' '),
                    (row.clinicalSignificance || '--').toString().replace(/\t/g, ' '),
                    (row.dbsnp || '--').toString().replace(/\t/g, ' ')
                ];
                csvContent += csvRow.join('\t') + '\n';
            });

            // Tạo file và download với extension .xlsx
            const blob = new Blob([csvContent], { type: 'application/vnd.ms-excel;charset=utf-8;' });
            const link = document.createElement('a');
            const url = URL.createObjectURL(blob);
            
            // Tạo tên file với timestamp
            const now = new Date();
            const timestamp = now.toISOString().slice(0, 19).replace(/:/g, '-').replace('T', '_');
            const profileCode = this.profileInfo?.code || 'profile';
            const fileName = `variant_export_${profileCode}_${timestamp}.xlsx`;
            
            link.setAttribute('href', url);
            link.setAttribute('download', fileName);
            link.style.visibility = 'hidden';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },

        loadDataSampleProfile: function () {
            var id = window.location.pathname.split('/').pop();
            var result = [];
            $.ajax({
                url: '/sample/api/sample-profile?accountId=' + id,
                method: 'GET',
                dataType: 'json',
                async: false,
                success: function (response) {
                    if (response.status == "200") {
                        result = response.data[0];
                        //Gán dữ liệu profile
                        profilePage.profileInfo = {
                            code: result.sampleId || '--',
                            fullname: result.name || '--',
                            age: (new Date().getFullYear() - result.dateOfBirth) || '--',
                            dob: result.dateOfBirth || '--',
                            gender: result.sex || '--',
                            address: result.address || '--',
                            nation: result.ethnicity || '--',
                            bmi: result.height && result.weight  ? (result.weight / (result.height / 100 * result.height / 100)).toFixed(2) : '--',
                            collection_area: result.sampleCollectionArea || '--',
                            height: result.height || '--',
                            weight: result.weight || '--',
                            blood_type: result.bloodType || '--'
                        };

                        //Gán dữ liệu bảng gia đình
                        var dataFamilyInfos = [];
                        result.familyInfos.forEach(item => {
                            dataFamilyInfos.push(
                                { id: item.familyId, senderId: item.senderIdStr, sidTopic: item.name, relationType: item.relationship, genderRelation: item.gender, ethnicityRelation: item.ethnicity, projectId: '--' }
                            );
                        });
                        profilePage.familyDataset = dataFamilyInfos;
                        //Gán dữ liệu bảng mẫu sinh học
                        var biologicalData = [];
                        result.adnInfos.forEach(item => {
                            biologicalData.push(
                                {
                                    sampleId: item.sampleIdStr,
                                    sid: "--",
                                    sampleType: item.type,
                                    component: '--',
                                    collectedDate: item.sampleCollectionDate,
                                    dnaExtractDate: '',
                                    freezingMethod: '--',
                                    storageMethod: '--',
                                }
                            );
                        });
                        profilePage.biologicalDataset = biologicalData;
                        //Gán dữ liệu bảng biến thể
                        var variantData = [];
                        var totalSubjects = result.variants.map(item => item.subject).reduce((a, b) => a + b, 0);
                        result.variants.forEach(item => {
                            variantData.push(
                                { dnaChange: item.code, variantClass: item.type, consequence: item.consequence, subjectsCurrent: item.subject, subjectsTotal: totalSubjects, genes: '--', impact: '--', dbsnp: item.dbSnp, clinicalSignificance: '--' }
                            );
                        });
                        profilePage.variantDataset = variantData;
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error loading sample profile data:', error);
                }
            });
            return result;
        },
    };

    $(document).ready(function () {
        profilePage.init();
    });
})();