import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/enviroment";
import DashboardFigure from "src/viewmodel/DashboardFigure.viewmodel";
import { ImportedFile } from "src/viewmodel/MarketFile";
import { MarketPrice, MarketPriceTrackingRequest } from "src/viewmodel/MarketPrice";

@Injectable({
    providedIn: 'root'
})

export default class MarketPriceTrackingService {
    constructor(private httpClient: HttpClient) {

    }

    public GetMaxPrice(): Observable<number> {
        return this.httpClient.get<number>(`${environment.apiUrl}/dashboard/maxprice`);
    }

    public GetMinPrice(): Observable<number> {
        return this.httpClient.get<number>(`${environment.apiUrl}/dashboard/minprice`);
    }

    public GetAveragePrice(): Observable<number> {
        return this.httpClient.get<number>(`${environment.apiUrl}/dashboard/averageprice`);
    }

    public UploadFile(file: File): Observable<ImportedFile> {
        var newFormData = new FormData();
        newFormData.append('file', file);

        return this.httpClient.post<ImportedFile>(`${environment.apiUrl}/file/importmarketprice`, newFormData);
    }

    public GetImportedFile(id: string): Observable<ImportedFile> {
        return this.httpClient.get<ImportedFile>(`${environment.apiUrl}/file?Id=${id}`);
    }

    public GetMarketPrice(marketPriceTrackingRequest: MarketPriceTrackingRequest): Observable<MarketPrice> {
        return this.httpClient.get<MarketPrice>(`${environment.apiUrl}/marketprice?startDate=${marketPriceTrackingRequest.startDate?.toISOString() ?? ""}&endDate=${marketPriceTrackingRequest.endDate?.toISOString() ?? ""}&pageSize=${marketPriceTrackingRequest.pageSize}&pageNumber=${marketPriceTrackingRequest.pageNumber}`);
    }
}