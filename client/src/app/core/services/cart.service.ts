import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { map } from 'rxjs';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  cart = signal<Cart | null>(null);
  itemCount = computed(() =>
    this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0)
  );
  selectedDelivery = signal<DeliveryMethod | null>(null);
  totals = computed(() => {
    const cart = this.cart();
    const delivery = this.selectedDelivery();

    if (!cart) return null;

    const subTotal = cart.items.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );
    const shipping = delivery ? delivery.price : 0;

    const discount = 0;
    return {
      subTotal,
      shipping,
      discount,
      total: subTotal + shipping - discount,
    };
  });
  getCart(id: string) {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map((cart) => {
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

  removeItemFromCart(productId: number, quantity = 1) {
    const cart = this.cart();
    if (!cart) return;
    const index = cart.items.findIndex(x => x.productId === productId);
    if (index !== -1) {
      if (cart.items[index].quantity > quantity) {
        cart.items[index].quantity -= quantity;
      } else {
        cart.items.splice(index, 1);
      }
      if (cart.items.length === 0) {
        this.deleteCart();
      } else {
        this.setCart(cart);
      }
    }
  }

  deleteCart() {
    this.http
      .delete(this.baseUrl + 'cart?id=' + this.cart()?.id)
      .subscribe(() => {
        localStorage.removeItem('cart-id');
        this.cart.set(null);
      });
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
