
$(window).load(function () {

});

$(document).ready(function () {

    $('.menu-dropdown').hover(
        function () {
            $(this).addClass('open');
        },
        function () {
            $(this).removeClass('open');
        }
    );
});
