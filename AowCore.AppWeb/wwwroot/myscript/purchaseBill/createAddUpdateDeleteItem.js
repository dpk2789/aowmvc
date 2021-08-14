function numberWithCommas(n) {
    return n.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function ClearItemAdd() {
    $('#txtDescription').val('');
    $('#txtStoreItem').val('');
    $('#hdnStoreItemId').val('');
    $('#hdnProductId').val('');
    $('#txtMRP').val('');
    $('#txtQuantity').val('');
    $('#txtAmount').val('');
    $('#btnupdate').hide();
    $('#btnnew').hide();
    $('#btnadd').show();
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

function recalculateExtendedPrice() {
    var quantity = parseFloat(document.getElementById("txtQuantity").value).toFixed(0);
    var unitPrice = parseFloat(document.getElementById("txtMRP").value).toFixed(2);

    if (isNaN(quantity)) {
        quantity = 1;
    }

    if (isNaN(unitPrice)) {
        unitPrice = 0;
    }

    document.getElementById("txtQuantity").value = quantity;
    document.getElementById("txtMRP").value = unitPrice;

    document.getElementById("txtAmount").value = (quantity * unitPrice).toFixed(2);
    //document.getElementById("hdnAmount").value = (quantity * unitPrice).toFixed(2);
}

function CalulateCurrentDue() {
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


$('.btnadd').on("click", function (e) {
    let golbalPreviousAmount = 0;
    let sNo = 0;
    let newcontent = "";
    let ItemType = $('#hdnItemType').val();
    let HeadName = $("#X option:selected").text();
    let txtStoreItem = $('#txtStoreItem').val();
    let txtDescription = $('#txtDescription').val();
    let hdnStoreItemId = $('#hdnStoreItemId').val();
    let hdnProductId = $('#hdnProductId').val();
    let hdnBatchId = $("#BatchId option:selected").val();
    let hdnProductAccountId = $('#hdnProductAccountId').val();
    let txtMRP = $('#txtMRP').val();
    let txtQuantity = $('#txtQuantity').val();
    let txtAmount = $('#txtAmount').val();
    let hdnAmount = $('#hdnAmount').val();
    let hdnPreviousAmount = golbalPreviousAmount;
    let taxAmount = 0;
    let taxPercent = txtMRP;

    golbalPreviousAmount = txtAmount;
    if (isNaN(txtQuantity) || txtQuantity === "") {
        txtQuantity = 0;
    }
    var hdnAccountCategoryName = $('#hdnAccountCategoryName').val();
    //  alert(hdnAccountCategoryName)
    $('.dvitemexistmsg').html("");
    newcontent = "";
    let itemstotal = 0;
    let itemstotalWithOutTax = 0;
    let dueMoney = 0;
    let x = 0;

    $('#tbodyitems tr').each(function () {
        let text = $(this).find("td").eq(4).html();
        if (text !== "Total") {
          
            let hdnappendhdnItemType = $(this).find(".hdnappendhdnItemType").val();
            let hdnAccountCategoryName = $(this).find('.hdnappendhdnAccountCategoryName').val();
            let hdnappendProductName = $(this).find('.hdnappendProductName').val();
            let amnt = $(this).find('.hdnappendAmount').val();

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

          
          
            let hdnappendMRP = $(this).find('.hdnappendMRP').val();
            let hdnappendQuantity = $(this).find('.hdnappendQuantity').val();
            let hdnappendProductId = $(this).find(".hdnappendProductId").val();
            let hdnappendhdnStoreItemId = $(this).find(".hdnappendhdnStoreItemId").val();
            let hdnappendProductAccountId = $(this).find(".hdnappendProductAccountId").val();
            let hdnappenItemDesc = $(this).find(".hdnappenItemDesc").val();


            newcontent += "<input type='hidden' class='hdnappendhdnAccountCategoryName' value=" + hdnAccountCategoryName + " />";
            newcontent += "<input type='hidden' class='hdnappendProductId' value=" + hdnappendProductId + " />";
            newcontent += "<input type='hidden' class='hdnappendBatchId' value=" + hdnBatchId + " />";
            newcontent += "<input type='hidden' class='hdnappendProductAccountId' value=" + hdnappendProductAccountId + " />";
            newcontent += "<input type='hidden' class='hdnappendProductName' value=" + txtStoreItem + " />";
            newcontent += "<input type='hidden' class='hdnappenItemDesc' value=" + hdnappenItemDesc + " />";
            newcontent += "<input type='hidden' class='hdnappendAmount' value=" + amnt + " />";
            newcontent += "<input type='hidden' class='hdnappendMRP' value=" + hdnappendMRP + " />";
            newcontent += "<input type='hidden' class='hdnappendQuantity' value=" + hdnappendQuantity + " />";
            newcontent += "<input type='hidden' class='hdnappendhdnStoreItemId' value=" + hdnappendhdnStoreItemId + " />";
            newcontent += "<input type='hidden' class='hdnappendhdnItemType' value=" + hdnappendhdnItemType + " />";
            newcontent += "<a  class='fa fa-edit edit' style='cursor:pointer;'/>";
            newcontent += "<a  class='fa fa-trash delete' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }
    });


    if (ItemType.replace(/\s/g, '') === "Discount") {
        itemstotal = parseFloat(itemstotal) - parseFloat(txtAmount);
    }
    else {
        itemstotal = parseFloat(itemstotal) + parseFloat(txtAmount);
    }
   
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
    newcontent += "<input type='hidden' class='hdnappendhdnAccountCategoryName' value=" + hdnAccountCategoryName.replace(/\s/g, '') + " />";
    newcontent += "<input type='hidden' class='hdnappendProductId' value=" + hdnProductId + " />";
    newcontent += "<input type='hidden' class='hdnappendBatchId' value=" + hdnBatchId + " />";
    newcontent += "<input type='hidden' class='hdnappendProductAccountId' value=" + hdnProductAccountId + " />";
    newcontent += "<input type='hidden' class='hdnappendProductName' value=" + txtStoreItem + " />";
    newcontent += "<input type='hidden' class='hdnappenItemDesc' value=" + txtDescription + " />";
    newcontent += "<input type='hidden' class='hdnappendMRP' value=" + txtMRP + " />";
    newcontent += "<input type='hidden' class='hdnappendQuantity' value=" + txtQuantity + " />";
    newcontent += "<input type='hidden' class='hdnappendAmount' value=" + parseFloat(txtAmount) + " />";
    newcontent += "<input type='hidden' class='hdnappendhdnStoreItemId' value=" + hdnStoreItemId + " />";
    newcontent += "<input type='hidden' class='hdnappendhdnItemType' value=" + ItemType + " />";
    newcontent += "<a  class='fa fa-edit edit' style='cursor:pointer;'/>";
    newcontent += "<a  class='fa fa-trash delete' style='cursor:pointer;'/>";
    newcontent += "</td >";
    newcontent += "</tr >";


    if (txtStoreItem === "SubTotal") {
        var subTotal = 0;
        $('#tbodyitems tr').each(function () {
            var text = $(this).find("td").eq(3).html();
            if (text !== "Total") {
                var amnt = $(this).find('.hdnappendAmount').val();
                if (hdnAccountCategoryName === "SalesDiscount") {
                    subTotal = parseFloat(subTotal) - parseFloat(amnt);
                }
                else {
                    subTotal = parseFloat(subTotal) + parseFloat(amnt);
                }
            }
        });

        newcontent += "<tr >";
        newcontent += "<td  class='txtStoreItem'>" + txtStoreItem + " </td >";
        newcontent += "<td  ></td >";
        newcontent += "<td  ><input type='hidden' class='itemstotalhdnclass' value=" + subTotal + " /></td >";
        newcontent += "<td  ></td >";
        newcontent += "<td  class='itemstotalclass'>" + numberWithCommas(parseFloat(subTotal).toFixed(2)) + " </td >";
        newcontent += "<td   >";
        newcontent += "<input type='hidden' class='hdnappendhdnAccountCategoryName' value=" + hdnAccountCategoryName + " />";
        newcontent += "<input type='hidden' class='hdnappendProductName' value=" + txtStoreItem + " />";
        newcontent += "<input type='hidden' class='hdnappendProductId' value=" + hdnProductId + " />";
        newcontent += "<input type='hidden' class='hdnappendProductAccountId' value=" + hdnProductAccountId + " />";
        newcontent += "<input type='hidden' class='hdnappendAmount' value=" + subTotal + " />";
        newcontent += "<input type='hidden' class='hdnappendhdnStoreItemId' value=" + hdnStoreItemId + " />";
        newcontent += "<a  class='fa fa-edit edit' style='cursor:pointer;'/>";
        newcontent += "<a  class='fa fa-trash delete' style='cursor:pointer;'/>";
        newcontent += "</td >";
        newcontent += "</tr >";
        itemstotal = subTotal;
    }

    //calculate total
    newcontent += "<tr >";
    newcontent += "<td ></td >";
    newcontent += "<td ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td  ><input type='hidden' id='hdnItemTotal' class='itemstotalhdnclass' value=" + itemstotal + " /></td >";
    newcontent += "<td  >Total</td >";
    newcontent += "<td  class='itemstotalclass'>" + numberWithCommas(parseFloat(itemstotal).toFixed(2)) + " </td >";
    newcontent += "</tr >";


    $('#tbodyitems').empty().append(newcontent);
    //document.getElementById("ProductsTotal").value = numberWithCommas(parseFloat(itemstotal).toFixed(2));
    document.getElementById("hdnProductsTotal").value = parseFloat(itemstotal).toFixed(2);
    document.getElementById("Total").value = parseFloat(itemstotal).toFixed(2);

    //  $(".showimg").hide();
    ClearItemAdd();
});

$('.btnupdate').on("click", function (e) {
    var totalamnt = 0;
    var newcontent = "";
    //var BatchId = $('#BatchId').val();
    //var BatchIdName = $("#BatchId option:selected").text();
    var txtDescription = $('#txtDescription').val();
    var txtStoreItem = $('#txtStoreItem').val();
    var hdnStoreItemId = $('#hdnStoreItemId').val();
    var txtMRP = $('#txtMRP').val();
    var txtQuantity = $('#txtQuantity').val();
    var txtAmount = $('#txtAmount').val();
    var hdnAmount = $('#hdnAmount').val();

    $('.dvitemexistmsg').html("");
    var isItemEdit = false;
    newcontent = "";
    $('#tbodyitems tr').each(function () {
        var text = $(this).find("td").eq(3).html();
        if (text != "Total") {
            var itemId = $(this).find('.hdnappendhdnStoreItemId').val();
            if (hdnStoreItemId == itemId) {
                isItemEdit = true;
                var amnt = $(this).find("td").eq(4).html();
                //  var amnt = $(this).find('.hdnappentAmount').val();
                newcontent += "<tr >";
                newcontent += "<td  class='txtStoreItem'>" + txtStoreItem + " </td >";
                newcontent += "<td  class='txtDescription'>" + txtDescription + " </td >";
                //newcontent += "<td   >";
                //newcontent += "<p class='txtDescription'>" + txtDescription + "</p>";
                //newcontent += "</td >";              
                newcontent += "<td  >" + txtMRP + " </td >";
                newcontent += "<td  >" + txtQuantity + " </td >";
                newcontent += "<td  >" + txtAmount + " </td >";
                newcontent += "<td   >";
                newcontent += "<input type='hidden' class='hdnappendAmount' value=" + hdnAmount + " />";
                newcontent += "<input type='hidden' class='hdnappendhdnStoreItemId' value=" + hdnStoreItemId + " />";
                newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
                newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
                newcontent += "</td >";
                newcontent += "</tr >";
                totalamnt = parseFloat(totalamnt) + parseFloat(hdnAmount);
            }
            else {
                //  var amnt = $(this).find('.hdnappentAmount').val();
                var amnt = $(this).find("td").eq(4).html();
                var txtDescription = $(this).find("td").eq(1).html();
                var productname = $(this).find("td").eq(0).html();
                var price = parseFloat($(this).find("td").eq(2).html()).toFixed(2);
                var qty = $(this).find("td").eq(3).html();
                var amount = $(this).find("td").eq(4).html();
                newcontent += "<tr >";
                newcontent += "<td >" + productname + " </td >";
                newcontent += "<td >" + txtDescription + " </td >";
                newcontent += "<td >" + price + " </td >";
                newcontent += "<td >" + qty + "</td >";
                newcontent += "<td >" + amount + "</td >";

                var hdnappendAmount = $(this).find(".hdnappendAmount").val();
                var hdnappendhdnStoreItemId = $(this).find(".hdnappendhdnStoreItemId").val();
                // alert('product id<>item id' + StatusId)
                newcontent += "<td   >";
                newcontent += "<input type='hidden' id='hdnappendAmount' class='hdnappendAmount' value=" + hdnappendAmount + " />";
                newcontent += "<input type='hidden' id='hdnappendhdnStoreItemId' class='hdnappendhdnStoreItemId' value=" + hdnappendhdnStoreItemId + " />";
                newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
                newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
                newcontent += "</td >";
                newcontent += "</tr >";
                totalamnt = parseFloat(totalamnt) + parseFloat(hdnappendAmount);
            }
        }
    });

    if (isItemEdit == false) {
        var amnt = $(this).find("td").eq(4).html();
        var txtDescription = $(this).find("td").eq(1).html();
        var productname = $(this).find("td").eq(0).html();
        var price = parseFloat($(this).find("td").eq(2).html()).toFixed(2);
        var qty = $(this).find("td").eq(3).html();
        var amount = parseFloat($(this).find("td").eq(4).html()).toFixed(2);

        newcontent += "<tr >";
        newcontent += "<td >" + productname + " </td >";
        newcontent += "<td >" + txtDescription + " </td >";
        newcontent += "<td >" + price + " </td >";
        newcontent += "<td >" + qty + "</td >";
        newcontent += "<td >" + amount + "</td >";
        var hdnappendAmount = $(this).find(".hdnappendAmount").val();
        var hdnappendhdnStoreItemId = $(this).find(".hdnappendhdnStoreItemId").val();
        // alert('product id<>item id' + StatusId)
        newcontent += "<td   >";
        newcontent += "<input type='hidden' id='hdnappendAmount' class='hdnappendAmount' value=" + hdnappendAmount + " />";
        newcontent += "<input type='hidden' id='hdnappendhdnStoreItemId' class='hdnappendhdnStoreItemId' value=" + hdnappendhdnStoreItemId + " />";
        newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
        newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
        newcontent += "</td >";
        newcontent += "</tr >";
        totalamnt = parseFloat(totalamnt) + parseFloat(hdnappendAmount);
    }

    ///calculate total
    newcontent += "<tr >";
    newcontent += "<td ></td >";
    newcontent += "<td ></td >";
    newcontent += "<td ></td >";
    newcontent += "<td >Total</td >";
    newcontent += "<td  align='right'>" + parseFloat(totalamnt).toFixed(2) + " </td >";
    newcontent += "<td ></td >";
    newcontent += "</tr >";

    $('#tbodyitems').empty().append(newcontent);
    document.getElementById("ProductsTotal").value = numberWithCommas(parseFloat(totalamnt).toFixed(2));
    document.getElementById("hdnProductsTotal").value = parseFloat(totalamnt).toFixed(2);
    ClearItemAdd()
});

function ReCalculateItemsTotal() {
    var newcontent = "";
    var BatchId = $('#BatchId').val();
    var BatchIdName = $("#BatchId option:selected").text();
    var txtStoreItem = $('#txtStoreItem').val();
    var hdnStoreItemId = $('#hdnStoreItemId').val();
    var txtMRP = $('#txtMRP').val();
    var txtQuantity = $('#txtQuantity').val();
    var txtAmount = $('#txtAmount').val();
    var hdnAmount = $('#hdnAmount').val();

    $('.dvitemexistmsg').html("");
    newcontent = "";
    var itemstotal = 0;
    $('#tbodyitems tr').each(function () {
        var text = $(this).find("td").eq(3).html();
        if (text != "Total") {
            var amnt = $(this).find('.hdnappentAmount').val();
            itemstotal = parseFloat(itemstotal) + parseFloat(amnt);
            newcontent += "<tr >";
            newcontent += "<td>" + $(this).find("td").eq(0).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(1).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(2).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(3).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(4).html(); + " </td >";
            newcontent += "<td   >";
            newcontent += "<input type='hidden' class='hdnappentAmount' value=" + amnt + " />";
            newcontent += "<a  class='glyphicon glyphicon-edit edit' style='cursor:pointer;'/>";
            newcontent += "<a  class='glyphicon glyphicon-trash delete' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }
    });

    //calculate total
    newcontent += "<tr >";
    newcontent += "<td ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td  ><input type='hidden' class='itemstotalhdnclass' value=" + itemstotal + " /></td >";
    newcontent += "<td  >Total</td >";
    newcontent += "<td  class='itemstotalclass'  align='right'>" + numberWithCommas(parseFloat(itemstotal).toFixed(2)) + " </td >";
    newcontent += "</tr >";

    $('#tbodyitems').empty().append(newcontent);
    document.getElementById("ProductsTotal").value = numberWithCommas(parseFloat(itemstotal).toFixed(2));
    document.getElementById("hdnProductsTotal").value = parseFloat(itemstotal).toFixed(2);
};

function yoCalculateItemsTotal() {
    //var BatchId = $('#BatchId').val();
    //var BatchIdName = $("#BatchId option:selected").text();
    //var txtStoreItem = $('#txtStoreItem').val();
    //var hdnStoreItemId = $('#hdnStoreItemId').val();
    //var txtMRP = $('#txtMRP').val();
    //var txtQuantity = $('#txtQuantity').val();
    //var txtAmount = $('#txtAmount').val();
    //var hdnAmount = $('#hdnAmount').val();

    $('.dvitemexistmsg').html("");
    var newcontent = "";
    newcontent = "";
    var itemstotal = 0;
    $('#tbodyitems tr').each(function () {
        var text = $(this).find("td").eq(4).html();
        if (text !== "Total") {
            var amnt = $(this).find('.hdnappendAmount').val();

            var hdnappendhdnItemType = $(this).find(".hdnappendhdnItemType").val();
            if (hdnappendhdnItemType.replace(/\s/g, '') === "Discount") {
                itemstotal = parseFloat(itemstotal) - parseFloat(amnt);
            }
            else if (hdnappendhdnItemType.replace(/\s/g, '') === "PayMentReceived") {
                // itemstotal = parseFloat(itemstotal) - parseFloat(amnt);                
            }
            else {
                itemstotal = parseFloat(itemstotal) + parseFloat(amnt);
            }

            newcontent += "<tr >";
            newcontent += "<td>" + $(this).find("td").eq(0).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(1).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(2).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(3).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(4).html(); + " </td >";
            newcontent += "<td>" + $(this).find("td").eq(5).html(); + " </td >";
            newcontent += "<td   >";

            var amnt = $(this).find('.hdnappendAmount').val();
            var hdnAccountCategoryName = $(this).find('.hdnappendhdnAccountCategoryName').val();
            var hdnappendProductName = $(this).find('.hdnappendProductName').val();
            var hdnappendMRP = $(this).find('.hdnappendMRP').val();
            var hdnappendQuantity = $(this).find('.hdnappendQuantity').val();
            var hdnappendAmount = $(this).find(".hdnappendAmount").val();
            var hdnappendProductId = $(this).find(".hdnappendProductId").val();          
            var hdnappendProductAccountId = $(this).find(".hdnappendProductAccountId").val();
         

            newcontent += "<input type='hidden' class='hdnappendhdnAccountCategoryName' value=" + hdnAccountCategoryName + " />";
            newcontent += "<input type='hidden' class='hdnappendProductId' value=" + hdnappendProductId + " />";
            newcontent += "<input type='hidden' class='hdnappendProductAccountId' value=" + hdnappendProductAccountId + " />";
            newcontent += "<input type='hidden' class='hdnappendProductName' value=" + txtStoreItem + " />";
            newcontent += "<input type='hidden' class='hdnappendAmount' value=" + amnt + " />";
            newcontent += "<input type='hidden' class='hdnappendMRP' value=" + hdnappendMRP + " />";
            newcontent += "<input type='hidden' class='hdnappendQuantity' value=" + hdnappendQuantity + " />";
            newcontent += "<input type='hidden' class='hdnappendhdnItemType' value=" + hdnappendhdnItemType + " />";
            newcontent += "<a  class='fa fa-edit edit' style='cursor:pointer;'/>";
            newcontent += "<a  class='fa fa-trash delete' style='cursor:pointer;'/>";
            newcontent += "</td >";
            newcontent += "</tr >";
        }
    });

    //calculate total
    newcontent += "<tr >";
    newcontent += "<td ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td  ></td >";
    newcontent += "<td  ><input type='hidden' id='hdnItemTotal' class='itemstotalhdnclass' value=" + itemstotal + " /></td >";
    newcontent += "<td  >Total</td >";
    newcontent += "<td  class='itemstotalclass'>" + numberWithCommas(parseFloat(itemstotal).toFixed(2)) + " </td >";
    newcontent += "</tr >";

    $('#tbodyitems').empty().append(newcontent);
    //document.getElementById("ProductsTotal").value = numberWithCommas(parseFloat(itemstotal).toFixed(2));
    document.getElementById("hdnProductsTotal").value = parseFloat(itemstotal).toFixed(2);
    document.getElementById("Total").value = parseFloat(itemstotal).toFixed(2);
};

$("#tbodyitems").on('click', '.edit', function (e) {
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

$("#tbodyitems").on('click', '.delete', function (e) {
    e.preventDefault();
    var tr = $(this).closest('tr');
    tr.css("background-color", "#FF3700");
    tr.remove();
    yoCalculateItemsTotal();
    ClearItemAdd();
});

$("#txtStoreItem").autocomplete({
    minLength: 1,
    source: function (request, response) {
        var url = $(this.element).data("url");
        $.getJSON(url, { term: request.term }, function (data) {
            response(data)
        })
    },
    appendTo: $(".modal-body ui-autocomplete-input"),
    select: function (event, ui) {
        $("#hdnProductId").val(ui.item.productId);
        $("#hdnStoreItemId").val(ui.item.StoreProductId);
        $("#txtStoreItem").val(ui.item.name);
        $("#txtDescription").val(ui.item.autoGenerateName);
        $('#txtDescription').focus()
        if (ui.item.Itemtype === "Taxation") {
            $("#txtMRP").val(ui.item.percent);
        } else {
            $("#txtMRP").val(ui.item.mRPPerUnit);
        }

        $("#hdnAccountCategoryName").val(ui.item.accountCategoryName);
        $("#hdnProductAccountId").val(ui.item.productAccountId);
        //if (ui.item.ProductAccountId === null) {
        //    alert("Please Set Item Accounts first")
        //    return false;
        //}
        $("#hdnItemType").val(ui.item.itemtype);
    },
    change: function (event, ui) {
        if (!ui.item) {
            $(event.target).val("");
        }
    }
});