namespace Good.Admin.Entity
{
    public class ActionsInputDTO
    {
        public List<string> ActionIds { get; set; }
        public string parentId { get; set; }
        public ActionType[] types { get; set; }
        public bool selectable { get; set; }
        public bool checkEmptyChildren { get; set; }
    }
}
