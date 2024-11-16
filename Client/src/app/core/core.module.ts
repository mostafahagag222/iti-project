import { SharedModule } from './../shared/shared.module';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { FooterComponent } from './footer/footer.component';


@NgModule({
  declarations: [NavBarComponent, SectionHeaderComponent, FooterComponent],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    BreadcrumbModule
  ],
  exports: [
    NavBarComponent,
    SectionHeaderComponent,
    FooterComponent
  ]
})
export class CoreModule { }
