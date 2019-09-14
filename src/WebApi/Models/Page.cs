using System;

namespace CasoNaranjitoSac.Models
{
    public partial class Page
    {
        public int IdPage { get; set; }
        public int IdSession { get; set; }
        public string UrlVisit { get; set; }
        public DateTime? Initial { get; set; }
        public DateTime? Ended { get; set; }

        public virtual Session IdSessionNavigation { get; set; }
    }
}
