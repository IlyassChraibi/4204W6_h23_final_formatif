import { CategorieService } from './../services/categorie.service';
import { Categorie } from 'src/app/models/categorie';
import {
  Component,
  ElementRef,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { HttpService } from '../services/http.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-categorie',
  templateUrl: './categorie.component.html',
  styleUrls: ['./categorie.component.css'],
})
export class CategorieComponent implements OnInit {
  affichage = './categorie-add.component.html';
  ajout = './my-component-footer.html';

  listCategories: Categorie[] = [];
  formAjoutCategorie: boolean = false;
  formUpdateCategorie: boolean = false;
  categorie: Categorie = new Categorie('', false);
  constructor(
    public httpService: HttpService,
    public CategorieService: CategorieService,
    private route: ActivatedRoute
  ) {}

  async ngOnInit() {
    this.listCategories = await this.CategorieService.getCategories();
  }

  toggleFormEdit() {
    this.formAjoutCategorie = false;
    this.formUpdateCategorie = true;
  }

  toggleFormAjout() {
    this.formUpdateCategorie = false;
    this.formAjoutCategorie = true;
    this.categorie.nom = '';
    this.categorie.isDisabled = false;
  }

  backToListCategorie() {
    this.formAjoutCategorie = false;
    this.formUpdateCategorie = false;
  }
}
