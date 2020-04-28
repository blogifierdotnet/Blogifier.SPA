using Blogifier.Core;
using Blogifier.Core.Data;
using Blogifier.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Collections.Generic;
using System;

namespace App.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BlogController : Controller
    {
        IFeedService _ss;
        SignInManager<AppUser> _sm;
        protected IFeedService _fs;
        protected IDataService _ds;

        public BlogController(IFeedService ss, SignInManager<AppUser> sm, IDataService ds, IFeedService fs)
        {
            _ss = ss;
            _sm = sm;
            _fs = fs;
            _ds = ds;
        }

        [HttpGet("feed/{type}")]
        public async Task<IActionResult> Rss(string type)
        {
            string host = Request.Scheme + "://" + Request.Host;
            var blog = await _ds.CustomFields.GetBlogSettings();
            var posts = await _fs.GetEntries(type, host);
            var items = new List<SyndicationItem>();

            var feed = new SyndicationFeed(
                blog.Title,
                blog.Description,
                new Uri(host),
                host,
                posts.FirstOrDefault().Published
            );

            if (posts != null && posts.Count() > 0)
            {
                foreach (var post in posts)
                {
                    var item = new SyndicationItem(
                        post.Title,
                        post.Description.MdToHtml(),
                        new Uri(post.Id),
                        post.Id,
                        post.Published
                    );
                    item.PublishDate = post.Published;
                    items.Add(item);
                }
            }
            feed.Items = items;

            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true
            };

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, settings))
                {
                    var rssFormatter = new Rss20FeedFormatter(feed, false);
                    rssFormatter.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                return File(stream.ToArray(), "application/xml; charset=utf-8");
            }
        }

        [HttpGet("admin")]
        [Authorize]
        public IActionResult Admin()
        {
            return Redirect("~/admin/posts");
        }

        [HttpPost("account/logout")]
        public async Task<IActionResult> Logout()
        {
            await _sm.SignOutAsync();
            return Redirect("~/");
        }
    }
}