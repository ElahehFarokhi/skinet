import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HeaderComponent } from './layout/header/header.component';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
baseUrl="https://localhost:5001/api/";
  title = 'skinet';
constructor(private http: HttpClient){

}
products: Product[]=[];

ngOnInit(): void{
  this.http.get<Pagination<Product>>(this.baseUrl + 'products')
  .subscribe({
    next: response=>{
      this.products = response.data
    },
    error: error => console.log('errod ===>',error),
    complete: () => console.log('complete')
  })
}


}
