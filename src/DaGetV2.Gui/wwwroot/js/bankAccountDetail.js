function closeOperation(id) {
    $.post("/operation/close", { idOperation: id })
        .done(function (data) {
            location.reload(true);
        });
}