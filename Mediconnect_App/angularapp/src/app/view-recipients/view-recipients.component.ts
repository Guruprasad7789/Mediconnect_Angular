import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-view-recipients',
  templateUrl: './view-recipients.component.html',
  styleUrls: ['./view-recipients.component.css']
})
export class ViewRecipientsComponent {
  public recdetails?: RecipientDetails[];

  constructor(http: HttpClient) {
    http.get<RecipientDetails[]>('/recipient').subscribe(result => {
      this.recdetails = result;
    }, error => console.error(error));
  }

  title = 'angularapp';
}
interface RecipientDetails {
  recipientid: string;
  organsinfo: string;
  bloodtype: string;
  age: string;
  name: string;
  relationship: string;
  contact: string;
  address: string;
  createddate: string;
}
