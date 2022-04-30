import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ApiService } from '../api.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  MatSnackBar,  
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition, 
} from '@angular/material/snack-bar';
import { MatDialogRef } from '@angular/material/dialog';
import { Product } from '../product';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})
export class DialogComponent implements OnInit {
  btnText = "Save"
  freshnessList = ["Brand New","Second Hand", "Refurbished"];
  productForm !: FormGroup;
  horizontalPosition: MatSnackBarHorizontalPosition = 'end';
  verticalPostion: MatSnackBarVerticalPosition = 'top';
  constructor(
    private builder:FormBuilder, 
    private api:ApiService,
    private snackBar: MatSnackBar,
    private dialogRef:MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public ediData:Product) { }

  ngOnInit(): void {
    this.productForm = this.builder.group({
      productName:['', Validators.required],
      category:['', Validators.required],
      date:['', Validators.required],
      freshness:['', Validators.required],
      price:['', Validators.required],
      comment:['', Validators.required],
    });
    if(this.ediData){
      this.btnText = "Update"
      this.productForm.patchValue(this.ediData)
    }
    
  }

  addProduct() {
    if(!this.ediData){
    if(this.productForm.valid){
        this.api.addProduct(this.productForm.value).subscribe({
          next :(
            () => {
              this.snackBar.open("Product Added successfully!","undo", {
                horizontalPosition:this.horizontalPosition,
                verticalPosition: this.verticalPostion,
                duration:2000
              }),
              this.dialogRef.close('save');
            }
          ),
          error: (err => console.log(err))
        }
        )
        console.log(this.productForm.value)
      }
  }
  else {
    this.updateProduct(this.productForm.value, <number>this.ediData.id);
  }
  }

  updateProduct (data:Product, id:number) {
    this.api.updateProduct(data,id).subscribe(
      {
        next: ()=> {
          this.snackBar.open("Updated Successfully","", {
            verticalPosition: this.verticalPostion,
            horizontalPosition: this.horizontalPosition,
            duration:2000
          }
          ),
          this.productForm.reset();
          this.dialogRef.close('update');
        }
      }
    )
  }


}
