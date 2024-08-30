namespace DocumentUpload.Service
{
    public class DocumentMetadata
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedOn { get; set; }
        public List<string> Tags { get; set; } // E.g., keywords for search
    }

}