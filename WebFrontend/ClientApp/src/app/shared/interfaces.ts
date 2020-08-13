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
