import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
import { FormGroup,FormBuilder,Validator, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-type-dialog',
  templateUrl: './type-dialog.component.html',
  styleUrls: ['./type-dialog.component.scss']
})
export class TypeDialogComponent implements OnInit {
  actionBtn :string="Save";
  TypeForm !: FormGroup;

  constructor(private formBuilder:FormBuilder,
    private dialogRef:MatDialogRef<TypeDialogComponent>
    ,   @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.TypeForm=this.formBuilder.group({
      Name:['',Validators.required],
    });

    if(this.data.Type){
      this.TypeForm.get("Name").setValue(this.data.Type.name);

    }
    if(this.data.action=="edit"){
      this.actionBtn="Update";
    }


  }

  makeAction(){


    if(this.data.action=="add"){
      this.dialogRef.close({event:'add',data:{Name:this.TypeForm.get("Name").value}});
    }
    if(this.data.action=="edit"){
      this.dialogRef.close({event:'edit',data:{Name:this.TypeForm.get("Name").value}});

    }
  }




}
