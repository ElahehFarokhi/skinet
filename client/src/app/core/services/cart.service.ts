import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.baseUrl;
  private http = inject(HttpClient);

  cart = signal<Cart | null>(null);

  getCart(id: string) {
    this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map(cart => {
        this.cart.set(cart);
      })
    );
  }

  setCart(cart: Cart) {
    this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: (cart) => this.cart.set(cart),
    });
  }

  addItemToCart(item: CartItem | Product, quantity = 1) {
    const cart = this.cart() ?? this.createCard();
    if (this.isProduct(item)) {
      item = this.mapProductToCartItem(item);
    }
    cart.items = this.addOrUpdateItems(cart.items, item, quantity);
    this.setCart(cart);
  }

  addOrUpdateItems(
    items: CartItem[],
    item: CartItem,
    quantity: number
  ): CartItem[] {
    const index = items.findIndex((x) => x.productId === item.productId);
    if (index === -1) {
      item.quantity = quantity;
      items.push(item);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  mapProductToCartItem(item: Product): CartItem {
    return {
      productId: item.id,
      productName: item.name,
      quantity: 0,
      price: item.price,
      brand: item.brand,
      type: item.type,
      pictureUrl: item.pictureUrl,
    };
  }

  //this is a type guard
  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }

  createCard(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }
}
