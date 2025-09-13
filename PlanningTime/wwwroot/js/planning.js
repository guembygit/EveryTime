$(document).ready(function () {
    var currentMonth = parseInt($("#hiddenMonth").val());
    var currentYear = parseInt($("#hiddenYear").val());

    // Navigation mois précédent
    $("#prevMonth").click(function () {
        var newMonth = currentMonth - 1;
        var newYear = currentYear;
        if (newMonth < 1) { newMonth = 12; newYear--; }
        loadMonth(newYear, newMonth);
    });

    // Navigation mois suivant
    $("#nextMonth").click(function () {
        var newMonth = currentMonth + 1;
        var newYear = currentYear;
        if (newMonth > 12) { newMonth = 1; newYear++; }
        loadMonth(newYear, newMonth);
    });

    // Fonction pour recharger le partial via AJAX
    function loadMonth(year, month) {
        $.get("/Planning/GetMonth", { year: year, month: month }, function (data) {
            $("#planningContainer").html(data);

            // Mettre à jour les champs cachés du nouveau partial
            currentMonth = parseInt($("#hiddenMonth").val());
            currentYear = parseInt($("#hiddenYear").val());
            $("#currentMonth").text($("#hiddenMonthName").val() + " " + currentYear);
        });
    }

    // Gestion du clic sur une cellule
    $(document).on("click", ".dayBtn:not([disabled])", function () {
        var date = $(this).data("date");
        var userEvents = [];

        // Récupérer tous les événements affichés dans cette cellule
        $(this).find("div").each(function () {
            var userName = $(this).text();
            var bgColor = $(this).css("background-color");
            if (userName) userEvents.push({ name: userName, color: bgColor });
        });

        var message = "Jour sélectionné : " + date + "\n";
        if (userEvents.length > 0) {
            message += "Événements :\n";
            userEvents.forEach(function (ue) {
                message += "- " + ue.name + "\n";
            });
        } else {
            message += "Aucun événement ce jour.";
        }

        alert(message);
    });
});
