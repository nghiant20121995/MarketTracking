import { Component } from '@angular/core';
import MarketPriceTrackingService from 'src/dataservice/MarketTrackingService';
import { ImportedFile } from 'src/viewmodel/MarketFile';

@Component({
  selector: 'app-marketprice',
  templateUrl: './marketprice.component.html',
  styleUrls: ['./marketprice.component.css']
})
export class MarketpriceComponent {
  isModalOpen: Boolean = false;
  visibleProgression: Boolean = false;
  importedFileProgression: Array<ImportedFile> = [];
  intervalProcess: any;
  selectedFile: File | null = null;

  constructor(private readonly marketPriceTracking: MarketPriceTrackingService) {

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
            this.importedFileProgression[i] = res;
          });
        }
      }
    }, 3000);
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
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
