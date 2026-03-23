namespace ManhwaDimension.ULT
{
    public class RoleRightDTParameters : DTParameters
    {
        public List<long> RoleIds { get; set; } = new List<long>();
        public List<long> RightsIds { get; set; } = new List<long>();

        public string SearchAll { get; set; } = "";
    }
}
