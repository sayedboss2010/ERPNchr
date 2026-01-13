//الإدارة
    const input = document.getElementById("CurrentBranchDeptText");
    const hiddenInput = document.getElementById("CurrentBranchDeptId");
    const options = document.querySelectorAll("#CurrentBranchDeptdatalist option");

input.addEventListener("input", function () {
    const val = this.value;
    const option = Array.from(options).find(opt => opt.value === val);
    if (option) {
        hiddenInput.value = option.getAttribute("data-id");
    } else {
        hiddenInput.value = ""; // لو المستخدم كتب حاجة مش موجودة
    }
});


//الوظيفة الحالية
const jobInput = document.getElementById("CurrentJobText");
const hiddenJobInput = document.getElementById("CurrentJobId");
const jobOptions = document.querySelectorAll("#ListHrJobsDatalistOptions option");

jobInput.addEventListener("input", function () {
    const val = this.value;
    const option = Array.from(jobOptions).find(opt => opt.value === val);
    if (option) {
        hiddenJobInput.value = option.getAttribute("data-id");
    } else {
        hiddenJobInput.value = ""; // لو المستخدم كتب حاجة مش موجودة
    }
});

//الشهر
const modelMonth = parseInt("@Model.Month"); // الرقم القادم من الكنترولر
const months = [
    "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو",
    "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"
];

const monthSelect = document.getElementById("Month");
for (let i = 0; i < months.length; i++)
{
    const opt = document.createElement("option");
    opt.value = i + 1; // 1 إلى 12
    opt.textContent = months[i];
    if (modelMonth === i + 1) {
        opt.selected = true; // يختار الشهر القادم من الكنترولر
    }
    monthSelect.appendChild(opt);
}

// السنة
// القيمة القادمة من الكنترولر
const modelYear = @Model.Year;
const select = document.getElementById("Year");
const currentYear = new Date().getFullYear();

for (let y = currentYear; y >= 1980; y--)
{
    const opt = document.createElement("option");
    opt.value = y;
    opt.textContent = y;
    if (y === modelYear) {
        opt.selected = true; // اختيار السنة القادمة من الكنترولر
    }
    select.appendChild(opt);
}

//salry
let oldSalary = $("#CurrentSalary").val().replace(/,/g, ""); // 🟢 نخزن المرتب القديم

// 1) تنسيق المرتب عند تحميل الصفحة (لو فيه قيمة قديمة)
$("input.amount").each(function () {
    handleInput(this);
});

// 2) تنسيق LIVE عند الكتابة + تحديث الشهر والسنة الحاليين عند الاختلاف فقط
$(document).on("input", "input.amount", function () {
    handleInput(this);

    let newSalary = $(this).val().replace(/,/g, "");

    if (newSalary !== oldSalary) {
        let now = new Date();
        let month = String(now.getMonth() + 1).padStart(2, '0');
        let year = now.getFullYear();

        if ($("#Month").val() !== month) {
            $("#Month").val(month);
        }
        if ($("#Year").val() !== String(year)) {
            $("#Year").val(year);
        }

        oldSalary = newSalary; // تحديث القيمة القديمة
    }
});
// 🟢 دالة تنسيق المرتب
function handleInput(input) {
    let rawValue = input.value;

    // إزالة أي حروف غير رقمية أو نقطة
    let value = rawValue.replace(/[^0-9.]/g, '');

    // السماح بنقطة عشرية واحدة فقط
    let parts = value.split('.');
    if (parts.length > 2) {
        value = parts[0] + '.' + parts.slice(1).join('');
    }

    // حالات خاصة: المستخدم كتب فقط نقطة أو رقم مع نقطة في النهاية
    if (value === '.' || /^\d+\.$/.test(value)) {
        input.dataset.raw = value;
        return;
    }

    if (!isNaN(value) && value !== '') {
        input.dataset.raw = value;

        // تقسيم الرقم قبل/بعد الفاصلة
        let [integerPart, decimalPart] = value.split('.');

        // تنسيق الجزء الصحيح
        integerPart = parseInt(integerPart || '0').toLocaleString('en-US');

        // تقطيع الجزء العشري لرقمين
        if (decimalPart !== undefined) {
            decimalPart = decimalPart.slice(0, 2);
        }

        // إعادة التجميع
        input.value = decimalPart !== undefined ? `${integerPart}.${decimalPart}` : integerPart;
    } else {
        input.value = '';
        input.dataset.raw = '';
    }
}

// اللغة الاجنبية 
$("#Name_AR,#Address_AR").keypress(function (e) {

    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
        if (unicode == 32)
            return true;
        else {
            if ((unicode < 48 || unicode > 57) && (unicode < 0x0600 || unicode > 0x06FF)) //if not a number or arabic
                return false; //disable key press
        }
    }
});

// English + spaces + numeric
$("#Name_EN,#Address_EN,#Email").keypress(function (e) {

    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
        if (unicode == 32)
            return true;
        else {
            if (!(unicode < 0x0600 || unicode > 0x06FF)) //if not english
                return false; //
        }
    }
});

$("#EmpCode").blur(function () {
    $.ajax({
        url: '/EmployeesData/HR_Employees/CheckEmpCode',
        data: {
            EmpCode: $("#EmpCode").val()
        },
        success: function (res) {
            if (res == 1) {
                alert('كود الموظف موجود مسبقا');
                $('#EmpCode').val('');
            }
        }
    });
});
$("#EmpCode ,#PhoneNumber").keypress(function (e) {
    var iKeyCode = e.charCode ? e.charCode : e.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;

    return true;
});

$('#IsManager, #CurrentBranchDept_ID').change(function () {

    if ($('#IsManager').is(':checked') && $('#CurrentBranchDept_ID').val() > 0) {

        //check if onther manager
        $.ajax({
            url: '/EmployeesData/HR_Employees/CheckBranchDeptManager',
            data: {
                branchDeptId: $("#CurrentBranchDept_ID").val()
            },
            success: function (res) {
                if (res == 1) {
                    alert('يوجد مدير لهذه الادارة بالفعل');
                    $('#IsManager').prop('checked', false);
                }
            }
        });
    }
});


$(document).ready(function () {
    if ($('#txtEmpId').val() > 0) {
        $('#EmpCode').attr("readonly", "readonly")

        $("#countryList").val($('#txtCountryId').val());
        $('#countryList').change();
    }
});
