namespace Backend.Models.DTOs;

public class TwoFactorInitResponseDto
{
    public bool TwoFactorRequired { get; set; } = true;
    public string TwoFactorToken { get; set; } = string.Empty; // short-lived reference to user/session
    public string DeliveryChannel { get; set; } = "email";
    public string MaskedDestination { get; set; } = string.Empty;
}

public class TwoFactorVerifyRequestDto
{
    public string TwoFactorToken { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}


