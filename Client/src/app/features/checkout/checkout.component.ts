import { Component, inject, OnInit } from '@angular/core';
import { OrderSummaryComponent } from "../../shared/components/order-summary/order-summary.component";
import {MatStepperModule} from "@angular/material/stepper";
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { StripeService } from '../../core/services/stripe.service';
import { StripeAddressElement } from '@stripe/stripe-js';
import { SnackbarService } from '../../core/services/snackbar.service';
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    MatButton,
    RouterLink

  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit {
  
  private stripeService = inject(StripeService)
  addressElement?: StripeAddressElement;
  private snackService = inject(SnackbarService)
  async ngOnInit() {
   try {
     this.addressElement = await this.stripeService.createAddressElements();
     this.addressElement.mount('#address-element');
   } catch (error : any) {
     this.snackService.error(error.message)
   }
   
  }
}
