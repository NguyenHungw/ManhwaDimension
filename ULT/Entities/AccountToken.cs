namespace ManhwaDimension.Util.Entities
{
    public class AccountToken
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public long? RoleId { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
	}
}
