import { HttpClient } from '@angular/common/http';
import { ElementRef, Injectable, ViewChild } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Categorie } from 'src/app/models/categorie';

@Injectable({
  providedIn: 'root',
})
export class CategorieService {
  constructor(public http: HttpClient) {}

  // Obtenir tous les categories
  async getCategories(): Promise<Categorie[]> {
    let response = await lastValueFrom(
      this.http.get<Categorie[]>(
        'https://localhost:5001/api/Categories/GetCategorie'
      )
    );
    console.log(response);
    return response;
  }
}
