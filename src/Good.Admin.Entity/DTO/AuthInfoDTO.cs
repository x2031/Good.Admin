using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.Entity
{
    public class TokenDTO
    {
      
        public string token { get; set; }
        public double expires_in { get; set; }
        public string token_type { get; set; }


    }
}
