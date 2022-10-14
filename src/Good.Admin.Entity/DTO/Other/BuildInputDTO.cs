using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.Entity
{
    public class BuildInputDTO
    {
        public string linkId { get; set; }
        public string areaName { get; set; }
        public List<string> tables { get; set; }
        public List<int> buildTypes { get; set; }
    }
}
