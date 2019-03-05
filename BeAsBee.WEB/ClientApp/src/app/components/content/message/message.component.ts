import { Component, OnInit, Input } from '@angular/core';
import { Message } from 'src/app/_models/message.model';
import { defaultImg } from 'src/app/_constants/defaults.const';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {

  defaultImg = defaultImg;
  currentUserId: string;

  @Input() message: Message;
  constructor() {
    this.currentUserId = localStorage.getItem('currentUserGuid');
  }

  ngOnInit() {
  }

}
