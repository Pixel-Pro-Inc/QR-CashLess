import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { FileUploadModule } from 'ng2-file-upload';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [],
  imports: [    
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    FileUploadModule,
    ReactiveFormsModule,
    NgxChartsModule
  ],

  exports: [
    BsDropdownModule,
    ToastrModule,
    FileUploadModule,
    ReactiveFormsModule,
    NgxChartsModule
  ]
})
export class SharedModule { }
