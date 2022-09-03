import { CustomError } from "ts-custom-error"

/**
 * @author Abel Tshimbalanga
 * 
 * This should be thrown when the current user is not a billable user.
 * 
 * This shouldn't happen cause the current users can't access billings and payments 
 * if not. 
 * 
 * To handle this error, show a message that they aren't billed and remove the option to 
 * navigate to the webpage and force them into home
 */
export class UnBillableUserException extends CustomError {
    public constructor(
        public code: number,
        message?: string,
    ) {
        super(message)
    }
}