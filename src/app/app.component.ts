import { Component, OnInit,ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ApiService } from './api.service';
import { DialogComponent } from './dialog/dialog.component';
import { Product } from './product';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Angular13CRUD';
  displayedColumns : string[] = ["productName","category","date", "freshness","price","comment","action"];
  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;
  datasource !: MatTableDataSource<Product>;

  constructor(
    private dialog:MatDialog,
    private products: ApiService,
    private snackBar: MatSnackBar) {
    
  }

  openDialog() {
    this.dialog.open(DialogComponent, {width:"30%"}).afterClosed().subscribe(
      val => {
        if(val==='save'){
          this.getProducts()
        }
      }
    )
  }

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {
    this.products.getProducts().subscribe(
      {
        next: (res) => {
          this.datasource = new MatTableDataSource(res);
          this.datasource.paginator = this.paginator;
          this.datasource.sort = this.sort
        },
        error: (err) => {

        }
      }
    )
  }

  filterTable(event:Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.datasource.filter = filterValue.trim().toLowerCase();
    if(this.datasource.paginator) {
      this.datasource.paginator.firstPage();
    }
  }
  editData(row : Product) {
    this.dialog.open(DialogComponent, {
      width:"30%",
      data:row
    }).afterClosed().subscribe((val)=> {
      if(val==='update'){
        this.getProducts();
      }
    })
  }

  deleteProduct(id:number) {
    this.products.deleteProduct(id).subscribe({
      next: ()=> {
        this.snackBar.open("Product deleted successfully!","", {
          duration: 2000,
          verticalPosition: 'top',
          horizontalPosition: 'end'
        })
        this.getProducts()
      },
      error: err=> console.log(err)
    })
  }
}
