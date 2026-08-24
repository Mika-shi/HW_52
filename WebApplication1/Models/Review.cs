using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Review
{
    public int Id { get; set; }

    public int PhoneId { get; set; }

    [Required(ErrorMessage = "Укажите имя автора")]
    public string AuthorName { get; set; } = "";

    [Required(ErrorMessage = "Напишите отзыв")]
    public string Text { get; set; } = "";

    [Range(0, 5, ErrorMessage = "Оценка должна быть от 0 до 5")]
    public int Rating { get; set; }

    public Phone? Phone { get; set; }
}