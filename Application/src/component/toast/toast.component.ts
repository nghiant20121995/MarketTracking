import { Component } from '@angular/core';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
})
export class ToastComponent {
  isVisible = false;
  message = '';

  showToast(msg: string, duration = 3000) {
    this.message = msg;
    this.isVisible = true;

    setTimeout(() => {
      this.isVisible = false;
    }, duration);
  }
}
