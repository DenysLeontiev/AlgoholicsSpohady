import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-qr-code-pop-up',
  templateUrl: './qr-code-pop-up.component.html',
  styleUrls: ['./qr-code-pop-up.component.css']
})
export class QrCodePopUpComponent implements OnInit {

  @Input() imagePath: SafeResourceUrl = "";
  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  navigateToMemories() {
    this.router.navigateByUrl('/memories');
  }
}
