import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-donor',
  templateUrl: './donor.component.html',
  styleUrls: ['./donor.component.css']
})
export class DonorComponent {
  errorMessage = "";
  showMsg: boolean = false;
  isSubmitted = false;
  organs = [
    { name: 'Heart', value: 'heart', checked: false },
    { name: 'Lungs', value: 'lungs', checked: false },
    { name: 'Liver', value: 'liver', checked: false },
    { name: 'Kidneys', value: 'kidneys', checked: false },
    { name: 'Other', value: 'other', checked: false }
  ];
  meds = [
    { name: 'HIV/AIDS', value: 'hiv/aids', checked: false },
    { name: 'Hepatitis B or C', value: 'hepatitisB/C', checked: false },
    { name: 'Cancer', value: 'cancer', checked: false },
    { name: 'Diabetes', value: 'diabetes', checked: false },
    { name: 'Other', value: 'medicalOther', checked: false }
  ];
  donorForm: FormGroup;
  constructor(
    private http: HttpClient,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.donorForm = this.fb.group({
      organinfos: [''],
      medinfos: [''],
      name: ['', Validators.required],
      relationship: ['', Validators.required],
      contact: ['', Validators.compose([Validators.required, Validators.min(1000000000), Validators.max(9999999999)])],
      address: ['', Validators.required],
      sign: ['', Validators.required],
      donationConsent: ['', Validators.required],
    });
  }

  clearinput() {
    this.donorForm.reset();
    this.isSubmitted = false;
    this.organs.forEach(item => item.checked = false);
    this.meds.forEach(item => item.checked = false);
  }

  saveDonor() {
    this.isSubmitted = true;
    if (this.donorForm.valid
      && this.checkOrgansSelected() && !this.checkOtherOrgansSelectedButNoContent()
      && this.checkMedsSelected() && !this.checkOtherMedsSelectedButNoContent()
    ) {
      const donData = this.donorForm.value as IDonorDetails;
      donData.contact = donData.contact.toString();
      const organsData = this.organs.filter((item, i) => this.organs[i].checked).map(data => data.value).join(', ');
      donData.organs = organsData + ', ' + this.donorForm.get('organinfos')?.value;
      const medsData = this.meds.filter((item, i) => this.meds[i].checked).map(data => data.value).join(', ');
      donData.medinfo = medsData + ', ' + this.donorForm.get('medinfos')?.value;
      this.http.post<any>('/donor', donData).subscribe({
        next: data => {
          this.showMsg = true;
          localStorage.setItem('donorDetails', JSON.stringify(donData));
          this.router.navigateByUrl('donation-card');
          this.clearinput();
        },
        error: error => {
          this.errorMessage = error.message;
        }
      });

      window.scrollTo(0, 0);
    } else {
      this.donorForm.markAllAsTouched();
    }
  }

  title = 'MediConnect';
  checkMedsSelected() {
    return !!this.meds.find(r => r.checked);
  }
  checkOrgansSelected() {
    return !!this.organs.find(r => r.checked);
  }
  checkOtherOrgansSelectedButNoContent() {
    return !!this.organs[this.organs.length - 1].checked && this.donorForm.get('organinfos')?.value.length === 0;
  }
  checkOtherMedsSelectedButNoContent() {
    return !!this.meds[this.meds.length - 1].checked && this.donorForm.get('medinfos')?.value.length === 0;
  }
}
interface IDonorDetails {
  donorid: string;
  organs: string;
  medinfo: string;
  name: string;
  relationship: string;
  contact: string;
  address: string;
  sign: string;
  createddate: string;
}

