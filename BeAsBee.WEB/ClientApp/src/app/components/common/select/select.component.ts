import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SelectItem } from 'src/app/_models/select-item.model';
import { UserPage } from 'src/app/_models/user-page.model';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss']
})
export class SelectComponent implements OnInit {

  @Input() items: SelectItem[];
  // set items(value: any) {
  //   if (value && value[0].checked == null) {
  //     this.itemsArray = value.map((item) => ({
  //       item: item,
  //       checked: false
  //     }));
  //   }
  // }

  @Output() itemsChange: EventEmitter<SelectItem[]> = new EventEmitter<SelectItem[]>();
  //public itemsArray: SelectItem[];

  constructor() { }

  ngOnInit() {
  }

  toggleItem() {
    this.itemsChange.emit(this.items);
    //  this.setToggeledTags.emit(new SelectedTagsResultModel(this.existsTags, this.tagType));
  }

  // setData(items: Offer[]) {
  //   this.allTags = items;
  //   if (this.existsTags) {
  //     this.existsTags.forEach(selectedItem => {
  //       const index = this.allTags.findIndex(item => parseInt(item.id, 10) === selectedItem.id);
  //       if (index >= 0) {
  //         this.allTags[index].checked = true;
  //       }
  //     });
  //   }

}

