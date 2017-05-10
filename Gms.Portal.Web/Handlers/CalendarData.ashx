<%@ WebHandler Language="C#" Class="CalendarData" %>

using System;
using System.Linq;
using System.Web;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Newtonsoft.Json.Linq;
using NHibernate.Linq;

public class CalendarData : IHttpHandler
{
    public bool IsReusable
    {
        get { return false; }
    }

    public void ProcessRequest(HttpContext context)
    {
        var response = context.Response;
        response.ContentType = "application/json; charset=utf-8";

        var session = Hb8Factory.InitSession();

        var events = (from n in session.Query<GM_Event>()
                      where n.DateDeleted == null
                      orderby n.DateCreated descending
                      select n).ToList();

        var converter = new EventEntityModelConverter(session);

        var models = events.Select(n => converter.Convert(n));

        var entities = (from n in models
                        where n.Entity != null &&
                              n.Entity.Phases != null
                        select n);

        var phases = (from n in entities
                      from m in n.Entity.Phases
                      select new
                      {
                          n.ID,
                          Title = GetItemName(n.Name, m.Name),
                          Start = m.StartDate,
                          End = m.EndDate,
                          Color = m.Color,
                          FormID = m.FormID,
                      });

        var array = new JArray();

        foreach (var item in phases)
        {
            var formUrl = GetFormUrl(item.FormID);

            var id = new JProperty("id", item.ID);
            var name = new JProperty("title", item.Title);
            var start = new JProperty("start", item.Start);
            var end = new JProperty("end", item.End);
            var color = new JProperty("color", item.Color);
            var url = new JProperty("url", formUrl);
            //var allDay = new JProperty("allDay", true);

            var jObj = new JObject(id, name, start, end, color, url);
            array.Add(jObj);
        }

        var d = array.ToString();
        response.Write(d);
    }

    private String GetFormUrl(Guid? formID)
    {
        if (formID == null)
            return String.Empty;

        var url = new UrlHelper("FormDataGrid.aspx");
        url["FormID"] = formID;

        return url.ToEncodedUrl();
    }

    protected String GetItemName(String eventName, String phaseName)
    {
        if (String.IsNullOrWhiteSpace(phaseName))
            return eventName;

        return $"{eventName}/{phaseName}";
    }



}