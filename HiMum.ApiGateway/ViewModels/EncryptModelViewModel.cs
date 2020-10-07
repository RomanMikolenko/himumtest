using System.ComponentModel.DataAnnotations;

namespace HiMum.ApiGateway.ViewModels
{
    public class EncryptModelViewModel
    {
        [Required]
        [MinLength(1)]
        public string EncryptData { get; set; }
    }
}
