import {Component, Inject} from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
import { Location } from '@angular/common';
declare var twttr: any;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  public tweets: Tweet[] = [];
  public cursor = '-1';
  public sortType = 'desc';
  public MediaType = MediaType;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute, private router: Router, private location: Location) {
  }

  ngOnInit() {
    const cursor = this.route.snapshot.paramMap.get('cursor');
    this.cursor = parseInt(cursor) > 0 ? cursor : '-1';
    this.sortType = this.route.snapshot.paramMap.get('sort') != null ? this.route.snapshot.paramMap.get('sort') : 'desc';
    this.loadData();
  }

  loadData(cursor: string = this.cursor, sortType: string = this.sortType): any {
    this.http.get<Tweet[]>(this.baseUrl + 'tweets/' + cursor + '/' + sortType).subscribe(result => {
      this.tweets = this.tweets.concat(result["data"]);
      this.cursor = result["cursor"];
      /*const url = this
        .router
        .createUrlTree([{ cursor: result["cursor"] }], { relativeTo: this.route })
        .toString();
      this.location.go(url);*/
      this.location.go('/' + cursor + '/' + sortType);

      if (twttr != null && twttr.widgets != null) {
        twttr.widgets.load(); // twitter widgets function to load all not loaded widgets
      }
    }, error => console.error(error));
  }

  onScrollDown() {
    if (/*this.pagination.cursor !== -1*/true) { //TODO: IT RETURNS 404 if there are nothing left
      this.loadData();
    } else {
      console.log('no results');
    }
    console.log('scrolled down');
  }

  onScrollUp() {
    console.log('scrolled up');
  }
}

interface Tweet {
  id: number,
  createdAt: string,
  user: User,
  text: string,
  media: Media[],
  isRetweet: boolean,
  retweetTweet: Tweet,
  isQuote: boolean,
  quotedTweet: Tweet
}

interface User {
  id: number,
  name: string,
  screenName: string,
  profileImageUrl: string
}

interface Media {
  id: number,
  mediaType: MediaType,
  url: string
}

enum MediaType {
  Photo = 0,
  Video = 1,
  GIF = 2
}
