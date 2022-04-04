export interface OrderItem {
  name: string;
  description: string;
  reference: string;
  price: string;
  weight: string;  
  fufilled: boolean; //received food
  purchased: boolean;
  paymentMethod: string;
  preparable: boolean;
  waitingForPayment: boolean;
  quantity: number;
  orderNumber: string;
  phoneNumber: string;
  category: string;
  prepTime: number;
  id_O: string;
}
