namespace DataAccess.DTO;

public class UserDto
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public Guid? RoleId { get; set; }
    
    public string? QrCode { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsDeleted { get; set; }
    public bool? IsActived { get; set;}
}