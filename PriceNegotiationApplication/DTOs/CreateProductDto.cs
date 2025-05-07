using System.ComponentModel.DataAnnotations;

public class CreateProductDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
}