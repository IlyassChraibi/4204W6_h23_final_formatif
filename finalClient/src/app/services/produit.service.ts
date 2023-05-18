import { HttpClient } from '@angular/common/http';
import { ElementRef, Injectable, ViewChild } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Produit } from 'src/app/models/produit';

@Injectable({
  providedIn: 'root',
})
export class ProduitService {
  constructor(public http: HttpClient) {}

 async rechercheProduit(categorie: string): Promise<Produit[]> {
  let response = await lastValueFrom(
    this.http.get<Produit[]>("https://localhost:5001/api/Produits/GetProduitsByCategorie/"+ categorie)
  );
  console.log(response);
  return response;
}


  // Obtenir tous les produits
  async getProduits(): Promise<Produit[]> {
    let response = await lastValueFrom(
      this.http.get<Produit[]>('https://localhost:5001/api/Produits/GetProduit')
    );
    console.log(response);
    return response;
  }

  // Créer un nouveau produit
  // todo vider le code de postProduit
  async postProduit(nom: string, prix: number, categorie: string,pictureInput?: ElementRef): Promise<void> {
    let newProduit = { nom : nom, prix : prix, nomCategorie : categorie };
    let x : Produit = await lastValueFrom(this.http.post<any>("https://localhost:5001/api/Produits/PostProduit", newProduit));
    console.log(x);
    await this.postImageProduit(x.id, pictureInput);
  }

  async postImageProduit(id : number, pictureInput?: ElementRef){
    if (pictureInput == undefined) {
      return;
    }
    let file = pictureInput.nativeElement.files[0];
    if (file == null) {
      return;
    }
    const formData = new FormData();
    formData.append('imageProduit', file, file.name);
    let x = await lastValueFrom(this.http.post<any>("https://localhost:5001/api/Produits/PostImageProduit/" + id, formData));
    console.log(x);
  }

  // Supprimer un produit
  async deleteProduit(id: number): Promise<void> {
    await lastValueFrom(this.http.delete<Produit>('https://localhost:5001/api/Produits/DeleteProduit/' + id));
  }
}
