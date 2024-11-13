import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl="https://localhost:5001/api/";
private http= inject(HttpClient)

  getProducts(){
    return this.http.get<Pagination<Product>>(this.baseUrl + 'products?pagesize=20&pageindex=1');
  }
}
