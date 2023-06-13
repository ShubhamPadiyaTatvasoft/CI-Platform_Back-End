using System;
using System.Collections.Generic;

namespace CI_API.Core.Models;

public partial class ContactU
{
    public long? UserId { get; set; }

    public long ContactId { get; set; }

    public string? UserName { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
