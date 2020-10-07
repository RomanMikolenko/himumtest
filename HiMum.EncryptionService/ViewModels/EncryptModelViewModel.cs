using System.ComponentModel.DataAnnotations;

namespace HiMum.EncryptionService.ViewModels
{
    public class EncryptModelViewModel
    {
        [Required]
        [MinLength(1)]
        public string EncryptData { get; set; }
    }
}
