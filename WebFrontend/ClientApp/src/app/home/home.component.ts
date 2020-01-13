import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  public tweets: Tweet[] = [];
  public cursor = -1;
  public MediaType = MediaType;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.loadData();
  }

  loadData(): any {
    this.http.get<Tweet[]>(this.baseUrl + 'tweets/' + this.cursor).subscribe(result => {
      this.tweets = this.tweets.concat(result["data"]);
      this.cursor = result["cursor"];
      if(twttr != undefined) {
        twttr.widgets.load(); // twitter widgets function to load all not loaded widgets
      }
    }, error => console.error(error));
  }

  onScrollDown() {
    if (/*this.pagination.cursor !== -1*/true) { //TODO: IT RETURNS 404 if there are nothing left
      /*this.noResults = false;
      this.cursor = this.pagination.cursor;*/

      /*const url = this
        .router
        .createUrlTree(['/search/', this.query, this.cursor])
        .toString();

      this.location.go(url);
*/
      this.loadData();
    } else {
      console.log('no results');
    }
    console.log('scrolled down');
  }

  onScrollUp() {
    // this.page -= 1;
    console.log('scrolled up');
  }
}

interface Tweet {
  id: number,
  originalId: number,
  createdAt: string,
  user: User,
  text: string,
  media: Media[]
}

interface User {
  id: number,
  originalId: number,
  name: string,
  screenName: string
}

interface Media {
  id: number,
  originalId: number,
  mediaType: MediaType,
  url: string
}

enum MediaType {
  Photo = 0,
  Video = 1,
  GIF = 2
}
