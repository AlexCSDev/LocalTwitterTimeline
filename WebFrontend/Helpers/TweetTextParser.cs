using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using TweetDatabase.Models;
using WebFrontend.Models;

namespace WebFrontend.Helpers
{
    public static class TweetTextParser
    {
        public static string ParseTweetText(TwitterStatus twitterStatus)
        {
            return string.Join("", BuildStrings(twitterStatus));
        }

        private static IEnumerable<string> BuildStrings(TwitterStatus twitterStatus)
        {
            if (twitterStatus is null)
            {
                yield break;
            }

            var nodes = FlowContentNodes(twitterStatus);

            foreach (var node in nodes)
            {
                switch (node.FlowContentNodeType)
                {
                    case FlowContentNodeType.Text:
                        yield return Run(node.Text);
                        break;

                    case FlowContentNodeType.Url:
                        yield return Link(node.Text);
                        break;

                    case FlowContentNodeType.Mention:
                        yield return Mention(node.Text);
                        break;

                    case FlowContentNodeType.HashTag:
                        yield return Hashtag(node.Text);
                        break;

                    case FlowContentNodeType.Media:
                        // Media is handled else where
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private static string Run(string text)
        {
            return ConvertXmlEntities(text);
        }

        private static string Link(string link)
        {
            return $"<a href=\"{link}\" target=\"blank\">{link}</a>";
        }

        private static string Mention(string text)
        {
            return $"<a href=\"https://twitter.com/{text}\" target=\"blank\">@{text}</a>";
        }

        private static string Hashtag(string text)
        {
            return $"<a href=\"https://twitter.com/hashtag/{text}?src=hashtag_click\" target=\"blank\">#{text}</a>";
        }

        private static string ConvertXmlEntities(string text)
        {
            return text
                .Replace("&lt;", "<", StringComparison.Ordinal)
                .Replace("&gt;", ">", StringComparison.Ordinal)
                .Replace("&quot;", "\"", StringComparison.Ordinal)
                .Replace("&apos;", "'", StringComparison.Ordinal)
                .Replace("&amp;", "&", StringComparison.Ordinal);
        }

        private static IEnumerable<FlowContentNode> FlowContentNodes(TwitterStatus twitterStatus)
        {
            var start = 0;
            var twitterString = new TwitterString(twitterStatus.FullText ?? twitterStatus.Text ?? string.Empty);

            foreach (var item in FlowControlItems(twitterStatus.Entities ?? new Entities()))
            {
                if (item.Start >= start)
                {
                    var len = item.Start - start;
                    var text = twitterString.Substring(start, len);
                    yield return new FlowContentNode(FlowContentNodeType.Text, text);
                }

                yield return new FlowContentNode(item.FlowContentNodeType, item.Text);
                start = item.End;
            }

            yield return new FlowContentNode(FlowContentNodeType.Text, twitterString.Substring(start));
        }

        private static IEnumerable<FlowContentItem> FlowControlItems(Entities entities)
        {
            var urls = entities.Urls
                 ?.Select(url => new FlowContentItem
                 (
                     nodeType: FlowContentNodeType.Url,
                     text: url.ExpandedUrl,
                     start: url.Indices[0],
                     end: url.Indices[1]
                 ))
                 ?? Enumerable.Empty<FlowContentItem>();

            var mentions = entities.Mentions
                ?.Select(mention => new FlowContentItem
                (
                    nodeType: FlowContentNodeType.Mention,
                    text: mention.ScreenName,
                    start: mention.Indices[0],
                    end: mention.Indices[1]
                ))
                ?? Enumerable.Empty<FlowContentItem>();

            var hashTags = entities.HashTags
                ?.Select(hashtag => new FlowContentItem
                (
                    nodeType: FlowContentNodeType.HashTag,
                    text: hashtag.Text,
                    start: hashtag.Indices[0],
                    end: hashtag.Indices[1]
                ))
                ?? Enumerable.Empty<FlowContentItem>();

            var media = entities.Media
                ?.Select(media => new FlowContentItem
                (
                    nodeType: FlowContentNodeType.Media,
                    text: media.Url,
                    start: media.Indices[0],
                    end: media.Indices[1]
                ))
                ?? Enumerable.Empty<FlowContentItem>();

            return urls
                .Concat(mentions)
                .Concat(hashTags)
                .Concat(media)
                .OrderBy(o => o.Start);
        }
    }
}
