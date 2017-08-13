<%@ WebHandler Language="C#" Class="PrintFormData" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;
using NVelocityTemplateEngine;

public class PrintFormData : IHttpHandler, IRequiresSessionState
{
    public bool IsReusable
    {
        get { return false; }
    }

    public void ProcessRequest(HttpContext context)
    {
        var request = context.Request;

        var response = context.Response;
        response.ContentType = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

        var requestUrl = new UrlHelper(request.Url);

        var loginToken = DataConverter.ToNullableGuid(requestUrl["LoginToken"]);
        if (loginToken == null)
        {
            UmUtil.Instance.GoToLogin();
            return;
        }

        if (!UmUtil.Instance.Login(loginToken.Value))
        {
            UmUtil.Instance.GoToLogin();
            return;
        }

        var formID = DataConverter.ToNullableGuid(requestUrl["FormID"]);
        var recordID = DataConverter.ToNullableGuid(requestUrl["RecordID"]);
        var templateID = DataConverter.ToNullableGuid(requestUrl["TemplateID"]);
        var templateType = DataConverter.ToString(requestUrl["TemplateType"]);
        var responseType = DataConverter.ToString(requestUrl["ResponseType"]);

        var session = Hb8Factory.InitSession();

        var dbForm = session.Query<GM_Form>().FirstOrDefault(n => n.ID == formID);
        if (dbForm == null)
            throw new Exception("Unable to load form");

        var converter = new FormEntityModelConverter(session);
        var model = converter.Convert(dbForm);

        var document = MongoDbUtil.GetDocument(dbForm.ID, recordID);

        var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
        if (formData == null)
            throw new Exception("Unable to find record");

        var templateText = GetTemplateText(model, templateID, templateType);
        if (templateText == null)
        {
            var templateName = ConfigurationManager.AppSettings["FormPrintTemplate"];
            if (String.IsNullOrWhiteSpace(templateName))
                throw new Exception("Invalid template file name");

            var templatePath = context.Server.MapPath(templateName);

            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Unable to finde template file", templatePath);

            templateText = File.ReadAllText(templatePath);
        }

        var velocityContext = new Dictionary<String, Object>
        {
            ["nvUtil"] = new NVelocityUtil(formData, model.Entity),
            ["entity"] = model.Entity,
            ["langPair"] = LanguageUtil.GetLanguage(),
            ["defaultFields"] = FormDataBase.DefaultFields
        };

        var velocityEngine = NVelocityEngineFactory.CreateNVelocityMemoryEngine();
        var processedText = velocityEngine.Process(velocityContext, templateText);

        if (String.IsNullOrWhiteSpace(responseType))
            responseType = GetTypeName(model, templateID);

        if (StringComparer.OrdinalIgnoreCase.Equals(responseType, "Html"))
            response.Write(processedText);
        else
        {
            var name = $"{model.Entity.Name} - {DateTime.Now:dd.MM.yyyy HH.mm}";
            var layout = GetLayoutName(model, templateID);

            PdfUtil.HtmlToPdf(response, processedText, name, layout);
        }
    }

    private String GetTypeName(FormModel model, Guid? templateID)
    {
        if (model == null || model.Entity == null || templateID == null)
            return null;

        var entity = model.Entity;
        if (entity.Templates != null)
        {
            var template = entity.Templates.FirstOrDefault(n => n.ID == templateID);
            if (template != null)
                return template.Type;
        }

        return null;
    }

    private String GetLayoutName(FormModel model, Guid? templateID)
    {
        if (model == null || model.Entity == null || templateID == null)
            return null;

        var entity = model.Entity;
        if (entity.Templates != null)
        {
            var template = entity.Templates.FirstOrDefault(n => n.ID == templateID);
            if (template != null)
                return template.Layout;
        }

        return null;
    }

    private String GetTemplateText(FormModel model, Guid? templateID, String type)
    {
        if (model == null || model.Entity == null || templateID == null)
            return null;

        var entity = model.Entity;
        if (entity.Rating != null && entity.Rating.ID == templateID)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(type, "Mail"))
                return entity.Rating.MailTemplate;

            return entity.Rating.PrintTemplate;
        }

        if (entity.Templates != null)
        {
            var template = entity.Templates.FirstOrDefault(n => n.ID == templateID);
            if (template != null)
                return template.Template;
        }

        return null;
    }
}