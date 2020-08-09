import {Component, Inject, Pipe, PipeTransform} from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
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
  public cursor = '-1';
  public sortType = 'desc';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute, private router: Router, private location: Location) {
  }

  ngOnInit() {
    const cursor = this.route.snapshot.paramMap.get('cursor');
    this.cursor = parseInt(cursor) > 0 ? cursor : '-1';
    this.sortType = this.route.snapshot.paramMap.get('sort') != null ? this.route.snapshot.paramMap.get('sort') : 'desc';
    this.loadData();
  }

  loadData(cursor: string = this.cursor, sortType: string = this.sortType): any {
    this.http.get<TwitterStatus[]>(this.baseUrl + 'tweets/' + cursor + '/' + sortType).subscribe(result => {
      this.tweets = this.tweets.concat(result["data"]);
      this.tweets.forEach(function(tweet) {
        if(tweet.originatingStatus == null) {
          tweet.originatingStatus = tweet; //restore link to self
        }
      });

      this.cursor = result["cursor"];
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

interface TwitterStatus {
  id: string,
  fullText: string,
  text: string,
  user: User,
  createdAt: string,
  entities: Entities,
  extendedEntities: Entities,
  isQuoted: boolean,
  quotedStatus: TwitterStatus,
  quoteCount: number,
  isRetweet: boolean,
  retweetedStatus: TwitterStatus,
  retweetedByMe: boolean,
  retweetCount: number,
  favoriteCount: number,
  favorited: boolean,
  inReplyToStatusId: string,
  inReplyToUserId: string,
  inReplyToScreenName: string,
  replyCount: number,
  isSensitive: boolean,
  relatedLinkInfo: RelatedLinkInfo,
  originatingStatus: TwitterStatus,
  statusLink: string,
  createdDate: Date,
  isMyTweet: boolean,
  mentionsMe: boolean,
  parsedFullText: string
}

//Entities
interface Entities {
  urls: UrlEntity[],
  mentions: MentionEntity[],
  hashTags: HashTagEntity[],
  media: Media[]
}

interface UrlEntity {
  url: string,
  displayUrl: string,
  expandedUrl: string,
  indices: number[]
}

interface MentionEntity {
  id: string,
  screenName: string,
  name: string,
  indices: number[]
}

interface HashTagEntity {
  text: string,
  indices: number[]
}

interface Media {
  url: string,
  displayUrl: string,
  expandedUrl: string,
  mediaUrl: string,
  indices: number[],
  videoInfo: VideoInfo
}

interface VideoInfo {
  variants: Variant[]
}

interface Variant {
  url: string
}

//User
interface User {
  id: string,
  name: string,
  screenName: string,
  profileImageUrl: string,
  profileBannerUrl: string,
  description: string,
  verified: boolean,
  location: string,
  url: string,
  tweets: number,
  friends: number,
  followers: number,
  entities: UserObjectEntities,
  createdAt: string,
  isFollowing: boolean,
  isFollowedBy: boolean,
  memberSince: string,
  profileImageUrlBigger: string,
  profileImageUrlOriginal: string
}

interface UserObjectEntities {
  url: UserObjectUrls
}

interface UserObjectUrls {
  urls: UrlEntity
}

//RelatedLinkInfo
interface RelatedLinkInfo {
  url: string,
  title: string,
  imageUrl: string,
  description: string,
  siteName: string,
  imageTwitterStatus: TwitterStatus
}
