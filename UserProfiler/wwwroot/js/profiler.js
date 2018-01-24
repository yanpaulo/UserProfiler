var storageKey = "profiler-{C04C0B48-97FF-4333-8AB1-94ECCD1A5DC6}";

$(function () {
    var self = this;
    self.userLocation = null;
    navigator.geolocation.getCurrentPosition(function (result) {
        self.userLocation = result.coords;
    });

    self.userId = localStorage.getItem(storageKey);
    if (self.userId == null) {
        createAnonymousUser();
    }
    else {
        createUserActivity("load");
    }

    $(window).unload(function () {
        createUserActivity("unload");
    });

    function createAnonymousUser() {
        var user = {
            appVersion: navigator.appVersion,
            userAgent: navigator.userAgent
        };
        $.ajax({
            url: "/api/AnonymousUsers",
            data: JSON.stringify(user),
            method: "POST",
            contentType: "application/json",
            success: function (data) {
                self.userId = data.Id;
                window.localStorage.setItem(storageKey, data.id);
                createAndPostUserActivity("load");
            }
        });
    }

    function createUserActivity(kind) {
        var activity = {
            kind: kind,
            anonymousUserId: self.userId,
            contentPage: {
                url: location.pathname
            }
        };
        if (self.userLocation) {
            activity.location = self.userLocation.latitude + ", " + self.userLocation.longitude;
        }

        $.ajax({
            url: "/api/UserActivities",
            data: JSON.stringify(activity),
            method: "POST",
            contentType: "application/json"
        });
    }
});