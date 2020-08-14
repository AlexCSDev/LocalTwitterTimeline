import {Component, Inject, Pipe, PipeTransform} from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute, NavigationEnd } from "@angular/router";
import { Location } from '@angular/common';
import { SafeHtmlPipe } from '../shared/safeHtml.pipe';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [SafeHtmlPipe]
})
export class HomeComponent {
  public tweets: TwitterStatus[] = [];
  public cursor = '-1'; // used by next page button
  public sortType = 'desc'; // used by next page button
  public isLoading = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute, private router: Router, private location: Location) {
    router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        this.loadData();
      }
    });
  }

  ngOnInit() {
    this.loadData();
  }

  loadData(): any {
    this.isLoading = true;
    window.scroll(0, 0);
    this.tweets = [];

    let cursorParam = this.route.snapshot.paramMap.get('cursor');
    let cursor = parseInt(cursorParam) > 0 ? cursorParam : '-1';
    let sortType = this.route.snapshot.paramMap.get('sort') != null ? this.route.snapshot.paramMap.get('sort') : 'desc';

    this.http.get<TwitterStatus[]>(this.baseUrl + 'tweets/' + cursor + '/' + sortType).subscribe(result => {
      this.tweets = result["data"];

      this.tweets.forEach(function(tweet) {
        if(tweet.originatingStatus == null) {
          tweet.originatingStatus = tweet; //restore link to self
        }
      });

      this.cursor = result["cursor"];
      this.sortType = sortType;
      this.isLoading = false;
    }, error => console.error(error));
  }
}
