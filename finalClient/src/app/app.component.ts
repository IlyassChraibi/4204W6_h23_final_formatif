import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  hideNav: boolean = false;

  constructor(private router: Router) {}

  logout(): void {
    localStorage.removeItem('token');
  }
}
