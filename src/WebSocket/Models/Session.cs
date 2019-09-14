using System;
using System.Collections.Generic;

namespace CasoNaranjitoSac.Models
{
    public partial class Session
    {
        public Session()
        {
            Link = new HashSet<Link>();
            Page = new HashSet<Page>();
        }

        public int IdSession { get; set; }
        public string Uuid { get; set; }
        public string UrlOrigin { get; set; }
        public DateTime? Created { get; set; }

        public virtual ICollection<Link> Link { get; set; }
        public virtual ICollection<Page> Page { get; set; }
    }
}
