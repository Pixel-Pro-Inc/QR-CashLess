import { __decorate } from "tslib";
import { Component } from '@angular/core';
let RegisterBranchComponent = class RegisterBranchComponent {
    constructor(branchService) {
        this.branchService = branchService;
        this.dto = {};
    }
    ngOnInit() {
    }
    register() {
        let n = Math.random() * 100000;
        n = Math.round(n);
        this.dto.id = 'rd' + n;
        //Set Phonenumbers
        let arrayPhoneNumber = [this.dto.phoneNumber1, this.dto.phoneNumber2];
        this.dto.PhoneNumbers = arrayPhoneNumber;
        //Set Times
        let arrayOpenTimes = [this.dto.openingTime_1, this.dto.openingTime_2, this.dto.openingTime_3, this.dto.openingTime_4,
            this.dto.openingTime_5, this.dto.openingTime_6, this.dto.openingTime_7, this.dto.openingTime_8];
        this.dto.OpeningTime = arrayOpenTimes;
        let arrayClosingTimes = [this.dto.closingTime_1, this.dto.closingTime_2, this.dto.closingTime_3, this.dto.closingTime_4,
            this.dto.closingTime_5, this.dto.closingTime_6, this.dto.closingTime_7, this.dto.closingTime_8];
        this.dto.ClosingTime = arrayClosingTimes;
        console.log(this.dto);
        this.branchService.submission(this.dto, 'branch/register');
        this.ngOnInit;
    }
    onFileChange(event) {
        const reader = new FileReader();
        if (event.target.files && event.target.files.length) {
            const [file] = event.target.files;
            reader.readAsDataURL(file);
            reader.onload = () => {
                this.dto.img = reader.result;
            };
        }
    }
};
RegisterBranchComponent = __decorate([
    Component({
        selector: 'app-register-branch',
        templateUrl: './register-branch.component.html',
        styleUrls: ['./register-branch.component.css']
    })
], RegisterBranchComponent);
export { RegisterBranchComponent };
//# sourceMappingURL=register-branch.component.js.map