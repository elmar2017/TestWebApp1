using System;
using System.Collections.Generic;

namespace TestWebApp
{
    public partial class Curve
    {
        public string Company { get; set; } = null!;
        public string Segment { get; set; } = null!;
        public string CurveType { get; set; } = null!;
        public DateTime ValidFrom { get; set; }
        public bool IsLastVersion { get; set; }
        public int VersionNumber { get; set; }
        public int? NextVersionNumber { get; set; }
        public int CalculationPeriod { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; } = null!;
        public DateTime CreationTime { get; set; }
        public string Creator { get; set; } = null!;
        public string ModifiedBy { get; set; } = null!;
    }
}
