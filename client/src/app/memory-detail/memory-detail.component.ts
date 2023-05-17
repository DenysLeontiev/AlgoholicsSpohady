import { Component, OnInit } from '@angular/core';
import { Memory } from '../_models/memory';
import { ActivatedRoute, Router } from '@angular/router';
import { MemoryService } from '../_services/memory.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { UserInMemory } from '../_models/userInMemory';

@Component({
  selector: 'app-memory-detail',
  templateUrl: './memory-detail.component.html',
  styleUrls: ['./memory-detail.component.css']
})
export class MemoryDetailComponent implements OnInit {

  memory: Memory | undefined;
  imagePath: SafeResourceUrl = "";
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  usersInMemory: UserInMemory[] = [];
  memoryId: string | null = "";

  constructor(private memoryService: MemoryService, 
              private activatedRoute: ActivatedRoute, 
              private sanitizer: DomSanitizer,
              private router: Router) { }

  ngOnInit(): void {
    this.getMemoryFromRoute();
    this.loadUsersInMemory();


    this.galleryOptions = [
      {
        width: '600px',
        height: '600px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Zoom,
        preview: false
      },
    ]
  }

  getImages() {
    if (!this.memory) {
      return [];
    }

    const imageUrls = [];
    for (const photo of this.memory.photos) {
      imageUrls.push({
        small: photo.photoUrl,
        medium: photo.photoUrl,
        large: photo.photoUrl,
      })
    }
    return imageUrls;
  }

  getMemoryFromRoute() {
    this.memoryId = this.activatedRoute.snapshot.paramMap.get('id');
    if (this.memoryId) {
      this.memoryService.getMemory(this.memoryId).subscribe((response) => {
        this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + response.memoryQrCode);
        this.memory = response;
        this.galleryImages = this.getImages();
      }, error => {
        console.log(error);
      })
    }
  }

  loadUsersInMemory() {
    if(this.memoryId) {
      this.memoryService.getUsersInMemory(this.memoryId).subscribe((response) => {
        this.usersInMemory = response;
        console.log(response);
      }, error => {
        console.log(error);
      })
    }
  }

  getQrCode() {
    if (this.memory) {
      this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + this.memory.memoryQrCode);
    }
  }

  redirectToMyMemories() {
    this.router.navigateByUrl('/memories');
  }
}
