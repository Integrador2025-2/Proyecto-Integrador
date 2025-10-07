namespace Backend.Models.Domain;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Google OAuth fields
    public string? GoogleId { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Provider { get; set; } = "local"; // "local" or "google"

    // Foreign Key
    public int RoleId { get; set; }
    
    // Navigation property
    public Role Role { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";
}


