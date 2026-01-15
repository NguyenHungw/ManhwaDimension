"use strict";
let table;
let editObj;
let postData;
let updatingItemId;
let tableUpdating;
const submitButton = document.getElementById('btnUpdateItem');
submitButton.addEventListener('click', function (e) {
    e.preventDefault();
    submitButton.setAttribute('data-kt-indicator', 'on');
    // Disable button to avoid multiple click
    submitButton.disabled = true;
    // Simulate form submission. For more info check the plugin's official documentation: https://sweetalert2.github.io/
    updateItem(updatingItemId);
});
$(document).ready(function () {
    loadData();
    const filterPostStatusId = $('#filterPostStatusId');
    const postStatusId = $('#post-postStatusId');

    const filterPostTypeId = $('#filterPostTypeId');
    const postTypeId = $('#post-postTypeId');

    const filterPostCategoryId = $('#filterPostCategoryId');
    const postCategoryId = $('#post-postCategoryId');

    const postAuthorId = $('#post-authorId');
    const filterPostAuthorId = $('#filterPostAuthorId');

    // Load all the data on page load
    (async () => {
        await loadDataAndPopulateSelect('postStatus/api/list', filterPostStatusId, postStatusId);
        await loadDataAndPopulateSelect('postType/api/list', filterPostTypeId, postTypeId);
        await loadDataAndPopulateSelect('postCategory/api/list', filterPostCategoryId, postCategoryId);
        await loadDataAndPopulateSelect('account/api/list', filterPostAuthorId, postAuthorId);
    })();
    document.querySelectorAll(".datepicker").forEach(function (item) {
        new tempusDominus.TempusDominus(item, datePickerOption);
    })
    $(".datepicker").on('dp.change', function () {
        this.value = moment(this.value).format("YYYY-MM-DD HH:mm:ss");
    });
    //Flat pickr format
    $("#filterCreatedTime_input").flatpickr({
        dateFormat: "d/m/Y",
        mode: "range",
    });
    $("#open-flatpickr").click(function () {
        $("#filterCreatedTime_input").click();
    });
    $("#clear-flatpickr").click(function () {
        $("#filterCreatedTime_input").val("");
    });

    $("#filterDoB_input").flatpickr({
        dateFormat: "d/m/Y",
        mode: "range",
    });
    $("#open-flatpickr-DoB").click(function () {
        $("#filterDoB_input").click();
    });
    $("#clear-flatpickr-DoB").click(function () {
        $("#filterDoB_input").val("");
    });

    $('.dataSelect').select2();


    $("#btnTableSearch").click(function () {
        tableSearch();
    });

    $("#tableData thead:nth-child(2) tr th input").keypress(function (e) {
        let key = e.which;
        if (key === 13) {
            $("#btnTableSearch").click();
        }
    });
    $("#btnTableResetSearch").click(function () {
        $(".tableheaderFillter").val("").trigger("change");
        tableSearch();
    });

    $('.remove-image').on('click', function () {
        $('#post-photo').attr('src', '/assets/media/images/blog/NoImage.png');
    });
});

