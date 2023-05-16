import { HttpClient } from '@angular/common/http';
import { ElementRef, Injectable } from '@angular/core';
import { lastValueFrom, throwIfEmpty } from 'rxjs';
import { LoginDTO } from 'src/app/models/loginDTO';
import { RegisterDTO } from 'src/app/models/registerDTO';
import { Categorie } from 'src/app/models/categorie';
import { Produit } from 'src/app/models/produit';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  // Liste des produits
  produits: Produit[] = [];

  // Liste des Categories
  categorie: Categorie[] = [];

  constructor(public http: HttpClient) {}

  // Inscription
  async register(registerDTO: RegisterDTO): Promise<void> {
    let request = await lastValueFrom(
      this.http.post<any>(
        'https://localhost:5001/api/users/register',
        registerDTO
      )
    );
    console.log(request);
  }

  // Connexion
  async login(loginDTO: LoginDTO): Promise<boolean> {
    let request = await lastValueFrom(
      this.http.post<any>('https://localhost:5001/api/users/login', loginDTO)
    );
    if (request.message === 'Connexion réussie') {
      console.log(request);
      localStorage.setItem('token', request.token);
      return true;
    }
    return false;
  }
}
