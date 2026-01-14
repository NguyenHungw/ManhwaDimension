namespace ManhwaDimension.ULT
{
    public class RoleMenuDTParameters : DTParameters
    {
        public List<long> RoleIds { get; set; } = new List<long>();
        public List<long> RightsIds { get; set; } = new List<long>();

        public string SearchAll { get; set; } = "";
    }
}
