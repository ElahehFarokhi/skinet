import { nanoid } from 'nanoid';
export type CartType = {
  id: string;
  items: CartItem[];
};

export type CartItem = {
  productId: number;
  productName: string;
  pictureUrl: string;
  brand: string;
  type: string;
  quantity: number;
  price: number;
};

export class Cart implements CartType {
  id = nanoid();
  items: CartItem[] = [];
}
