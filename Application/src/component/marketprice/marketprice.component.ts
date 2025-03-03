import { Component } from '@angular/core';
import MarketPriceTrackingService from 'src/dataservice/MarketTrackingService';

@Component({
  selector: 'app-marketprice',
  templateUrl: './marketprice.component.html',
  styleUrls: ['./marketprice.component.css']
})
export class MarketpriceComponent {
  isModalOpen: Boolean = false;
  selectedFile: File | null = null;

  constructor(private readonly marketPriceTracking: MarketPriceTrackingService) {

  }

  openModal() {
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  uploadFile() {
    if (this.selectedFile) {
      this.marketPriceTracking.UploadFile(this.selectedFile).subscribe((res) => {
        this.closeModal();
      });
    } else {
      alert('Please select a file first.');
    }
  }
}
