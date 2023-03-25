using Good.Admin.Util;
using Newtonsoft.Json;

namespace Good.Admin.Entity
{
    public class ActionDTO : TreeModel
    {
        public ActionType Type { get; set; }
        public string Url { get; set; }
        public string path { get => Url; }
        public bool NeedAction { get; set; }
        public string TypeText { get => Type.ToString(); }
        public string NeedActionText { get => NeedAction ? "是" : "否"; }
        public object children { get => Children; }
        public string title { get => Text; }
        public string value { get => Id; }
        public string key { get => Id; }
        public bool selectable { get; set; }
        [JsonIgnore]
        public string Icon { get; set; }
        public string icon { get => Icon; }
        public int Sort { get; set; }
        public List<string> PermissionValues { get; set; }
    }
}
