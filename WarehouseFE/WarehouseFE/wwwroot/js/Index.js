$(document).ready(function () {
    
    if (typeof $.cookie('token') === "undefined"
        || $.cookie('token') === null
        || $.cookie('token').length === 0)
    {        
        location.href = '/Login';
    }   
    loadInitialData();
    configureSearchInput();
    configureClearSearch();
    configureLogOff();
});

function loadInitialData() {    
    jQuery.ajax({
        url: 'http://localhost:57882/api/Item/',
        type: 'GET',        
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'BEARER ' + $.cookie('token'));
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.cookie('token', "");
            $.cookie('userName', "");
            location.href = '/Login';
           
        },
        success: function (response) {
            if (response.length === 0)
                return;

            $("#myGrid tbody").html("");

            var newTbody = "";

            $.each(response, function () {
                newTbody += "<tr><th scope = 'row' >" + this.sku + "</th>";
                newTbody += "<td>" + this.description + "</td>";
                newTbody += "<td>" + this.stockQuantity + "</td></tr>";                
            });

            $("#myGrid tbody").html(newTbody);
        }
    });
}

function loadSearchData(text) {
    jQuery.ajax({
        url: 'http://localhost:57882/api/Item/Search/'+text,
        type: 'GET',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'BEARER ' + $.cookie('token'));
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.cookie('token', "");
            $.cookie('userName', "");
            location.href = '/Login';            
        },
        success: function (response) {
            $("#myGrid tbody").html("");

            if (response.length === 0)
                return;

            var newTbody = "";

            $.each(response, function () {
                newTbody += "<tr><th scope = 'row' >" + this.sku + "</th>";
                newTbody += "<td>" + this.description + "</td>";
                newTbody += "<td>" + this.stockQuantity + "</td></tr>";
            });

            $("#myGrid tbody").html(newTbody);
        }
    });
}

function configureSearchInput() {
    $("#skuText").keyup(function () {
        if ($(this).val().length === 0) {
            loadInitialData();
        }
        else {
            loadSearchData($(this).val());
        }        
    })
}

function configureClearSearch() {
    $("#clearSearch").click(function () {
        $("#skuText").val("");
        loadInitialData();
    });
}

function configureLogOff() {
    $("#divLogOff").show();

    $("#loggedUser").text($.cookie('userName'));

    $("#alogoff").click(function (e) {
        e.preventDefault();
        $.cookie('token', "");
        $.cookie('userName', "");
        location.href = '/Login';
    });
}