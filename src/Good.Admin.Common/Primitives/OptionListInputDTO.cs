using System.Collections.Generic;

namespace Good.Admin.Common.Primitives
{
    public class OptionListInputDTO
    {
        public List<string> selectedValues { get; set; }
        public string q { get; set; }
    }
}