using System;

namespace CasoNaranjitoSac.Models
{
    public partial class Link
    {
        public int IdLink { get; set; }
        public int IdSession { get; set; }
        public string UrlLink { get; set; }
        public DateTime? Created { get; set; }

        public virtual Session IdSessionNavigation { get; set; }
    }
}
