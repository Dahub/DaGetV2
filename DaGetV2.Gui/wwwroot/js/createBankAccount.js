$("#postForm").click(function () {   
    preparOperationTypesForPost();
    $("#createBankAccountForm").submit();
});

$('#addOperationType').click(function () {
    addOperationTypeTr();
});

function deleteOperationType(elem) {
    $(elem).parent().parent().parent().remove();
}

function addOperationTypeTr() {
    var html = '<tr>';
    html += '<td>';
    html += '<input type="hidden" id="operationTypeId" value="">'
    html += '<input type="text" id="operationTypeWording" class="form-control" value="" />';
    html += '</td>';   
    html += '<td>';
    html += ' <span><i onclick="deleteOperationType(this)" class="material-icons md-36" style="cursor: pointer;">delete_outline</i></span>';
    html += '</td>';
    html += '</tr>';

    $("#operationTypeTableBody").append(html);
}

function preparOperationTypesForPost() {
    $('#operationTypesToPost').html('');

    var count = 0;
    var existingWordings = []
    $('#operationTypeTableBody > tr').each(function () {
        var id = $(this).find('#operationTypeId').val();
        var wording = $(this).find('#operationTypeWording').val().trim();

        if (isEmptyOrSpaces(wording)) {
            $(this).remove();
        }
        else if ($.inArray(wording, existingWordings) !== -1) {
            $(this).remove();
        }
        else {

            existingWordings.push(wording);
            $('#operationTypesToPost').append('<input type="hidden" name="OperationTypes[' + count + '].Key" value="' + id + '" />');
            $('#operationTypesToPost').append('<input type="hidden" name="OperationTypes[' + count + '].Value" value="' + wording + '" />');
            count++;
        }
    });
}