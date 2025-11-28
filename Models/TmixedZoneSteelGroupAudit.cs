using System;
using System.Collections.Generic;

namespace WinFormsApp2.Models;

public partial class TmixedZoneSteelGroupAudit
{
    public int AuditId { get; set; }

    public int FsgGroupId { get; set; }

    public int FsgRow { get; set; }

    public int? OldValue { get; set; }

    public int NewValue { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationTime { get; set; }

    public string? PerformedBy { get; set; }
}
