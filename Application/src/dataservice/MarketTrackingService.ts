import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/enviroment";
import DashboardFigure from "src/viewmodel/DashboardFigure.viewmodel";
import { ImportedFile } from "src/viewmodel/MarketFile";

@Injectable({
    providedIn: 'root'
})

export default class MarketPriceTrackingService {
    constructor(private httpClient: HttpClient) {

    }

    public GetDashboardFigure(): Observable<DashboardFigure> {
        return this.httpClient.get<DashboardFigure>(`${environment.apiUrl}/market-tracking`);
    }

    public UploadFile(file: File): Observable<ImportedFile> {
        var newFormData = new FormData();
        newFormData.append('file', file);
        return this.httpClient.post<ImportedFile>(`${environment.apiUrl}/file/import/marketprice`, newFormData);
    }
}