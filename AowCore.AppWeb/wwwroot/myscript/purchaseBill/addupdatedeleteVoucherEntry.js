function ClearAddCrDr() {
    //   $('#CrDrType').val('0');
    $('#hdnProductAccountId').val('');
    $('#hdnProductId').val('');
    $('#txtStoreItem').val('');
    $('#txtAmount').val('');
    $('#btnupdate').hide();
    $('#btnnew').hide();
    $('#btnadd').show();
    $('#CrDrType').focus();
}

let sNo = 0;
function Add() {
    //   $('#CrDrType').val('0');
    var num1 = sNo;
    var answer = parseInt(num1) + parseInt(1);
    sNo++;
    return (answer);
}

var loading = $("#loading");
$('#CrDrType').on("change", function (e) {
    let crDrType = $("#CrDrType option:selected").text();
    $(document).on({
        ajaxStart: function () {
            loading.show();
        },
        ajaxStop: function () {
            loading.hide();
        }
    });
    if (crDrType === 'Cr') {
        $("#txtAmount").prop("disabled", true);
        $("#txtAmountCr").prop("disabled", false);
        $("#txtStoreItem").focus();
    }
    if (crDrType === 'Dr') {
        $("#txtAmountCr").prop("disabled", true);
        $("#txtAmount").prop("disabled", false);
        $("#txtStoreItem").prop("disabled", false);
        $("#txtStoreItem").focus();
    }

});

$("#txtStoreItem").autocomplete({
    minLength: 1,
    source: function (request, response) {
        let url = $(this.element).data("url");
        let crdr = $("#CrDrType option:selected").text();
        let voucherName = $('#VoucherName').val();
        $.getJSON(url, { term: request.term, HeadName: voucherName, crdr: crdr }, function (data) {
            response(data)
        })
    },
    appendTo: $(".modal-body ui-autocomplete-input"),
    select: function (event, ui) {
        if (ui.item.value === "no company") {
            window.location = "/";
        }
        var headName = $("#CrDrType option:selected").text();
        if (headName === 'Cr') {
            $('#txtAmountCr').focus()
        }
        if (headName === 'Dr') {
            $('#txtAmount').focus()
        }
        $("#hdnProductId").val(ui.item.id);
        $("#txtStoreItem").val(ui.item.value);
        $("#hdnAccountCategoryName").val(ui.item.AccountCategoryName);

    },
    change: function (event, ui) {
        if (!ui.item) {
            // alert(xhr.responseText);
            $(event.target).val("");
        }
    }
});


