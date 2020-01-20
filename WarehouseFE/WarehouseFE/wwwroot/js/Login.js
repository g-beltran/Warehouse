$(document).ready(function () {

    $("#loginForm").submit(function (e) {
        e.preventDefault();

        var userName = $('#userName').val();
        var password = $('#password').val();

        $.post("http://localhost:57882/api/Login/Auth", {
            userName: userName, password: password
        }).done(function (data) {            

            $.cookie('token', data.token);
            $.cookie('userName', data.userName);

            if (data.token.length > 0) {
                location.href = '/Index'
            }
            else {
                alert("Invalid User/Password");
            }
        }).fail(function (xhr, status, error) {
            alert("Please verify if the API is running");            
        });

    });

    $("#divLogOff").hide();
    $("#loggedUser").text("");

});
