import { Component, Output,EventEmitter, OnInit } from '@angular/core';

import { navnarData } from './nav-data';

interface SideNavToggle{
  screenWidth:number;
  collapsed:boolean;
}

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit  {
  ngOnInit(): void {

  }

  @Output() onToggleSideNav: EventEmitter<SideNavToggle>= new EventEmitter();
  collapsed=false;
  screenWidth=0;
  navData=navnarData;

  closeSidenav(){
    this.collapsed=false;
    this.onToggleSideNav.emit({collapsed:this.collapsed,screenWidth: this.screenWidth});
  }
  toggleCollapse(){
    this.collapsed= !this.collapsed;
    this.onToggleSideNav.emit({collapsed:this.collapsed,screenWidth: this.screenWidth});
  }

}
