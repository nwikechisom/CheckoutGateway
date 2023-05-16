using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckoutGateway.DataLayer.Models;
public class BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public Guid Id { get; set; }
}

public class Auditable : BaseModel
{
    public string Creator { get; set; }
    public DateTime Created { get; set; }
    public string Modifier { get; set; }
    public DateTime Modified { get; set; }
}