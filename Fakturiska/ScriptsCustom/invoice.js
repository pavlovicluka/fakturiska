$(document).ready(function () {
    $(".ui-autocomplete-input").css("z-index", 100);
}); 

var currentModalId = "#invoiceModal";
var dropzoneForm = null;
function prepareEditModal() {
    if (dropzoneForm !== null) {
        dropzoneForm.destroy();
        dropzoneForm = null;
    }

    $(currentModalId).find(".receiverAutocomplete").autocomplete({
        select: function (a, b) {
            setCompanyInfo("CompanyReceiver", b.item);
        },
        source: function (request, response) {
            var id = $(this.element[0]).attr("id");
            var fieldCase = 1;
            if (id.indexOf("Name") !== -1) {
                fieldCase = 1;
            } else if (id.indexOf("PersonalNumber") !== -1) {
                fieldCase = 2;
            } else if (id.indexOf("PIB") !== -1) {
                fieldCase = 3;
            }

            $.ajax({
                url: "/Invoice/Autocomplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term, fieldCase: fieldCase },
                success: function (data) {
                    response($.map(data, function (item) {

                        var lab;
                        switch (fieldCase) {
                            case 1:
                                lab = item.Name;
                                break;
                            case 2:
                                lab = item.PersonalNumber;
                                break;
                            case 3:
                                lab = item.PIB;
                                break;
                        }

                        return {
                            label: lab,
                            CompanyGuid: item.CompanyGuid,
                            Name: item.Name,
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

    $(currentModalId).find(".payerAutocomplete").autocomplete({
        select: function (a, b) {
            setCompanyInfo("CompanyPayer", b.item);
        },
        source: function (request, response) {
            var id = $(this.element[0]).attr("id");
            var fieldCase = 1;
            if (id.indexOf("Name") !== -1) {
                fieldCase = 1;
            } else if (id.indexOf("PersonalNumber") !== -1) {
                fieldCase = 2;
            } else if (id.indexOf("PIB") !== -1) {
                fieldCase = 3;
            }

            $.ajax({
                url: "/Invoice/Autocomplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term, fieldCase: fieldCase },
                success: function (data) {
                    response($.map(data, function (item) {

                        var lab;
                        switch (fieldCase) {
                            case 1:
                                lab = item.Name;
                                break;
                            case 2:
                                lab = item.PersonalNumber;
                                break;
                            case 3:
                                lab = item.PIB;
                                break;
                        }

                        return {
                            label: lab,
                            CompanyGuid: item.CompanyGuid,
                            Name: item.Name,
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

    $(".receiverAutocomplete").on('keydown', function () {
        var key = event.keyCode || event.charCode;
        if (key == 8 || key == 46) {
            if ($(this).val().length === 0) {
                clearCompany("CompanyReceiver");
                return false;
            }
        }
    });

    $(".payerAutocomplete").on('keydown', function () {
        var key = event.keyCode || event.charCode;
        if (key == 8 || key == 46) {
            if ($(this).val().length === 0) {
                clearCompany("CompanyPayer");
                return false;
            }
        }  
    });

    disableEnableFields("CompanyReceiver", true);
    disableEnableFields("CompanyPayer", true);
}

function prepareCreateModal() {
    prepareEditModal();

    var dragCounter = 0;
    if (dropzoneForm === null) {
        dropzoneForm = new Dropzone(document.getElementById("invoiceModal"), {
            url: "/Invoice/Upload",
            acceptedFiles: "application/pdf, image/jpeg, image/png",
            autoProcessQueue: false,
            clickable: document.getElementById("addFile"),
            maxFiles: 1,
            previewsContainer: "#dzContainer",
            init: function () {
                this.on("maxfilesexceeded", function (file) {
                    invoiceFile = null;
                    this.removeAllFiles();
                    this.addFile(file);
                });
            },
            success: function (file, response) {
                console.log("odgovor" + response);
            },
            dragenter: function () {
                dragCounter++;
                $("#dimmerModal").show();
            },
            dragleave: function () {
                dragCounter--;
                if (dragCounter === 0) {
                    $("#dimmerModal").hide();
                }
            },
            drop: function () {
                dragCounter = 0;
                $("#dimmerModal").hide();
            }
        });
    }
}

function setCompanyInfo(companyType, c) {
    disableEnableFields(companyType, true);

    $(currentModalId).find("#" + companyType + "_" + "CompanyGuid").val(c.CompanyGuid);
    $(currentModalId).find("#" + companyType + "_" + "Name").val(c.Name);
    $(currentModalId).find("#" + companyType + "_" + "PhoneNumber").val(c.PhoneNumber);
    $(currentModalId).find("#" + companyType + "_" + "FaxNumber").val(c.FaxNumber);
    $(currentModalId).find("#" + companyType + "_" + "Address").val(c.Address);
    $(currentModalId).find("#" + companyType + "_" + "Website").val(c.Website);
    $(currentModalId).find("#" + companyType + "_" + "Email").val(c.Email);
    $(currentModalId).find("#" + companyType + "_" + "PersonalNumber").val(c.PersonalNumber);
    $(currentModalId).find("#" + companyType + "_" + "PIB").val(c.PIB);
    $(currentModalId).find("#" + companyType + "_" + "MIB").val(c.MIB);
    $(currentModalId).find("#" + companyType + "_" + "AccountNumber").val(c.AccountNumber);
    $(currentModalId).find("#" + companyType + "_" + "BankCode").val(c.BankCode); 
}

function clearCompany(companyType) {
    $(currentModalId).find("#" + companyType + "_" + "CompanyGuid").val("");
    $(currentModalId).find("#" + companyType + "_" + "Name").val("");
    $(currentModalId).find("#" + companyType + "_" + "PhoneNumber").val("");
    $(currentModalId).find("#" + companyType + "_" + "FaxNumber").val("");
    $(currentModalId).find("#" + companyType + "_" + "Address").val("");
    $(currentModalId).find("#" + companyType + "_" + "Website").val("");
    $(currentModalId).find("#" + companyType + "_" + "Email").val("");
    $(currentModalId).find("#" + companyType + "_" + "PersonalNumber").val("");
    $(currentModalId).find("#" + companyType + "_" + "PIB").val("");
    $(currentModalId).find("#" + companyType + "_" + "MIB").val("");
    $(currentModalId).find("#" + companyType + "_" + "AccountNumber").val("");
    $(currentModalId).find("#" + companyType + "_" + "BankCode").val("");

    disableEnableFields(companyType, false);
}

function disableEnableFields(companyType, disabled) {
    if (($(currentModalId).find("#" + companyType + "_" + "Name").val() !== null && $(currentModalId).find("#" + companyType + "_" + "Name").val() !== "") || disabled === false) {
        $(currentModalId).find("#" + companyType + "_" + "PhoneNumber").prop("disabled", disabled);
        $(currentModalId).find("#" + companyType + "_" + "FaxNumber").prop("disabled", disabled);
        $(currentModalId).find("#" + companyType + "_" + "Address").prop("disabled", disabled);
        $(currentModalId).find("#" + companyType + "_" + "Website").prop("disabled", disabled);
        $(currentModalId).find("#" + companyType + "_" + "Email").prop("disabled", disabled);
        $(currentModalId).find("#" + companyType + "_" + "MIB").prop("disabled", disabled);
        $(currentModalId).find("#" + companyType + "_" + "AccountNumber").prop("disabled", disabled);
        $(currentModalId).find("#" + companyType + "_" + "BankCode").prop("disabled", disabled);
    }
}

