import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { DeliveryMethod } from '../../shared/model/deliveryMethod';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl
  private http = inject(HttpClient)
  deliveryMethods: DeliveryMethod[] = [];
  
 getDeliveryMethods() {
    return this.http.get<DeliveryMethod[]>(this.baseUrl + 'payment/delivery-methods').pipe(
      map(methods => {
        this.deliveryMethods = methods.sort((a, b) => b.price - a.price);
        return methods;
     })
    );
  }


}
