using System;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormDataBinary
    {
        public FormDataBinary()
        {
        }

        public FormDataBinary(String fileName, byte[] fileBytes)
        {
            FileName = fileName;
            FileBytes = fileBytes;
        }

        public FormDataBinary(FormDataBinary formDataBinary)
        {
            FileName = formDataBinary.FileName;

            if (formDataBinary.FileBytes != null)
            {
                var oldBytes = formDataBinary.FileBytes;
                var newBytes = new byte[oldBytes.Length];

                Buffer.BlockCopy(oldBytes, 0, newBytes, 0, oldBytes.Length);

                FileBytes = newBytes;
            }
        }

        public String FileName { get; set; }

        public byte[] FileBytes { get; set; }

        public override string ToString()
        {
            return $"{FileName} ({GetSize()})";
        }

        private String GetSize()
        {
            if (FileBytes == null || FileBytes.Length == 0)
                return null;

            if (FileBytes.Length > 1073741824)
                return $"{FileBytes.Length / 1024D / 1024D / 1024D:0.##} GB";

            if (FileBytes.Length > 1048576)
                return $"{FileBytes.Length / 1024D / 1024D:0.##} MB";

            if (FileBytes.Length > 1024)
                return $"{FileBytes.Length / 1024D:0.##} KB";

            return $"{FileBytes.Length:0.##} Bytes";
        }
    }
}