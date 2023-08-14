/*  ---------------------------------------------------
    Template Name: Male Fashion
    Description: Male Fashion - ecommerce teplate
    Author: Colorib
    Author URI: https://www.colorib.com/
    Version: 1.0
    Created: Colorib
---------------------------------------------------------  */

'use strict';

(function ($) {
    $(document).ready(function () {
        // Make an AJAX request to get the updated basket count
        function updateBasketCount() {
            var basketCountElement = $("#basketCount"); // Replace with the actual ID of your basket count element

            $.ajax({
                url: 'https://localhost:44392/basket/GetBasketCount', // Replace with the actual URL to your endpoint
                type: 'GET',
                success: function (data) {
                    basketCountElement.text(data);
                },
                error: function () {
                    console.log('Error retrieving basket count.');
                }
            });
        }
        function updateProductCount() {
            var productCount = $("#productCount"); // Replace with the actual ID of your basket count element
            var itemId = $("#minusIcon").data("id");
            $.ajax({
                
                url: `https://localhost:44392/basket/GetProductCount/${itemId}`, // Replace with the actual URL to your endpoint
                type: 'GET',
                success: function (data) {
                    productCount.text(data);
                    
                    if (productCount.text() == '0') {
                        $.ajax({

                            url: `https://localhost:44392/basket/RemoveItem/${itemId}`, // Replace with the actual URL to your endpoint
                            type: 'GET',
                            success: function () {
                                location.reload();
                            },
                            error: function () {
                                console.log('Error retrieving basket count.');
                            }
                        });
                    }
                },
                error: function () {
                    console.log('Error retrieving basket count.');
                }
            });
            
        }
        function updateTotalPrice() {
            var productCount = $(".totalPriceArea"); // Replace with the actual ID of your basket count element

            $.ajax({
                url: `https://localhost:44392/basket/GetTotalPrice`, // Replace with the actual URL to your endpoint
                type: 'GET',
                success: function (data) {
                    productCount.text(`${data}`);
                    console.log(data)
                },
                error: function () {
                    console.log('Error retrieving basket count.');
                }
            });
        }

        updateBasketCount();
        updateProductCount();
        updateTotalPrice();
        

        ////////////////////////////////////////////////////
        $("#minusIcon").click(function () {
            var itemId = $(this).data("id");
            $.ajax({
                url: `https://localhost:44392/basket/AddBasket/${itemId}`, // Replace with your actual route
                method: "POST",
                success: function (response) {
                    updateBasketCount();
                    updateProductCount();
                    updateTotalPrice();
                },
                error: function (xhr, status, error) {
                    console.error("Error adding item: " + error);
                }
                
            });
            
        });

        $("#plusIcon").click(function () {
            var itemId = $(this).data("id");
            
            $.ajax({
                url: `https://localhost:44392/basket/DecreaseBasket/${itemId}`,
                method: "POST",
                success: function (response) {
                    updateBasketCount();
                    updateProductCount();
                    updateTotalPrice();
                },
                error: function (xhr, status, error) {
                    console.error("Error deleting item: " + error);
                }
            });
            
        });
    });



    $(document).ready(function () {
        var firstName = $('#firstName').val();
        if (firstName != null) {
            var intials = $('#firstName').val().charAt(0);
            var profileImage = $('#profileImage').text(intials);
        }
        
    });
    $(".thumbProductPhoto").click(function() {
                var newImageSrc = $(this).data("src"); // Get the new image source from the data-src attribute
        $("#mainImage").attr("src", "/assets/img/product/"+ newImageSrc); // Change the source of the main image
                $("#mainImageLink").attr("href", newImageSrc); // Change the href of the main image link
            });
    //$(document).on("keyup", "#input-search", function () {
    //    $("#searchList ul").remove();
    //    var search = $("#input-search").val().trim();
    //    $.ajax({
    //        method: "get",
    //        url: "/home/search?search=" + search,
    //        success: function (res) {
    //            $("#searchList").append(res);
    //        }
    //    })
    //})



    /*------------------
        Preloader
    --------------------*/
    $(window).on('load', function () {
        $(".loader").fadeOut();
        $("#preloder").delay(200).fadeOut("slow");

        /*------------------
            Gallery filter
        --------------------*/
        $('.filter__controls li').on('click', function () {
            $('.filter__controls li').removeClass('active');
            $(this).addClass('active');
        });
        if ($('.product__filter').length > 0) {
            var containerEl = document.querySelector('.product__filter');
            var mixer = mixitup(containerEl);
        }
    });

    /*------------------
        Background Set
    --------------------*/
    $('.set-bg').each(function () {
        var bg = $(this).data('setbg');
        $(this).css('background-image', 'url(' + bg + ')');
    });

    //Search Switch
    $('.search-switch').on('click', function () {
        $('.search-model').fadeIn(400);
    });

    $('.search-close-switch').on('click', function () {
        $('.search-model').fadeOut(400, function () {
            $('#search-input').val('');
        });
    });

    /*------------------
		Navigation
	--------------------*/
    $(".mobile-menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    /*------------------
        Accordin Active
    --------------------*/
    $('.collapse').on('shown.bs.collapse', function () {
        $(this).prev().addClass('active');
    });

    $('.collapse').on('hidden.bs.collapse', function () {
        $(this).prev().removeClass('active');
    });

    //Canvas Menu
    $(".canvas__open").on('click', function () {
        $(".offcanvas-menu-wrapper").addClass("active");
        $(".offcanvas-menu-overlay").addClass("active");
    });

    $(".offcanvas-menu-overlay").on('click', function () {
        $(".offcanvas-menu-wrapper").removeClass("active");
        $(".offcanvas-menu-overlay").removeClass("active");
    });

    /*-----------------------
        Hero Slider
    ------------------------*/
    $(".hero__slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: false,
        nav: true,
        navText: ["<span class='arrow_left'><span/>", "<span class='arrow_right'><span/>"],
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: false
    });

    /*--------------------------
        Select
    ----------------------------*/
    $("select").niceSelect();

    /*-------------------
		Radio Btn
	--------------------- */
    $(".product__color__select label, .shop__sidebar__size label, .product__details__option__size label").on('click', function () {
        $(".product__color__select label, .shop__sidebar__size label, .product__details__option__size label").removeClass('active');
        $(this).addClass('active');
    });

    /*-------------------
		Scroll
	--------------------- */
    $(".nice-scroll").niceScroll({
        cursorcolor: "#0d0d0d",
        cursorwidth: "5px",
        background: "#e5e5e5",
        cursorborder: "",
        autohidemode: true,
        horizrailenabled: false
    });

    /*------------------
        CountDown
    --------------------*/
    // For demo preview start
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    if(mm == 12) {
        mm = '01';
        yyyy = yyyy + 1;
    } else {
        mm = parseInt(mm) + 1;
        mm = String(mm).padStart(2, '0');
    }
    var timerdate = mm + '/' + dd + '/' + yyyy;
    // For demo preview end


    // Uncomment below and use your date //

    /* var timerdate = "2020/12/30" */

    $("#countdown").countdown(timerdate, function (event) {
        $(this).html(event.strftime("<div class='cd-item'><span>%D</span> <p>Days</p> </div>" + "<div class='cd-item'><span>%H</span> <p>Hours</p> </div>" + "<div class='cd-item'><span>%M</span> <p>Minutes</p> </div>" + "<div class='cd-item'><span>%S</span> <p>Seconds</p> </div>"));
    });

    /*------------------
		Magnific
	--------------------*/
    $('.video-popup').magnificPopup({
        type: 'iframe'
    });

    /*-------------------
		Quantity change
	--------------------- */
    var proQty = $('.pro-qty');
    proQty.prepend('<span class="fa fa-angle-up dec qtybtn"></span>');
    proQty.append('<span class="fa fa-angle-down inc qtybtn"></span>');
    proQty.on('click', '.qtybtn', function () {
        var $button = $(this);
        var oldValue = $button.parent().find('input').val();
        if ($button.hasClass('inc')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        $button.parent().find('input').val(newVal);
    });

    var proQty = $('.pro-qty-2');
    proQty.prepend('<span class="fa fa-angle-left dec qtybtn"></span>');
    proQty.append('<span class="fa fa-angle-right inc qtybtn"></span>');
    proQty.on('click', '.qtybtn', function () {
        var $button = $(this);
        var oldValue = $button.parent().find('input').val();
        if ($button.hasClass('inc')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        $button.parent().find('input').val(newVal);
    });

    /*------------------
        Achieve Counter
    --------------------*/
    $('.cn_num').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });

})(jQuery);