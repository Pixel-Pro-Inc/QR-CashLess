import { User } from "./user";

// The Billeduser extends user here cause it is the working model and needs to reflect things in the API
export interface Billeduser extends User {

  LastPaidDate:Date;
  DuePaymentDate:Date;
  // We cant have a dictionary in typescript so we will just list the branches ids for now
  BilledBranchIds:string[];
  
}
