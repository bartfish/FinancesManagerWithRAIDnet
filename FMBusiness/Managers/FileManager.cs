using System;
using System.Linq;
using log4net;
using System.Reflection;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using FMBusiness.Classes;
using System.Text;

namespace FMBusiness.Managers
{
    public static class FileManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static PdfToDownload GeneratePdf<T>(List<T> tableData)
        {
            try
            {
                if (tableData.Count == 0)
                {
                    throw new ArgumentNullException(paramName: nameof(tableData));
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4);
                    Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

                    string pdfFileName = string.Format(typeof(T).Name + DateTime.Now.ToString("yyyyMMdd") + ".pdf");

                    PdfWriter.GetInstance(pdfDoc, stream).CloseStream = false;
                    pdfDoc.Open();

                    PropertyInfo[] props = typeof(T).GetProperties();
                    PdfPTable pdfTable = new PdfPTable(props.Length);
                    pdfTable.WidthPercentage = 100;

                    foreach (var prop in props)
                    {
                        pdfTable.AddCell(new Phrase(prop.Name, font5));
                    }

                    foreach (var r in tableData)
                    {
                        pdfTable.AddCell(new Phrase(r.ToString(), font5));
                    }

                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();

                    Byte[] bytes = stream.ToArray();
                    return new PdfToDownload(bytes, pdfFileName);
                }
            }
            catch (Exception e)
            {
                _log.ErrorFormat("There was an error with generating PDF file. Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace);
                throw;
            }
        }

        public static CsvToDownload GenerateCsv<T>(List<T> tableData)
        {
            try
            {
                StringBuilder strBuilder = GenerateGridContentToString(tableData);
                string filename = GenerateFileName(typeof(T).Name);

                return new CsvToDownload(strBuilder, filename);
                
            }
            catch (Exception e)
            {
                _log.ErrorFormat("There was an error with generating PDF file. Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace);
                throw;
            }
        }

        private static StringBuilder GenerateGridContentToString<T>(List<T> tableData)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            StringBuilder sb = new StringBuilder();

            // Add columns
            foreach (var prop in props)
            {
                sb.Append(prop.Name).Append(";");
            }
            sb.AppendLine();

            // Add rows with data
            for (int i = 0; i < tableData.Count(); i++)
            {
                foreach (var prop in props)
                {
                    var a = tableData.ElementAt(i).GetType().GetProperty(prop.Name).GetValue(tableData.ElementAt(i));
                    sb.Append($"{a};");
                }
                sb.AppendLine();

            }
            return sb;
        }

        private static string GenerateFileName(string objName)
        {
            return string.Format(objName + DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        }
    }
}
