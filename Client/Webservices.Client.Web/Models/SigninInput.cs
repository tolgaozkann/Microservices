using System.ComponentModel.DataAnnotations;

namespace Webservices.Client.Web.Models;

public class SigninInput
{
    [Display(Name = "Email Address")]
    public string Email { get; set; }
    [Display(Name = "Password")]
    public string Password { get; set; }
    [Display(Name = "Rememeber Me?")]
    public bool RememberMe { get; set; }
}