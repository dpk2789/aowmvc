
function ClearItemVarientAdd() {
    $('#txtVarientDescription').val('');
    $('#txtVarientItem').val('');   
    $('#hdnVarientId').val('');
    $('#txtVarientMRP').val('');
    $('#txtVarientQuantity').val('');
    $('#txtVarientAmount').val('');
    $('#btnVarientUpdate').hide();
    $('#btVarientDelete').hide();
    $('#btnVarientAdd').show();
}

function ClearOthers() {
    $('#Head').val('');
    $('#Invoice').val('');
    $('#Date').val('');
    $('#Particular').val('');
    $('#ParticularId').val('');
    $('#CustomerName').val('');
    $('#Total').val('');
    $('#MembershipId').val('');
    //  $('#Net').val('');
}

function recalculateExtendedPriceVarient() {
    var quantity = parseFloat(document.getElementById("txtVarientQuantity").value).toFixed(0);
    var unitPrice = parseFloat(document.getElementById("txtVarientMRP").value).toFixed(2);

    if (isNaN(quantity)) {
        quantity = 1;
    }

    if (isNaN(unitPrice)) {
        unitPrice = 0;
    }

    document.getElementById("txtVarientQuantity").value = quantity;
    document.getElementById("txtVarientMRP").value = unitPrice;

    document.getElementById("txtVarientAmount").value = (quantity * unitPrice).toFixed(2);
    //document.getElementById("hdnAmount").value = (quantity * unitPrice).toFixed(2);
}

function CalulateCurrentDueVarient() {
    var amountTaken = parseFloat(document.getElementById("AmountTaken").value).toFixed(0);
    var amountPaid = parseFloat(document.getElementById("AmountPaid").value).toFixed(2);
    var total = parseFloat(document.getElementById("Total").value).toFixed(2);
    var outStandingAmount = parseFloat(document.getElementById("OutStandingAmount").value).toFixed(2);
    var hdnDue = parseFloat(document.getElementById("HdnDue").value).toFixed(2);
    var productsTotal;
    if (isNaN(total) && isNaN(outStandingAmount)) {
        productsTotal = 0;
        outStandingAmount = 0;
        document.getElementById("Due").value = numberWithCommas((hdnDue - amountTaken).toFixed(2));
        document.getElementById("HdnDue").value = (hdnDue - amountTaken).toFixed(2);

        return true;
    }

    if (isNaN(total)) {
        productsTotal = 0;
        document.getElementById("Due").value = numberWithCommas((outStandingAmount - amountTaken).toFixed(2));
        document.getElementById("HdnDue").value = (outStandingAmount - amountTaken).toFixed(2);
        //document.getElementById("Due").value = (outStandingAmount - amountTaken).toFixed(2);
        return true;
    }


    if (isNaN(amountTaken)) {
        amountTaken = 0;
    }

    if (isNaN(amountPaid)) {
        amountPaid = 0;
    }

    document.getElementById("AmountTaken").value = amountTaken;
    document.getElementById("AmountPaid").value = amountPaid;

    document.getElementById("Due").value = numberWithCommas((total - amountTaken).toFixed(2));
    document.getElementById("HdnDue").value = (total - amountTaken).toFixed(2);
}


