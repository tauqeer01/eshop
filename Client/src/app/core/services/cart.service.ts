import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/model/cart';
import { Product } from '../../shared/model/product';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  cart = signal<Cart | null>(null);
  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0);
  })
totals = computed(() =>{
  const cart = this.cart();
  if(!cart) return null;
  const subtotals = cart.items.reduce((sum, item) => sum + item.quantity * item.price, 0);
  const shipping = 0;
  const discount = 0;
  const total = subtotals + shipping - discount;
  return {
    subtotals,
    shipping,
    discount,
    total
  }
})
  // get cart
  getCart(id: string) {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map(cart =>{
        this.cart.set(cart);
        return cart
      })
    )
  }

  // update cart
  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: cart => this.cart.set(cart)
    })
  }

  // add item to cart
  addItemToCart(item: CartItem | Product, quantity = 1) {
    const cart = this.cart() ?? this.createCart();
    if (this.isProduct(item)) {
      item = this.mapProductItemToCartItem(item);
    }
    cart.items = cart.items || []; // Initialize cart.items to an empty array if it's null or undefined
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
    this.setCart(cart);
  }

  private addOrUpdateItem(items: CartItem[], item: CartItem, quantity: number): CartItem[] {
    const index = items.findIndex(i => i.productId === item.productId);
    if (index === -1) {
      item.quantity = quantity;
      items.push(item);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  // remove item from cart
  removeItemFromCart(productId: number, quantity =1) {
   const cart = this.cart();
   if(!cart) return;
   const index = cart.items.findIndex(i => i.productId === productId);
   if(index !== -1){
    if(cart.items[index].quantity > quantity){
      cart.items[index].quantity -= quantity;
    } else {
      cart.items.splice(index, 1);
    }
    if(cart.items.length === 0){
      this.deleteCart();
    }
    else{
      this.setCart(cart);
    }
   }
  }
  deleteCart() {
    this.http.delete(this.baseUrl + 'cart?id=' + this.cart()?.id).subscribe({
      next: () =>{
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      }
    })
  }

  private mapProductItemToCartItem(item: Product): CartItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      type: item.type
    }
  }

  // private method
  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }

  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }
}