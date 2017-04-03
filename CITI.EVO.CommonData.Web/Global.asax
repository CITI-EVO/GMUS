<%@ Application Language="C#" %>
<%@ Import Namespace="CITI.EVO.Tools.Security" %>
<%@ Import Namespace="CITI.EVO.Tools.Utils" %>

<script RunAt="server">

    /// <summary>
    /// Fired when an application initializes or is first called. It's invoked for all HttpApplication object instances.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_Init(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Fired just before an application is destroyed. This is the ideal location for cleaning up previously used resources.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_Disposed(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Fired when an unhandled exception is encountered within the application.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_Error(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Fired when the first instance of the HttpApplication class is created. It allows you to create objects that are accessible by all HttpApplication instances.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_Start(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Fired when the last instance of an HttpApplication class is destroyed. It's fired only once during an application's lifetime.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_End(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired when an application request is received. It's the first event fired for a request, which is often a page request (URL) that a user enters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        LanguageUtil.SetLanguage();
    }

    /// <summary>
    /// The last event fired for an application request.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_EndRequest(object sender, EventArgs e)
    {
        Hb8Factory.ReleaseSession();
    }

    /// <summary>
    /// Fired before the ASP.NET page framework begins executing an event handler like a page or Web service.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired when the ASP.NET page framework is finished executing an event handler.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_PostRequestHandlerExecute(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired before the ASP.NET page framework sends HTTP headers to a requesting client (browser).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Applcation_PreSendRequestHeaders(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired before the ASP.NET page framework sends content to a requesting client (browser).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_PreSendContent(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired when the ASP.NET page framework gets the current state (Session state) related to the current request.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_AcquireRequestState(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired when the ASP.NET page framework completes execution of all event handlers. This results in all state modules to save their current state data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_ReleaseRequestState(object sender, EventArgs e)
    {
    }

    //: Fired when the ASP.NET page framework completes an authorization request. It allows caching modules to serve the request from the cache, thus bypassing handler execution.
    protected void Application_ResolveRequestCache(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired when the ASP.NET page framework completes handler execution to allow caching modules to store responses to be used to handle subsequent requests.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_UpdateRequestCache(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired when the security module has established the current user's identity as valid. At this point, the user's credentials have been validated.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired when the security module has verified that a user can access resources.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Application_AuthorizeRequest(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Fired when a new user visits the application Web site.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Session_Start(object sender, EventArgs e)
    {
        UmUtil.Instance.Login();
    }

    /// <summary>
    /// Fired when a user's session times out, ends, or they leave the application Web site.
    /// Code that runs when a session ends.
    /// Note: The Session_End event is raised only when the sessionstate mode
    /// is set to InProc in the Web.config file. If session mode is set to StateServer
    /// or SQLServer, the event is not raised.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Session_End(object sender, EventArgs e)
    {
    }

</script>
