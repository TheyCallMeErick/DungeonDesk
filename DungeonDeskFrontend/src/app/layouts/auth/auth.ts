import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-auth',
  imports: [RouterOutlet],
  templateUrl: './auth.html',
  styleUrl: './auth.css',
})
export class Auth implements OnInit {
  private leftData = {
    adventures: [
      {
        name: 'Dune',
        imageUrl: '/images/dune.png',
      },
      {
        name: 'Dungeons and Dragons',
        imageUrl: '/images/dungeons_and_dragons.png',
      },
      {
        name: 'Call of Cthulhu',
        imageUrl: '/images/call_of_cthulhu.png',
      },
    ],
    quotes: [
      {
        phrase: 'The cave you fear to enter holds the treasure you seek.',
        author: 'Joseph Campbell',
      },
      {
        phrase: 'To confront a person with his shadow is to show him his own light.',
        author: 'C. G. Jung',
      },
      {
        phrase:
          'Knowing your own darkness is the best method for dealing with the darknesses of other people.',
        author: 'C. G. Jung',
      },
      {
        phrase: 'Walk in the light before the dawn of darkness.',
        author: 'Lailah Gifty Akita',
      },
      {
        phrase: 'Everyone is a moon, and has a dark side which he never shows to anybody.',
        author: 'Mark Twain',
      },
    ],
  };

  public showingData = {
    adventure: this.leftData.adventures[0],
    quote: this.leftData.quotes[0],
  } as {
    adventure: {
      name: string;
      imageUrl: string;
    };
    quote: {
      phrase: string;
      author: string;
    };
  };
  nextIndex<T>(array: T[], current: T): number {
    const index = array.indexOf(current);
    return (index + 1) % array.length;
  }
  observeTimeout() {
    return new Observable((observer) => {
      setInterval(() => {
        const left = this.leftData;
        const adventureIndex = this.nextIndex(left.adventures, this.showingData.adventure);
        const quoteIndex = this.nextIndex(left.quotes, this.showingData.quote);
        observer.next({
          adventure: left.adventures[adventureIndex],
          quote: left.quotes[quoteIndex],
        });
      }, 4000);
    });
  }

  ngOnInit(): void {
    this.observeTimeout().subscribe((data: any) => (this.showingData = data));
  }
}
