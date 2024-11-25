using System;
using System.Collections.Generic;

namespace DocumentUpload.Service;

public partial class TdhcomplianceDocument
{
    public int Id { get; set; }

    public string? DocumentId { get; set; }

    public string? EntityId { get; set; }

    public string? FileName { get; set; }

    public string? CreatedBy { get; set; }

    public string? CreatedDate { get; set; }

    public string? ComplianceId { get; set; }
}
