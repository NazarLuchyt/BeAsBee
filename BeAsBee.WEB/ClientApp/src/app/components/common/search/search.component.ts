import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {

  @Output() inputtedText: EventEmitter<string> = new EventEmitter<string>();

  searchText: string;

  constructor() {
    this.searchText = 'test1';
   }

  clear() {
    this.searchText = null;
  }
}
