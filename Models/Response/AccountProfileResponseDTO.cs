namespace ManhwaDimension.Models.Response
{
    public class AccountProfileResponseDTO
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleColor { get; set; }
        public string FullName { get; set; } = null!;
        public string? Photo { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public long? WarehouseId { get; set; } = null!;
        public bool? IsVneIdLinked { get; set; }
        public bool? IsSyncSampleData { get; set; }

        public string DisplayName { get; set; } = null!;
    }
}