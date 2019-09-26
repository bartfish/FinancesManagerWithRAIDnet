using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FMBusiness.Classes
{
    public interface IFile
    {
        string Type { get; set; }
        string Name { get; set; }

        void Download();
    }
    public interface IPDF
    {
        byte[] ByteStream { get; set; }
    }
    public interface ICsv
    {
        StringBuilder StringStream { get; set; }
    }

    public class PdfToDownload : IFile, IPDF
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public Byte[] ByteStream { get; set; }

        public PdfToDownload(byte[] stream, string name)
        {
            ByteStream = stream;
            Type = "application/pdf";
            Name = name;
        }

        public void Download()
        {
            var response = HttpContext.Current.Response;
            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition", "attachment;filename=" + this.Name.ToString() + ".pdf");
            response.ContentType = Type;
            response.WriteFile(ByteStream.ToString());
            response.End();
        }
    }

    public class CsvToDownload : IFile, ICsv
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public StringBuilder StringStream { get; set; }

        public CsvToDownload(StringBuilder stream, string name)
        {
            StringStream = stream;
            Type = "text/csv";
            Name = name;
        }

        public void Download()
        {
            var response = HttpContext.Current.Response;
            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition", "attachment;filename=" + this.Name.ToString() + ".csv");
            response.ContentType = Type;
            response.Write(StringStream);
            response.End();
        }
    }
}
