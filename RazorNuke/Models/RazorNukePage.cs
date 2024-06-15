using RSecurityBackend.Models.Auth.Db;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorNuke.Models
{
    public class RazorNukePage
    {
        /// <summary>
        /// page id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// parent page id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// parent page
        /// </summary>
        public virtual RazorNukePage? Parent { get; set; }

        /// <summary>
        /// page order
        /// </summary>
        public int PageOrder { get; set; }

        /// <summary>
        /// published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// title in menu
        /// </summary>
        public required string TitleInMenu { get; set; }

        /// <summary>
        /// page title
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// full page title including parents titles
        /// </summary>
        public required string FullTitle { get; set; }

        /// <summary>
        /// page url slug
        /// </summary>
        public required string UrlSlug { get; set; }

        /// <summary>
        /// full page url including parents urls
        /// </summary>
        public required string FullUrl { get; set; }

        /// <summary>
        /// page html text
        /// </summary>
        public required string HtmlText { get; set; }

        /// <summary>
        /// page plain text (for search)
        /// </summary>
        public required string PlainText { get; set; }

        /// <summary>
        /// create date
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// create user id
        /// </summary>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// create uer
        /// </summary>
        public virtual RAppUser? CreateUser { get; set; }

        /// <summary>
        /// last modified
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// selected
        /// </summary>
        [NotMapped]
        public bool Selected { get; set; }

    }
}
