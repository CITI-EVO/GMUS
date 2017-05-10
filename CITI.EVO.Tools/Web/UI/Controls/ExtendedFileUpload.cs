using System;
using System.IO;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Label = CITI.EVO.Tools.Web.UI.Controls.Label;
using Panel = CITI.EVO.Tools.Web.UI.Controls.Panel;

namespace CITI.EVO.Tools.Web.UI.Controls
{
	public class ExtendedFileUpload : Panel
	{
		private readonly Panel _uploadPanel;
		private readonly Panel _commandsPanel;

		private readonly Panel _fileUploadPanel;
		private readonly FileUpload _fileUpload;

		private readonly Panel _uploadLinkPanel;
		private readonly LinkButton _uploadLink;

		private readonly Panel _downloadLinkPanel;
		private readonly LinkButton _downloadLink;

		private readonly Panel _deleteLinkPanel;
		private readonly LinkButton _deleteLink;

		private readonly Panel _fileNamePanel;
		private readonly Label _fileNameLabel;

		public event EventHandler Upload;
		protected virtual void OnUpload()
		{
			if (Upload != null)
				Upload(this, EventArgs.Empty);
		}

		public event EventHandler Download;
		protected virtual void OnDownload()
		{
			if (Download != null)
			{
				Download(this, EventArgs.Empty);
			}
		}

		public event EventHandler Delete;
		protected virtual void OnDelete()
		{
			if (Delete != null)
				Delete(this, EventArgs.Empty);
		}

		public ExtendedFileUpload()
		{
			_fileUploadPanel = new Panel();
			_fileUpload = new FileUpload();
			_fileUploadPanel.Controls.Add(_fileUpload);

			_uploadLinkPanel = new Panel();
			_uploadLink = new LinkButton();
			_uploadLink.Text = "ატვირთვა";
			_uploadLink.Click += uploadLink_Click;
			_uploadLinkPanel.Controls.Add(_uploadLink);

			_downloadLinkPanel = new Panel();
			_downloadLink = new LinkButton();
			_downloadLink.Text = "ჩამოტვირთვა";
			_downloadLink.Click += donwloadLink_Click;
			_downloadLinkPanel.Controls.Add(_downloadLink);

			_deleteLinkPanel = new Panel();
			_deleteLink = new LinkButton();
			_deleteLink.Text = "წაშლა";
			_deleteLink.Click += deleteLink_Click;
			_deleteLinkPanel.Controls.Add(_deleteLink);

			_uploadPanel = new Panel();
			_uploadPanel.Controls.Add(_fileUploadPanel);
			_uploadPanel.Controls.Add(_uploadLinkPanel);

			_fileNamePanel = new Panel();
			_fileNameLabel = new Label();
			_fileNamePanel.Controls.Add(_fileNameLabel);

			_commandsPanel = new Panel();
			_commandsPanel.Controls.Add(_uploadLinkPanel);
			_commandsPanel.Controls.Add(_downloadLinkPanel);
			_commandsPanel.Controls.Add(_deleteLinkPanel);

			base.Controls.Add(_uploadPanel);
			base.Controls.Add(_fileNamePanel);
			base.Controls.Add(_commandsPanel);
		}

		public byte[] Bytes
		{
			get { return ViewState["Bytes"] as byte[]; }
			set { ViewState["Bytes"] = value; }
		}

		public bool EnableDonwload
		{
			get { return _downloadLinkPanel.Enabled; }
			set { _downloadLinkPanel.Enabled = value; }
		}

		public bool DownloadVisible
		{
			get { return _downloadLinkPanel.Visible; }
			set { _downloadLinkPanel.Visible = value; }
		}

		public bool EnableUpload
		{
			get { return _fileUploadPanel.Enabled; }
			set { _fileUploadPanel.Enabled = value; }
		}

		public bool UploadVisible
		{
			get { return _fileUploadPanel.Visible; }
			set { _fileUploadPanel.Visible = value; }
		}

		public bool EnableDelete
		{
			get { return _deleteLinkPanel.Enabled; }
			set { _deleteLinkPanel.Enabled = value; }
		}

		public bool DeleteVisible
		{
			get { return _deleteLinkPanel.Visible; }
			set { _deleteLinkPanel.Visible = value; }
		}

		public String FileName
		{
			get { return Convert.ToString(ViewState["FileName"]); }
			set { ViewState["FileName"] = value; }
		}

