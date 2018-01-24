var storageKey = "profiler-{C04C0B48-97FF-4333-8AB1-94ECCD1A5DC6}";

$(function () {
    var self = this;
    self.userLocation = null;
    navigator.geolocation.getCurrentPosition(function (result) {
        self.userLocation = result.coords;
    });

    self.userId = localStorage.getItem(storageKey);
    if (self.userId == null) {
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
    else {
        createAndPostUserActivity("load");
    }

    $(window).unload(function () {
        createAndPostUserActivity("unload");
    });

    function createAndPostUserActivity(kind) {
        postUserActivity(createUserActivity(kind))
    };

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

        return activity;
    }

    function postUserActivity(activity) {
        $.ajax({
            url: "/api/UserActivities",
            data: JSON.stringify(activity),
            method: "POST",
            contentType: "application/json"
        });
    }
});