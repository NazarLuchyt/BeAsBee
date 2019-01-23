import { Component, OnInit, Input } from '@angular/core';
import { UserPage } from 'src/app/_models/user-page.model';
import { defaultImg } from 'src/app/_constants/defaults.const';


@Component({
  selector: 'app-user-preview',
  templateUrl: './user-preview.component.html',
  styleUrls: ['./user-preview.component.scss']
})
export class UserPreviewComponent implements OnInit {
  defaultImg = defaultImg;
  findedUsers: UserPage[];

  @Input()
  set setfindedUsers(users: UserPage[]) {
    this.findedUsers = users;
  }
  get setfindedUsers() { return this.findedUsers; }

  constructor() { }

  ngOnInit() {
  }

  createNewChat(id: string) {
    window.confirm(id);
  }
}
