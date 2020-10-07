using System.ComponentModel.DataAnnotations;

namespace HiMum.EncryptionService.ViewModels
{
    public class DecryptModelViewModel
    {
        [Required]
        [MinLength(16)]
        public string DecryptData { get; set; }
    }
}
