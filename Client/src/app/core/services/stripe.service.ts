import { Injectable, inject } from '@angular/core';
import {ConfirmationToken, loadStripe, Stripe, StripeAddressElement, StripeAddressElementOptions, StripeElement, StripeElements, StripePaymentElement} from '@stripe/stripe-js';
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
  private paymentElements?: StripePaymentElement;
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
 
  async createPaymentElement() {
    if(!this.paymentElements) {
      const elements = await this.initializeElements();
      if (elements)
      {
        this.paymentElements= elements.create('payment');
      } else {
        throw new Error('Elements instance has not been loaded');
      }
    }
    return this.paymentElements;
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

  async createConfirmationToken() {
    const stripe = await this.getStripeInstence();
    const elements = await this.initializeElements();
    const retult = await elements.submit();
    if (retult.error) throw new Error(retult.error.message);
    if (stripe) {
      return await stripe.createConfirmationToken({elements});
    }
    else {
      throw new Error('Stripe is not initialized');
    }
  }
  async confirmPayment(confirmationToken : ConfirmationToken) {
    const stripe = await this.getStripeInstence();
    const elements = await this.initializeElements();
    const retult = await elements.submit();
    if (retult.error) throw new Error(retult.error.message);
    const clientSecret = this.cartService.cart()?.clientSecret;
    if (stripe && clientSecret) {
      return await stripe.confirmPayment({
        clientSecret: clientSecret,
        confirmParams: {
          confirmation_token: confirmationToken.id
        },
        redirect:'if_required'
      })
    } else {
      throw new Error('Stripe is not initialized');
    }
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
    this.paymentElements = undefined;
  }
  
}
