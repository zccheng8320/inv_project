$(document).ready(function () {
var id = "@Model.ID";
if (navigator.userAgent.indexOf("MSIE") > 0) {
    //IE
    document.onkeydown = function () {
        switch (event.keyCode) {
            case 49:
                window.location.href = '/Customer/Search';
                break;
            case 50:
                alert('2');
                break;
            case 51:
                window.location.href = '/Customer/Insert';
                break;
            case 52:
                alert('4');
                break;
            case 53:
                alert('5');
                break;
            case 54:
                alert('6');
                break;
            case 65:
                alert('A');
                break;
            case 67:
                alert('C');
                break;
            case 87:
                alert('W');
                break;
            case 85:
                alert('U');
                break;
        }
    }
}
else {
    window.onkeydown = function () {
        switch (event.keyCode) {
            case 49:
                window.location.href = '/Customer/Search';
                break;
            case 50:
                window.location.href = '/Customer/Insert';
                break;
            case 51:
                window.location.href = '/Customer/Update?id=' + id;
                break;
            case 52:
                var password = prompt("請輸入密碼", "")
                if (password != "" && password != null)
                    window.location.href = '/Customer/Delete?id=' + id + '&password=' + password;
                break;
            case 53:
                Sajax(0);
                break;
            case 54:
                alert('6');
                break;
            case 65:
                alert('A');
                break;
            case 67:
                alert('C');
                break;
            case 87:
                alert('W');
                break;
            case 85:
                alert('U');
                break;
        }
    }
}
var apiurl = location.protocol + "//" + location.hostname + ":" + location.port + "/api/Customer";

    function Sajax(tmp) {
    $.ajax({
        url: apiurl + "?id=" + id + "&preOrNext=" + tmp,
        type: 'GET',
        success: function (data) {
            $("#tbodyshow").empty();
            id = data.ID;
            $("#tbodyshow").append(
                '<tr><td colspan="4" class="h1">客戶編號: ' + data.CUST_CODE + '</td ></tr>' +
                '<tr>< td colspan = "4" class= "h1" > 客戶名稱:' + data.CUST_NAME + '</td ></tr >' +
                '<tr>< td colspan = "4" class= "h1" > 公司地址:' + data.ADDRESS1 + '</td ></tr >' +
                '<tr>< td colspan = "4" class= "h1" > 發票地址:' + data.ADDRESS2 + '</td ></tr >' +
                '<tr><td colspan="2" class="h1">統一編號:' + data.UNIFORM + '</td>' +
                '<td colspan="2" class="h1">付款方式:' + data.PAY_WAY + '</td></tr>' +
                '<tr><td colspan="2" class="h1">聯絡人:' + data.ATTENTION + '</td>' +
                '<td class="h1">稅金 :' + data.TAX + '</td><td class="h1">% </td></tr>' +
                '<tr><td colspan="2" class="h1">電話:' + data.TELEPHONE + '</td>' +
                '<td colspan="2" class="h1">業務編號:' + data.SAL_NO + '</td></tr>' +
                '<tr><td colspan="2" class="h1">傳真:' + data.FAX + '</td>' +
                '<td class="h1">結帳日:' + data.PAY_DATE + '</td>' +
                '<td class="h1">標籤列印:' + data.PRT_LAB + ' </td></tr > ' +
                '<tr><td colspan="3" class="h1">備註:' + data.REMARK + '</td>' +
                '<td colspan="2" class="h1">異動日期:' + data.TRN_DATE + '</td></tr>' 
               );

        }
    });
}
});