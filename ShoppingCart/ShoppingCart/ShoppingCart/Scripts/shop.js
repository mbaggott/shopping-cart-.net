
$(document).ready(function () {

});

$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

/* Ajax post to increase the display quantity of a product with requested product id
 * in the cart, on the shop page */
function addCart(id) {

    $.ajax({
        type: 'POST',
        url: 'Cart/AddToCart',
        data: { id: id },
        success: function (prodQTY) {
            var inputID = "#inputQTY_" + id;
            $(inputID).val(prodQTY).change();
            updateCartQTY(id);
        }
    });

}

/* Ajax post to decrease the display quantity of a product with requested product id
 * in the cart, on the shop page */
function removeCart(id) {

    $.ajax({
        type: 'POST',
        url: 'Cart/RemoveFromCart',
        data: { id: id },
        success: function (prodQTY) {
            var inputID = "#inputQTY_" + id;
            $(inputID).val(prodQTY).change();
            updateCartQTY(id);
        }
    });

}

$('[data-onload]').each(function () {
    eval($(this).data('onload'));
});

/* Ajax post to update the display quantity of a product with requested product id
 * in the cart, on the shop page, when the page is refreshed */
function updateQTY(qtyID) {

    $.ajax({
        type: 'POST',
        url: 'Shop/UpdateQTY',
        data: { qtyID: qtyID },
        success: function (qty) {
            var inputID = "#inputQTY_" + qtyID;
            $(inputID).val(qty).change();
        }
    });
}

/* Ajax post to update Cart when Quantity is changed. Updates Quantity, Sub Total, and Total Cost. */
function updateCartQTY(qtyID) {

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: 'Cart/UpdateQTY',
        data: { qtyID: qtyID },
        success: function (response) {
            // Retrieve JSON Data.
            var prodID = response.prodid;
            var quantity = response.quantity;
            var subTotal = response.subtotal;
            var totalCost = response.totalcost;

            // Convert Strings to Float to 2 decimal places.
            subTotal = parseFloat(subTotal).toFixed(2);
            totalCost = parseFloat(totalCost).toFixed(2);

            var inputID = "#inputQTY_" + prodID;
            var subTotalID = "#subTotal_" + prodID;

            // Update values on Page.
            $(inputID).val(quantity).change();
            $(subTotalID).html("$" + subTotal);
            $('#totalCost').html("$" + totalCost);
        }
    });
}
