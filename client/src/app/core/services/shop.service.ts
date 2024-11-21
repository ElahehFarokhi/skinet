import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { ShopParams } from '../../shared/models/shopParams';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  types:string[]=[]
  brands:string[]=[]

  getProduct(id: number) {
    return this.http.get<Product>(
      this.baseUrl + `products/${id}`
    );
  }

  getProducts(shopParams: ShopParams) {
    let params= new HttpParams();
    if (shopParams.brands.length > 0) {
      params = params.append('brands', shopParams.brands.join(','));
    }
    if (shopParams.types.length > 0) {
      params = params.append('types', shopParams.types.join(','));
    }
    if (shopParams.search) {
      params = params.append('search',shopParams.search);
    }

    params = params.append('sort', shopParams.sort);

    params = params.append('pageSize',shopParams.pageSize);
    params = params.append('pageIndex',shopParams.pageNumber);

    return this.http.get<Pagination<Product>>(
      this.baseUrl + 'products',{params}
    );
  }

  getTypes(){

    if (this.types.length > 0) return;

    return this.http.get<string[]>(
      this.baseUrl + 'products/types'
    ).subscribe(response=>{
      this.types = response;
    });
  }

  getBrands(){

    if (this.brands.length > 0) return;

    return this.http.get<string[]>(
      this.baseUrl + 'products/brands'
    ).subscribe(response=>{
      this.brands = response;
    });
  }

}
