// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



/* Блок на главной */
var indexNewsLine = 1;

$('#news-line .preview-arrows--prev').click(function () {
    console.log('click prev');
    indexNewsLine--;
    if (indexNewsLine < 0) {
        indexNewsLine = 0;
    }
    $('#news-line .preview-inner').animate({ 'left': -indexNewsLine * 850 }, 500);
});

$('#news-line .preview-arrows--next').click(function () {
    console.log('click next');
    indexNewsLine++;
    if (indexNewsLine > 2) {
        indexNewsLine = 2;
    }
    
    $('#news-line .preview-inner').animate({ 'left': -indexNewsLine * 850 }, 500);
});

//.hover(курсор на элементе, курсор покидает элемент);
$('#news-line .preview-arrows--prev, #news-line .preview-arrows--next').hover(
    //курсор навели на элемент
    function () {
        $(this).animate({opacity: 0.5}, 300);
    },
    //курсор покидает элемент
    function () {
        $(this).animate({ opacity: 1 }, 300);
    }
);
/*  Блок на главной  */


/* Слайдер на главной */
var indexNewsSlider1 = 0;

$('#news-slider--1 .preview-arrow--prev').click(function () {
    console.log('click prev');
    indexNewsSlider1--;
    if (indexNewsSlider1 < 0) {
        indexNewsSlider1 = 0;
    }

    $('#news-slider--1 .carousel-inner').animate({ 'left': -indexNewsSlider1 * 850 }, 500);
});

$('#news-slider--1 .preview-arrow--next').click(function () {
    console.log('click next');
    indexNewsSlider1++;
    if (indexNewsSlider1 > 2) {
        indexNewsSlider1 = 2;
    }

    $('#news-slider--1 .carousel-inner').animate({ 'left': -indexNewsSlider1 * 850 }, 500);
});

/*  Слайдер на главной  */



/* Слайдер недвижимость */
var indexNewsSlider2 = 0;

$('#news-slider--2 .preview-arrow--prev').click(function () {
    console.log('click prev');
    indexNewsSlider2--;
    if (indexNewsSlider2 < 0) {
        indexNewsSlider2 = 0;
    }

    $('#news-slider--2 .preview-inner').animate({ 'left': -indexNewsSlider2 * 550 }, 500);
});

$('#news-slider--2 .preview-arrow--next').click(function () {
    console.log('click next');
    indexNewsSlider2++;
    if (indexNewsSlider2 > 5) {
        indexNewsSlider2 = 5;
    }

    $('#news-slider--2 .preview-inner').animate({ 'left': -indexNewsSlider2 * 550 }, 500);
});
/*  Слайдер недвижимость  */



/* Слайдер спорт */

/*  Слайдер спорт  */