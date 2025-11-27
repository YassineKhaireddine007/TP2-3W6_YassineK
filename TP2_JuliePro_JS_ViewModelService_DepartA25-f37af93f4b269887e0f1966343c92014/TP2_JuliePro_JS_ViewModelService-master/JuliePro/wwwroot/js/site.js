// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    
    $('.trainer-card').on('mouseenter', function () {
        $(this).find('.trainer-details').addClass('show');
    });

    
    $('.trainer-card').on('mouseleave', function () {
        $(this).find('.trainer-details').removeClass('show');
    });

});


$(document).ready(function () {
    $('.pagination .page-link').on('click', function () {
        let pageId = $(this).data('page-id');
        $('#pageIndexInput').val(pageId);
        $('#filterForm').submit();
    });
});