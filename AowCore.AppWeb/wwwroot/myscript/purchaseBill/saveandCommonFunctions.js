var loading = $("#loading");
$(document).on({
    ajaxStart: function () {
        loading.show();
    },
    ajaxStop: function () {
        loading.hide();
    }
});
function JsonDate(jsonDate) {
    if (!jsonDate) {
        return jsonDate;
    }
    //    var yojsonDate = (new Date()).toJSON();
    var date = new Date(parseInt(jsonDate.substr(6)));
    var hours = date.getHours();
    var ampm = hours >= 12 ? 'pm' : 'am';
    var month = date.getMonth() + 1;
    return date.getFullYear() + '/' + month + '/' + date.getDate() + " " + date.getHours() + ":" + date.getMinutes() + " " + ampm;
    //return yojsonDate;
}

$("#LedgerName").autocomplete({
    minLength: 1,
    source: function (request, response) {
        var url = $(this.element).data("url");
        $.getJSON(url,
            { term: request.term },
            function (data) {
                response(data)
            });
    },
    appendTo: $(".modal-body ui-autocomplete-input"),
    select: function (event, ui) {
        $("#LedgerId").val(ui.item.id);
        $("#LedgerName").val(ui.item.Name);
        $('#VoucherNumber').focus()
    },
    change: function (event, ui) {
        if (!ui.item) {
            $(event.target).val("");
        }
    }

});

$('.SaveOrder').on("click",
    function (e) {
        e.preventDefault();
        let buttonValue = $(this).closest("input");
        let voucherId = $('.voucherId').val();

        let date = $('#sandbox-container').val();

        if (date == null || date == "") {
            alert("Please Enter Date");
            return false;
        }
        let loading = $("#loading");
        $(document).on({
            ajaxStart: function () {
                loading.show();
            },
            ajaxStop: function () {
                loading.hide();
            }
        });
        let OrderDetl = [];
        $('#tbodyitems tr').each(function () {
            let text = $(this).find("td").eq(4).html();
            if (text !== "Total") {

                let Name = $(this).find("td").eq(1).text().trim();
                //let Quantity = parseFloat($(this).find("td").eq(3).html()).toFixed(2);;
                let ItemAmount = $(this).find(".hdnappendAmount").val();
                let MRPPerUnit = $(this).find(".hdnappendMRP").val();
                let Quantity = $(this).find(".hdnappendQuantity").val();
                let ProductId = $(this).find(".hdnappendProductId").val();
                let productLedgerId = $(this).find(".hdnappendProductAccountId").val();
                let hdnAccountCategoryName = $(this).find(".hdnappendhdnAccountCategoryName").val();
                let hdnappendhdnItemType = $(this).find(".hdnappendhdnItemType").val();
                OrderDetl.push({
                    'Name': Name,
                    'ProductId': ProductId,
                    // 'ProductLedgerId': productLedgerId,
                    'MRPPerUnit': MRPPerUnit,
                    'Quantity': Quantity,
                    'ItemAmount': ItemAmount,
                    'AccountCategoryName': hdnAccountCategoryName,
                    'ItemType': hdnappendhdnItemType
                });
            }
        });

        let sundryItems = [];
        $('#tbodysundryitems tr').each(function () {
            let text = $(this).find("td").eq(1).html();
            if (text != "Total") {
                let sundtryItemId = $(this).find(".hdnappendSundryProductId").val();
                let ledgerId = $(this).find(".hdnappendSundryAccountId").val();
                let SundryAmount = $(this).find(".hdnappendSundryAmount").val();
                let sundryPercent = $(this).find(".hdnappendSundryPercent").val();

                sundryItems.push({
                    'ProductId': sundtryItemId,
                    'LedgerId': ledgerId,
                    'ItemAmount': SundryAmount,
                    'Percent': sundryPercent
                });
            }
        });

        //  alert(JSON.stringify(OrderDetl) + '  ' + OrderDetl.length)
        alert(JSON.stringify(sundryItems) + '  ' + sundryItems.length)
        //if (OrderDetl.length == 0) {
        //    $('#OrderDetl').val(JSON.stringify(OrderDetl))
        //    alert("You Need to add item")
        //    return false;
        //}
        //if ($("#frmitems").valid()) {
        let voucherName = $('#VoucherName').val();
        $(".SaveOrder").attr("class", "btn btn-primary SaveOrder disabled");
        let form = $('#addPurchaseBill');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
        let url = form.prop('action');
        let Invoice = $('#VoucherNumber').val();
        let AccountId = $('#LedgerId').val();
        let termsDays = $('#terms').val();
        let Note = $('#editor1').val();
        let Total = $('#Total').val();
        $.ajax({
            url: url,
            type: "POST",
            data: {
                __RequestVerificationToken: token,
                data: JSON.stringify(OrderDetl),
                data2: JSON.stringify(sundryItems),
                VoucherId: voucherId,
                voucherName: voucherName,
                Invoice: Invoice,
                Date: date,
                termsDays: termsDays,
                AccountId: AccountId,
                Total: Total,
                Note: Note,
                buttonValue: buttonValue.val()
            },
            dataType: "json",
            success: function (data) {
                if (data.success == true) {
                    $('#loading').html(data);
                    $("#SaveClose").attr("class", "btn btn-default SaveOrder");
                    $("#SaveNext").attr("class", "btn btn-success SaveOrder");
                    showNotification("Value Saved successfully", "success");
                    ClearOthers();
                    if (data.voucherName == "Purchase Bill") {
                        window.location = "/MyBooks/VouchersWithItems/Index?voucherName=Purchase Bill";
                    }
                    else {
                        window.location = "/MyBooks/VouchersWithItems/Index?voucherName=Sale Invoice";
                    }

                    //$('#AddItems').hide();
                    // location.reload();
                    // window.location = data.newLocation + "?vouchername=" + data.vouchername;
                }
                if (data.success == false) {
                    showNotification(data.message, "error");
                    $("#SaveClose").attr("class", "btn btn-default SaveOrder");
                    $("#SaveNext").attr("class", "btn btn-success SaveOrder");
                }
                else {
                    showNotification(data.message, "error");
                    $(".SaveOrder").attr("class", "btn btn-primary SaveOrder");
                }
                //if (data.success == false) {
                //    showNotification(data.message, "failed");
                //   // $("#MessageToClient").text(data.message);
                //    $(".SaveOrder").attr("class", "btn btn-primary SaveOrder");
                //}
            },
            error: function (xhr, status, error) {
                $("#MessageToClient").text(xhr.responseText);
                $(".SaveOrder").attr("class", "btn btn-primary SaveOrder");
                showNotification("Something happan wrong", "failed");
            }
        });

    });