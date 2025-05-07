using Swashbuckle.AspNetCore.Annotations;

public class StartNegotiationDto
{
    public Guid? ProductId { get; set; }
    
    public string? ProductName { get; set; }
}

