UserTicket = function () {
};

UserTicket.prototype.LoadTickets = function () {
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

UserTicket.prototype.TicketToHtml = function (ticket) {
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
    s += "</tr>";
    return s;
};

UserTicket.prototype.DateToString = function (date) {
    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var year = date.getFullYear()
    var hour = date.getHours();
    var minute = date.getMinutes();
    return day + "-" + month + "-" + year + " " + hour + ":" + minute;
};

UserTicket.prototype.Init = function () {
    var that = this;

    $('form#addTicketForm').submit(function (evt) {
        var ticket = {
            CreatedBy: $('#ticketCreatedBy').val(),
            Description: $('#ticketDescription').val()
        };

        $.ajax({
            url: "api/tickets",
            type: "POST",
            data: JSON.stringify(ticket),
            contentType: "application/json",
            success: function () {
                that.LoadTickets();
            }
        });
        return false;
    });

    that.LoadTickets();
};