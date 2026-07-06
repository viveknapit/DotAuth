using System;
using System.Collections.Generic;
using System.Text;
using DotAuth.Domain.Enums;

namespace DotAuth.Presentation
{
    public sealed class DotAuthOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public DatabaseProvider Provider { get; set; }
    }
}
