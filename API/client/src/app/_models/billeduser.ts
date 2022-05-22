export interface Billeduser {
     
  username: string;
  token: string;
  LastPaidDate:Date;
  DuePaymentDate:Date;
  // We cant have a dictionary in typescript so we will just list the branches for now
  BilledBranchIds:string[];
}
