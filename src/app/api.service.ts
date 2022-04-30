import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from './product';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  url = "http://localhost:3000/product-list/";
  constructor(private http:HttpClient) { }

  addProduct(data:Product):Observable<Product> {
    return this.http.post<Product>(this.url,data);
  }

  getProducts() :Observable<Product[]> {
    return this.http.get<Product[]>(this.url);
  }

  updateProduct(data:Product, id:number) :Observable<void> {
    return this.http.put<void>(this.url+id,data);
  }
  deleteProduct(id:number):Observable<void> {
    return this.http.delete<void>(this.url+id);
  }
}
