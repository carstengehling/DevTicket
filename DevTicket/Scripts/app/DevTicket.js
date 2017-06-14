DevTicket = function () {
};

DevTicket.prototype.LoadTickets = function () {
    var that = this;

    $('table#tickets tbody').empty();
    $.getJSON("/api/tickets",
        function (data) {
            $.each(data, function () {
                $('table#tickets tbody').append(that.TicketToHtml(this));
            });
        }
    );
};

DevTicket.prototype.TicketToHtml = function (ticket) {
    var created = this.DateToString(new Date(ticket.Created));
    var pickedUpBy = ticket.PickedUpBy ? ticket.PickedUpBy : "";
    if (ticket.PickedUp)
        pickedUp = this.DateToString(new Date(ticket.PickedUp));
    else
        pickedUp = "";

    var s = "<tr>";
    s += "<td><b>" + ticket.CreatedBy + "</b><br/><i>" + created + "</i></td>";
    s += "<td>" + ticket.Description + "</td>";
    s += "<td><b>" + pickedUpBy + "</b><br/><i>" + pickedUp + "</i></td>";

    if (pickedUp == "") {
        s += "<td>";
        s += "<a href=\"#\" class=\"pickupTicket\" data-id=\"" + ticket.Id + "\">Pickup</a> ";
        s += "</td>";
    }
    s += "<td>";
    s += "<a href=\"#\" class=\"deleteTicket\" data-id=\"" + ticket.Id + "\">Delete</a> ";
    s += "</td>";
    s += "</tr>";
    return s;
};

DevTicket.prototype.DateToString = function (date) {
    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var year = date.getFullYear();
    var hour = date.getHours();
    var minute = date.getMinutes();
    return day + "-" + month + "-" + year + " " + hour + ":" + minute;
};

DevTicket.prototype.PickupTicket = function (id, name) {
    var that = this;

    var user = {
        Name: name
    };

    $.ajax({
        url: "api/tickets/" + id + "/pickup",
        type: "POST",
        data: JSON.stringify(user),
        contentType: "application/json",
        success: function () {
            that.LoadTickets();
        }
    });
};

DevTicket.prototype.Init = function () {
    var that = this;

    $('#tickets').on("click", 'a.pickupTicket', function () {
        that.PickupTicket($(this).data("id"), $('#userName').val());
        return false;
    });

    that.LoadTickets();
};