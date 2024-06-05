using RSecurityBackend.Models.Auth.Db;

namespace RazorNuke.Models
{
    /// <summary>
    /// RazorNukePage history
    /// </summary>
    public class RazorNukePageSnapshot
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// page id
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// page
        /// </summary>
        public required RazorNukePage Page { get; set; }

        /// <summary>
        /// id - this record is then modified by this user and made obsolete
        /// </summary>
        public Guid? MadeObsoleteByUserId { get; set; }

        /// <summary>
        /// this record is then modified by this user and made obsolete
        /// </summary>
        public virtual RAppUser? MadeObsoleteByUser { get; set; }

        /// <summary>
        /// record date
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// a description of the modfication
        /// </summary>
        public required string Note { get; set; }

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
    }
}
