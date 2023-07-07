import { AfterViewChecked, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';
import { Pagination } from '../_models/pagination';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { CreateMessage } from '../_models/createMessage';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit, AfterViewChecked {

  @ViewChild('messagesContainer') messagesContainer!: ElementRef; // inputForm
  // @ViewChild('inputForm') inputForm!: NgForm; // inputForm

  @Input() messages: Message[] | undefined = [];
  @Input() pagination?: Pagination;

  @Input() memoryId: string | null = "";
  @Input() currentUserId: string | null = "";

  createMessage: CreateMessage = {} as CreateMessage;

  constructor(public messageService: MessageService,
    private activatedRoute: ActivatedRoute) { }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  ngOnInit(): void {
    this.createMessage.memoryId = this.memoryId!;
  }

  sendMessage() {
    if (this.createMessage.messageText) {
      this.messageService.createMessage(this.createMessage).then(() => {
        this.createMessage.messageText = ''; // reset text
      });
    }
  }

  // sendMessage() {
  //   if (this.createMessage.messageText) {
  //     this.messageService.createMessage(this.createMessage).subscribe((response) => {
  //       console.log(response);
  //     }, error => {
  //       console.log(error);
  //     })
  //   }
  // }

  scrollToBottom() {
    const container = this.messagesContainer.nativeElement;
    container.scrollTop = container.scrollHeight;
  }

  // pageChanged(event: any) {
  //   if (this.pageNumber !== event.page) {
  //     this.pageNumber = event.page;
  //     this.loadMessages();
  //   }
  // }
}
