using System.ComponentModel.DataAnnotations;

public class VaultInputModel
{
    [Required(ErrorMessage = "Fältet får inte vara tomt.")]
    [StringLength(200, ErrorMessage = "Max 200 tecken.")]
    [RegularExpression(@"^[a-zA-Z0-9\s\.\,\!\?\@\#\$\%\&\*\(\)\-]*$", ErrorMessage = "Otillåtna tecken.")]
    public string Secret { get; set; }
}
