namespace JWT.Dtos;
public class UserProfileDTO
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public DateTime CreatedAt { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}