		public String UploadCaption
		{
			get { return _uploadLink.Text; }
			set { _uploadLink.Text = value; }
		}

		public String DownloadCaption
		{
			get { return _downloadLink.Text; }
			set { _downloadLink.Text = value; }
		}

		public String DeleteCaption
		{
			get { return _deleteLink.Text; }
			set { _deleteLink.Text = value; }
		}

		public String DownloadMode
		{
			get { return Convert.ToString(ViewState["DownloadMode"]); }
			set { ViewState["DownloadMode"] = value; }
		}

		public String FileUploadCssClass
		{
			get { return _fileUploadPanel.CssClass; }
			set { _fileUploadPanel.CssClass = value; }
		}

		public CssStyleCollection FileUploadStyle
		{
			get { return _fileUploadPanel.Style; }
		}

		public String UploadLinkCssClass
		{
			get { return _uploadLinkPanel.CssClass; }
			set { _uploadLinkPanel.CssClass = value; }
		}

		public CssStyleCollection UploadLinkStyle
		{
			get { return _uploadLinkPanel.Style; }
		}

		public String DownloadLinkCssClass
		{
			get { return _downloadLinkPanel.CssClass; }
			set { _downloadLinkPanel.CssClass = value; }
		}

		public CssStyleCollection DownloadLinkStyle
		{
			get { return _downloadLinkPanel.Style; }
		}

		public String DeleteLinkCssClass
		{
			get { return _deleteLinkPanel.CssClass; }
			set { _deleteLinkPanel.CssClass = value; }
		}

		public CssStyleCollection DeleteLinkStyle
		{
			get { return _deleteLinkPanel.Style; }
		}

		protected override void OnPreRender(EventArgs e)
		{
			SetVisibility();
			base.OnPreRender(e);
		}

		private void donwloadLink_Click(object sender, EventArgs e)
		{
			var comparer = StringComparer.OrdinalIgnoreCase;
			if (comparer.Equals(DownloadMode, "Manual"))
			{
				OnDownload();
			}
			else
			{
				var auto = comparer.Equals(DownloadMode, "Auto");
				DoDownload(auto);
			}
		}

		private void deleteLink_Click(object sender, EventArgs e)
		{
			Bytes = null;
			FileName = null;

			SetVisibility();
			OnDelete();
		}

		private void uploadLink_Click(object sender, EventArgs e)
		{
			Bytes = _fileUpload.FileBytes;
			FileName = _fileUpload.FileName;

			SetVisibility();
			OnUpload();
		}

		private void DoDownload(bool auto)
		{
			if (Bytes == null || Bytes.Length == 0)
			{
				return;
			}

			var context = HttpContext.Current;

			var server = context.Server;
			var response = context.Response;

			if (response.HeadersWritten())
			{
				return;
			}

			var correctFileName = (String.IsNullOrWhiteSpace(FileName) ? "Unknown" : FileName);
			var urlFileName = HttpUtility.UrlPathEncode(correctFileName);

			if (auto)
			{
				var disposition = new ContentDisposition
				{
					FileName = urlFileName,
					Inline = false,
				};

				response.Clear();
				response.Buffer = true;
				response.ContentType = "application/octet-stream";
				response.AddHeader("Content-Disposition", disposition.ToString());
				response.BinaryWrite(Bytes);
				response.End();
			}
			else
			{
				var currentFileHash = CryptographyUtil.ComputeMD5(Bytes);
				var fullFileName = $"{currentFileHash}_{correctFileName}";

				var tempFolderVirtualPath = "~/Temp";
				var tempFileVirtualName = $"{tempFolderVirtualPath}/{fullFileName}";

				var tempFolderPath = server.MapPath(tempFolderVirtualPath);
				if (!Directory.Exists(tempFolderPath))
				{
					Directory.CreateDirectory(tempFolderPath);
				}

				var tempFilePath = server.MapPath(tempFileVirtualName);
				if (!File.Exists(tempFilePath))
				{
					File.WriteAllBytes(tempFilePath, Bytes);
				}

				response.Clear();
				response.Redirect(tempFileVirtualName);
			}
		}

		private void SetVisibility()
		{
			_fileUploadPanel.Visible = true;
			_fileNamePanel.Visible = false;

			if (Bytes != null && Bytes.Length > 0 && !String.IsNullOrWhiteSpace(FileName))
			{
				_fileUploadPanel.Visible = false;
				_fileNamePanel.Visible = true;

				_fileNameLabel.Text = FileName;
			}
		}
	}
}