function loadData() {
    table = $('#tableData').DataTable({
        processing: true,
        serverSide: true,
        paging: true,
        searching: {regex: true},
        order: [1, 'desc'],
        ajax: {
            url: systemURL + "post/api/list-server-side",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                d.searchAll = $("#search-input").val();
                d.postStatusIds = $("#filterPostStatusId").val();
                d.postTypeIds = $("#filterPostTypeId").val();
                d.postCategoryIds = $("#filterPostCategoryId").val();
                d.authorIds = $("#filterPostAuthorId").val();
                return JSON.stringify(d);
            }
        },
        columns: [
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    const info = table.page.info();
                    return meta.row + 1 + info.page * info.length;
                }
            },
            {
                data: 'photo',
                render: function (data) {
                    let content;
                    let defaultImg = "/assets/media/images/blog/NoImage.png";
                    if (data) {
                        content = `<div class='symbol symbol-100px symbol-lg-160px symbol-fixed position-relative imgPost'><img src='${data}' alt='image'></div>`;
                    } else {
                        content = `<div class='symbol symbol-100px symbol-lg-160px symbol-fixed position-relative imgPost'><img src='${defaultImg}' alt='image'></div>`;
                    }
                    return content;
                }
            },
            {
                data: 'postAuthorName',
                render: function (data) {
                    return data;
                }
            },
            {
                data: 'name',
                render: function (data) {
                    return data;
                }
            },
            {
                data: 'postStatusId',
                render: function (data, type, row) {
                    return `<span class="badge py-3 px-4 fs-7">${row.postStatusName}</span>`;
                }
            },
            {
                data: 'postTypeId',
                render: function (data, type, row) {
                    return `<span class="badge py-3 px-4 fs-7">${row.postTypeName}</span>`;
                }
            },
            {
                data: 'postCategoryId',
                render: function (data, type, row) {
                    return `<span class="badge py-3 px-4 fs-7">${row.postCategoryName}</span>`;
                }
            },
            {
                data: 'createdTime',
                render: function (data) {
                    return moment(data).format("DD/MM/YYYY HH:mm:ss");
                }
            },
            {
                data: 'id',
                render: function (data, type, row) {
                    return `<div class='d-flex justify-content-center gap-2'>
                            <a title='Cập nhật' onclick='editItem(${row.id})' class='me-2 btn_manage'>
                                <span class='svg-icon-success svg-icon  svg-icon-1 svg_teh009'><span class='svg-icon-primary svg-icon  svg-icon-1'> <svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path opacity='0.3' fill-rule='evenodd' clip-rule='evenodd' d='M2 4.63158C2 3.1782 3.1782 2 4.63158 2H13.47C14.0155 2 14.278 2.66919 13.8778 3.04006L12.4556 4.35821C11.9009 4.87228 11.1726 5.15789 10.4163 5.15789H7.1579C6.05333 5.15789 5.15789 6.05333 5.15789 7.1579V16.8421C5.15789 17.9467 6.05333 18.8421 7.1579 18.8421H16.8421C17.9467 18.8421 18.8421 17.9467 18.8421 16.8421V13.7518C18.8421 12.927 19.1817 12.1387 19.7809 11.572L20.9878 10.4308C21.3703 10.0691 22 10.3403 22 10.8668V19.3684C22 20.8218 20.8218 22 19.3684 22H4.63158C3.1782 22 2 20.8218 2 19.3684V4.63158Z' fill='currentColor'></path><path d='M10.9256 11.1882C10.5351 10.7977 10.5351 10.1645 10.9256 9.77397L18.0669 2.6327C18.8479 1.85165 20.1143 1.85165 20.8953 2.6327L21.3665 3.10391C22.1476 3.88496 22.1476 5.15129 21.3665 5.93234L14.2252 13.0736C13.8347 13.4641 13.2016 13.4641 12.811 13.0736L10.9256 11.1882Z' fill='currentColor'></path><path d='M8.82343 12.0064L8.08852 14.3348C7.8655 15.0414 8.46151 15.7366 9.19388 15.6242L11.8974 15.2092C12.4642 15.1222 12.6916 14.4278 12.2861 14.0223L9.98595 11.7221C9.61452 11.3507 8.98154 11.5055 8.82343 12.0064Z' fill='currentColor'></path></svg></span></span>
                            </a>
                            <a title='Xóa' onclick='deleteItem(${row.id})' class='me-2 btn_manage'>
                                <span class='svg-icon-success svg-icon  svg-icon-1 svg_teh009'><span class='svg-icon-danger svg-icon  svg-icon-1'><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M5 9C5 8.44772 5.44772 8 6 8H18C18.5523 8 19 8.44772 19 9V18C19 19.6569 17.6569 21 16 21H8C6.34315 21 5 19.6569 5 18V9Z' fill='currentColor'></path><path opacity='0.5' d='M5 5C5 4.44772 5.44772 4 6 4H18C18.5523 4 19 4.44772 19 5V5C19 5.55228 18.5523 6 18 6H6C5.44772 6 5 5.55228 5 5V5Z' fill='currentColor'></path><path opacity='0.5' d='M9 4C9 3.44772 9.44772 3 10 3H14C14.5523 3 15 3.44772 15 4V4H9V4Z' fill='currentColor'></path></svg></span>
                            </a>
                        </div>`;
                }
            }
        ],
        columnDefs: [
            {targets: [0, 1, 8], orderable: false},
            {targets: 0, className: 'text-center'}
        ],
        drawCallback: function () {
            $('#tableData tfoot').html("");
            $("#tableData thead:nth-child(1) tr").clone(true).appendTo("#tableData tfoot");
            $("#searchFilter").removeClass("d-none");
        }
    });
}

function getItemById(id) {
    let item;
    item = table.ajax.json().data.find(c => c.id === id);
    return item;
}

