var currentFinancialStatusData = [];
var financialStageStatusData = [];

async function documentLoad(id) {
    await loadDataSelectCurrentFinancialStatus();
    await loadDataSelectFinancialStageStatus();
    await loadDataTargetCompletedCategory();
    $("#currentFinancialStatus").select2();
    currentFinancialStatusData.forEach(function (item, index) {
        $('#currentFinancialStatus').append(new Option(`${index + 1}. ${item.name}`, item.id, false, false));
    });

    $("#financialStageStatus").select2();
    financialStageStatusData.forEach(function (item, index) {
        $('#financialStageStatus').append(new Option(`${index + 1}. ${item.name}`, item.id, false, false));
    });

    await loadDataCurrentFinancialStatusAccountViewModel(id);
    await loadDataFinancialStageStatusAccountViewModel(id);
    await loadDataTargetCompletedDetail(id);

    $('#currentFinancialStatus').on('change', async function () {
        var obj = {
            id: 0,
            currentFinancialStatusId: $(this).val()
        }

        try {
            var response = await httpService.postAsync("currentFinancialStatusAccount/api/AddOrUpdate", JSON.stringify(obj));
            if (response.status == "201") {
            }
        } catch (e) {

        }
    });

    $('#financialStageStatus').on('change', async function () {
        var obj = {
            id: 0,
            financialStageStatusId: $(this).val()
        }

        try {
            var response = await httpService.postAsync("financialStageStatusAccount/api/AddOrUpdate", JSON.stringify(obj));
            if (response.status == "201") {
            }
        } catch (e) {

        }
    });
}

async function loadDataFinancialStageStatusAccountViewModel(id) {
    try {
        var response = await httpService.getAsync("financialStageStatusAccount/api/ListByAccountAdmin?accountId=" + id);
        if (response.status == "200") {
            var item = response.data[0];
            $('#financialStageStatus').val(item.financialStageStatusId).trigger('change');
        }
    } catch (e) {

    }
}

async function loadDataCurrentFinancialStatusAccountViewModel(id) {
    try {
        var response = await httpService.getAsync("currentFinancialStatusAccount/api/ListByAccountAdmin?accountId=" + id);
        if (response.status == "200") {
            var item = response.data[0];
            $('#currentFinancialStatus').val(item.currentFinancialStatusId).trigger('change');
        }
    } catch (e) {

    }
}

async function loadDataSelectCurrentFinancialStatus() {
    try {
        const response = await $.ajax({
            url: systemURL + 'currentFinancialStatus/api/list',
            type: 'GET',
            contentType: 'application/json',
        });
        currentFinancialStatusData = response.data.sort((a, b) => a.id - b.id);
    } catch (error) {
        //console.error('Error loading current financial status data:', error);
    }
}

async function loadDataSelectFinancialStageStatus() {
    try {
        const response = await $.ajax({
            url: systemURL + 'financialStageStatus/api/list',
            type: 'GET',
            contentType: 'application/json',
        });
        financialStageStatusData = response.data.sort((a, b) => a.id - b.id);
    } catch (error) {
        //console.error('Error loading financial stage status data:', error);
    }
}

//async function loadDataTargetCompletedDetail() {
//    try {
//        var response = await httpService.getAsync("TargetCompletedDetail/api/ListByAccountId");
//        var data = response.data;
//        data.forEach(function (item, index) {

//        });
//    } catch (e) {
//        console.log("Lỗi khi gọi API:", e);
//    }
//}

async function loadDataTargetCompletedDetail(id) {
    try {
        var response = await httpService.getAsync("TargetCompletedDetail/api/ListByAccountIdAdmin?accountId=" + id);
        var data = response.data;

        data.forEach(function (item) {

            $(`.category-checkbox[data-id=${item.targetCompletedCategoryId}]`).prop('checked', item.name == true ? true : false);
        });
    } catch (e) {
        console.log("Lỗi khi gọi API:", e);
    }
}


async function loadDataTargetCompletedCategory() {
    try {
        var response = await httpService.getAsync("targetCompletedCategory/api/list");
        if (response.status == "200") {
            var htmlInt = "";
            var itemsPerRow = 4;
            var listOptionMarketTrackingTime = `<option value="0" disabled="disabled">Lựa chọn</option>`;
            response.data.sort(function (a, b) {
                return a.id - b.id;
            });
            $(".summary-infomation tbody").html("");
            for (var i = 0; i < response.data.length; i += itemsPerRow) {
                htmlInt += "<tr>";
                for (var j = 0; j < itemsPerRow && (i + j) < response.data.length; j++) {
                    var item = response.data[i + j];
                    htmlInt += `<td>${item.name}</td>
                                <td>
                                    <input type="checkbox" class="category-checkbox" data-id="${item.id}" disabled>
                                    <span class="checkmark"></span>
                                </td>`;
                }
                htmlInt += "</tr>";
            }

            $(".summary-infomation tbody").append(htmlInt);

            // Gắn sự kiện change cho tất cả các checkbox
            $(".category-checkbox").on("change", function () {
                var isChecked = $(this).is(":checked");
                var categoryId = $(this).data("id");

                addOrUpdate(categoryId, isChecked);
            });
        }
    } catch (e) {
        console.log(e); // Xử lý lỗi nếu có
    }
}

// Hàm xử lý thêm hoặc cập nhật dữ liệu khi checkbox thay đổi
async function addOrUpdate(categoryId, isChecked) {
    try {
        var data = {
            id: 0,
            targetCompletedCategoryId: categoryId,
            name: isChecked
        };


        var response = await httpService.postAsync("TargetCompletedDetail/api/AddAsync", data);
        if (response.status == "200") {
            console.log("Cập nhật thành công");
        } else {
            console.log("Có lỗi xảy ra khi cập nhật");
        }
    } catch (e) {
        console.log("Lỗi khi gọi API:", e);
    }
}
