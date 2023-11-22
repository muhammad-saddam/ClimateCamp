using System.ComponentModel.DataAnnotations;

namespace ClimateCamp.Common.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}