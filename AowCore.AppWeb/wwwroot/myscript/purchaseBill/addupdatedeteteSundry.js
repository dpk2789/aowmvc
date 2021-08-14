
$("#txtSundryItem").autocomplete({
    minLength: 1,
    source: function (request, response) {
        const url = $(this.element).data("url");
        $.getJSON(url,
            { term: request.term },
            function (data) {
                response(data)
            });
    },
    appendTo: $(".modal-body ui-autocomplete-input"),
    select: function (event, ui) {
        $("#txtSundryItem").val(ui.item.name);
        $("#hdnSundryProductId").val(ui.item.productId);
        let itemtotal = $("#hdnItemTotal").val();
        if (ui.item.itemtype === "sundryitem" || ui.item.itemtype === "SundryItem") {
            $("#txtPercent").val(ui.item.percent);
            let percent = ui.item.percent;
            let amount = 0;
            amount = (percent * itemtotal / 100);
            $("#txtDebit").val(amount);

        } else {
            $("#txtPercent").val(ui.item.mRPPerUnit);
        }
        $("#hdnSundryAccountCategoryName").val(ui.item.itemCategoryName);
        $("#hdnSundryAccountId").val(ui.item.productLedgerId);
    },
    change: function (event, ui) {
        if (!ui.item) {
            $(event.target).val("");
        }
    }

});

function ClearSundryAdd() {
    $('#txtSundryItem').val('');
    $('#txtPercent').val('');
    $('#txtDebit').val('');
    $('#hdnSundryProductId').val('');
    $('#hdnSundryAccountId').val('');
    $('#hdnSundryAccountCategoryName').val('');
    $('#hdnSundryItemType').val('');
    $('#btnupdate').hide();
    $('#btnnew').hide();
    $('#btnadd').show();
}

