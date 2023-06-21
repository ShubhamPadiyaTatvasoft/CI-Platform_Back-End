using System;
using System.Collections.Generic;

namespace CI_API.Core.Models;

public partial class PasswordReset
{
    public string Token { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }
}
