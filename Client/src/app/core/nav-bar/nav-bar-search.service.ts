import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NavBarSearchService {
  searchSource = new BehaviorSubject<string>(null);
  search$ = this.searchSource.asObservable();
  constructor() { }
}
