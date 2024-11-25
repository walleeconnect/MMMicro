using System;
using System.Collections.Generic;

namespace DocumentUpload.Service;

public partial class Tdhdocument
{
    public int? Id { get; set; }

    public string DocumentId { get; set; } = null!;

    public string? EntityId { get; set; }

    public string? GroupId { get; set; }

    public string? ModuleId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public string? UploadedBy { get; set; }

    public string? UploadedDate { get; set; }
}
