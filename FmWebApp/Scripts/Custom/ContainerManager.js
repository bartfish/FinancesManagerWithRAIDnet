
var ContainerManager = (function () {

	var mainHeight, timeForFadingAnimationMs = 500;

    var ShowMainContentContainer = function () {

        $("main").fadeIn(timeForFadingAnimationMs);

    };

    var GenerateMainHeight = function () {

		$("main").height($(window).height() * 0.85);
		mainHeight = $("main").height();
    }; 

	var GenerateMainContentHeight = function () {

		$("#content").height(mainHeight);

	};

	var GenerateNavContainerHeight = function () {

		$("#nav-container").height(mainHeight);

    };

    var GenerateMobileNavHeight = function () {

        if ($(window).width() < 768) {
            $("#nav-container").height($(window).height());
        }
    };

	var TurnOnActiveNavigationButtons = function () {


		$("#nav-container a").each(function () {

			$(this).on("click", function (e) {
				e.preventDefault;

				if (!$(this).hasClass("current-view-opened")) {
					$("#nav-container a").removeClass("current-view-opened");
					$(this).addClass("current-view-opened");

					console.log("item clicked");
					//return false;
				}

			});

		});

	};

	var ShowDarkBgContainer = function () {

		$(".dialog-bg-container").fadeIn(timeForFadingAnimationMs);

	};

	var HideDarkBgContainer = function () {

		$(".dialog-bg-container").fadeOut();

	};

	var ShowDialogContainer = function () {

        $(".dialog-container").fadeIn(timeForFadingAnimationMs);

	};

	var HideDialogContainer = function () {

		ResetFormFields(".dialog-container");
		$(".dialog-container").fadeOut();
	};

	var ShowBgAndDialogContainers = function () {

		ShowDarkBgContainer();
		ShowDialogContainer();
	};

	var HideBgAndDialogContainers = function () {

		HideDarkBgContainer();
		HideDialogContainer();

	};

	var ShowActionInfo = function (actionText) {

		HideDialogContainer();
        $(".action-info-container p").text(actionText);
		$(".action-info-container").fadeIn(timeForFadingAnimationMs);

	};
	
	var HideActionInfo = function () {
		
		HideDarkBgContainer();
		ResetFormFields(".action-info-container");
        $(".action-info-container p").text("");
		$(".action-info-container").fadeOut();

    };

    var HideActionInfoAndRefreshDash = function () {

        HideActionInfo();
        location.reload();
    };

	var ShowLoadingAnimation = function () {
		
		$("#loader-bg-container").fadeIn(timeForFadingAnimationMs);
		$("#loader-container").fadeIn(timeForFadingAnimationMs);

	};

	var HideLoadingAnimation = function () {

		$("#loader-bg-container").fadeOut(timeForFadingAnimationMs);
		$("#loader-container").fadeOut(timeForFadingAnimationMs);

	};

    var ShowContentLoadingAnimation = function () {

        $("#view-content").hide();
        $("#loader-bg-content-container").fadeIn();
        $("#loader-content-container").fadeIn();

    };
    
    var HideContentLoadingAnimation = function () {

        setTimeout(function () {

            $("#loader-bg-content-container").fadeOut(timeForFadingAnimationMs);
            $("#loader-content-container").fadeOut(timeForFadingAnimationMs);

            $("#view-content").show();

        }, 500);

    };
    /*
    var SuccessDashboardAnimationWithHistory = function () {

        window.actionRouter.navigationSucceeded(this);
        HideContentLoadingAnimation();

    };

    var CompletedDashboardAnimationWithHistory = function () {

        window.actionRouter.navigationCompleted(this);
        HideContentLoadingAnimation();
    };

    var FailureDashboardAnimationWithHistory = function () {

        window.actionRouter.navigationFailed(this);
        HideContentLoadingAnimation();
    };
    */

    var ShowMobileNav = function () {

        $(".go-mobile-nav").on("click", function (e) {

            e.preventDefault();

            if ($("#nav-container").hasClass("go-mobile-nav-opened")) {
                $("#nav-container").removeClass("go-mobile-nav-opened");
            } else {
                $("#nav-container").addClass("go-mobile-nav-opened");
            }

            return false;

        });
    };


    var HideMobileNav = function () {

        $(".close-mobile-nav").on("click", function (e) {

            e.preventDefault();

            if ($("#nav-container").hasClass("go-mobile-nav-opened")) {
                $("#nav-container").removeClass("go-mobile-nav-opened");
            } else {
                $("#nav-container").addClass("go-mobile-nav-opened");
            }

            return false;

        });
    };

    var HideMobileNavAfterClick = function () {

        $("nav a").on("click", function (e) {

            if ($("#nav-container").hasClass("go-mobile-nav-opened")) {
                $("#nav-container").removeClass("go-mobile-nav-opened");
            }

        });
    };

	var ResetFormFields = function (parentClassOrId) {

		$(parentClassOrId).click(function () {
			$(this).closest('form').find("input[type=text], textarea, select").val("");
        });

	};

	var OnlyNumbersAllowed = function () {

		$(".only-numbers-allowed").on("keypress", function (event) {

			if (event.which < 48 || event.which > 57) {
				event.preventDefault();
			}

		});

    };

    var onEnterClicked = function () {

        $(window).on("keyup", function (e) {
            if (e.keyCode === 13 || e.which === 13) {
                e.preventDefault;
                
                $("#incomesContentTableForm").submit(); // grid submition

                return false;
            }
        });

    }; 

	var init = function () {

        ShowMainContentContainer();

		GenerateMainHeight();
		GenerateMainContentHeight();
        GenerateNavContainerHeight();
        GenerateMobileNavHeight();

		TurnOnActiveNavigationButtons();
		ShowDarkBgContainer();
		HideDarkBgContainer();
		ShowDialogContainer();
		HideDialogContainer();
		ShowBgAndDialogContainers();

		ShowActionInfo(actionText);
        HideActionInfo();
        HideActionInfoAndRefreshDash();

		ShowLoadingAnimation();
		HideLoadingAnimation();

        ShowContentLoadingAnimation();
        HideContentLoadingAnimation();
        /*
        SuccessDashboardAnimationWithHistory();
        CompletedDashboardAnimationWithHistory();
        FailureDashboardAnimationWithHistory();
        */
		HideBgAndDialogContainers();
		ResetFormFields();
        OnlyNumbersAllowed();
        onEnterClicked();
        ShowMobileNav();
        HideMobileNav();
        HideMobileNavAfterClick();
	};

	return {

        init: init,
        ShowMainContentContainer: ShowMainContentContainer,
		GenerateMainHeight: GenerateMainHeight,
		GenerateMainContentHeight: GenerateMainContentHeight,
        GenerateNavContainerHeight: GenerateNavContainerHeight,
        GenerateMobileNavHeight: GenerateMobileNavHeight,
		TurnOnActiveNavigationButtons: TurnOnActiveNavigationButtons,
		ShowDarkBgContainer: ShowDarkBgContainer,
		HideDarkBgContainer: HideDarkBgContainer,
		ShowDialogContainer: ShowDialogContainer,
		HideDialogContainer: HideDialogContainer,
		ShowBgAndDialogContainers: ShowBgAndDialogContainers,
		ShowActionInfo: ShowActionInfo,
        HideActionInfo: HideActionInfo,
        HideActionInfoAndRefreshDash: HideActionInfoAndRefreshDash,
		ShowLoadingAnimation: ShowLoadingAnimation,
        HideLoadingAnimation: HideLoadingAnimation,
        ShowContentLoadingAnimation: ShowContentLoadingAnimation,
        HideContentLoadingAnimation: HideContentLoadingAnimation,
        /*
        SuccessDashboardAnimationWithHistory: SuccessDashboardAnimationWithHistory,
        CompletedDashboardAnimationWithHistory: CompletedDashboardAnimationWithHistory,
        FailureDashboardAnimationWithHistory: FailureDashboardAnimationWithHistory,
        */
		HideBgAndDialogContainers: HideBgAndDialogContainers,
		ResetFormFields: ResetFormFields,
        OnlyNumbersAllowed: OnlyNumbersAllowed,
        onEnterClicked: onEnterClicked,
        ShowMobileNav: ShowMobileNav,
        HideMobileNav: HideMobileNav,
        HideMobileNavAfterClick: HideMobileNavAfterClick
        
	};
	
}()); 