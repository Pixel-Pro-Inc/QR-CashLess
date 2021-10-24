export interface OrderItem {
  id: number;
  name: string;
  description: string;
  price: string;
  reference: string;
  fufilled: boolean; //received food
  purchased: boolean;
  quantity: number;
  calledforbill: boolean;
}