$('.btnVarientAdd').on("click", function (e) {
    let newcontent = "";  
    let ItemType = $('#hdnItemType').val();   
    let txtStoreItem = $('#txtVarientItem').val();
    let txtDescription = $('#txtVarientDescription').val();   
    let hdnVarientId = $('#hdnVarientId').val();
    let txtMRP = $('#txtVarientMRP').val();
    let txtQuantity = $('#txtVarientQuantity').val();
    let txtAmount = $('#txtVarientAmount').val();
    let hdnAmount = $('#hdnVarientAmount').val();
  

    golbalPreviousAmount = txtAmount;
    if (isNaN(txtQuantity) || txtQuantity === "") {
        txtQuantity = 0;
    }
    let hdnAccountCategoryName = $('#hdnAccountCategoryName').val();
    //  alert(hdnAccountCategoryName)
    $('.dvitemexistmsg').html("");
    newcontent = "";
    let itemstotal = 0;
    let itemstotalWithOutTax = 0;
    let dueMoney = 0;
    let x = 0;

    $('#tbodyVarientItems tr').each(function () {
        let text = $(this).find("td").eq(4).html();
        if (text !== "Total") {

            let hdnappendhdnItemType = $(this).find(".hdnappendVarientItemType").val();         
            let hdnappendProductName = $(this).find('.hdnappendVarientProductName').val();
            let amnt = $(this).find('.hdnappendVarientAmount').val();

            itemstotal = parseFloat(itemstotal) + parseFloat(amnt);
            itemstotalWithOutTax = parseFloat(itemstotalWithOutTax) + parseFloat(amnt);
            dueMoney = parseFloat(dueMoney) + parseFloat(amnt);
            x = parseFloat(x) + parseFloat(1);

            newcontent += "<tr >";
            newcontent += "<td>" + x + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(1).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(2).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(3).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(4).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(5).html(); + " </td >";
            newcontent += "<td   >";


            let hdnappendMRP = $(this).find('.hdnappendVarientMRP').val();
            let hdnappendQuantity = $(this).find('.hdnappendVarientQuantity').val();
            let hdnappendProductId = $(this).find(".hdnappendVarientProductId").val();          
            let hdnappenItemDesc = $(this).find(".hdnappenVarientItemDesc").val();

           
            newcontent += "<input type='hidden' class='hdnappendVarientProductId' value=" + hdnappendProductId + " />";      
            newcontent += "<input type='hidden' class='hdnappendVarientProductName' value=" + hdnappendProductName + " />";
            newcontent += "<input type='hidden' class='hdnappenVarientItemDesc' value=" + hdnappenItemDesc + " />";
            newcontent += "<input type='hidden' class='hdnappendVarientAmount' value=" + amnt + " />";
            newcontent += "<input type='hidden' class='hdnappendVarientMRP' value=" + hdnappendMRP + " />";
            newcontent += "<input type='hidden' class='hdnappendVarientQuantity' value=" + hdnappendQuantity + " />";
            newcontent += "<input type='hidden' class='hdnappendhdnVarientItemType' value=" + hdnappendhdnItemType + " />";
            newcontent += "<a  class='fa fa-edit editVarient' style='cursor:pointer;'/>";
            newcontent += "<a  class='fa fa-trash deleteVarient' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }
    });

    itemstotal = parseFloat(itemstotal) + parseFloat(txtAmount);
    x = parseFloat(x) + parseFloat(1);

    //alert(hdnBatchId);
    newcontent += "<tr >";
    newcontent += "<td  >" + x + " </td >";
    newcontent += "<td  class='txtStoreItem'>" + txtStoreItem + " </td >";
    newcontent += "<td  class='txtDescription'>" + txtDescription + " </td >";
    newcontent += "<td  >" + txtMRP + " </td >";
    newcontent += "<td  >" + txtQuantity + " </td >";
    newcontent += "<td  >" + txtAmount + " </td >";
    newcontent += "<td   >";   
    newcontent += "<input type='hidden' class='hdnappendVarientProductId' value=" + hdnVarientId + " />";
    newcontent += "<input type='hidden' class='hdnappendVarientProductName' value=" + txtStoreItem + " />";    
    newcontent += "<input type='hidden' class='hdnappenVarientItemDesc' value=" + txtDescription + " />";
    newcontent += "<input type='hidden' class='hdnappendVarientMRP' value=" + txtMRP + " />";
    newcontent += "<input type='hidden' class='hdnappendVarientQuantity' value=" + txtQuantity + " />";
    newcontent += "<input type='hidden' class='hdnappendVarientAmount' value=" + parseFloat(txtAmount) + " />";   
    newcontent += "<input type='hidden' class='hdnappendVarientItemType' value=" + ItemType + " />";
    newcontent += "<a  class='fa fa-edit editVarient' style='cursor:pointer;'/>";
    newcontent += "<a  class='fa fa-trash deleteVarient' style='cursor:pointer;'/>";
    newcontent += "</td >";
    newcontent += "</tr >";


    if (txtStoreItem === "SubTotal") {
        let subTotal = 0;
        $('#tbodyVarientItems tr').each(function () {
            let text = $(this).find("td").eq(3).html();
            if (text !== "Total") {
                let amnt = $(this).find('.hdnappendVarientAmount').val();
                subTotal = parseFloat(subTotal) + parseFloat(amnt);
            }
        });

        newcontent += "<tr >";
        newcontent += "<td  ></td >";
        newcontent += "<td  ></td >";
        newcontent += "<td  ><input type='hidden' class='itemsVarienttotalhdnclass' value=" + subTotal + " /></td >";
        newcontent += "<td  ></td >";
        newcontent += "<td  class='itemstotalclass'>" + numberWithCommas(parseFloat(subTotal).toFixed(2)) + " </td >";
        newcontent += "<td   >";        
        newcontent += "<input type='hidden' class='hdnappendAmount' value=" + subTotal + " />";       
        newcontent += "<a  class='fa fa-edit editVarient' style='cursor:pointer;'/>";
        newcontent += "<a  class='fa fa-trash deleteVarient' style='cursor:pointer;'/>";
        newcontent += "</td >";
        newcontent += "</tr >";
        itemstotal = subTotal;
    }

    //calculate total
    newcontent += "<tr >";
    newcontent += "<td ></td >";
    newcontent += "<td ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td  ><input type='hidden' id='hdnItemTotal' class='itemsVarienttotalhdnclass' value=" + itemstotal + " /></td >";
    newcontent += "<td  >Total</td >";
    newcontent += "<td  class='itemsVarienttotalclass'>" + numberWithCommas(parseFloat(itemstotal).toFixed(2)) + " </td >";
    newcontent += "</tr >";


    $('#tbodyVarientItems').empty().append(newcontent);
    //document.getElementById("ProductsTotal").value = numberWithCommas(parseFloat(itemstotal).toFixed(2));
    document.getElementById("hdnVarientProductsTotal").value = parseFloat(itemstotal).toFixed(2);
   // document.getElementById("Total").value = parseFloat(itemstotal).toFixed(2);
   // document.getElementById("DueAmount").value = parseFloat(dueMoney).toFixed(2);
    //  $(".showimg").hide();
    ClearItemVarientAdd();
});

function yoCalculateItemsTotalVarient() {
    //var BatchId = $('#BatchId').val();
    //var BatchIdName = $("#BatchId option:selected").text();
    //var txtStoreItem = $('#txtStoreItem').val();
    //var hdnStoreItemId = $('#hdnStoreItemId').val();
    //var txtMRP = $('#txtMRP').val();
    //var txtQuantity = $('#txtQuantity').val();
    //var txtAmount = $('#txtAmount').val();
    //var hdnAmount = $('#hdnAmount').val();

    $('.dvitemexistmsg').html("");
    let newcontent = "";
    newcontent = "";
    let itemstotal = 0;
    $('#tbodyVarientItems tr').each(function () {
        var text = $(this).find("td").eq(4).html();
        if (text !== "Total") {
            let amnt = $(this).find('.hdnappendVarientAmount').val();
            let hdnappendhdnItemType = $(this).find(".hdnappendVarientItemType").val();

            itemstotal = parseFloat(itemstotal) + parseFloat(amnt);

            newcontent += "<tr >";
            newcontent += "<td>" + $(this).find("td").eq(0).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(1).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(2).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(3).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(4).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(5).html(); + " </td >";
            newcontent += "<td   >";

           
            let hdnappendMRP = $(this).find('.hdnappendVarientMRP').val();
            let hdnappendQuantity = $(this).find('.hdnappendVarientQuantity').val();
            let hdnappendProductId = $(this).find(".hdnappendVarientProductId").val();
            let hdnappenItemDesc = $(this).find(".hdnappenVarientItemDesc").val();           
      

            newcontent += "<input type='hidden' class='hdnappendVarientProductId' value=" + hdnappendProductId + " />";
            newcontent += "<input type='hidden' class='hdnappendVarientProductName' value=" + txtStoreItem + " />";
            newcontent += "<input type='hidden' class='hdnappenVarientItemDesc' value=" + hdnappenItemDesc + " />";
            newcontent += "<input type='hidden' class='hdnappendVarientAmount' value=" + amnt + " />";
            newcontent += "<input type='hidden' class='hdnappendVarientMRP' value=" + hdnappendMRP + " />";
            newcontent += "<input type='hidden' class='hdnappendVarientQuantity' value=" + hdnappendQuantity + " />";
            newcontent += "<input type='hidden' class='hdnappendhdnVarientItemType' value=" + hdnappendhdnItemType + " />";
            newcontent += "<a  class='fa fa-edit editVarient' style='cursor:pointer;'/>";
            newcontent += "<a  class='fa fa-trash deleteVarient' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }
    });

    //calculate total
    newcontent += "<tr >";
    newcontent += "<td ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td  ><input type='hidden' class='itemsVarienttotalhdnclass' value=" + itemstotal + " /></td >";
    newcontent += "<td  >Total</td >";
    newcontent += "<td  class='itemsVarienttotalclass'>" + numberWithCommas(parseFloat(itemstotal).toFixed(2)) + " </td >";
    newcontent += "</tr >";

    $('#tbodyVarientItems').empty().append(newcontent);
    //document.getElementById("ProductsTotal").value = numberWithCommas(parseFloat(itemstotal).toFixed(2));
   // document.getElementById("hdnProductsTotal").value = parseFloat(itemstotal).toFixed(2);
   // document.getElementById("Total").value = parseFloat(itemstotal).toFixed(2);
};

$("#tbodyVarientItems").on('click', '.editVarient', function (e) {
    e.preventDefault();
    var tr = $(this).closest('tr');

    var txtDescription = tr.find("td").eq(1).html();
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

$("#tbodyVarientItems").on('click', '.deleteVarient', function (e) {
    e.preventDefault();
    var tr = $(this).closest('tr');
    tr.css("background-color", "#FF3700");
    tr.remove();
    yoCalculateItemsTotalVarient();
    ClearItemVarientAdd();
});

$("#txtVarientItem").autocomplete({
    minLength: 1,
    source: function (request, response) {
        let url = $(this.element).data("url");
        let voucherItemId = $("#VoucherItemId").val();
        let varientProductId = $("#varientProductId").val();
        
        //alert(voucherItemId);
        $.getJSON(url, { term: request.term, voucherItemId: voucherItemId, varientProductId: varientProductId}, function (data) {
            response(data)
        })
    },
    appendTo: $(".modal-body ui-autocomplete-input-abc"),
    select: function (event, ui) {
        $("#hdnVarientId").val(ui.item.id);
        $("#txtVarientItem").val(ui.item.value);
        $("#txtVarientDescription").val(ui.item.label);
        $('#txtVarientDescription').focus()
     
        $("#hdnItemType").val(ui.item.Itemtype);
    },
    change: function (event, ui) {
        if (!ui.item) {
            $(event.target).val("");
        }
    }
});


$('.SaveItemVarient').on("click",
    function (e) {
        e.preventDefault();
       
        let VoucherItemId = $('#VoucherItemId').val();
        //alert(VoucherItemId);
      
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
        $('#tbodyVarientItems tr').each(function () {
            let text = $(this).find("td").eq(4).html();
            if (text !== "Total") {

                let Name = $(this).find("td").eq(1).text().trim();
                //let Quantity = parseFloat($(this).find("td").eq(3).html()).toFixed(2);;
                let ItemAmount = $(this).find(".hdnappendVarientAmount").val();
                let MRPPerUnit = $(this).find(".hdnappendVarientMRP").val();
                let Quantity = $(this).find(".hdnappendVarientQuantity").val();
                let ProductId = $(this).find(".hdnappendVarientProductId").val();            
                OrderDetl.push({
                    'Name': Name,
                    'VarientProductId': ProductId,                 
                    'MRPPerUnit': MRPPerUnit,
                    'Quantity': Quantity,
                    'ItemAmount': ItemAmount,                   
                });
            }
        });


      //   alert(JSON.stringify(OrderDetl) + '  ' + OrderDetl.length)       
        //$('#loading').html('<img src="/Images/default.gif" /> loading...');
        $(".SaveItemVarient").attr("class", "btn btn-primary SaveItemVarient disabled");
        let form = $('#addItemVarientForm');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
        let url = form.prop('action');
      
        $.ajax({
            url: url,
            type: "POST",
            data: {
                __RequestVerificationToken: token,
                VoucherItemId: VoucherItemId,
                data: JSON.stringify(OrderDetl)               
            },
            dataType: "json",
            success: function (data) {
                if (data.success === true) {
                    $('#loading').html(data);
                    $('#itemVarientModal').modal('hide');
                    showNotification(data.message, "success");                    
                }
                if (data.success === false) {
                    showNotification(data.message, "error");
                    $("#SaveClose").attr("class", "btn btn-default SaveOrder");
                    $("#SaveNext").attr("class", "btn btn-success SaveOrder");
                }
               
               
            },
            error: function (xhr, status, error) {
                $("#MessageToClient").text(xhr.responseText);
                $(".SaveOrder").attr("class", "btn btn-primary SaveOrder");
                showNotification("Something happan wrong", "failed");
            }
        });

    });