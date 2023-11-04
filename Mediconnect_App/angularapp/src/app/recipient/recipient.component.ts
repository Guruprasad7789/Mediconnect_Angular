import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-recipient',
  templateUrl: './recipient.component.html',
  styleUrls: ['./recipient.component.css']
})
export class RecipientComponent {
  errorMessage = "";
  showMsg: boolean = false;
  recipientForm: FormGroup;
  isSubmitted = false;
  organs = [
    { name: 'Heart', value: 'heart' },
    { name: 'Lungs', value: 'lungs' },
    { name: 'Liver', value: 'liver' },
    { name: 'Kidneys', value: 'kidneys' },
    { name: 'Other', value: 'other' }
  ];
  timeout: any;
  constructor(private http: HttpClient, private fb: FormBuilder) {
    const organControls = this.organs.map(
      () => new FormControl('')
    );
    this.recipientForm = this.fb.group({
      organsinfos: [''],
      bloodtype: ['', Validators.required],
      age: ['', Validators.required],
      name: ['', Validators.required],
      relationship: ['', Validators.required],
      contact: ['', Validators.compose([Validators.required, Validators.min(1000000000), Validators.max(9999999999)])],
      address: ['', Validators.required],
      selectedOrgans: this.fb.array(organControls)
    });
  }
  clearinput() {
    this.recipientForm.reset();
    this.isSubmitted = false;
  }
  saveRecipient() {
    this.isSubmitted = true;
    if (this.recipientForm.valid && (this.recipientForm.get('selectedOrgans')?.value[this.organs.length - 1] && this.recipientForm.get('organsinfos')?.value.length > 0
      || this.recipientForm.get('selectedOrgans')?.value.slice(0, 4).includes(true))) {
      const recipient = JSON.parse(JSON.stringify(this.recipientForm.value as IRecipientDetails));
      recipient.contact = recipient.contact.toString();
      delete recipient.selectedOrgans;
      const organs = this.organs.filter((item, i) => this.recipientForm.get('selectedOrgans')?.value[i]).map(data => data.value).join(', ');
      recipient.organsinfo = organs + ', ' + this.recipientForm.get('organsinfos')?.value;
      this.http.post<any>('/recipient', recipient).subscribe({
        next: data => {
          if (data && data.success) {
            this.showMsg = true;
            this.errorMessage = '';
            this.timeout = setTimeout(() => this.showMsg = false, 2000);

          }
        },
        error: error => {
          this.errorMessage = error.message;
        }
      })
      this.clearinput();
      window.scrollTo(0, 0);
    } else {
      this.recipientForm.markAllAsTouched();
    }
  }

  title = 'MediConnect';
  ngOnDestroy() {
    if (this.timeout) {
      clearTimeout(this.timeout);
    }
  }
}
interface IRecipientDetails {
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

