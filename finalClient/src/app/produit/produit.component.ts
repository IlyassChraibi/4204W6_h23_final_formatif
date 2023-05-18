import { CategorieService } from './../services/categorie.service';
import { Categorie } from 'src/app/models/categorie';
import { ProduitService } from './../services/produit.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpService } from 'src/app/services/http.service';
import { Produit } from 'src/app/models/produit';

@Component({
  selector: 'app-produit',
  templateUrl: './produit.component.html',
  styleUrls: ['./produit.component.css'],
})
export class ProduitComponent implements OnInit {
  @ViewChild('produitImageInput', { static: false }) pictureInput?: ElementRef;
  @ViewChild("updateProduitImageInput", {static : false}) pictureInput2 ?: ElementRef;
  public listProduits: Produit[] = [];
  public listCategories: Categorie[] = [];
  public produit: Produit = new Produit('', 0, 1);
  public categorieSelectionnee: number = 0;
  public formAjoutProduit: boolean = false;
  public formUpdateProduit: boolean = false;

  inputProduitNom : string = "";
  inputProduitPrix : number = 0;
  inputProduitCategorie : string = "";
  selectedProduitId : number = 0;

  constructor(
    public httpService: HttpService,
    public ProduitService: ProduitService,
    public CategorieService: CategorieService,
    private route: ActivatedRoute
  ) {}

  async ngOnInit() {
    this.listProduits = await this.ProduitService.getProduits();
    this.listCategories = await this.CategorieService.getCategories();
  }
  // todo vider la méthode createProduits
  async createProduits(nom: string, prix: number, categorie: string) {
    await this.ProduitService.postProduit(nom, prix, categorie, this.pictureInput);
    this.listProduits = await this.ProduitService.getProduits();
    this.backToListProduit();
  }

  async rechercheParCategorie() {
    this.listProduits = await this.ProduitService.rechercheProduit(this.inputProduitCategorie);
  }

  
  

  async deleteProduit(id: number) {
    await this.ProduitService.deleteProduit(id);
    this.listProduits = await this.ProduitService.getProduits();
  }

  async getProduitToUpdate(id: number) {
    this.toggleFormEdit();
    this.selectedProduitId = id;
  }
  async updateProduitImage() {
    await this.ProduitService.postImageProduit(this.selectedProduitId, this.pictureInput2);
    this.listProduits = await this.ProduitService.getProduits();
    this.backToListProduit();
  }

  toggleFormEdit() {
    this.formAjoutProduit = false;
    this.formUpdateProduit = true;
  }

  toggleFormAjout() {
    this.formUpdateProduit = false;
    this.formAjoutProduit = true;
    this.produit.nom = '';
    this.produit.prix = 0;
  }

  backToListProduit() {
    this.formAjoutProduit = false;
    this.formUpdateProduit = false;
  }
}
