import { Component, OnInit } from '@angular/core';
import MarketPriceTrackingService from 'src/dataservice/MarketTrackingService';
import { ImportedFile } from 'src/viewmodel/MarketFile';
import { MarketPriceTracking, MarketPriceTrackingRequest } from 'src/viewmodel/MarketPrice';

@Component({
  selector: 'app-marketprice',
  templateUrl: './marketprice.component.html',
  styleUrls: ['./marketprice.component.css']
})
export class MarketpriceComponent implements OnInit {
  isModalOpen: Boolean = false;
  visibleProgression: Boolean = false;
  importedFileProgression: Array<ImportedFile> = [];
  intervalProcess: any;
  selectedFile: File | null = null;
  startDate: String;
  endDate: String;
  priceTrackings: Array<MarketPriceTracking> = [];

  constructor(private readonly marketPriceTracking: MarketPriceTrackingService) {

  }
  ngOnInit(): void {
    var newFilter = new MarketPriceTrackingRequest(0, 30);
    this.marketPriceTracking.GetMarketPrice(newFilter).subscribe(res => {
      this.priceTrackings = res.marketPriceTrackings;
    });
  }

  onSearch(): void {
    var newFilter = new MarketPriceTrackingRequest(0, 30);
    this.marketPriceTracking.GetMarketPrice(newFilter).subscribe(res => {
      this.priceTrackings = res.marketPriceTrackings;
    });
  }

  onEndDateChange(event: Event) {

  }

  openModal() {
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
    this.clearProcess();
  }

  clearProcess() {
    if (this.intervalProcess) {
      clearInterval(this.intervalProcess);
    }
  }

  startProcess() {
    this.intervalProcess = setInterval(() => {
      for (let i = 0; i < this.importedFileProgression.length; i++) {
        const element = this.importedFileProgression[i];
        if (!element.isProcessed && !element.isDeleted) {
          this.marketPriceTracking.GetImportedFile(element.id).subscribe(res => {
            if (!element.isProcessed) {
              this.importedFileProgression[i] = res;
              this.onSearch();
            }
          });
        }
      }
    }, 3000);
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }
  
  onFilterTransaction() {

  }

  uploadFile() {
    if (this.selectedFile) {
      this.marketPriceTracking.UploadFile(this.selectedFile).subscribe((res) => {
        this.closeModal();
        this.clearProcess();
        this.importedFileProgression.unshift(res);
        this.visibleProgression = true;
        this.startProcess();
      });
    } else {
      alert('Please select a file first.');
    }
  }
}
