export interface OrderItem {
  name: string;
  description: string;
  reference: string;
  price: string;
  weight: string;  
  fufilled: boolean; //received food
  purchased: boolean;
  preparable: boolean;
  waitingForPayment: boolean;
  quantity: number;
  orderNumber: string;
}