$('.btnadd').on("click", function (e) {
    let newcontent = "";
    let AccountId = $('#hdnProductAccountId').val();
    let ledgerId = $('#hdnProductId').val();
    let ledgerName = $("#txtStoreItem").val();
    let CrDrType = $('#CrDrType').val();
    let CrDrTypeName = $("#CrDrType option:selected").text();
    let txtAmount = $('#txtAmount').val();
    let txtAmountCr = $('#txtAmountCr').val();
    let itemType = $('#hdnItemType').val();

    $('.dvitemexistmsg').html("");
    newcontent = "";
    let x = 0;
    $('#tbodyorder tr').each(function () {
        x = parseFloat(x) + parseFloat(1);

        let hdnappendhdnItemType = $(this).find(".hdnappendhdnItemType").val();
        let hdnappendProductId = $(this).find(".hdnappendProductId").val();
        let hdnappendLedgerName = $(this).find(".hdnappendLedgerName").val();
        let hdnappendAmount = $(this).find(".hdnappendAmount").val();
        let hdnappendAmountCr = $(this).find(".hdnappendAmountCr").val();


        newcontent += "<tr >";
        newcontent += "<td>" + x + " </td >";
        newcontent += "<td>" + $(this).find("td").eq(1).html(); + " </td >";
        newcontent += "<td>" + $(this).find("td").eq(2).html(); + " </td >";
        newcontent += "<td>" + $(this).find("td").eq(3).html(); + " </td >";
        newcontent += "<td >" + parseFloat($(this).find("td").eq(4).html()).toFixed(2); + "</td >";

        newcontent += "<td  >";
        newcontent += "<input type='hidden' class='hdnappendProductId' value=" + hdnappendProductId + " />";
        newcontent += "<input type='hidden' class='hdnappendLedgerName' value=" + hdnappendLedgerName + " />";
        newcontent += "<input type='hidden' class='hdnappendAmount' value=" + hdnappendAmount + " />";
        newcontent += "<input type='hidden' class='hdnappendAmountCr' value=" + hdnappendAmountCr + " />";
        newcontent += "<input type='hidden' class='hdnappendhdnItemType' value=" + hdnappendhdnItemType + " />";
        //newcontent += "<a  class='fa fa-edit edit' style='cursor:pointer;'/>";
        newcontent += "<a  class='fa fa-eraser delete' style='cursor:pointer;'/>";
        newcontent += "</td >";
        newcontent += "</tr >";
    });

    // totalamnt = parseFloat(totalamnt) + parseFloat(Amount);
    sNo = parseFloat(sNo) + parseFloat(1);
    if (txtAmountCr === "")
        txtAmountCr = 0;
    if (txtAmount === "")
        txtAmount = 0;


    newcontent += "<tr >";
    newcontent += "<td  >" + sNo + " </td >";
    newcontent += "<td   >";
    newcontent += "<input type='hidden' class='cdid' value=" + CrDrType + " />";
    newcontent += "<p class='crdrname'>" + CrDrTypeName + "</p>";
    newcontent += "</td >";
    newcontent += "<td   >";
    newcontent += "<input type='hidden' class='pid' value=" + ledgerId + " />";
    newcontent += "<p class='productname'>" + ledgerName + "</p>";
    newcontent += "</td >";
    newcontent += "<td  >" + txtAmount + " </td >";
    newcontent += "<td  >" + txtAmountCr + " </td >";
    newcontent += "<td  >";
    newcontent += "<input type='hidden' class='hdnappendLedgerName' />";
    newcontent += "<input type='hidden' class='hdnappendProductId' value=" + ledgerId + " />";
    newcontent += "<input type='hidden' class='hdnappendAmount' value=" + txtAmount + " />";
    newcontent += "<input type='hidden' class='hdnappendAmountCr' value=" + txtAmountCr + " />";
    newcontent += "<input type='hidden' class='hdnappendhdnItemType' value=" + itemType + " />";
    //newcontent += "<a  class='fa fa-edit edit' style='cursor:pointer;'/>";
    newcontent += "<a  class='fa fa-eraser delete' style='cursor:pointer;'/>";
    newcontent += "</td >";
    newcontent += "</tr >";

    $('#tbodyorder').empty().append(newcontent);
    //$(".hdnappendLedgerName").val(ledgerName);
    //  $(".showimg").hide();
    ClearAddCrDr();
});

$('.btnupdate').on("click", function (e) {
    let totalamnt = 0;
    let newcontent = "";
    let ledgerId = $('#pid').val();
    let AccountIdName = $("#AccountId option:selected").text();
    let CrDrType = $('#CrDrType').val();
    let CrDrTypeName = $("#CrDrType option:selected").text();
    let txtAmount = $('#txtAmount').val();

    $('.dvitemexistmsg').html("");
    var isItemEdit = false;
    newcontent = "";
    $('#tbodyorder tr').each(function () {
        let itemidrw = $(this).find('.pid').val();
        if (AccountId == itemidrw) {
            isItemEdit = true;
            newcontent += "<tr >";
            newcontent += "<td   >";
            newcontent += "<input type='hidden' class='cdid' value=" + CrDrType + " />";
            newcontent += "<p class='crdrname'>" + CrDrTypeName + "</p>";
            newcontent += "</td >";
            newcontent += "<td   >";
            newcontent += "<input type='hidden' class='pid' value=" + ledgerId + " />";
            newcontent += "<p class='productname'>" + AccountIdName + "</p>";
            newcontent += "</td >";
            newcontent += "<td  >" + txtAmount + " </td >";
            newcontent += "<td   >";
            newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
            newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }
        else {
            newcontent += "<tr >";
            newcontent += "<td>" + $(this).find("td").eq(0).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(1).html(); + " </td >";
            newcontent += "<td  >" + parseFloat($(this).find("td").eq(2).html()).toFixed(2); + "</td >";
            newcontent += "<td   >";
            newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
            newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }

    });
    if (isItemEdit == false) {
        newcontent += "<tr >";
        newcontent += "<td   >";
        newcontent += "<input type='hidden' class='cdid' value=" + CrDrType + " />";
        newcontent += "<p class='crdrname'>" + CrDrTypeName + "</p>";
        newcontent += "</td >";
        newcontent += "<td   >";
        newcontent += "<input type='hidden' class='pid' value=" + ledgerId + " />";
        newcontent += "<p class='productname'>" + AccountIdName + "</p>";
        newcontent += "</td >";
        newcontent += "<td  >" + txtAmount + " </td >";
        newcontent += "<td   >";
        newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
        newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
        newcontent += "</td >";
        newcontent += "</tr >";
    }

    newcontent += "<tr >";
    newcontent += "<td ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td  >Total</td >";
    newcontent += "<td    align='right'>" + parseFloat(totalamnt).toFixed(2) + " </td >";
    newcontent += "</tr >";

    $('#tbodyorder').empty().append(newcontent);
    ClearAddClearAddCrDr()
});

