import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot',
  templateUrl: './forgot.component.html',
  styleUrls: ['./forgot.component.css']
})
export class ForgotComponent {
  title = 'MediConnect';
  constructor(
    private router: Router
  ) { }

  ngOnInit(): void { }
}
