using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models
{
    public class GetSubmitViewModel
    {
        public GetSubmitViewModel()
        {
            this.filenames = new List<string>();
        }
        public List<string> filenames { get; set; }
        public string coursename { get; set; }
        public string username { get; set; }
        public string mode { get; set; }
    }
}