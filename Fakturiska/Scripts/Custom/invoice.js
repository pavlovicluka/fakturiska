$(document).ready(function () {
    $('.datepicker').datepicker({
        format: 'DD/MM/YYYY'
    });

    $(".ui-autocomplete-input").css("z-index", 100);

    $("#CompanyReceiver_Name").autocomplete({
        select: function (a, b) {
            $(this).val(b.item.Name);

            $("#CompanyReceiver_" + "PhoneNumber").val(b.item.PhoneNumber);
            $("#CompanyReceiver_" + "FaxNumber").val(b.item.FaxNumber);
            $("#CompanyReceiver_" + "Address").val(b.item.Address);
            $("#CompanyReceiver_" + "Website").val(b.item.Website);
            $("#CompanyReceiver_" + "Email").val(b.item.Email);
            $("#CompanyReceiver_" + "PersonalNumber").val(b.item.PersonalNumber);
            $("#CompanyReceiver_" + "PIB").val(b.item.PIB);
            $("#CompanyReceiver_" + "MIB").val(b.item.MIB);
            $("#CompanyReceiver_" + "AccountNumber").val(b.item.AccountNumber);
            $("#CompanyReceiver_" + "BankCode").val(b.item.BankCode);
        },
        source: function (request, response) {
            $.ajax({
                url: "/Invoice/Autocomplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        console.log(item.Name);
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
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: function (resultsCount) {}
        }
    });

    $("#CompanyPayer_Name").autocomplete({
        select: function (a, b) {
            $(this).val(b.item.Name);

            $("#CompanyPayer_" + "PhoneNumber").val(b.item.PhoneNumber);
            $("#CompanyPayer_" + "FaxNumber").val(b.item.FaxNumber);
            $("#CompanyPayer_" + "Address").val(b.item.Address);
            $("#CompanyPayer_" + "Website").val(b.item.Website);
            $("#CompanyPayer_" + "Email").val(b.item.Email);
            $("#CompanyPayer_" + "PersonalNumber").val(b.item.PersonalNumber);
            $("#CompanyPayer_" + "PIB").val(b.item.PIB);
            $("#CompanyPayer_" + "MIB").val(b.item.MIB);
            $("#CompanyPayer_" + "AccountNumber").val(b.item.AccountNumber);
            $("#CompanyPayer_" + "BankCode").val(b.item.BankCode);
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
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: function (resultsCount) { }
        }
    });
})