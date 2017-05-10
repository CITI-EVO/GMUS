using System.Web;
using SelectPdf;

namespace CITI.EVO.Tools.Utils
{
    public static class PdfUtil
    {
        public static void HtmlToPdf(HttpResponse responce, string html, string title)
        {
            var pageSize = PdfPageSize.A4;
            var pdfOrientation = PdfPageOrientation.Portrait;

            var webPageWidth = 1024;
            var webPageHeight = 0;

            var converter = new HtmlToPdf();

            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            var doc = converter.ConvertHtmlString(html);

            var name = $"{title}.pdf";
            doc.Save(responce, true, name);

            doc.Close();
        }
    }
}