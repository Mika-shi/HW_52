using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Brand
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Укажите название бренда")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Название бренда состоять из символов от 2 до 50")]
    public string Name { get; set; } = "";
    
    [Required(ErrorMessage = "Укажите почту")]
    [EmailAddress(ErrorMessage = "неправильная почта")]
    public string Email { get; set; } = "";
    
    [Required(ErrorMessage = "Укажите дату основания бренда")]
    [DataType(DataType.Date)]
    public DateTime DateOfFoundation { get; set; }
    
}