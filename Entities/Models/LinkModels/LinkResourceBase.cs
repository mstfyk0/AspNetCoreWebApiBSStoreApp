using System.Collections.Generic;

namespace Entities.Models.LinkModels
{
    public class LinkResourceBase
    {

        public LinkResourceBase()
        {
            
        }

        public List<Link> Links { get; set; } = new List<Link>();
    }
}