async function deleteItem(id) {
    let updatingObj = table.ajax.json().data.find(c => c.id === id);
    let objName = id > 0 ? updatingObj.name : "item";
    Swal.fire({
        title: 'Xác nhận thay đổi?',
        text: "Xóa " + objName + "",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Xoá',
        cancelButtonText: 'Huỷ'
    }).then((result) => {
        if (result.value) {
            //CALL AJAX TO DELETE
            $.ajax({
                url: systemURL + "post/api/delete",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({"id": id}),
                success: function (responseData) {
                    // debugger;
                    if (responseData.status === 200 && responseData.message === "SUCCESS") {
                        Swal.fire(
                            'Thành công!',
                            'Đã xoá ' + updatingObj.name + '.',
                            'success'
                        );
                        reGenTable();
                    }
                },
                error: function () {
                    Swal.fire(
                        'Lỗi!',
                        'Đã xảy ra lỗi, vui lòng thử lại',
                        'error'
                    );
                }
            });

        } else {
            // Remove loading indication
            submitButton.removeAttribute('data-kt-indicator');
            // Enable button
            submitButton.disabled = false;
        }
    })
}

async function updateItem(id) {
    var actionName = (id === 0 ? "Bạn muốn tạo mới" : "Cập nhật");
    var obj;
    if (id > 0) {
        obj = await getItemById(id);
    }
    let objName = id > 0 ? obj.name : "item";
    validateInputNumber();
    var updatingObj = {
        "id": id,
        "overview": $("#post-overview").val(),
        "photo": $("#post-photo").attr("src"),
        "authorId": $("#post-authorId").val(),
        "publishedTime": formatDatetimeUpdate($("#post-publishedTime").val()),
        "active": $("#post-active").val() == '' ? 'true' : 'false',
        "name": $("#post-name").val(),
        "description": CKEDITOR.instances["post-description"].getData(),
        // "vipDescription": $("#post-vipDescription").val(),
        "postStatusId": $("#post-postStatusId").val(),
        "postTypeId": $("#post-postTypeId").val(),
        "postCategoryId": $("#post-postCategoryId").val(),
        "accountId": $("#post-accountId").val(),
        "createdTime": formatDatetimeUpdate($("#post-createdTime").val()),

    };
    Swal.fire({
        title: 'Xác nhận thay đổi?',
        text: "" + actionName + " " + objName + "",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#443',
        confirmButtonText: 'Xác nhận',
        cancelButtonText: 'Huỷ'
    }).then((result) => {
        if (result.value) {
            $("#modal-id").modal('hide');
            //CALL AJAX TO UPDATE
            if (id > 0) {
                $.ajax({
                    url: systemURL + "post/api/update",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(updatingObj),
                    success: function (responseData) {
                        // debugger;
                        if (responseData.status === "200" && responseData.message === "SUCCESS") {
                            Swal.fire(
                                'Thành Công!',
                                'Đã cập nhật "' + objName + '" ',
                                'success'
                            );
                            reGenTable();
                            // Remove loading indication
                            submitButton.removeAttribute('data-kt-indicator');
                            // Enable button
                            submitButton.disabled = false;
                        }
                    },
                    error: function (e) {
                        //console.log(e.message);
                        Swal.fire(
                            'Lỗi!',
                            'Đã xảy ra lỗi, vui lòng thử lại',
                            'error'
                        );
                        // Remove loading indication
                        submitButton.removeAttribute('data-kt-indicator');
                        // Enable button
                        submitButton.disabled = false;
                    }
                });
            }
            //CALL AJAX TO CREATE
            if (id === 0) {
                updatingObj.id = 1;
                delete updatingObj["id"]
                updatingObj.active = true;
                updatingObj.createdTime = new Date();
                console.log(updatingObj);
                $.ajax({
                    url: systemURL + "post/api/add",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(updatingObj),
                    success: function (responseData) {
                        // debugger;
                        if (responseData.status === "201" && responseData.message === "CREATED") {
                            Swal.fire(
                                'Thành công!',
                                'Đã cập nhật dữ liệu',
                                'success'
                            );
                            updatingObj = responseData.data;
                            reGenTable();
                            // Remove loading indication
                            submitButton.removeAttribute('data-kt-indicator');
                            // Enable button
                            submitButton.disabled = false;
                        }
                    },
                    error: function (e) {
                        //console.log(e.message);
                        Swal.fire(
                            'Lỗi!',
                            'Đã xảy ra lỗi, vui lòng thử lại',
                            'error'
                        );
                        // Remove loading indication
                        submitButton.removeAttribute('data-kt-indicator');
                        // Enable button
                        submitButton.disabled = false;
                    }
                });
            }
        } else {
            // Remove loading indication
            submitButton.removeAttribute('data-kt-indicator');
            // Enable button
            submitButton.disabled = false;
        }
    })
}

