/*  ---------------------------------------------------
    Template Name: Male Fashion
    Description: Male Fashion - ecommerce teplate
    Author: Colorib
    Author URI: https://www.colorib.com/
    Version: 1.0
    Created: Colorib
---------------------------------------------------------  */

'use strict';

function onSignIn(googleUser) {
    // Get user information from googleUser object
    var profile = googleUser.getBasicProfile();
    var id_token = googleUser.getAuthResponse().id_token;

    // Send the ID token to your server for verification and user registration.
    // Implement server-side code to validate the ID token.
    // You should also handle any error scenarios here.
}


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

    // Add a click event listener to the "Share" buttons
    var sharedProductId; // Variable to store the shared product ID

    // Add a click event listener to the "Share" buttons
    $(document).on("click", ".share-button", function () {
        sharedProductId = $(this).data("productid"); // Capture and store the product ID
    });

    function handleUserSelection(userId) {
        if (sharedProductId !== undefined) {
            console.log(`Product ID selected for sharing: ${sharedProductId}`);
            console.log(`User ID selected for sending: ${userId}`);

            // Construct your message with both userId and sharedProductId
            var message = `https://localhost:44392/shop/product/${sharedProductId}`;

            // Redirect with the constructed message
            window.location.href = `/OwnProduct/SendNewProductEmail?email=${userId}&message=${message}`;
        } else {
            console.log("Product ID is not defined.");
        }
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
        function updateWishlistCount() {
            var basketCountElement = $(".wishlistCount");

            $.ajax({
                url: '/wishlist/GetWishlistCount', // Use relative URL
                type: 'GET',
                success: function (data) {
                    basketCountElement.text(data);
                },
                error: function () {
                    console.log('Error retrieving basket count.');
                }
            });
        }
        updateWishlistCount();
        const wishlistButton = $('.wishlistbutton');
        wishlistButton.click(function () {
           
            updateWishlistCount();
        });

           
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
                    CheckoutTotal(); // Calculate the total price after receiving the subtotal
                },
                error: function () {
                    console.log('Error retrieving total price.');
                }
            });
        }

        function CheckoutPromo() {
            var discount = $(".discountArea");

            $.ajax({
                url: '/basket/GetDiscount',
                type: 'GET',
                success: function (data) {
                    discount.text(data);
                    CheckoutTotal();
                },
                error: function () {
                    console.log('Error retrieving discount.');
                }
            });
        }

        function CheckoutTotal() {
            var subtotal = parseInt($(".totalPriceArea").text());
            var discount = parseInt($(".discountArea").text());
            console.log("Subtotal:", subtotal);
            console.log("Discount:", discount);

            var total = subtotal + discount;
            console.log("total:", total);
            $("#total").text(total.toFixed(2)); // Display total with 2 decimal places
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
            updateWishlistCount();
        }

        updateBasketInteractions();



        $("#placeOrder").click(function (e) {
            e.preventDefault();


            // Get all of the required input fields
            var requiredInputs = $(':required');

            // Check if any of the required input fields are empty
            var hasEmptyInput = false;
            requiredInputs.each(function () {
                if ($(this).val() === '') {
                    hasEmptyInput = true;

                    // Add a red border to the empty input field
                    $(this).addClass('border-danger');
                }
            });

            // If there is an empty input field, prevent the form from submitting
            if (hasEmptyInput) {
                alert('Please fill in all required fields.');
                return false;
            }

            // Otherwise, submit the form
            else {
                $('#checkout__form').submit();
            }

            var totalText = $("#total").text();
            var total = parseInt(totalText);
            window.location.href = "/basket/checkout?price=" + total;
            if (!isNaN(total)) {
                console.log(total);
            } else {
                console.log("Total is not a valid integer.");
            }
        });

        
        $("#promoAddButton").click(function () {
            var promo = $("#promoInput").val()
            console.log(promo)
            $.ajax({
                url: '/basket/GetPromo?promo=' + promo, // Use relative URL
                type: 'GET',
                success: function (data) {
                    if (data == true) {
                        var subtotal = $("#subtotalArea").text();
                        var discount = subtotal * 30 / 100;
                        $("#discountArea").text("-" + discount + ".00");
                        CheckoutTotal();
                    }
                    else {
                        alert("Please enter correct promo code")
                    }
                },
                error: function () {
                    console.log('Error retrieving basket count.');
                }
            });
        });
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
                
                if (startDate <= now && finishDate >= now) {
                    
                    Counter(finishDate, number); // Start countdown for the first sale in the first container
                }
                else {
                   
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

   
    const productId = $("#productId").text(); // Get the product ID dynamically

   



        





    ///////////////////////////

    $(document).ready(function () {
        var firstName = $('.firstName').val();
        if (firstName != null) {
            var intials = $('.firstName').val().charAt(0);
            var profileImage = $('.profileImage').text(intials);
        }
        
    });
    $(".thumbProductPhoto").click(function() {
                var newImageSrc = $(this).data("src"); // Get the new image source from the data-src attribute
        $("#mainImage").attr("src", "/assets/img/product/"+ newImageSrc); // Change the source of the main image
                $("#mainImageLink").attr("href", newImageSrc); // Change the href of the main image link
    });
    $(document).on("keyup", "#search-input", function () {
        $("#searchList ul").remove();
        var search = $("#search-input").val().trim();
        $.ajax({
            method: "get",
            url: "/home/search?search=" + search,
            success: function (res) {
                $("#searchList").append(res);
            }
        })
    })
    

    ///Price Sort Product
   
    $('#sortOptions').change(function () {
        var sortOption = $(this).val();
        var categoryId = $('.selectedCategoryId').text();
        var minPrice = $('.selectedMinPrice').text();
        var maxPrice = $('.selectedMaxPrice').text();
        console.log("Sort Option: " + sortOption);
        console.log("Category ID: " + categoryId);
        console.log("min: " + minPrice);
        console.log("max: " + maxPrice);

        $.ajax({
            url: "/shop/FilterProducts",
            type: 'GET',
            data: { str: sortOption, categoryId: categoryId, min: minPrice, max: maxPrice },
            success: function (response) {
                $('#product-list-partial').html(response);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });

    $(".category-link").click(function (e) {
        e.preventDefault();
        var categoryId = $(this).data("category-id");
        $('.selectedCategoryId').text(categoryId);
        console.log("Category ID: " + categoryId);
        var sortOption = $('#sortOptions').val();

        var minPrice = $('.selectedMinPrice').text();
        var maxPrice = $('.selectedMaxPrice').text();

        $.ajax({
            url: "/shop/FilterProducts",
            type: "GET",
            data: { str: sortOption, categoryId: categoryId, min: minPrice, max: maxPrice },
            success: function (data) {
                $("#product-list-partial").html(data)
            },
            error: function () {
                alert("An error occurred while fetching data.");
            }
        });
    });


    function sideBarPrice() {
        $.ajax({
            url: "/shop/ShopOrderPrice?str=htl",
            type: 'GET',
            success: function (data) {
                var price = data[0].price;
                var increment = price/5;
                var start = 0;
                var end = increment;
                var listItems = [];

                while (end <= price) {
                    listItems.push("$" + start + " - " + "$" + end);
                    start += increment;
                    end += increment;
                }

                if (start < price) {
                    listItems.push("$" + start + " - " + "$" + price + "+");
                }

                var sidebar = $("#sidebarPrice");

                $.each(listItems, function (index, listItem) {
                    var li = $('<li style="cursor:pointer;" class="minmaxPrice">').text(listItem);
                    sidebar.append(li);
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown);
            }
        });
    }

    sideBarPrice()


    // Attach the click event handler to a parent element
    $("#sidebarPrice").on("click", ".minmaxPrice", function (e) {
        e.preventDefault();
        var categoryId = $(this).text();
        console.log(categoryId);
        var selectedCategoryId = $('.selectedCategoryId').text();

        // Split the categoryId into minimum and maximum parts
        var parts = categoryId.split(" - ");

        if (parts.length === 2) {
            // Remove the "$" sign and parse to integers
            var min = parseInt(parts[0].replace("$", ""), 10);
            var max = parseInt(parts[1].replace("$", ""), 10);

            // Now you have min and max as integers
            console.log("Minimum: " + min);
            console.log("Maximum: " + max);
            $(".selectedMinPrice").text(min);
            $(".selectedMaxPrice").text(max);
            // Perform your AJAX request or other operations here
            $.ajax({
                url: "/shop/FilterProducts?categoryId=" + selectedCategoryId + "&min=" + min + "&max=" + max,
                type: "get",
                success: function (data) {
                    $("#product-list-partial").html(data);
                },
                error: function () {
                    alert("An error occurred while fetching data.");
                }
            });
        } else {
            console.log("Invalid categoryId format");
        }
    });

    //$("#sortOptions").change(function (e) {
    //    e.preventDefault();
    //    var str = $(this).val(); // Use .val() to get the selected value
    //    console.log(str); // Log the selected value for debugging
    //    $.ajax({
    //        url: "/shop/OrderProductForPrice?str=" + str,
    //        type: "get",
    //        success: function (data) {
    //            $("#product-list-partial").html(data);
    //        },
    //        error: function () {
    //            alert("An error occurred while fetching data.");
    //        }
    //    });
    //});










    // Function to handle the click event


        var isRed = false;

        $('#addToCartButton').click(function () {
            if (isRed) {
                $(this).css('color', 'black').css('color', 'white');
            } else {
                $(this).css('color', 'red').css('color', 'black');
            }
            isRed = !isRed;
        });

    $(".order-button").on("click", function () {
        var button = $(this);
        var productId = button.data("id");
        var storageKey = "orderClicked_" + productId;

        // Check if the button has already been clicked using local storage
        if (localStorage.getItem(storageKey) === "true") {
            alert("This product has already been ordered.");
            return;
        }

        // Disable the button
        button.prop("disabled", true);

        // Store a flag in local storage to indicate that the button has been clicked
        localStorage.setItem(storageKey, "true");

        // Perform any other actions you need here, e.g., making an AJAX request to handle the order.
    });

    // On page load, disable buttons that were previously clicked
    $(document).ready(function () {
        $(".order-button").each(function () {
            var button = $(this);
            var productId = button.data("id");
            var storageKey = "orderClicked_" + productId;

            if (localStorage.getItem(storageKey) === "true") {
                button.prop("disabled", true);
            }
        });
    });





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







    $('.swalBtn').submit(function (e) {
                e.preventDefault(); // Prevent the form from submitting normally

                // Perform any validation here if needed

                // Assuming validation passes, show success alert
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Your message has been sent successfully!',
                });

                // Clear the form fields if needed
                $('form')[0].reset();
    });


    $(document).ready(function () {
        var totalPrice = 0;
        var totalPriceElement = $(".totalPrice");
        var allButtons = $(".productButton");
        var area = $(".area");
        var selectedList = $("#selectedList");
        var selectedCategory = null;

        function clearSelected() {
            selectedCategory = null;
            area.empty();
            allButtons.removeClass("selected");
        }
        $("form").submit(function (event) {
            var inputs = $(".form-control");
            var hasEmptyField = false;

            inputs.each(function () {
                if ($(this).val() === "") {
                    hasEmptyField = true;
                    return false; // Stop iterating through the inputs
                }
            });

            if (hasEmptyField) {
                event.preventDefault(); // Prevent form submission
                alert("Please fill in all fields.");
            }
        });

        allButtons.click(function () {

            var button = $(this);
            var buttonId = button.data("category");

            if (buttonId === selectedCategory) {
                clearSelected();
                return;
            }

            clearSelected();
            button.addClass("selected");
            selectedCategory = buttonId;

            $.ajax({
                url: `https://localhost:44392/shop/GetCategoryProduct?categoryName=${buttonId}`,
                success: function (results) {
                    results.forEach(function (result) {
                        var product = `
                                            <div class="col-md-3">
                                                <div class="card mt-5">
                                                    <div class="card-body">
                                                        <div class="form-group">
                                                            <img src="/assets/img/product/${result.images[0].imgUrl}" width="150" height="100" />
                                                        </div>
                                                        <div class="form-group">
                                                            <p>${result.name}</p>
                                                            <span>$${result.price}</span>
                                                        </div>
                                                        <a class="btn btn-primary selectButton text-light">Select</a>
                                                    </div>
                                                </div>
                                            </div>
                                        `;

                        area.append(product);
                    });

                    $(".selectButton").click(function () {
                        var productName = $(this).siblings(".form-group").find("p").text();
                        var productPrice = $(this).siblings(".form-group").find("span").text();
                        var categoryName = selectedCategory.toLowerCase();

                        // Set the value of the input field for the selected category
                        $("#" + categoryName).val(productName + " - " + productPrice);
                    });

                    selectedList.on("click", ".deleteButton", function () {
                        var listItem = $(this).parent();
                        var productPriceFormatted = listItem.find("span").text();

                        listItem.html('');
                    });

                },
                error: function (xhr, status, error) {
                    console.error("AJAX request failed:", status, error);
                }
            });
        });
        $(".clearButton").click(function (event) {
            event.preventDefault();
            var inputField = $(this).prev(".form-control");
            inputField.val("");
        });


    });


})(jQuery);