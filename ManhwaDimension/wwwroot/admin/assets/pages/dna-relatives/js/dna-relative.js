'use strict';
(function () {
    const dnaPage = {
        table: null,
        dataset: [],
        initTable: function () {
            var self = this;
            this.table = $('#kt_dna_datatable').DataTable({
                processing: true,
                serverSide: true,
                paging: true,
                searching: { regex: true },
                order: [1, 'desc'],
                "oLanguage": {
                    "sUrl": "/js/Vietnamese.json"
                },
                ajax: {
                    url: systemURL + "relationship/api/list-server-side",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: function (d) {
                        d.searchAll = $("#searchInput").val();
                        d.degreeFilter = searchFilters.degree ?? 0;
                        d.accountId = parseInt(accountSelectedId) || 0;
                        // Thêm các bộ lọc relationship
                        d.relationships = [];
                        if ($('#relationshipMother').is(':checked')) {
                            d.relationships.push('mother');
                        }
                        if ($('#relationshipFather').is(':checked')) {
                            d.relationships.push('father');
                        }
                        if ($('#relationshipBoth').is(':checked')) {
                            d.relationships.push('both');
                        }

                        // Thêm các bộ lọc family
                        d.families = [];
                        $('[id^="family"]:checked').each(function () {
                            d.families.push($(this).attr('id').replace('family', '')); // Cập nhật tên họ
                        });

                        return JSON.stringify(d);
                    }
                },
                columns: [
                    {
                        data: 'degree',
                        className: 'text-center',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                // favoriteStatus: 0 = trắng (thấp nhất), 1 = xám, 2 = vàng (cao nhất)
                                let fillColor = 'white'; // Trắng (mặc định - thấp nhất)
                                if (row.degree <= 2) {
                                    fillColor = '#FFD700'; // Vàng (cao nhất)
                                }
                                else if (row.degree == 3) {
                                    fillColor = '#6c757d'; // Xám (trung bình)
                                }
                               
                                const strokeColor = '#666666';
                                return `
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 16 16" fill="none" class="favorite-star" data-id="${row.id}" style="cursor: pointer;">
                                            <path d="M7.99902 1.88477C8.13989 1.88477 8.42552 1.99849 8.70605 2.5625V2.56348L9.87891 4.90918C10.0039 5.16286 10.2123 5.38006 10.4277 5.53906C10.6443 5.69888 10.9138 5.83362 11.1914 5.87988L13.3174 6.2334C13.9111 6.33235 14.1063 6.56585 14.1514 6.70605C14.1961 6.84508 14.1751 7.14695 13.748 7.57129L12.0928 9.22656C11.878 9.44146 11.7345 9.72905 11.6553 10.0059C11.576 10.2829 11.5464 10.6013 11.6123 10.8955L11.6133 10.8994L12.0859 12.9453C12.2628 13.7129 12.0846 13.9891 11.9961 14.0537C11.9068 14.1188 11.5869 14.2042 10.9072 13.8027H10.9082L8.91504 12.623C8.63908 12.4595 8.3061 12.3936 8.00195 12.3936C7.69756 12.3936 7.36599 12.4599 7.08887 12.6211L7.08496 12.623L5.0918 13.8027C4.41788 14.2027 4.0968 14.1181 4.00586 14.0518C3.91524 13.9853 3.7381 13.707 3.91406 12.9463V12.9453L4.38672 10.8994L4.3877 10.8955C4.45358 10.6013 4.42398 10.2829 4.34473 10.0059C4.26549 9.72907 4.12195 9.44145 3.90723 9.22656L2.25391 7.57324C1.82924 7.14858 1.80677 6.84534 1.85156 6.70508C1.89631 6.5653 2.08897 6.33129 2.68164 6.23242L4.80859 5.87988L4.81055 5.87891C5.08518 5.8322 5.3526 5.69734 5.56738 5.53809C5.78119 5.3795 5.98838 5.16245 6.11328 4.90918L6.11426 4.91016L7.28711 2.56445C7.57091 1.99983 7.85723 1.88481 7.99902 1.88477Z" fill="${fillColor}" stroke="${strokeColor}" stroke-linecap="round" stroke-linejoin="round" />
                                        </svg>
                                `;
                            }
                            return row.favoriteStatus || 0;
                        }
                    },
                    {
                        data: 'id',
                        className: 'text-center',
                        render: function (data, type, row, meta) {
                            var start = 0;
                            if (meta && meta.settings) {
                                try {
                                    var api = new $.fn.dataTable.Api(meta.settings);
                                    var info = api.page.info();
                                    start = info && typeof info.start === 'number' ? info.start : 0;
                                } catch (e) { start = 0; }
                            }
                            return meta.row + 1 + start;
                        }
                    },
                    {
                        data: 'fullName',
                        render: function (data, type, row) {
                            return `
                                <div class="d-flex flex-column">
                                    <div class='fw-bold'>${row.fullName}</div>
                                    <div class="text-muted fs-7">${row.gender}</div>
                                </div>
                            `;
                        }
                    },
                    {
                        data: 'relationshipVn'
                    },
                    {
                        data: 'degree',
                        className: 'text-center'
                    },
                    {
                        data: 'dnaMatchPercent',
                        render: function (data, type, row) {
                            // Xác định màu chấm tròn dựa trên tỷ lệ
                            let dotColor = '#6c757d'; // Mặc định xám
                            if (row.dnaMatchPercent >= 40) {
                                dotColor = '#198754'; // Xanh lá
                            } else if (row.dnaMatchPercent >= 25) {
                                dotColor = '#fd7e14'; // Cam
                            }
                            return `
                                <div class="d-flex align-items-center gap-2">
                                    <span class="badge badge-circle" style="background-color: ${dotColor}; width: 8px; height: 8px; padding: 0;"></span>
                                    <span>${row.dnaMatchPercent}% trùng nhau, ${row.nsnps} ADN</span>
                                </div>
                            `;
                        }
                    },
                    {
                        data: 'id',
                        className: 'text-center',
                        orderable: false,
                        render: function (data, type, row) {
                            return `
                                
                            `;
                        }
                    }
                ],
                columnDefs: [
                    { orderable: false, targets: [6] }
                ],
                order: [[1, 'asc']], // Sắp xếp mặc định theo cột STT (index 1) tăng dần
                aLengthMenu: [
                    [5, 10, 25, 50],
                    [5, 10, 25, 50]
                ],
                pageLength: 10,
                drawCallback: function (settings) {
                    var totalItem = settings.aiDisplay.length || 0;
                    $(".total-table").html(`Hiển thị <span class='fw-bold text-primary'>${totalItem}</span> mối quan hệ`);
                }
            });

            // Bind event cho nút yêu thích - cycle qua 3 trạng thái: trắng -> xám -> vàng -> trắng
            $('#kt_dna_datatable').on('click', '.favorite-star', function () {
                const id = $(this).data('id');
                const row = self.dataset.find(r => r.id === id);
                if (row) {
                    // Chuyển đổi: 0 (trắng - thấp nhất) -> 1 (xám) -> 2 (vàng - cao nhất) -> 0 (trắng)
                    row.favoriteStatus = (row.favoriteStatus || 0) + 1;
                    if (row.favoriteStatus > 2) {
                        row.favoriteStatus = 0;
                    }
                    self.table.draw();
                }
            });

            // Bind event cho nút hành động
            $('#kt_dna_datatable').on('click', '.btn-primary[data-id]', function () {
                const id = $(this).data('id');
                // Xử lý khi click vào nút hành động
                console.log('Xem chi tiết:', id);
                // Có thể điều hướng đến trang chi tiết hoặc mở modal
            });
        },
        init: function () {
            this.initTable();
        }
    };
    $(document).ready(function () {
        dnaPage.init();
    });
})();