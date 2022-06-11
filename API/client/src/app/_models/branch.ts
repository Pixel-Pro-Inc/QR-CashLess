import { Time } from "@angular/common";

export interface Branch {
    name: string;
    id: string;
    location: string;
    img: string;
    lastActive: number;
    phoneNumbers: number[];
    openingTime: Time;
    closingTime: Time;
    openingTimeTomorrow: Time;
  }