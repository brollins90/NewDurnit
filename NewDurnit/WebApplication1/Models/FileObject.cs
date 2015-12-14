namespace WebApplication1.Models
{
  using System;

  public class FileObject
  {
    public Guid FileId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public string FilePath { get; set; }
  }
}