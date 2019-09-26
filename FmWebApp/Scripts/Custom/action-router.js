/*window.actionRouter = (function () {
    // to make a history fallback work (https://github.com/devote/HTML5-History-API)
    var location = window.history.location || window.location;

    var popstateInProgress,
        routeTable = {}, // contains pairs 'path' -> 'action link'
        options = {
            actionLinkSelector: '.js-action-link',
            defaultRoute: '/',
            titleSelector: '.js-page-title:first'
        };

    function init(opts) {
        _.extend(options, opts);

        // subscribe on popstate to handle going 'back' and 'forward'
        $(window).on('popstate', function (e) {
            // subscribing on popstate using jQuery state is in the 'originalEvent' field
            var state = e.originalEvent.state;

            // use a special field of state to track history of ajax action links
            if (state && state.role === 'page') {
                popstateInProgress = true;
                navigate(location.pathname);
            }
        });

        buildRouteTable(options.actionLinkSelector);
        initializeHistory();
    };

    // build route table from action links
    function buildRouteTable(actionLinkSelector) {
        _.each($(actionLinkSelector), function (actionLink) {
            var $actionLink = $(actionLink);
            routeTable[$actionLink.attr('href')] = $actionLink;
        });
    };

    function initializeHistory() {
        // if a browser doesn't support HTML5 History API
        // the url will be in format '<host>/#<pathname>'
        // so at the init phase we should navigate a browser to the correct page
        if (!Modernizr.history && location.pathname !== options.defaultRoute) {
            navigate(location.pathname);
        }

        // make browser back button work for the initial page state
        history.replaceState({ role: 'page' }, null, location.pathname);
    };

    function navigate(path) {
        // get an action link for path
        var $actionLink = _.has(routeTable, path)
            ? routeTable[path]
            : routeTable[options.defaultRoute];

        // click that link to trigger ajax loading of new content
        $actionLink.focus().trigger('click');
    };

    // ######################################################
    // Callbacks called by unobtrusive ajax (Ajax.ActionLink)
    // ######################################################
    function navigationSucceeded(actionLink) {
        var $title = $(options.titleSelector),
            actionPath = $(actionLink).attr('href');

        // change the document title if loaded via ajax content has a special field with title
        if ($title.length) {
            document.title = $title.val();
        }

        // push a new history state if it's not called during popstate and the pathname has changed
        if (!popstateInProgress && location.pathname !== actionPath) {
            history.pushState({ role: 'page' }, null, actionPath);
        }
    };

    function navigationFailed(actionLink) {
        $(actionLink).blur();
        alert('Sorry, an error occured!');
    };

    function navigationCompleted() {
        popstateInProgress = false;
    };
    // ######################################################

    return {
        init: init,
        navigationSucceeded: navigationSucceeded,
        navigationFailed: navigationFailed,
        navigationCompleted: navigationCompleted
    };
})(jQuery);*/