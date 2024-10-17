import { Injectable, inject } from '@angular/core';
import {loadStripe, Stripe, StripeAddressElement, StripeAddressElementOptions, StripeElement, StripeElements} from '@stripe/stripe-js';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { CartService } from './cart.service';
import { Cart } from '../../shared/model/cart';
import { firstValueFrom, map } from 'rxjs';
import { AccountService } from './account.service';
@Injectable({
  providedIn: 'root'
})
export class StripeService {
  baseUrl = environment.apiUrl
  private http = inject(HttpClient);
  private cartService = inject(CartService)
  private stripePromise: Promise<Stripe | null>;
  private elements?: StripeElements;
  private addressElements?: StripeAddressElement;

  private accountService = inject(AccountService)
  constructor() {
    this.stripePromise=loadStripe(environment.stripePublicKey)
  }
  
  getStripeInstence() {
    return this.stripePromise;
  }
  
  async initializeElements() {
    if(!this.elements) {
      const stripe = await this.getStripeInstence();
      if (stripe) {
        const cart = await firstValueFrom(this.createOrUpdatePaymentIntent());
        this.elements= stripe.elements({clientSecret :cart.clientSecret, appearance:{labels:'floating'}})
      }
      else {
        throw new Error('Stripe is not initialized');
      }
    }
    return this.elements;
  }

  async createAddressElements() {
    if (!this.addressElements) {
      const elements = await this.initializeElements();
      if (elements) {
        const user = this.accountService.currentUser();
        let defaultValues: StripeAddressElementOptions['defaultValues'] = {};
        if (user) {
          defaultValues.name = user.fistName + ' ' + user.lastName;
        }
        if (user?.address) {
          defaultValues.address = {
            line1: user.address.line1,
            line2: user.address.line2,
            city: user.address.city,
            state: user.address.state,
            postal_code: user.address.postalCode,
            country: user.address.country
          }
        }
        const options: StripeAddressElementOptions = {
          mode: 'shipping',
          defaultValues
        };
        this.addressElements=elements.create('address', options);
      } else {
        throw new Error('Elements instance has not been loaded');
      }
    }
    return this.addressElements;
  }


  createOrUpdatePaymentIntent() {
    const cart = this.cartService.cart();
    if (!cart) throw new Error('No cart found!');
  
    return this.http.post<Cart>(`${this.baseUrl}payment/${cart.id}`, {}).pipe(
      map(cart => {
        this.cartService.setCart(cart);
        return cart;
      })
    );
  }

  disposeElements() {
    this.elements = undefined;
    this.addressElements = undefined;
  }
  
}
