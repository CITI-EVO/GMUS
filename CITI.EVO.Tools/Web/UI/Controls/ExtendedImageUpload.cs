using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Panel = CITI.EVO.Tools.Web.UI.Controls.Panel;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class ExtendedImageUpload : Panel
    {
        private readonly ASPxBinaryImage _binaryImage;
        private readonly ExtendedFileUpload _fileUpload;

        private readonly Panel _imagePanel;
        private readonly Panel _fileUploadPanel;

        public ExtendedImageUpload()
        {
            _binaryImage = new ASPxBinaryImage();
            _binaryImage.BinaryStorageMode = BinaryStorageMode.Session;
            _binaryImage.StoreContentBytesInViewState = true;
            _binaryImage.ContentBytes = null;


            _fileUpload = new ExtendedFileUpload();

            _imagePanel = new Panel();
            _fileUploadPanel = new Panel();

            _fileUpload.Upload += fileUpload_Upload;
            _fileUpload.Delete += fileUpload_Delete;
            _fileUpload.Download += fileUpload_Download;

            _imagePanel.Controls.Add(_binaryImage);
            _fileUploadPanel.Controls.Add(_fileUpload);

            base.Controls.Add(_imagePanel);
            base.Controls.Add(_fileUploadPanel);
        }

        public String FileName
        {
            get { return _fileUpload.FileName; }
            set { _fileUpload.FileName = value; }
        }

        public byte[] ImageBytes
        {
            get { return _binaryImage.ContentBytes; }
            set { _binaryImage.ContentBytes = value; }
        }

        public String NoImageUrl
        {
            get { return Convert.ToString(ViewState["NoImageUrl"]); }
            set { ViewState["NoImageUrl"] = value; }
        }

        public bool EnableDonwload
        {
            get { return _fileUpload.EnableDonwload; }
            set { _fileUpload.EnableDonwload = value; }
        }

        public bool DownloadVisible
        {
            get { return _fileUpload.DownloadVisible; }
            set { _fileUpload.DownloadVisible = value; }
        }

        public bool EnableUpload
        {
            get { return _fileUpload.EnableUpload; }
            set { _fileUpload.EnableUpload = value; }
        }

        public bool UploadVisible
        {
            get { return _fileUpload.UploadVisible; }
            set { _fileUpload.UploadVisible = value; }
        }

        public bool EnableDelete
        {
            get { return _fileUpload.EnableDelete; }
            set { _fileUpload.EnableDelete = value; }
        }

        public bool DeleteVisible
        {
            get { return _fileUpload.DeleteVisible; }
            set { _fileUpload.DeleteVisible = value; }
        }

        public String UploadCaption
        {
            get { return _fileUpload.UploadCaption; }
            set { _fileUpload.UploadCaption = value; }
        }

        public String DownloadCaption
        {
            get { return _fileUpload.DownloadCaption; }
            set { _fileUpload.DownloadCaption = value; }
        }

        public String DeleteCaption
        {
            get { return _fileUpload.DeleteCaption; }
            set { _fileUpload.DeleteCaption = value; }
        }

        public String FileUploadCssClass
        {
            get { return _fileUpload.FileUploadCssClass; }
            set { _fileUpload.FileUploadCssClass = value; }
        }

        public CssStyleCollection FileUploadStyle
        {
            get { return _fileUpload.FileUploadStyle; }
        }

        public String UploadLinkCssClass
        {
            get { return _fileUpload.UploadLinkCssClass; }
            set { _fileUpload.UploadLinkCssClass = value; }
        }

        public CssStyleCollection UploadLinkStyle
        {
            get { return _fileUpload.UploadLinkStyle; }
        }

        public String DownloadLinkCssClass
        {
            get { return _fileUpload.DownloadLinkCssClass; }
            set { _fileUpload.DownloadLinkCssClass = value; }
        }

        public CssStyleCollection DownloadLinkStyle
        {
            get { return _fileUpload.DownloadLinkStyle; }
        }

        public String DeleteLinkCssClass
        {
            get { return _fileUpload.DeleteLinkCssClass; }
            set { _fileUpload.DeleteLinkCssClass = value; }
        }

        public CssStyleCollection DeleteLinkStyle
        {
            get { return _fileUpload.DeleteLinkStyle; }
        }

        public Unit ImageWidth
        {
            get { return _binaryImage.Width; }
            set { _binaryImage.Width = value; }
        }

        public Unit ImageHeight
        {
            get { return _binaryImage.Height; }
            set { _binaryImage.Height = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (ImageBytes == null || ImageBytes.Length == 0)
            {
                var path = HttpContext.Current.Server.MapPath(NoImageUrl);
                ImageBytes = File.ReadAllBytes(path);
            }

            base.OnLoad(e);
        }

        private void fileUpload_Upload(object sender, EventArgs e)
        {
            ImageBytes = _fileUpload.Bytes;
            _fileUpload.Bytes = null;
        }

        private void fileUpload_Delete(object sender, EventArgs e)
        {
            ImageBytes = null;
        }

        private void fileUpload_Download(object sender, EventArgs e)
        {
            if (ImageBytes == null || ImageBytes.Length == 0)
            {
                return;
            }

            var response = HttpContext.Current.Response;
            if (response.HeadersWritten())
            {
                return;
            }

            var correctFileName = (String.IsNullOrWhiteSpace(FileName) ? "Unknown" : FileName);
            correctFileName = HttpUtility.UrlPathEncode(correctFileName);

            response.Clear();
            response.Buffer = true;
            response.ContentType = "application/octet-stream";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", correctFileName));
            response.BinaryWrite(ImageBytes);
            response.End();
        }
    }
}
