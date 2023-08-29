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
    $(document).on("keyup", "#usernameInput", function () {
        var search = $("#usernameInput").val().trim();
        $.ajax({
            url: '/Account/GetUser?userName=' + search,  // Replace with your server endpoint URL
            type: 'GET',
            success: function (data) {
                modalSearchResults.innerHTML = '';

                data.forEach(result => {
                    const userDiv = document.createElement('div');
                    userDiv.innerHTML = `
                                                                            <p>${result.userName}</p>
                                                                            <button class="btn btn-success select-button" data-userid="${result.email}">Send</button>
                                                                        `;
                    modalSearchResults.appendChild(userDiv);
                });

                // Attach click event to Select buttons
                const selectButtons = document.querySelectorAll('.select-button');
                selectButtons.forEach(button => {
                    button.addEventListener('click', () => {
                        const userId = button.getAttribute('data-userid');
                        // Call a function to handle the user selection
                        handleUserSelection(userId);
                    });
                });
            },
            error: function () {
                modalSearchResults.innerHTML = 'Error occurred while searching.';
            }
        });
    })

    function searchForUserInModal(username) {
        const modalSearchResults = document.getElementById('modalSearchResults');

        // Clear previous results
        modalSearchResults.innerHTML = 'Searching...';

        // Perform AJAX request to fetch user data

    }

    function handleUserSelection(userId) {
        // Replace this with your actual logic to handle the selected user
        console.log(`User selected with ID: ${userId}`);
    }

    $(document).ready(function () {
        function updateBasketCount() {
            var basketCountElement = $("#basketCount");

            $.ajax({
                url: '/basket/GetBasketCount', // Use relative URL
                type: 'GET',
                success: function (data) {
                    basketCountElement.text(data);
                },
                error: function () {
                    console.log('Error retrieving basket count.');
                }
            });
        }

        function updateProductCount(itemId) {
            var productCount = $("#productCount");
            $.ajax({
                url: `/basket/GetProductCount/${itemId}`,
                type: 'GET',
                success: function (data) {
                    productCount.text(data);
                    if (data == 0) {
                        removeItemFromBasket(itemId);
                    }
                },
                error: function () {
                    console.log('Error retrieving product count.');
                }
            });
        }

        function updateTotalPrice() {
            var totalPriceArea = $(".totalPriceArea");

            $.ajax({
                url: '/basket/GetTotalPrice',
                type: 'GET',
                success: function (data) {
                    totalPriceArea.text(data);
                },
                error: function () {
                    console.log('Error retrieving total price.');
                }
            });
        }

        function removeItemFromBasket(itemId) {
            $.ajax({
                url: `/basket/RemoveItem/${itemId}`,
                type: 'POST',
                success: function () {
                    location.reload();
                },
                error: function () {
                    console.log('Error removing item from basket.');
                }
            });
        }

        function updateBasketInteractions(itemId) {
            updateBasketCount();
            updateTotalPrice();
        }

        $(".plusIcon").click(function () {
            var itemId = $(this).data("id");

            $.ajax({
                url: `/basket/DecreaseBasket/${itemId}`,
                method: "POST",
                success: function () {
                    console.log(itemId)
                    updateBasketInteractions();
                    updateProductCount(itemId);
                },
                error: function (xhr, status, error) {
                    console.error("Error decreasing item: " + error);
                }
            });
        });

        $(".minusIcon").click(function () {
            var itemId = $(this).data("id");

            $.ajax({
                url: `/basket/AddBasket/${itemId}`,
                method: "POST",
                success: function () {
                    console.log(itemId)
                    updateBasketInteractions();
                    updateProductCount(itemId);
                },
                error: function (xhr, status, error) {
                    console.error("Error adding item: " + error);
                }
            });
        });
        
        $("#removeAllButton").click(function () {
            
            $.ajax({
                url: `/basket/RemoveAllItems`,
                method: "POST",
                success: function () {
                    updateBasketInteractions();
                    location.reload()
                },
                error: function (xhr, status, error) {
                    console.error("Error adding item: " + error);
                }
            });
        });
        $(".fa-trash").click(function () {
            var itemId = $(this).data("id");
            removeItemFromBasket(itemId);
        });

        updateBasketInteractions();
    });


    //////COUNTDOWN/////////////
    function Countdown(saleName, number) {
        $.ajax({
            url: "/shop/FinishDateOfSale?name=" + saleName, // Remove the query string here
            method: "GET",
            data: { name: saleName },
            success: function (response) {
                var startDate = new Date(response.startDate).getTime();
                var finishDate = new Date(response.finishDate).getTime();
                var now = new Date().getTime();
                console.log(startDate)
                console.log(finishDate)
                console.log(now)
                if (startDate <= now && finishDate >= now) {
                    console.log("Salam")
                    Counter(finishDate, number); // Start countdown for the first sale in the first container
                }
                else {
                    console.log("HIIII")
                }
            },
            error: function () {
                console.error("Failed to fetch finish date");
            }
        });

    }


    function Counter(countDownDate, containerIndex) {
        var myInterval;
        function updateCountdown() {
            
            var now = new Date().getTime();
            var distance = countDownDate - now;

            if (countDownDate < now) {
                clearInterval(myInterval);

            } else {
                if(containerIndex == 0) $("#bargainSection").removeClass("d-none")
                var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                if (minutes < 10) {
                    minutes = "0" + minutes;
                }
                if (hours < 10) {
                    hours = "0" + hours;
                }
                if (seconds < 10) {
                    seconds = "0" + seconds;
                } 

                document.getElementsByClassName("hourArea")[containerIndex].innerHTML = hours;
                document.getElementsByClassName("minuteArea")[containerIndex].innerHTML = minutes;
                document.getElementsByClassName("secondArea")[containerIndex].innerHTML = seconds;
                if (hours == minutes && minutes == seconds && minutes == "00") { 
                    location.reload();
                    console.log("hi")
                }
            }
        }

        updateCountdown();
        myInterval = setInterval(updateCountdown, 1000);
    }

    Countdown("Daiyly", 1);
    Countdown("NightBargain", 0);


    ///////////////////////////

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