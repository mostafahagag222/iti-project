import { Component, OnInit } from '@angular/core';
interface SideNavToggle{
  screenWidth:number;
  collapsed:boolean;
}



@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.scss']
})
export class AdminHomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  isSideNavCollapsed=false;
  screenWidth=0;

  onToggleSideNav(data: SideNavToggle){
    this.screenWidth=data.screenWidth;
    this.isSideNavCollapsed=data.collapsed;

  }
}
