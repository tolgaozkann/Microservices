using System.ComponentModel.DataAnnotations;

namespace Webservices.Client.Web.Models;

public class SigninInput
{
    [Required]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Password")]
    public string Password { get; set; }
    [Display(Name = "Rememeber Me?")]
    public bool RememberMe { get; set; }
}