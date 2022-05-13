export interface MenuItem {
  id: number;
  name: string;
  description: string;
  category: string;
  price: string;
  imgUrl: string;
  prepTime: string;
  minimumPrice: number;
  rate: number;
  availability: boolean;
  publicId: string;
  subCategory: string;
  weight:string;
  flavours: string[];
  meatTemperatures: string[];
  sauces: string[];
  selectedFlavour: string;
  selectedMeatTemperature: string;
  selectedSauces: string[];
}
