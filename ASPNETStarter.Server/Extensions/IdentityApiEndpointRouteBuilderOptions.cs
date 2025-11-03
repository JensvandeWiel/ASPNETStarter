namespace ASPNETStarter.Server.Extensions;

public class IdentityApiEndpointRouteBuilderOptions
{
    public bool ExcludeRegisterPost { get; set; } = false;
    public bool ExcludeLoginPost { get; set; } = false;
    public bool ExcludeLogoutPost { get; set; } = false;
    public bool ExcludeRefreshPost { get; set; } = false;
    public bool ExcludeConfirmEmailGet { get; set; } = true;
    public bool ExcludeResendConfirmationEmailPost { get; set; } = true;
    public bool ExcludeForgotPasswordPost { get; set; } = true;
    public bool ExcludeResetPasswordPost { get; set; } = true;
    public bool ExcludeManageGroup { get; set; } = false;
    public bool Exclude2faPost { get; set; } = true;
    public bool ExcludeInfoGet { get; set; } = false;
    public bool ExcludeInfoPost { get; set; } = true;
    public bool DisableEmailConfirmation { get; set; } = true;
}