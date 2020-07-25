using System.ComponentModel.DataAnnotations;

namespace InfTechWeb.Models
{
    public class ExtensionModel
    {
        public int ExtensionId { get; set; }
        
        [Display(Name = "Название:")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression("^[a-z]{2,10}$" , ErrorMessage = "Некорректное расширение")]
        public string ExtensionName { get; set; }
        public byte[] ExtensionIco { get; set; }
    }
}