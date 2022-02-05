using System.IO;

namespace plannerBackEnd.Common.EmailManager
{
    public class EmailAttachment
    {
        public MemoryStream Stream { get; set; } = null;
        public string FileName { get; set; } = null;
        public byte[] Content { get; set; } = null;

        public EmailAttachment(MemoryStream stream, string filename)
        {
            Stream = stream;
            FileName = filename;
        }

        public override string ToString()
        {
            StreamReader reader = new StreamReader(Stream);
            return reader.ReadToEnd();
        }
    }
}