import { nanoid } from 'nanoid';
export type CartType = {
  id: string;
  items: CartItem[];
  deliveryMethodId?: number;
  paymentIntentId?: string;
  clientSecret?: string;
  coupon?: Coupon;
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
  deliveryMethodId?: number;
  paymentIntentId?: string;
  clientSecret?: string;
  coupon?: Coupon;
}

export type Coupon = {
  name: string;
  amountOff?: number;
  percentOff?: number;
  promotionCode: string;
  couponId: string;
}
