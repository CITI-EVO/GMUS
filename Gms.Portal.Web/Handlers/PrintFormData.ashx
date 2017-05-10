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

        var templateName = ConfigurationManager.AppSettings["FormPrintTemplate"];
        if (String.IsNullOrWhiteSpace(templateName))
            throw new Exception("Invalid template file name");

        var allControls = FormStructureUtil.PreOrderTraversal(model.Entity).ToDictionary(n => n.ID);

        var privateFields = formData.PrivateFields;
        privateFields.UnionWith(FormDataBase.DefaultFields);
        privateFields.Add(FormDataConstants.DateOfAcceptField);
        privateFields.Add(FormDataConstants.DateOfSubmitField);
        privateFields.Add(FormDataConstants.StatusChangeDateField);

        foreach (var fieldKey in privateFields)
            formData.Remove(fieldKey);

        var notPrintables = (from n in allControls.Values
                             where n.NotPrintable.GetValueOrDefault()
                             select n);

        foreach (var control in notPrintables)
        {
            if (control is ContentEntity)
            {
                var subControls = FormStructureUtil.PreOrderTraversal((ContentEntity)control);
                foreach (var subControl in subControls)
                {
                    var fieldKey = Convert.ToString(subControl.ID);
                    formData.Remove(fieldKey);
                }
            }
            else
            {
                var fieldKey = Convert.ToString(control.ID);
                formData.Remove(fieldKey);
            }
        }

        var compitabilityDict = allControls.Values.ToDictionary(n => Convert.ToString(n.ID), n => n.Name);
        var adpFormData = FormDataUtil.Transform(formData, compitabilityDict);

        var templatePath = context.Server.MapPath(templateName);

        if (!File.Exists(templatePath))
            throw new FileNotFoundException("Unable to finde template file", templatePath);

        var templateText = File.ReadAllText(templatePath);

        var velocityContext = new Dictionary<String, Object>(formData);
        velocityContext["FormSchema"] = model.Entity;
        velocityContext["Controls"] = allControls;
        velocityContext["FormData"] = adpFormData;
        velocityContext["DefaultFields"] = FormDataBase.DefaultFields;
        velocityContext["LanguagePair"] = requestUrl["languagePair"];
        velocityContext["Util"] = new NVelocityUtil();

        var velocityEngine = NVelocityEngineFactory.CreateNVelocityMemoryEngine();

        var processedText = velocityEngine.Process(velocityContext, templateText);

        var name = $"{model.Entity.Name} - {DateTime.Now:dd.MM.yyyy HH.mm}";
        PdfUtil.HtmlToPdf(response, processedText, name);
    }
}