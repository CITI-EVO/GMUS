<%@ WebHandler Language="C#" Class="Download" %>

using System;
using System.Net.Mime;
using System.Text;
using System.Web;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;

public class Download : IHttpHandler
{
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void ProcessRequest(HttpContext context)
    {
        var request = context.Request;
        var response = context.Response;

        var requestUrl = new UrlHelper(request.Url);

        var ownerID = DataConverter.ToNullableGuid(requestUrl[FormDataConstants.OwnerIDField]);
        var recordID = DataConverter.ToNullableGuid(requestUrl[FormDataConstants.IDField]);
        var fieldID = DataConverter.ToNullableGuid(requestUrl["FieldID"]);

        var document = MongoDbUtil.GetDocument(ownerID, recordID);
        if (document == null)
        {
            response.End();
            return;
        }

        BsonValue bsonValue;
        if (!document.TryGetValue(Convert.ToString(fieldID), out bsonValue))
        {
            response.End();
            return;
        }

        if (bsonValue.IsBsonNull || !bsonValue.IsBsonDocument)
        {
            response.End();
            return;
        }

        var binaryDoc = bsonValue.AsBsonDocument;

        var fileName = binaryDoc[FormDataConstants.FileNameField].AsString;

        var bsonBinary = binaryDoc[FormDataConstants.FileBytesField].AsBsonBinaryData;
        var fileBytes = bsonBinary.Bytes;

        var disposition = new ContentDisposition
        {
            Inline = true,
            FileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8)
        };

        var mimeType = MimeTypeUtil.GetMimeType(fileName);

        response.ContentType = mimeType;
        response.Headers["Content-Disposition"] = disposition.ToString();
        response.BinaryWrite(fileBytes);
    }
}