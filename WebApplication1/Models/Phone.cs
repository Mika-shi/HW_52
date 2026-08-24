using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Phone
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Укажите название телефона")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Укажите компанию")]
    public string Company { get; set; } = "";

    [Range(1, 1000000, ErrorMessage = "Цена должна быть больше 0")]
    public int Price { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }
}