App.controller('PartyRemaining_controller', function ($scope, $http, RequestService) {
    $scope.PartyList = [];
    $scope.isCash = true;
    $scope.isBankAccount = false;
    $scope.isCheckbook = false;
    $scope.PartyRemaining = {
        PartyRemaining_Id:0,
        Party_Id: 0,
        RemainingType: null,
        isPayed: false,
        TotalAmount: 0,
        UserAmount: 0,
        RemainingAmount: 0        
    };
    $scope.PartyRemainings = [];
    
    $scope.isPayed = false;
    $scope.RemainingType = null;
    $scope.RemainingTypes = ["Paying", "Receiving"];
    $scope.isCash_clicked = function () {
        $scope.isCash = true;
        $scope.isBankAccount = false;
        $scope.isCheckbook = false;
        $("#isCash").prop("checked", true);
        $("#isBankAccount").prop("checked", false);
        $("#isCheckbook").prop("checked", false);
        $('#checkbook_content').remove();
    };
    $scope.isBankAccount_clicked = function () {
        $scope.isCash = false;
        $scope.isBankAccount = true;
        $scope.isCheckbook = false;
        $("#isCash").prop("checked", false);
        $("#isBankAccount").prop("checked", true);
        $("#isCheckbook").prop("checked", false);
        $('#checkbook_content').remove();
        var temp = '';
        // temp += '<div class="form-group" id="checkbook_id">';
        temp += '   <div id="checkbook_content">';
        temp += '                       <div class="col-md-12">';
        temp += '                               <div class="form-group">';
        temp += '                                   <label class="control-label col-md-2 text-center">Bank Account no</label>';
        temp += '                                     <div class="col-md-4">';
        temp += '                                           <input name="BankAccountNo"  ng-model="BankAccountNo"  id="BankAccountNo" class="form-control" required/>';
        temp += '                                    </div>';
        temp += '                               </div>';
        temp += '                       </div>';
        // temp += '</div>';
        temp += '</div>';
        $('#checkbook_id').append(temp);
    };
    $scope.isCheckbook_clicked = function () {
        $scope.isCash = false;
        $scope.isBankAccount = false;
        $scope.isCheckbook = true;
        $("#isCash").prop("checked", false);
        $("#isCheckbook").prop("checked", true);
        $("#isBankAccount").prop("checked", false);
        $('#checkbook_content').remove();
        var temp = '';
        // temp += '<div class="form-group" id="checkbook_id">';
        temp += '   <div id="checkbook_content">';
        temp += '                       <div class="col-md-12">';
        temp += '                               <div class="form-group">';
        temp += '                                   <label class="control-label col-md-2">Cheque no</label>';
        temp += '                                     <div class="col-md-4">';
        temp += '                                       <input name="CheckNo" id="CheckNo" ng-model="CheckNo" class="form-control" required/>';
        temp += '                                    </div>';
        temp += '                               </div>';
        temp += '                               <div class="form-group">';
        temp += '                                   <label class="control-label col-md-2 text-center">Bank Account no</label>';
        temp += '                                     <div class="col-md-4">';
        temp += '                                           <input name="BankAccountNo" id="BankAccountNo"  ng-model="BankAccountNo"  class="form-control" required/>';
        temp += '                                    </div>';
        temp += '                               </div>';
        temp += '                       </div>';
        // temp += '</div>';
        temp += '</div>';
        $('#checkbook_id').append(temp);
            //if (!$('#isBankAccount').is(':checked')) {  }
    };
   
    $scope.Get_party = function () {
        var ret = RequestService.JsonCall("GET", "PartyRemainings", "GetParties");
        if (ret != '' && !ret.status) {
            $scope.PartyList = JSON.parse(ret);
        }
    };
    
    $scope.Get_party();

    
    $scope.isPayed_clicked = function () {
                if ($scope.PartyRemaining.isPayed) {
                    $scope.PartyRemaining.isPayed = false;
                    $scope.PartyRemaining.UserAmount = 0;
                    $scope.isCash = true;
                    $scope.isBankAccount = false;
                    $scope.isCheckbook = false;
                }
                if (!$scope.PartyRemaining.isPayed && $scope.PartyRemaining.TotalAmount > 0 && $scope.PartyRemaining.UserAmount == 0)
                {
                    $scope.PartyRemaining.isPayed = true;
                    $scope.PartyRemaining.UserAmount = ($scope.PartyRemaining.UserAmount == 0) ? $scope.PartyRemaining.TotalAmount : $scope.PartyRemaining.UserAmount;
                }
                else {
                    $scope.PartyRemaining.isPayed = false;
                    $scope.isCash = true;
                    $scope.isBankAccount = false;
                    $scope.isCheckbook = false;
                    $scope.PartyRemaining.UserAmount = 0;
                }
                $scope.AmountCalculation();
            };
    $scope.AmountCalculation = function () {
                if ($scope.PartyRemaining.UserAmount > 0) {
                    $scope.PartyRemaining.isPayed = true;
                    if ($scope.isCash && !$scope.isBankAccount && !$scope.isCheckbook) {
                        $("#isCash").prop("checked", true);
                        $("#isBankAccount").prop("checked", false);
                        $("#isCheckbook").prop("checked", false);
                        $('#checkbook_content').remove();
                    }
                }
                else {
                    $scope.PartyRemaining.isPayed = false;
                    $scope.isCash = true;
                    $scope.isBankAccount = false;
                    $scope.isCheckbook = false;
                }

                if ($scope.PartyRemaining.TotalAmount) {
                    if ($scope.PartyRemaining.UserAmount > $scope.PartyRemaining.TotalAmount) {
                        $scope.PartyRemaining.UserAmount = $scope.PartyRemaining.TotalAmount;
                    }
                    $scope.PartyRemaining.RemainingAmount = $scope.PartyRemaining.TotalAmount - $scope.PartyRemaining.UserAmount;
                }
            }

    $scope.Save = function () {
        if ($scope.PartyRemaining.PartyRemaining_Id == 0) {         //Insert
            var ret = RequestService.JsonCallParam('post', 'PartyRemainings', 'Insert', { PartyRemaining: JSON.stringify($scope.PartyRemaining) });
            if (!ret.status && ret != '') {
                var pay_or_Received = ($scope.PartyRemaining.RemainingType == 'Paying') ? 'Pay' : 'Received';
                $scope.PartyRemaining.PartyRemaining_Id = ret;
                var party = $scope.Get_party_From_Id($scope.PartyRemaining.Party_Id);
                 var mTransaction = {
                    Transaction_id: 0,
                    Transaction_item_id: $scope.PartyRemaining.PartyRemaining_Id ,
                    Transaction_item_type: ($scope.PartyRemaining.RemainingType == 'Paying') ? 'PartyRemaining Payed' : 'PartyRemaining Received',
                    Transaction_Description: pay_or_Received + ' Remaining of Party: ' + party.Party_Name,
                    isByCash: ($scope.isCash == "true") ? true : false,
                    BankAccountNo: ($scope.isBankAccount == "true") ? $scope.BankAccountNo : 'N/A',
                    checkno: ($scope.isCheckbook == "true" || $scope.isBankAccount == "true") ? $scope.CheckNo : 0,
                    Debit: ($scope.PartyRemaining.RemainingType == 'Paying') ? $scope.PartyRemaining.UserAmount : 0,
                    Credit: ($scope.PartyRemaining.RemainingType != 'Paying') ? $scope.PartyRemaining.UserAmount : 0,
                    Opening_ClosingDays_id: 0,
                    status: true
                };
                 var ret1_1 = RequestService.JsonCallParam('post', 'Transaction', 'JSON_Transaction', { 'Transaction': JSON.stringify(mTransaction) });
            }
        }       //Insert
        $scope.Clear();
    }
    $scope.Clear = function () {
        $scope.isCash = true;
        $scope.isBankAccount = false;
        $scope.isCheckbook = false;
        $scope.PartyRemaining = {
            PartyRemaining_Id: 0,
            Party_Id: 0,
            RemainingType: null,
            isPayed: false,
            TotalAmount: 0,
            UserAmount: 0,
            RemainingAmount: 0
        };
        $scope.Get_PartyRemainings();
    }
    $scope.Get_PartyRemainings = function () {
        $scope.PartyRemainings = [];
        var ret = RequestService.JsonCall('get', 'PartyRemainings', 'GetAll');
        if (!ret.status && ret != '') {
            var ret_parse = JSON.parse(ret);
            angular.forEach(ret_parse, function (obj) {
                var newobj = {
                    PartyRemaining_Id: obj.PartyRemaining_Id,
                    Party_Id: obj.Party_Id,
                    Party: obj.Party,
                    RemainingType: obj.RemainingType,
                    isPayed: obj.isPayed,
                    TotalAmount: obj.TotalAmount,
                    UserAmount: obj.UserAmount,
                    RemainingAmount1: obj.RemainingAmount,
                    RemainingAmount2: obj.RemainingAmount

                };
                $scope.PartyRemainings.push(newobj);
            });
        }
    };
    $scope.Get_PartyRemainings();
    $scope.ValidateGrid = function () {
        angular.forEach($scope.PartyRemainings, function (obj) {
            if (obj.RemainingAmount2 > obj.RemainingAmount1) {
                obj.RemainingAmount2 = obj.RemainingAmount1;
            }
        });
    };
    $scope.pay_or_Received_click = function (obj) {
        var newObj = {
            PartyRemaining_Id: obj.PartyRemaining_Id,
            Party_Id: obj.Party_Id,
            Party: obj.Party,
            RemainingType: obj.RemainingType,
            isPayed: obj.isPayed,
            TotalAmount: obj.TotalAmount,
            UserAmount: parseFloat(obj.UserAmount) + parseFloat(obj.RemainingAmount2),
            RemainingAmount: parseFloat(obj.TotalAmount) - (parseFloat(obj.UserAmount) + parseFloat(obj.RemainingAmount2))
        };
        RequestService.JsonCallParam('put', 'PartyRemainings', 'Update', { PartyRemaining: JSON.stringify(newObj) });

        var pay_or_Received = (obj.RemainingType == 'Paying') ? 'Pay' : 'Received';
        var party = $scope.Get_party_From_Id(obj.Party_Id);
        var mTransaction = {
            Transaction_id: 0,
            Transaction_item_id: obj.PartyRemaining_Id,
            Transaction_item_type: (obj.RemainingType == 'Paying') ? 'PartyRemaining Payed' : 'PartyRemaining Received',
            Transaction_Description: pay_or_Received + ' Remaining of Party: ' + party.Party_Name,
            isByCash: true,
            BankAccountNo: 'N/A',
            checkno: 0,
            Debit: (obj.RemainingType == 'Paying') ? obj.RemainingAmount2 : 0,
            Credit: (obj.RemainingType != 'Paying') ? obj.RemainingAmount2 : 0,
            Opening_ClosingDays_id: 0,
            status: true
        };
        var ret1_1 = RequestService.JsonCallParam('post', 'Transaction', 'JSON_Transaction', { 'Transaction': JSON.stringify(mTransaction) });
        $scope.Clear();
        $scope.Get_PartyRemainings();
    }

    $scope.Get_party_From_Id = function (Party_Id) {
        var ret = null;
        angular.forEach($scope.PartyList, function (party) {
            if (party.Party_Id == Party_Id) {
                ret = party;
            }
        });
        return ret;
    };
});

