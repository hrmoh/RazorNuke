namespace RazorNuke.Models.ViewModels
{
    public class RazorNukeMenuItem
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
        /// page order
        /// </summary>
        public int PageOrder { get; set; }

        /// <summary>
        /// title in menu
        /// </summary>
        public string TitleInMenu { get; set; }

        /// <summary>
        /// full page url including parents urls
        /// </summary>
        public string FullUrl { get; set; }

        /// <summary>
        /// selected
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// children
        /// </summary>
        public RazorNukeMenuItem[] Children { get; set; }
    }
}
