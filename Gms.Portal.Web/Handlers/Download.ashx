<%@ WebHandler Language="C#" Class="Download" %>

using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Web;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
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

        var historyID = DataConverter.ToNullableGuid(requestUrl["HistoryID"]);
        if (historyID != null)
        {
            var historyCollection = MongoDbUtil.GetCollection(MongoDbUtil.HistoryCollectionName);
            var historyDocument = MongoDbUtil.GetDocument(historyCollection, historyID);

            var historyDict = BsonDocumentConverter.ConvertToDictionary(historyDocument);

            var rawData = historyDict.GetValueOrDefault("RawData") as byte[];
            if (rawData != null)
            {
                var pdfFileName = $"{0:yyyy-MM-dd_HH.mm.ss}.pdf";
                var pdfDisposition = new ContentDisposition
                {
                    Inline = true,
                    FileName = HttpUtility.UrlEncode(pdfFileName, Encoding.UTF8)
                };

                response.ContentType = MimeTypeUtil.GetMimeType(pdfFileName);
                response.Headers["Content-Disposition"] = pdfDisposition.ToString();
                response.BinaryWrite(rawData);
            }

            return;
        }

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