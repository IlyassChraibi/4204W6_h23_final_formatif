import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginDTO } from 'src/app/models/loginDTO';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginDTO: LoginDTO = new LoginDTO('', '');
  isLoggedIn = false;

  constructor(public httpService: HttpService, public router: Router) {}

  ngOnInit() {}

  async login(loginDTO: LoginDTO) {
    if (await this.httpService.login(loginDTO)) {
      this.router.navigate(['produit']);
    }
  }
}
