function closeOperation(id) {
    $.post("/operation/close", { idOperation: id })
        .done(function(data) {
            location.reload(false);
        });
}

function openOperation(id) {
    $.post("/operation/open", { idOperation: id })
        .done(function(data) {
            location.reload(false);
        });
}