$('.btnaccountadd').on("click", function (e) {
    let otheraccounttotal = 0;
    let newcontent = "";
    let hdnSundryItemType = $('#hdnSundryItemType').val();
    let HeadName = $("#X option:selected").text();
    let hdnSundryProductId = $('#hdnSundryProductId').val();
    let hdnSundryAccountId = $('#hdnSundryAccountId').val();
    let hdnTotal = $('#hdnItemTotal').val();
    let hdnSundryAccountCategoryName = $('#hdnSundryAccountCategoryName').val();
    let txtSundryItem = $('#txtSundryItem').val();
    let txtPercent = $('#txtPercent').val();
    let txtDebit = $('#txtDebit').val();
    let hdnAmount = $('#hdnDebitAmount').val();
    $('.dvitemexistmsg').html("");
    newcontent = "";
    var itemstotal = 0;
    $('#tbodysundryitems tr').each(function () {
        let amnt = $(this).find('.hdnappendSundryAmount').val();
        let text = $(this).find("td").eq(1).html();
        if (text !== "Total") {

            let hdnappendSundryName = $(this).find(".hdnappendSundryName").val();
            let hdnappendSundryPercent = $(this).find('.hdnappendSundryPercent').val();
            let hdnappendSundryProductId = $(this).find('.hdnappendSundryProductId').val();


            itemstotal = parseFloat(itemstotal) + parseFloat(amnt);
            newcontent += "<tr >";
            newcontent += "<td>" + $(this).find("td").eq(0).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(1).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(2).html(); + " </td >";
            newcontent += "<td   >";
            newcontent += "<input type='hidden' class='hdnappendSundryName' value=" + hdnappendSundryName + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryAccountCategoryName' value=" + hdnSundryAccountCategoryName + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryProductId' value=" + hdnappendSundryProductId + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryPercent' value=" + hdnappendSundryPercent + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryAccountId' value=" + hdnSundryAccountId + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryAmount' value=" + amnt + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryItemType' value=" + hdnSundryItemType + " />";
            newcontent += "<a  class='fa fa-edit sundryedit' style='cursor:pointer;'/>";
            newcontent += "<a  class='fa fa-trash sundrydelete' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }
    });

    itemstotal = parseFloat(itemstotal) + parseFloat(txtDebit);

    newcontent += "<tr >";
    newcontent += "<td  >" + txtSundryItem + " </td >";
    newcontent += "<td  >" + txtPercent + " </td >";
    newcontent += "<td  >" + txtDebit + " </td >";
    newcontent += "<td   >";
    newcontent += "<input type='hidden' class='hdnappendSundryName' value=" + txtSundryItem + " />";
    newcontent += "<input type='hidden' class='hdnappendSundryAccountCategoryName' value=" + hdnSundryAccountCategoryName + " />";
    newcontent += "<input type='hidden' class='hdnappendSundryProductId' value=" + hdnSundryProductId + " />";
    newcontent += "<input type='hidden' class='hdnappendSundryPercent' value=" + txtPercent + " />";
    newcontent += "<input type='hidden' class='hdnappendSundryAccountId' value=" + hdnSundryAccountId + " />";
    newcontent += "<input type='hidden' class='hdnappendSundryAmount' value=" + txtDebit + " />";
    newcontent += "<input type='hidden' class='hdnappendSundryItemType' value=" + hdnSundryItemType + " />";
    newcontent += "<a  class='fa fa-edit sundryedit' style='cursor:pointer;'/>";
    newcontent += "<a  class='fa fa-trash sundrydelete' style='cursor:pointer;'/>";
    newcontent += "</td >";
    newcontent += "</tr >";

    //calculate total
    newcontent += "<tr >";
    newcontent += "<td  ></td >";
    newcontent += "<td  >Total</td >";
    newcontent += "<td class='otheraccounttotalclass'>" + parseFloat(itemstotal).toFixed(2) + " </td >";
    newcontent += "</tr >";
    $('#tbodysundryitems').empty().append(newcontent);
    document.getElementById("hdnSundryTotal").value = parseFloat(itemstotal).toFixed(2);
    document.getElementById("Total").value = parseFloat(itemstotal) + parseFloat(hdnTotal);
    if (hdnSundryAccountCategoryName.replace(/\s/g, '') === "Discount") {
        hdnTotal = parseFloat(hdnTotal) - parseFloat(txtDebit);
        document.getElementById("Total").value = hdnTotal;
    }

    $(".showimg").hide();
    ClearSundryAdd();
});

$('.btnaccountupdate').on("click", function (e) {
    var otheraccounttotal = 0;
    var newcontent = "";
    var OtherAccountId = $('#OtherAccountId').val();
    var OtherAccountIdName = $("#OtherAccountId option:selected").text();
    var txtDebit = $('#txtDebit').val();


    $('.dvitemexistmsg').html("");
    var isItemEdit = false;
    newcontent = "";
    $('#tbodyotheraccounts tr').each(function () {
        var itemidrw = $(this).find('.pid').val();
        if (AccountId == itemidrw) {
            isItemEdit = true;
            newcontent += "<tr >";
            newcontent += "<td   >";
            newcontent += "<input type='hidden' class='oaid' value=" + OtherAccountId + " />";
            newcontent += "<p class='oaname'>" + OtherAccountIdName + "</p>";
            newcontent += "</td >";
            newcontent += "<td  >" + txtDebit + " </td >";
            newcontent += "<td  >";
            newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
            newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }
        else {
            newcontent += "<tr >";
            newcontent += "<td>" + $(this).find("td").eq(0).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(1).html(); + " </td >";
            newcontent += "<td   >";
            newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
            newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }

    });
    if (isItemEdit === false) {
        newcontent += "<tr >";
        newcontent += "<td   >";
        newcontent += "<input type='hidden' class='oaid' value=" + OtherAccountId + " />";
        newcontent += "<p class='oaname'>" + OtherAccountIdName + "</p>";
        newcontent += "</td >";
        newcontent += "<td  >" + txtDebit + " </td >";
        newcontent += "<td  >";
        newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
        newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
        newcontent += "</td >";
        newcontent += "</tr >";
    }

    //calculate total
    newcontent += "<tr >";
    newcontent += "<td  >Total</td >";
    newcontent += "<td  class='otheraccounttotalclass' align='right'>" + parseFloat(otheraccounttotal).toFixed(2) + " </td >";
    newcontent += "</tr >";

    $('#tbodyotheraccounts').empty().append(newcontent);
    document.getElementById("VoucherTotal").value = parseFloat($('#itemstotalclass').html()) + otheraccounttotal;
    document.getElementById("hdnVoucherTotal").value = parseFloat(itemstotal).toFixed(2) + otheraccounttotal;
    ClearAdd();
});


$("#tbodysundryitems").on('click', '.sundryedit', function (e) {
    e.preventDefault();
    const tr = $(this).closest('tr');

    const txtDescription = tr.find("td").eq(1).html();
    var txtStoreItem = tr.find("td").eq(0).html();
    var txtMRP = tr.find("td").eq(2).html();
    var txtQuantity = tr.find("td").eq(3).html();
    var txtAmount = tr.find("td").eq(4).html();

    $('#txtDescription').val(txtDescription);
    $('#txtStoreItem').val(txtStoreItem);
    $('#txtMRP').val(txtMRP);
    $('#txtQuantity').val(txtQuantity);
    $('#txtAmount').val(txtAmount);

    $('#btnupdate').show();
    $('#btnnew').show();
    $('#btnadd').hide();
});

$("#tbodysundryitems").on('click', '.sundrydelete', function (e) {
    e.preventDefault();
    var tr = $(this).closest('tr');
    tr.css("background-color", "#ff3700");
    tr.remove();
    yoCalculateSundryTotal();
    ClearSundryAdd();
});

function yoCalculateSundryTotal() {
    let hdnProductsTotal = $('#hdnItemTotal').val();
    $('.dvitemexistmsg').html("");
    var newcontent = "";
    newcontent = "";
    let itemstotal = 0;
    $('#tbodysundryitems tr').each(function () {
        let text = $(this).find("td").eq(1).html();
        if (text !== "Total") {
            let amnt = $(this).find('.hdnappendSundryAmount').val();

            var hdnappendhdnItemType = $(this).find(".hdnappendSundryItemType").val();
            itemstotal = parseFloat(itemstotal) + parseFloat(amnt);
            newcontent += "<tr >";
            newcontent += "<td>" + $(this).find("td").eq(0).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(1).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(2).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(3).html(); + " </td >";
            newcontent += "<td   >";


            let hdnAccountCategoryName = $(this).find('.hdnappendSundryAccountCategoryName').val();
            var hdnappendProductName = $(this).find('.hdnappendSundryName').val();
            var hdnappendAmount = $(this).find(".hdnappendSundryAmount").val();
            let hdnappendProductId = $(this).find(".hdnappendSundryProductId").val();
            let hdnappendProductAccountId = $(this).find(".hdnappendSundryAccountId").val();

            newcontent += "<input type='hidden' class='hdnappendSundryAccountCategoryName' value=" + hdnAccountCategoryName + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryProductId' value=" + hdnappendProductId + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryAccountId' value=" + hdnappendProductAccountId + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryName' value=" + txtStoreItem + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryAmount' value=" + amnt + " />";
            newcontent += "<input type='hidden' class='hdnappendSundryItemType' value=" + hdnappendhdnItemType + " />";
            newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
            newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }
    });

    //calculate total
    newcontent += "<tr >";
    newcontent += "<td  ><input type='hidden' class='itemstotalhdnclass' value=" + itemstotal + " /></td >";
    newcontent += "<td  >Total</td >";
    newcontent += "<td  class='itemstotalclass'>" + numberWithCommas(parseFloat(itemstotal).toFixed(2)) + " </td >";
    newcontent += "</tr >";

    $('#tbodysundryitems').empty().append(newcontent);
    //document.getElementById("ProductsTotal").value = numberWithCommas(parseFloat(itemstotal).toFixed(2));
    document.getElementById("hdnSundryTotal").value = parseFloat(itemstotal).toFixed(2);

    document.getElementById("Total").value = numberWithCommas(parseFloat(hdnProductsTotal) + parseFloat(itemstotal));
    document.getElementById("hdnProductsTotal").value = parseFloat(hdnProductsTotal) + parseFloat(itemstotal);
};