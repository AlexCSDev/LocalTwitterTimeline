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
  public cursor = "-1";
  public MediaType = MediaType;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute, private router: Router, private location: Location) {
  }

  ngOnInit() {
    const cursor = this.route.snapshot.paramMap.get("cursor");
    this.loadData(parseInt(cursor) > 0 ? cursor : "-1");
  }

  loadData(cursor: string = this.cursor): any {
    this.http.get<Tweet[]>(this.baseUrl + 'tweets/' + cursor).subscribe(result => {
      this.tweets = this.tweets.concat(result["data"]);
      this.cursor = result["cursor"];
      /*const url = this
        .router
        .createUrlTree([{ cursor: result["cursor"] }], { relativeTo: this.route })
        .toString();
      this.location.go(url);*/
      this.location.go('/' + cursor);

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
