﻿$(function () {
    //Configurações internas do script
    var settings = {
        //Chave do localStorage que armazena o id do usuário
        userIdKey: "profiler-{C04C0B48-97FF-4333-8AB1-94ECCD1A5DC6}",
        //Chave do sessionStorage que armazena a última localização de usuário disponível
        locationKey: "profiler-{1694CB31-CAD4-4C61-8A98-5CFCB09F2115}"
    };

    //Referência deste objeto para utilizar nas funções locais.
    var self = this;

    //Utiliza a última localização conhecida
    self.userLocation = sessionStorage.getItem(settings.locationKey);

    //Tenta atualizar a localização conhecida.
    navigator.geolocation.getCurrentPosition(
        function (result) {
            self.userLocation = result.coords;
        }, function () {
            self.userLocation = null;
        });

    //Registra os eventos relevantes.
    $(window).unload(function () {
        createActivity("unload");
    });


    //Funções locais
    //

    //Cria e posta um "activity". Se um usuário não existir, cria um usuário e em seguida posta a "activity".
    function createActivityOrUser() {
        self.userId = localStorage.getItem(settings.userIdKey);
        if (self.userId == null) {
            createAnonymousUser();
        }
        else {
            createActivity("load");
        }
    }

    //Cria um usuário e armazena seu código no localStorage.
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
                self.userId = data.id;
                window.localStorage.setItem(settings.userIdKey, data.id);
                createActivity("load");
            }
        });
    }

    //Cria um activity para o usuário atual.
    function createActivity(kind) {
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
            contentType: "application/json",
            statusCode: {
                400: deleteUser
            }
        });
    }

    //"Esquece" o usuário atual.
    function deleteUser() {
        if (localStorage.getItem(settings.userIdKey)) {
            localStorage.removeItem(settings.userIdKey);
        }
    }
});