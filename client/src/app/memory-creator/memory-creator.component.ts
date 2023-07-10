import { Component, OnInit } from '@angular/core';
import { MemoryService } from '../_services/memory.service';
import { MemoryForCreation } from '../_models/memoryForCreation';
import { ToastrService } from 'ngx-toastr';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Memory } from '../_models/memory';

@Component({
  selector: 'app-memory-creator',
  templateUrl: './memory-creator.component.html',
  styleUrls: ['./memory-creator.component.css']
})
export class MemoryCreatorComponent implements OnInit {

  isPopUpActive: boolean = false;
  imagePath: SafeResourceUrl = "";
  selectedFiles: File[] = [];
  
  memoryForCreation: MemoryForCreation = {
    title: '',
    description: '',
  }


  constructor(private memoryService: MemoryService, private toastr: ToastrService, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
  }

  onFileSelected(event: any) {
    const files = event.target.files;
    for (let i = 0; i < files.length; i++) {
      this.selectedFiles.push(files[i]);
    }
  }

  createMemory() {
    if (this.selectedFiles.length === 0) {
      return;
    }

    const formData = new FormData();

    formData.append('title', this.memoryForCreation?.title);
    formData.append('description', this.memoryForCreation?.description);

    for (let i = 0; i < this.selectedFiles.length; i++) {
      formData.append('files', this.selectedFiles[i], this.selectedFiles[i].name);
    }

    console.log(formData);

    this.memoryService.createMemory(formData).subscribe((response) => {
      let memory = response as Memory;
      this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + memory.memoryQrCode);
      this.isPopUpActive = true;
      this.toastr.success("Spohad створено");
    }, error => {
      console.log(error);
      this.toastr.error(error);
    })
  }

}
