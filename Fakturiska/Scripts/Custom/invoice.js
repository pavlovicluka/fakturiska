$(document).ready(function () {
    $('.datepicker').datepicker({
        format: 'DD/MM/YYYY'
    });

    $(".ui-autocomplete-input").css("z-index", 100);
}); 

function prepareModal() {
    currentModalId = "#invoiceModal";

    $(currentModalId).find("#CompanyReceiver_Name").autocomplete({
        select: function (a, b) {
            $(this).val(b.item.Name);

            $(currentModalId).find("#CompanyReceiver_" + "PhoneNumber").val(b.item.PhoneNumber);
            $(currentModalId).find("#CompanyReceiver_" + "FaxNumber").val(b.item.FaxNumber);
            $(currentModalId).find("#CompanyReceiver_" + "Address").val(b.item.Address);
            $(currentModalId).find("#CompanyReceiver_" + "Website").val(b.item.Website);
            $(currentModalId).find("#CompanyReceiver_" + "Email").val(b.item.Email);
            $(currentModalId).find("#CompanyReceiver_" + "PersonalNumber").val(b.item.PersonalNumber);
            $(currentModalId).find("#CompanyReceiver_" + "PIB").val(b.item.PIB);
            $(currentModalId).find("#CompanyReceiver_" + "MIB").val(b.item.MIB);
            $(currentModalId).find("#CompanyReceiver_" + "AccountNumber").val(b.item.AccountNumber);
            $(currentModalId).find("#CompanyReceiver_" + "BankCode").val(b.item.BankCode);
        },
        source: function (request, response) {
            $.ajax({
                url: "/Invoice/Autocomplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Name, value: item.Name,
                            PhoneNumber: item.PhoneNumber,
                            FaxNumber: item.FaxNumber,
                            Address: item.Address,
                            Website: item.Website,
                            Email: item.Email,
                            PersonalNumber: item.PersonalNumber,
                            PIB: item.PIB,
                            MIB: item.MIB,
                            AccountNumber: item.AccountNumber,
                            BankCode: item.BankCode
                        };
                    }));

                }
            });
        },
        messages: {
            noResults: "", results: function (resultsCount) { }
        }
    });

    $(currentModalId).find("#CompanyPayer_Name").autocomplete({
        select: function (a, b) {
            $(this).val(b.item.Name);

            $(currentModalId).find("#CompanyPayer_" + "PhoneNumber").val(b.item.PhoneNumber);
            $(currentModalId).find("#CompanyPayer_" + "FaxNumber").val(b.item.FaxNumber);
            $(currentModalId).find("#CompanyPayer_" + "Address").val(b.item.Address);
            $(currentModalId).find("#CompanyPayer_" + "Website").val(b.item.Website);
            $(currentModalId).find("#CompanyPayer_" + "Email").val(b.item.Email);
            $(currentModalId).find("#CompanyPayer_" + "PersonalNumber").val(b.item.PersonalNumber);
            $(currentModalId).find("#CompanyPayer_" + "PIB").val(b.item.PIB);
            $(currentModalId).find("#CompanyPayer_" + "MIB").val(b.item.MIB);
            $(currentModalId).find("#CompanyPayer_" + "AccountNumber").val(b.item.AccountNumber);
            $(currentModalId).find("#CompanyPayer_" + "BankCode").val(b.item.BankCode);
        },
        source: function (request, response) {
            $.ajax({
                url: "/Invoice/Autocomplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Name, value: item.Name,
                            PhoneNumber: item.PhoneNumber,
                            FaxNumber: item.FaxNumber,
                            Address: item.Address,
                            Website: item.Website,
                            Email: item.Email,
                            PersonalNumber: item.PersonalNumber,
                            PIB: item.PIB,
                            MIB: item.MIB,
                            AccountNumber: item.AccountNumber,
                            BankCode: item.BankCode
                        };
                    }));

                }
            });
        },
        messages: {
            noResults: "", results: function (resultsCount) { }
        }
    });

    $(currentModalId).find('#receiverCheckbox').change(function () {
        if (this.checked) {
            $(currentModalId).find(".receiverFields").removeAttr("disabled");
            $(currentModalId).find(".receiverFields").attr("data-val", "true");
            $(currentModalId).find('.receiverFields').data('unobtrusiveValidation');
            $(currentModalId).find('.receiverFields').data('validator');
            $.validator.unobtrusive.parse('.receiverFields');
        } else {
            $(currentModalId).find(".receiverFields").attr("disabled", "true");
            $(currentModalId).find(".receiverValidation").empty();
            $(currentModalId).find(".receiverFields").attr("data-val", "false");
            $(currentModalId).find('.receiverFields').removeData('unobtrusiveValidation');
            $(currentModalId).find('.receiverFields').removeData('validator');
            $.validator.unobtrusive.parse('.receiverFields');
        }
    });

    $(currentModalId).find('#payerCheckbox').change(function () {
        if (this.checked) {
            $(currentModalId).find(".payerFields").removeAttr("disabled");
            $(currentModalId).find(".payerFields").attr("data-val", "true");
            $(currentModalId).find('.payerFields').data('unobtrusiveValidation');
            $(currentModalId).find('.payerFields').data('validator');
            $.validator.unobtrusive.parse('.payerFields');
        } else {
            $(currentModalId).find(".payerFields").attr("disabled", "true");
            $(currentModalId).find(".payerValidation").empty();
            $(currentModalId).find(".payerFields").attr("data-val", "false");
            $(currentModalId).find('.payerFields').removeData('unobtrusiveValidation');
            $(currentModalId).find('.payerFields').removeData('validator');
            $.validator.unobtrusive.parse('.payerFields');
        }
    });
}