function quickChangeApprovedState(state) {
    let id = state.getAttribute("data-approved-id");
    let isApproved = $(state).is(':checked');
    $(state).prop('checked', !isApproved);
    let obj = {
        id: id,
        isApproved: !isApproved
    };
    Swal.fire({
        title: localizer.title.main,
        html: `${localizer.swal.message.replace("___action___", localizer.action.change.toLowerCase()).replace("___title___", localizer.title.approvePost.toLowerCase()).replace("___item___ ", ``)}?`,
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#443',
        cancelButtonText: localizer.action.no,
        confirmButtonText: localizer.action.yes
    }).then(result => {
        if (result.value) {
            $.ajax({
                url: "/Post/api/ChangeApprovedState",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(obj),
                success: function (res) {
                    if (res.status == 200 && res.title == "Success") {
                        $(state).prop('checked', isApproved);
                    }
                }
            });
        }
    })
}

async function loadDataAndPopulateSelect(url, selectElement, appendElement) {
    try {
        const response = await $.ajax({
            url: systemURL + url,
            type: 'GET',
            contentType: 'application/json',
        });
        const data = response.data;
        $(selectElement).select2();
        data.forEach(item => {
            const displayName = item.name || item.username;
            $(appendElement)
                .append(new Option(displayName, item.id, false, false))
                .trigger('change');
            $(selectElement)
                .append(new Option(displayName, item.id, false, false));
        });
    } catch (error) {
        console.error('Error loading data:', error);
    }
}

async function editItem(id) {
    updatingItemId = id;
    $("#modal-id").modal('show');
    $("#modal-title-post").text(id > 0 ? "Cập nhật bài đăng #" + id + "" : "Tạo mới bài đăng");
    $("#btnUpdateItem").text(id > 0 ? "Cập nhật" : "Tạo bài đăng");
    if (id > 0) {
        editObj = await getItemById(id);
    }
    $("#post-id").val(id > 0 ? editObj.id : "0");
    $("#post-overview").val(id > 0 ? editObj.overview : "");
    $("#post-photo").attr("src", id > 0 && editObj.photo ? editObj.photo : "/assets/media/images/blog/NoImage.png");
    $("#post-publishedTime").val(id > 0 ? moment(editObj.publishedTime).format("DD/MM/YYYY HH:mm:ss") : moment(new Date()).format("DD/MM/YYYY HH:mm:ss"));
    $("#post-name").val(id > 0 ? editObj.name : "");
    // $("#post-description").val(id > 0 ? editObj.description : "").trigger("change");    $("#post-vipDescription").val(id > 0 ? editObj.vipDescription : "");
    $("#post-postStatusId").val(id > 0 ? editObj.postStatusId : "").trigger("change");
    $("#post-postTypeId").val(id > 0 ? editObj.postTypeId : "").trigger("change");
    $("#post-postCategoryId").val(id > 0 ? editObj.postCategoryId : "").trigger("change");
    $("#post-authorId").val(id > 0 ? editObj.authorId : "").trigger("change");
    $("#post-accountId").val(id > 0 ? editObj.accountId : "").trigger("change");
    $("#post-createdTime").val(id > 0 ? moment(editObj.createdTime).format("DD/MM/YYYY HH:mm:ss") : moment(new Date()).format("DD/MM/YYYY HH:mm:ss"));
    if (CKEDITOR.instances["post-description"].getData() === "" && editObj.description !== "") {
        CKEDITOR.instances["post-description"].setData(editObj.description);
    }
    formatNumber();
}

function tableSearch() {
    table.column(1).search($("#tableData thead:nth-child(2) tr th:nth-child(2) input").val());
    table.column(2).search($("#tableData thead:nth-child(2) tr th:nth-child(3) input").val());
    table.column(3).search($("#tableData thead:nth-child(2) tr th:nth-child(4) input").val());
    table.column(4).search($("#tableData thead:nth-child(2) tr th:nth-child(5) input").val());
    table.column(5).search($("#tableData thead:nth-child(2) tr th:nth-child(6) input").val());
    table.column(6).search($("#tableData thead:nth-child(2) tr th:nth-child(7) input").val());
    table.column(7).search($("#tableData thead:nth-child(2) tr th:nth-child(8) input").val());
    table.draw();
}