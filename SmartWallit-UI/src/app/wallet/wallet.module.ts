import { NgModule } from '@angular/core';
import { WalletComponent } from './wallet.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { WalletRoutingModule } from './wallet-routing.module';



@NgModule({
  declarations: [
    WalletComponent
  ],
  imports: [
    SharedModule,
    WalletRoutingModule
  ],
})
export class WalletModule { }