$("#tbodyorder").on('click', '.edit', function (e) {
    e.preventDefault();
    let tr = $(this).closest('tr');


    var CrDrType = tr.find(".cdid").val();
    var ledgerId = tr.find(".pid").val();
    var ledgerName = tr.find(".hdnappendLedgerName").val();
    var ledgerAmount = tr.find(".hdnappendAmount").val();
    var txtAmount = tr.find("td").eq(2).html();

    $('#CrDrType').val(CrDrType);
    $('#hdnProductId').val(ledgerId);
    $('#hdnledgerName').val(ledgerName);
    $('#txtStoreItem').val(ledgerName);
    $('#txtAmount').val(ledgerAmount);


    $('#btnupdate').show();
    $('#btnnew').show();
    $('#btnadd').hide();
})

$("#tbodyorder").on('click', '.delete', function (e) {
    // e.preventDefault();
    var tr = $(this).closest('tr');
    tr.css("background-color", "#FF3700");
    tr.remove();
    //  ReCalculateTotal();
})

$('.SaveOrder').on("click", function (e) {
    e.preventDefault();
    //$(".ajax-loading-block-window").show();
    var OrderDetl = [];

    $('#tbodyorder tr').each(function () {
        var CrDrType = $(this).find(".cdid").val();
        var LedgerId = $(this).find(".hdnappendProductId").val();
        var drAmount = $(this).find(".hdnappendAmount").val();
        var crAmount = $(this).find(".hdnappendAmountCr").val();
        //var ledgerName = $(this).find(".hdnappendLedgerName").val();

        var hdnappendhdnItemType = $(this).find(".hdnappendhdnItemType").val();
        OrderDetl.push({
            'CrDrType': CrDrType, 'LedgerId': LedgerId, 'DebitAmount': drAmount, 'CreditAmount': crAmount
        });
    });

    alert(JSON.stringify(OrderDetl))
    if (OrderDetl.length > 0) {
        $('#OrderDetl').val(JSON.stringify(OrderDetl))
    }

    let form = $('#questionForm');
    let token = $('input[name="__RequestVerificationToken"]', form).val();
    let url = form.prop('action');
    $('#loading').html('<img src="/Content/ajax-loader.gif" /> loading...');
    $(".SaveOrder").attr("class", "btn btn-primary SaveOrder disabled");
    let Invoice = $('#VoucherNumber').val();
    let Date = $('#Date').val();
    //  let Date = $('#Date').val();
    let voucherData = {
        Date: $('#Date').val(),
        Invoice: $('#VoucherNumber').val(),
        DateCreated: new Date(),
        Note: $('#editor1').val()
    };
    let Id = $('#VoucherId').val();
    alert(Id);
    $.ajax({
        url: url,
        type: "POST",
        data: {
            __RequestVerificationToken: token, Id: Id, data: JSON.stringify(OrderDetl), voucherData: JSON.stringify(voucherData), Invoice: Invoice, Date: Date
        },
        dataType: "json",
        success: function (data) {
            if (data.success == true) {
                $('#loading').html(data);
                $(".SaveOrder").attr("class", "btn btn-success SaveOrder");
                showNotification("Value Saved successfully", "success");
                window.location = "/MyBooks/JournalEntries";
            }
        },
        error: function (xhr, status) {
            alert(xhr.responseText);
            $(".ajax-loading-block-window").hide();
            $(".SaveOrder").attr("class", "btn btn-primary SaveOrder");
            showNotification(data.message, "failed");
        }
    });
    //}
    //else {
    //    //alert('error');
    //    $(".ajax-loading-block-window").hide();
    //}
})