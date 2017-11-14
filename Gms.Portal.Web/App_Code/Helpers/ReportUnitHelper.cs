using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Gms.Portal.Web.Helpers
{
    public static class ReportUnitHelper
    {
        public static void Export(HttpResponse response, string targetType, DataSet dataSet)
        {
            var fileName = GetDownloadFileName(targetType);

            var bytes = GetReportGridBytes(targetType, dataSet);
            var contnetDisposition = new ContentDisposition
            {
                Inline = true,
                FileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8)
            };

            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.ContentType = MimeTypeUtil.GetMimeType(fileName);
            response.Headers["Content-Disposition"] = contnetDisposition.ToString();
            response.BinaryWrite(bytes);
            response.Flush();
            response.End();
        }
        public static void Export(HttpResponse response, string targetType, DataTable dataTable)
        {
            var fileName = GetDownloadFileName(targetType);

            var bytes = GetReportGridBytes(targetType, dataTable);
            var contnetDisposition = new ContentDisposition
            {
                Inline = true,
                FileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8)
            };

            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.ContentType = MimeTypeUtil.GetMimeType(fileName);
            response.Headers["Content-Disposition"] = contnetDisposition.ToString();
            response.BinaryWrite(bytes);
            response.Flush();
            response.End();
        }

        public static byte[] GetReportGridBytes(String targetType, DataSet dataSet)
        {
            if (targetType == "PDF")
            {
                using (var stream = new MemoryStream())
                {
                    var pdfDoc = new Document();
                    var writer = PdfWriter.GetInstance(pdfDoc, stream);

                    pdfDoc.Open();

                    foreach (DataTable dataTable in dataSet.Tables)
                    {
                        var table = GetPdfGrid(dataTable);
                        pdfDoc.Add(table);
                    }

                    pdfDoc.Close();

                    return stream.ToArray();
                }
            }

            if (targetType == "Excel")
            {
                return ExcelUtil.ConvertToExcel(dataSet);
            }

            if (targetType == "CSV")
            {
                var dataTable = dataSet.Tables.Cast<DataTable>().FirstOrDefault();
                return ExcelUtil.ConvertToCSV(dataTable);
            }

            return null;
        }
        public static byte[] GetReportGridBytes(String targetType, DataTable dataTable)
        {
            if (targetType == "PDF")
            {
                using (var stream = new MemoryStream())
                {
                    var pdfDoc = new Document();
                    var writer = PdfWriter.GetInstance(pdfDoc, stream);

                    pdfDoc.Open();

                    var table = GetPdfGrid(dataTable);
                    pdfDoc.Add(table);

                    pdfDoc.Close();

                    return stream.ToArray();
                }
            }

            if (targetType == "Excel")
            {
                return ExcelUtil.ConvertToExcel(dataTable);
            }

            if (targetType == "CSV")
            {
                return ExcelUtil.ConvertToCSV(dataTable);
            }

            return null;
        }

        public static String GetDownloadFileName(String targetType)
        {
            if (targetType == "Excel")
                return String.Format("report_{0:dd.MM.yyyy}.xlsx", DateTime.Now);

            if (targetType == "CSV")
                return String.Format("report_{0:dd.MM.yyyy}.csv", DateTime.Now);

            if (targetType == "PDF")
                return String.Format("report_{0:dd.MM.yyyy}.pdf", DateTime.Now);

            if (targetType == "Image")
                return String.Format("report_{0:dd.MM.yyyy}.png", DateTime.Now);

            throw new Exception();
        }

        private static PdfPTable GetPdfGrid(DataTable dataSource)
        {
            var sylfaenPath = String.Format("{0}\\fonts\\sylfaen.ttf", Environment.GetEnvironmentVariable("SystemRoot"));
            var sylfaen = BaseFont.CreateFont(sylfaenPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            var pdfFont = new Font(sylfaen, 9f, Font.NORMAL, BaseColor.BLACK);


            //var pdfFont = FontFactory.GetFont("Sylfaen", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 11F);
            var localFont = new System.Drawing.Font("Sylfaen", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            var widths = new List<int>();
            var table = new PdfPTable(dataSource.Columns.Count);

            foreach (DataColumn dataColumn in dataSource.Columns)
            {
                var maxWidth = 10;


                if (dataSource.AsEnumerable().Any())
                {
                    maxWidth = (from n in dataSource.AsEnumerable()
                                   let t = n[dataColumn]
                                   let w = GetTextWidth(t, localFont)
                                   select w).Max() + 1;
                }

                widths.Add(maxWidth);

                var text = HttpUtility.HtmlDecode(dataColumn.ColumnName);
                var cell = new PdfPCell(new Phrase(12, text, pdfFont))
                {
                    BackgroundColor = new BaseColor(System.Drawing.Color.Gainsboro)
                };

                table.AddCell(cell);
            }

            table.SetWidths(widths.ToArray());

            foreach (DataRow dataRow in dataSource.Rows)
            {
                foreach (DataColumn dataColumn in dataSource.Columns)
                {
                    var value = Convert.ToString(dataRow[dataColumn]);
                    var text = HttpUtility.HtmlDecode(value);
                    var cell = new PdfPCell(new Phrase(12, text, pdfFont));

                    table.AddCell(cell);
                }
            }


            return table;
        }

        private static iTextSharp.text.Image GetPdfImage(System.Drawing.Image image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);

                stream.Seek(0, SeekOrigin.Begin);

                var prfImage = iTextSharp.text.Image.GetInstance(image, ImageFormat.Png);
                return prfImage;
            }
        }

        private static int GetTextWidth(Object value, System.Drawing.Font font)
        {
            var text = Convert.ToString(value);
            if (String.IsNullOrWhiteSpace(text))
                return 1;

            using (var bmp = new System.Drawing.Bitmap(1, 1))
            {
                using (var graphics = System.Drawing.Graphics.FromImage(bmp))
                {
                    var size = graphics.MeasureString(text, font);
                    return (int)size.Width;
                }
            }
        }


    }
}