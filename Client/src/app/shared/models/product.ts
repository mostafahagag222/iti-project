export interface IProduct {
  id: number;
  name: string;
  description: string;
  price: number;
  unitsInStock: number;
  unitsSold: number;
  manufactureDate: Date;
  height: number;
  width: number;
  length: number;
  weight: number;
  color: string;
  productType: string;
  productBrand: string;
  pictures: string[];
}
