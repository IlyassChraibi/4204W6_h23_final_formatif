import { Component, OnInit } from '@angular/core';
import { RegisterDTO } from 'src/app/models/registerDTO';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerDTO : RegisterDTO = new RegisterDTO("", "", "", "");

  constructor(public httpService : HttpService) { }

  ngOnInit() {
  }

}
