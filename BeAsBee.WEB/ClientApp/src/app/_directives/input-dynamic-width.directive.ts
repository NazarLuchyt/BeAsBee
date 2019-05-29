import { Directive, ElementRef, HostListener, AfterViewInit, Input, OnInit } from '@angular/core';
import { NgModel } from '@angular/forms';

@Directive({
  selector: '[appInputDynamicWidth]'
})
export class InputDynamicWidthDirective implements OnInit {




  defaulyMaxWidth = 250;

  input: HTMLInputElement;
  @Input() ngModel: string;

  @HostListener('keypress', ['$event'])
  keyPress(event: KeyboardEvent) {
    const currentText = this.input.value + event.key;
    this.input.style.width = this.getWidth(currentText);
  }

  constructor(private elementRef: ElementRef) {
    this.input = this.elementRef.nativeElement;
  }

  ngOnInit() {
    this.input.style.width = this.getWidth(this.ngModel);
  }


  getWidth(text: string): string {
    const tempCanvas = document.createElement('canvas').getContext('2d');
    const computed = (<any>window).getComputedStyle(this.elementRef.nativeElement);

    tempCanvas.font = computed.font;
    const currentWidth = tempCanvas.measureText(text).width;
    return currentWidth + 'px';
  }

  //   updateText() {
  //     let siblings = Array.prototype.slice.call(this.parentNode.childNodes);
  //     let siblingsFiltered: any[] = siblings.filter(node => node !== this.elementRef.nativeElement && node.offsetWidth);
  //     let siblingsWidthSum = _.sumBy(siblingsFiltered, (node) => node.offsetWidth);
  //     this.maxWidth = this.elementRef.nativeElement.parentNode.offsetWidth - siblingsWidthSum - this.offset;
  //     this.truncateNeeded = this.elementRef.nativeElement.offsetWidth > this.maxWidth;
  //     if (this.truncateNeeded && this.maxWidth > 0) {
  //         let computed = (<any>window).getComputedStyle(this.elementRef.nativeElement);
  //         this.elementRef.nativeElement.innerHTML = this.getTextForSize(this.elementRef.nativeElement.innerHTML, computed);
  //         this.parentNode.classList.add("truncated");
  //     }
  // }

  // getAvarageCharSize(text) {
  //     return this.elementRef.nativeElement.offsetWidth / text.length
  // }


  // getTextForSize(text: string, computed: any) {
  //     let context = document.createElement("canvas").getContext("2d");
  //     context.font = computed.font;
  //     let isUppercase = computed.textTransform === 'uppercase';
  //     let currentWidth = context.measureText(isUppercase ? text.toUpperCase() : text).width;
  //     let avgSize = this.getAvarageCharSize(text);
  //     let predictedSize = Math.round(this.maxWidth / avgSize);
  //     let textCopy = text.slice(0, predictedSize - this.trail.length) + this.trail;
  //     let predictedWidth = context.measureText(isUppercase ? textCopy.toUpperCase() : textCopy).width;
  //     let itterateCount = 0;
  //     if (predictedWidth > this.maxWidth) {
  //         currentWidth = predictedWidth;
  //         while (currentWidth > this.maxWidth && itterateCount < MAX_LOOP_COUNT) {
  //             itterateCount++;
  //             textCopy = textCopy.slice(0, textCopy.length - (this.trail.length + 1)) + this.trail;
  //             currentWidth = context.measureText(isUppercase ? textCopy.toUpperCase() : textCopy).width;
  //         }
  //         text = textCopy;
  //     }
  //     else {
  //         currentWidth = predictedWidth;
  //         while (currentWidth < this.maxWidth && itterateCount < MAX_LOOP_COUNT) {
  //             itterateCount++;
  //             textCopy = text.slice(0, textCopy.length - (this.trail.length - 1)) + this.trail;
  //             currentWidth = context.measureText(isUppercase ? textCopy.toUpperCase() : textCopy).width;
  //             if (textCopy.length >= text.length) {
  //                 break;
  //             }
  //         }
  //         if (currentWidth > this.maxWidth) {
  //             text = textCopy.slice(0, textCopy.length - (this.trail.length + 1)) + this.trail;
  //         }
  //     }
  //     return text;
  // }


}
