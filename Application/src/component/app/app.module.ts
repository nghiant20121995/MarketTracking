import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { SidebarComponent } from '../../component/sidebar/sidebar.component';
import { DashboardComponent } from '../../component/dashboard/dashboard.component';
import { RouterModule, Routes } from '@angular/router';
import { MarketpriceComponent } from '../marketprice/marketprice.component';
import { ToastComponent } from '../toast/toast.component';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'market-price', component: MarketpriceComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent,
    DashboardComponent,
    MarketpriceComponent,
    ToastComponent
  ],
  imports: [
    BrowserModule,
    NoopAnimationsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RouterModule.forRoot(routes)
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports: [RouterModule]
})
export class AppModule { }
