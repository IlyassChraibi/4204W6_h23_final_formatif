import { Picture } from './picture';

export class Produit {
  constructor(
    public nom: string,
    public prix: number,
    public id: number = 0
  ) {}
}
