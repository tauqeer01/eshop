import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
loading = false;
busyRequestsCount = 0;
busy() {
  this.busyRequestsCount++;
  this.loading = true;
}

idle() {
  this.busyRequestsCount--;
  if (this.busyRequestsCount <= 0) {
    this.busyRequestsCount = 0;
    this.loading = false;
  }
}
 
}
