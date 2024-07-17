$(document).ready(function () {
    $('#reg-btn').click(function () {
        $(this).text('Back');
        $(this).attr('id', 'backBtn');
        $(this).click(function () {
            window.location.href = 'loginPage.aspx';
        });
    });
});