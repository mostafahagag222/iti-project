import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
import { FormGroup,FormBuilder,Validator, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-brand-dialog',
  templateUrl: './brand-dialog.component.html',
  styleUrls: ['./brand-dialog.component.scss']
})
export class BrandDialogComponent implements OnInit {

  actionBtn :string="Save";
  barndForm !: FormGroup;

  constructor(private formBuilder:FormBuilder,
    private dialogRef:MatDialogRef<BrandDialogComponent>
    ,   @Inject(MAT_DIALOG_DATA) public data: any
) { }

  ngOnInit(): void {
    this.barndForm=this.formBuilder.group({
      Name:['',Validators.required],
      Origin:['',Validators.required],
    });

    if(this.data.brand){
      this.barndForm.get("Name").setValue(this.data.brand.name);
      this.barndForm.get("Origin").setValue(this.data.brand.origin);

    }

    if(this.data.action=="edit"){
      this.actionBtn="Update";
    }


  }

  makeAction(){


    if(this.data.action=="add"){
      this.dialogRef.close({event:'add',data:{Name:this.barndForm.get("Name").value,Origin:this.barndForm.get("Origin").value}});
    }
    if(this.data.action=="edit"){
      this.dialogRef.close({event:'edit',data:{Name:this.barndForm.get("Name").value,Origin:this.barndForm.get("Origin").value}});

    }
  }

}
