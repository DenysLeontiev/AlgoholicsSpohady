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
export class MessagesComponent implements OnInit, AfterViewChecked  {

  @ViewChild('messagesContainer') messagesContainer!: ElementRef; // inputForm
  // @ViewChild('inputForm') inputForm!: NgForm; // inputForm

  @Input() memoryId: string | null = "";
  @Input() currentUserId: string | null = "";

  createMessage: CreateMessage = {} as CreateMessage;
  
  messages: Message[] | undefined = [];
  pagination?: Pagination;
  pageNumber: number = 1;
  pageSize: number = 100;

  constructor(private messageService: MessageService, private activatedRoute: ActivatedRoute,
            private accountService: AccountService) { }

  ngAfterViewChecked(): void {
    this.scrollToBottom();

  }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    if (!this.memoryId) return;
    this.messageService.getMessagesForMemory(this.pageNumber, this.pageSize, this.memoryId).subscribe((response) => {
      this.messages = response.result;
      this.pagination = response.pagination;
      this.createMessage.memoryId = this.memoryId!;
    });

  }

  sendMessage(){
    if(this.createMessage.messageText) {
      this.messageService.createMessage(this.createMessage).subscribe((response) => {
      }, error => {
        console.log(error);
      })
    }
  }

  scrollToBottom() {
    const container = this.messagesContainer.nativeElement;
    container.scrollTop = container.scrollHeight;
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }
}
