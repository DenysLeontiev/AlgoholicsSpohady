import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Memory } from '../_models/memory';
import { ActivatedRoute, Router } from '@angular/router';
import { MemoryService } from '../_services/memory.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { UserInMemory } from '../_models/userInMemory';
import { AddUserToMemory } from '../_models/addUserToMemory';
import { ToastrService } from 'ngx-toastr';
import { RemoveUserFromMemory } from '../_models/removeUserFromMemory';
import { AccountService } from '../_services/account.service';
import { map, take } from 'rxjs/operators';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from '../_services/message.service';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { UserJwt } from '../_models/userJwt';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-memory-detail',
  templateUrl: './memory-detail.component.html',
  styleUrls: ['./memory-detail.component.css']
})
export class MemoryDetailComponent implements OnInit, OnDestroy {

  @ViewChild('memberTabs') memberTabs?: TabsetComponent;
  activeTab?: TabDirective;
  areMessagesLoad: boolean = false;

  memory: Memory | undefined;
  imagePath: SafeResourceUrl = "";
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  usersInMemory: UserInMemory[] = [];
  memoryId: string | null = "";

  currentUserId: string | null = "";

  messages: Message[] | undefined = [];
  messagePagination?: Pagination;
  messagPageNumber: number = 1;
  messagePageSize: number = 100;

  addUserToMemory: AddUserToMemory = {
    memoryId: '',
    userName: '',
  }

  removeUserFromMemory: RemoveUserFromMemory = {
    memoryId: '',
    userName: '',
  }

  userJwt?: UserJwt;

  constructor(public memoryService: MemoryService,
    private activatedRoute: ActivatedRoute,
    private sanitizer: DomSanitizer,
    private router: Router,
    private toastr: ToastrService,
    private accountService: AccountService,
    private messageService: MessageService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe((userJwt) => {
      if (userJwt) {
        this.userJwt = userJwt;
      }
    })
  }

  ngOnInit(): void {
    this.getCurrentUserId();
    this.getMemoryFromRoute();

    this.galleryOptions = [
      {
        width: '600px',
        height: '600px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Fade,
        preview: false
      },
    ]
  }

  ngOnDestroy(): void {
    this.memoryService.stopHubConnection();
    this.messageService.stopHubConnection();
  }

  getCurrentUserId() {
    this.accountService.currentUser$.subscribe((response) => {
      this.currentUserId = response?.id!;
    })

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
        this.addUserToMemory.memoryId = this.memoryId!;
        this.removeUserFromMemory.memoryId = this.memoryId!;
        this.galleryImages = this.getImages();
      }, error => {
        console.log(error);
      })
    }
  }

  loadUsersInMemory() {
    if (this.memoryId) {
      this.memoryService.getUsersInMemory(this.memoryId).subscribe((response) => {
        this.usersInMemory = response;
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

  // addUserMemory() {
  //   return this.memoryService.addUserToMemory(this.addUserToMemory).subscribe((response) => {
  //     this.toastr.info(`Користувача ${this.addUserToMemory.userName} додано до цього спогаду як учасника`);
  //     window.location.reload();
  //   }, error => {
  //     console.log(error);
  //     this.toastr.error("Сталася помилка.Користувача не додано");
  //   });
  // }

  addNewUserToMemory() {
    this.memoryService.addUserToMemory(this.addUserToMemory).then((error) => {
      this.toastr.success("Користувача - " + this.addUserToMemory.userName + " - додано до Спогаду");
      this.addUserToMemory.userName = '';
    }).catch((error) => {
      console.log(error);
    })
  }

  // removeUserMemory(userName: string) {
  //   this.removeUserFromMemory.userName = userName;

  //   this.memoryService.removeUserFromMemory(this.removeUserFromMemory).subscribe((response) => {
  //     this.toastr.info(`Користувача під іменем ${userName} видалено`);
  //     window.location.reload();
  //   }, error => {
  //     console.log(error);
  //     this.toastr.error(error);
  //   })
  // }

  removeUserMemory(userName: string) {
    this.removeUserFromMemory.userName = userName;

    this.memoryService.removeUserFromMemory(this.removeUserFromMemory)?.then(() => {
      this.toastr.info("Користувача " + userName + " - видалено зі Спогаду");
      console.log("User is removed");
    });
  }

  // setNewOwner(ownerId: string) {
  //   return this.memoryService.setNewOwner(this.memoryId!, ownerId).subscribe((response) => {
  //     this.toastr.show("Тепер у цього Спогада новий власник ;)");
  //     console.log(response);
  //     window.location.reload();
  //   }, error => {
  //     console.log(error);
  //   });
  // }

  setNewOwner(ownerId: string) {
    return this.memoryService.setNewOwner(this.memoryId!, ownerId).then(() => {
      console.log("New memory owner is set");
      window.location.reload();
    }).catch((error) => {
      console.log(error);
    });
  }

  // loadMessages() {
  //   if (!this.memoryId) return;
  //   this.messageService.getMessagesForMemory(this.messagPageNumber, this.messagePageSize, this.memoryId).subscribe((response) => {
  //     this.messages = response.result;
  //     this.messagePagination = response.pagination;

  //     // this.messages = response;
  //     console.log(response);
  //   });
  // }

  loadMessages() {
    if (!this.memoryId) return;
    this.messageService.getMessagesForMemory(this.memoryId).subscribe((response) => {
      this.messages = response;
      console.log(response);
    });
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Обговорення' && this.userJwt && this.memoryId) {
      // this.loadMessages();
      this.messageService.createHubConnection(this.userJwt, this.memoryId); // here we get messages via signalR
    }
    else {
      this.messageService.stopHubConnection();
    }

    if (this.activeTab.heading === 'Учасники' && this.userJwt && this.memoryId) {
      this.memoryService.startHubConnection(this.userJwt, this.memoryId); // here we get usersInMemory via signalR
      // this.loadUsersInMemory();
    }
    else {
      this.memoryService.stopHubConnection();
    }

    if (this.activeTab.heading === 'Фото') {
      this.loadUsersInMemory();
    }
  }
}
