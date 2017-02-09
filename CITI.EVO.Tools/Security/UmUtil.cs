using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.SessionState;
using CITI.EVO.Proxies;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Svc.Contracts;
using CITI.EVO.UserManagement.Svc.Enums;
using log4net;

namespace CITI.EVO.Tools.Security
{
	[Serializable]
	public class UmUtil : ISerializable, IDisposable
	{
		#region win32Api

		[DllImport("Kernel32", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
		private static extern int GetCurrentThreadId();

		#endregion

		#region contsants

		private const String requestLoginTokenKey = "loginToken";
		private const String requestUtcHashCodeKey = "__utchc";
		private const String requestReturnUrlKey = "ReturnUrl";

		private const String sessionInstanceKey = "UmUtil_Instance";
		private const String isActualRequestKey = "UmUtil_IsActual";

		private const String cookieLoginInfoKey = "login_info";
		private const String cookieLoginTokenKey = "login_token";
		private const String cookieLoginNameKey = "login_name";
		private const String cookiePasswordKey = "login_password";

		#endregion

		#region static fields

		private static readonly Encoding defEncoding;
		private static readonly StringComparer defComparer;
		private static readonly ISet<String> resetInitedSessions;

		private static Lazy<ILog> loggerLazy;
		private static bool assemblyResolverInited;

		[NonSerialized]
		private static readonly IDictionary<int, UmUtil> lazyInstances;

		#endregion

		#region private fields

		private readonly ISet<Guid> displayedMessages;

		private bool disposed;

		#endregion

		#region singleton

		public static UmUtil Instance
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			get
			{
				if (Session == null)
				{
					return GetOutOfSessionInstance();
				}

				InitAssemblyResolver();
				ResetSessionIfNeeded();

				var instance = Session[sessionInstanceKey] as UmUtil;
				if (instance == null)
				{
					instance = new UmUtil();
					Session[sessionInstanceKey] = instance;
				}

				return instance;
			}
		}

		#endregion

		#region contructors

		private UmUtil()
		{
			displayedMessages = new HashSet<Guid>();
		}

		static UmUtil()
		{
			defEncoding = Encoding.UTF8;
			defComparer = StringComparer.OrdinalIgnoreCase;
			resetInitedSessions = new HashSet<String>(defComparer);

			var capacity = HostingEnvironment.MaxConcurrentThreadsPerCPU;
			capacity = Math.Max(capacity, 31);

			var concurrencyLevel = HostingEnvironment.MaxConcurrentThreadsPerCPU;
			concurrencyLevel = Math.Max(concurrencyLevel, Environment.ProcessorCount * 4);

			lazyInstances = new ConcurrentDictionary<int, UmUtil>(concurrencyLevel, capacity);

			loggerLazy = new Lazy<ILog>(LoadLogger);
		}


		~UmUtil()
		{
			Dispose(false);
		}

		protected UmUtil(SerializationInfo info, StreamingContext context)
			: this()
		{
			currentToken = (Guid?)info.GetValue("currentToken", typeof(Guid?));
			currentUser = (UserContract)info.GetValue("currentUser", typeof(UserContract));
			currentUserGroups = (IList<GroupContract>)info.GetValue("currentUserGroups", typeof(List<GroupContract>));
			currentUserAttributes = (IDictionary<String, String>)info.GetValue("currentUserAttributes", typeof(Dictionary<String, String>));

			userProjects = (IDictionary<Guid, ProjectContract>)info.GetValue("userProjects", typeof(Dictionary<Guid, ProjectContract>));
			permissionsCache = (IDictionary<String, PermissionContract>)info.GetValue("permissionsCache", typeof(Dictionary<String, PermissionContract>));
		}

		#endregion

		#region ISerializable

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("currentToken", currentToken);
			info.AddValue("currentUser", currentUser);
			info.AddValue("currentUserGroups", currentUserGroups);
			info.AddValue("currentUserAttributes", currentUserAttributes);

			info.AddValue("userProjects", userProjects);
			info.AddValue("permissionsCache", permissionsCache);
		}

		#endregion

		#region static properties

		public static HttpContext Context
		{
			get
			{
				return HttpContext.Current;
			}
		}

		public static HttpRequest Request
		{
			get
			{
				if (Context != null)
				{
					return Context.Request;
				}

				return null;
			}
		}

		public static HttpResponse Response
		{
			get
			{
				if (Context != null)
				{
					return Context.Response;
				}

				return null;
			}
		}

		public static HttpSessionState Session
		{
			get
			{
				if (Context != null)
				{
					return Context.Session;
				}

				return null;
			}
		}

		public static ILog Logger
		{
			get { return loggerLazy.Value; }
		}

		#endregion

		#region properties
		public bool IsLogged
		{
			get
			{
				return IsTokenValid(CurrentToken);
			}
		}

		public bool IsPasswordExpired
		{
			get
			{
				var result = (IsLogged && CurrentUser != null && CurrentUser.PasswordExpirationDate < DateTime.Now);
				return result;
			}
		}

		private Guid? currentToken;
		public Guid? CurrentToken
		{
			get
			{
				return currentToken;
			}
		}

		private UserContract currentUser;
		public UserContract CurrentUser
		{
			get
			{
				if (CurrentToken == null || ProjectID == null)
				{
					return null;
				}

				if (currentUser == null)
				{
					currentUser = UserManagementProxy.GetCurrentUser(CurrentToken.Value);
				}

				return currentUser;
			}
		}

		private IList<GroupContract> currentUserGroups;
		public IList<GroupContract> CurrentUserGroups
		{
			get
			{
				if (CurrentToken == null || ProjectID == null || CurrentUser == null)
				{
					return null;
				}

				if (currentUserGroups == null)
				{
					currentUserGroups = UserManagementProxy.GetUserGroups(CurrentToken.Value, CurrentUser.ID, ProjectID.Value);
					currentUserGroups = (currentUserGroups ?? new List<GroupContract>());
				}

				return currentUserGroups;
			}
		}

		private IDictionary<String, String> currentUserAttributes;
		public IDictionary<String, String> CurrentUserAttributes
		{
			get
			{
				if (CurrentToken == null || ProjectID == null)
				{
					return null;
				}

				if (currentUserAttributes == null)
				{
					currentUserAttributes = UserManagementProxy.GetUserAttributesDictionary(CurrentToken.Value, CurrentUser.ID, ProjectID.Value);
					currentUserAttributes = (currentUserAttributes ?? new Dictionary<String, String>());
				}

				return currentUserAttributes;
			}
		}

		private IDictionary<Guid, ProjectContract> userProjects;
		public IDictionary<Guid, ProjectContract> UserProjects
		{
			get
			{
				if (CurrentToken == null || ProjectID == null)
				{
					return null;
				}

				if (userProjects == null)
					userProjects = GetProjectsByUserToken().ToDictionary(n => n.ID);

				return userProjects;
			}
		}

		private IDictionary<String, PermissionContract> permissionsCache;
		public IDictionary<String, PermissionContract> PermissionsCache
		{
			get
			{
				if (CurrentToken == null || ProjectID == null)
				{
					return null;
				}

				if (permissionsCache == null)
					permissionsCache = new Dictionary<String, PermissionContract>();

				return permissionsCache;
			}
		}

		#endregion

		#region private properties

		private Guid? ProjectID
		{
			get { return PermissionUtil.ModuleID; }
		}

		private bool IgnoreGroupMembership
		{
			get { return PermissionUtil.IgnoreGroupMembership; }
		}

		private bool EnabledPermissionsCache
		{
			get { return PermissionUtil.EnabledPermissionsCache; }
		}

		private bool EnabledHierachycalSearch
		{
			get { return PermissionUtil.EnabledHierachycalSearch; }
		}

		#endregion

		#region utils

		public bool IsCurrentLoginInCookies()
		{
			if (Request == null)
			{
				return false;
			}

			var userInfoCookie = Request.Cookies[cookieLoginInfoKey];
			if (userInfoCookie == null)
			{
				return false;
			}

			var loginName = userInfoCookie[cookieLoginNameKey];
			var password = userInfoCookie[cookiePasswordKey];

			return (!String.IsNullOrEmpty(loginName) && !String.IsNullOrEmpty(password));
		}

		public void SaveCurrentLoginCookies()
		{
			if (!IsLogged)
			{
				return;
			}

			if (Response == null)
			{
				return;
			}

			var loginUser = CurrentUser;

			var loginInfoCookie = Response.Cookies[cookieLoginInfoKey];
			loginInfoCookie = (loginInfoCookie ?? new HttpCookie(cookieLoginInfoKey));

			var passwordHash = CryptographyUtil.ComputeMD5(loginUser.Password);

			loginInfoCookie[cookieLoginNameKey] = loginUser.LoginName;
			loginInfoCookie[cookiePasswordKey] = passwordHash;

			Response.Cookies.Set(loginInfoCookie);
		}

		public void ClearCurrentLoginCookies()
		{
			if (Response == null)
			{
				return;
			}

			var loginInfoCookie = Response.Cookies[cookieLoginInfoKey];
			loginInfoCookie = (loginInfoCookie ?? new HttpCookie(cookieLoginInfoKey));

			loginInfoCookie.Expires = DateTime.Now.AddDays(15);
			loginInfoCookie[cookieLoginNameKey] = String.Empty;
			loginInfoCookie[cookiePasswordKey] = String.Empty;

			Response.Cookies.Set(loginInfoCookie);
		}

		public void GoToLogin()
		{
			var loginPageUrl = PermissionUtil.LoginPage;
			if (String.IsNullOrWhiteSpace(loginPageUrl))
			{
				return;
			}

			if (Request == null || Response == null)
			{
				return;
			}

			var loginUri = new Uri(loginPageUrl, UriKind.RelativeOrAbsolute);

			var loginPagePath = loginUri.GetLeftPart(UriPartial.Path);
			var currPagePath = Request.Url.GetLeftPart(UriPartial.Path);

			if (defComparer.Equals(loginPagePath, currPagePath))
			{
				return;
			}

			var returnUrlHelper = new UrlHelper(Request.Url.ToString());
			returnUrlHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();

			var returnUrl = returnUrlHelper.ToString();

			var urlHelper = new UrlHelper(loginPageUrl);
			urlHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();
			urlHelper[requestUtcHashCodeKey] = (uint)DateTime.Now.Ticks.GetHashCode();
			urlHelper[requestReturnUrlKey] = GetEncodedUrl(returnUrl);

			Response.Redirect(urlHelper.ToString());
		}
		public void GoToLogin(String returnUrl)
		{
			var loginPageUrl = PermissionUtil.LoginPage;
			if (String.IsNullOrWhiteSpace(loginPageUrl))
			{
				return;
			}

			if (Request == null || Response == null)
			{
				return;
			}

			var loginUri = new Uri(loginPageUrl, UriKind.RelativeOrAbsolute);

			var loginPagePath = loginUri.GetLeftPart(UriPartial.Path);
			var currPagePath = Request.Url.GetLeftPart(UriPartial.Path);

			if (defComparer.Equals(loginPagePath, currPagePath))
			{
				return;
			}

			var urlHelper = new UrlHelper(loginPageUrl);
			urlHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();

			if (!String.IsNullOrWhiteSpace(returnUrl))
			{
				var returnUrlHelper = new UrlHelper(returnUrl);
				returnUrlHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();

				returnUrl = returnUrlHelper.ToString();

				urlHelper[requestUtcHashCodeKey] = (uint)DateTime.Now.Ticks.GetHashCode();
				urlHelper[requestReturnUrlKey] = GetEncodedUrl(returnUrl);
			}

			Response.Redirect(urlHelper.ToString());
		}

		public void GoToLogout()
		{
			GoToLogout(Request.Url.ToString());
		}
		public void GoToLogout(bool clearSession)
		{
			GoToLogout(Request.Url.ToString(), clearSession);
		}

		public void GoToLogout(String returnUrl)
		{
			GoToLogout(returnUrl, false);
		}
		public void GoToLogout(String returnUrl, bool clearSession)
		{
			//TODO: Move to special Method
			Logout();

			var logoutPageUrl = PermissionUtil.LogoutPage;
			if (String.IsNullOrWhiteSpace(logoutPageUrl))
			{
				return;
			}

			if (Request == null || Response == null)
			{
				return;
			}

			var logioutUri = new Uri(logoutPageUrl, UriKind.RelativeOrAbsolute);

			var logoutPagePath = logioutUri.GetLeftPart(UriPartial.Path);
			var currPagePath = Request.Url.GetLeftPart(UriPartial.Path);

			if (defComparer.Equals(logoutPagePath, currPagePath))
			{
				return;
			}

			var urlHelper = new UrlHelper(logoutPageUrl);
			urlHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();

			if (!String.IsNullOrWhiteSpace(returnUrl))
			{
				var returnUrlHelper = new UrlHelper(returnUrl);
				returnUrlHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();

				returnUrl = returnUrlHelper.ToString();

				urlHelper[requestUtcHashCodeKey] = (uint)DateTime.Now.Ticks.GetHashCode();
				urlHelper[requestReturnUrlKey] = GetEncodedUrl(returnUrl);
			}

			if (Session != null && clearSession)
			{
				Session.Clear();
			}

			Response.Redirect(urlHelper.ToString());
		}

		public void GoToChangePassword()
		{
			GoToChangePassword(Request.Url.ToString());
		}
		public void GoToChangePassword(String returnUrl)
		{
			var changePasswordPageUrl = PermissionUtil.ChangePasswordPage;
			if (String.IsNullOrWhiteSpace(changePasswordPageUrl))
			{
				return;
			}

			if (Request == null || Response == null)
			{
				return;
			}

			var logioutUri = new Uri(changePasswordPageUrl, UriKind.RelativeOrAbsolute);

			var logoutPagePath = logioutUri.GetLeftPart(UriPartial.Path);
			var currPagePath = Request.Url.GetLeftPart(UriPartial.Path);

			if (defComparer.Equals(logoutPagePath, currPagePath))
			{
				return;
			}

			var urlHelper = new UrlHelper(changePasswordPageUrl);
			urlHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();
			urlHelper[requestLoginTokenKey] = CurrentToken;

			if (!String.IsNullOrWhiteSpace(returnUrl))
			{
				var returnUrlHelper = new UrlHelper(returnUrl);
				returnUrlHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();

				returnUrl = returnUrlHelper.ToString();

				urlHelper[requestUtcHashCodeKey] = (uint)DateTime.Now.Ticks.GetHashCode();
				urlHelper[requestReturnUrlKey] = GetEncodedUrl(returnUrl);
			}

			Response.Redirect(urlHelper.ToString());
		}

		public bool Login()
		{
			var loginInfo = ExtractLoginInfo();

			if (IsLogged)
			{
				if (loginInfo != null &&
					loginInfo.LoginToken != null &&
					loginInfo.LoginToken == CurrentToken)
				{
					return true;
				}
				else
				{
					if (loginInfo != null)
					{
						loginInfo.LoginToken = CurrentToken;
					}
					SaveCurrentTokenCookies();
				}
			}

			if (loginInfo == null)
			{
				return false;
			}

			if (loginInfo.LoginToken != null)
			{
				return Login(loginInfo.LoginToken.Value);
			}

			var loginName = loginInfo.LoginName;
			var password = loginInfo.Password;

			if (String.IsNullOrEmpty(loginName) || String.IsNullOrEmpty(password))
			{
				return false;
			}

			return Login(loginName, password, true);
		}

		public bool Login(Guid token)
		{
			if (ProjectID == null)
			{
				return false;
			}

			if (!IsTokenValid(token))
			{
				return false;
			}

			if (CurrentToken == token)
			{
				return true;
			}

			ResetInstanceData();

			var loginUser = UserManagementProxy.GetCurrentUser(token);

			var success = loginUser != null;
			if (success)
			{
				var userGroups = UserManagementProxy.GetUserGroups(token, loginUser.ID, ProjectID.Value);

				if (!loginUser.IsSuperAdmin && !IgnoreGroupMembership && (userGroups == null || userGroups.Count == 0))
				{
					return false;
				}

				currentToken = token;
				currentUser = loginUser;
				currentUserGroups = userGroups;

				SaveCurrentTokenCookies();
			}
			LogEvent("LoginByToken", loginUser, token, success);

			return success;
		}

		public bool Login(String loginName, String password)
		{
			return Login(loginName, password, false);
		}

		public bool Login(String loginName, String password, bool encryptedPassword)
		{
			if (ProjectID == null)
			{
				return false;
			}

			if (IsLogged && defComparer.Equals(CurrentUser.LoginName, loginName))
			{
				if (encryptedPassword)
				{
					var passwordHash = CryptographyUtil.ComputeMD5(CurrentUser.Password);
					if (defComparer.Equals(passwordHash, password))
					{
						return true;
					}
				}
				else if (CurrentUser.Password == password)
				{
					return true;
				}
			}

			ResetInstanceData();

			var token = UserManagementProxy.Login(loginName, password, encryptedPassword);

			var success = token != null;
			if (success)
			{
				var loginUser = UserManagementProxy.GetCurrentUser(token.Value);
				if (loginUser == null)
				{
					return false;
				}

				var userGroups = UserManagementProxy.GetUserGroups(token.Value, loginUser.ID, ProjectID.Value);

				if (!loginUser.IsSuperAdmin && !IgnoreGroupMembership && (userGroups == null || userGroups.Count == 0))
				{
					return false;
				}

				currentToken = token;
				currentUser = loginUser;
				currentUserGroups = userGroups;

				SaveCurrentTokenCookies();
			}

			LogEvent("LoginByName", loginName, password, token, success);

			return success;
		}

		public void Logout()
		{
			if (CurrentToken != null)
			{
				LogEvent("Logout");

				UserManagementProxy.Logout(CurrentToken.Value);
			}

			ResetInstanceData();

			ClearCurrentTokenCookies();
			ClearCurrentLoginCookies();
		}

		public bool HasMessages()
		{
			if (!IsLogged)
			{
				return false;
			}

			if (UserManagementProxy.HasMessages(CurrentToken.Value, ProjectID))
			{
				var allMessages = UserManagementProxy.GetMessages(CurrentToken.Value, ProjectID);
				if (allMessages != null && allMessages.Count > 0)
				{
					var hasMessages = (from n in allMessages
									   where n != null && !displayedMessages.Contains(n.ID)
									   select n).Any();

					return hasMessages;
				}
			}

			return false;
		}

		public bool HasAccess(String key)
		{
			var permissionElement = PermissionUtil.GetPermissionElement(key);
			if (permissionElement == null)
			{
				return false;
			}

			var resourcePath = permissionElement.ResourcePath;
			var ruleValue = PermissionUtil.ParseRuleValue(permissionElement.RuleValue);

			return HasAccess(resourcePath, ruleValue);
		}

		public bool HasAccess(String resourcePath, RulePermissionsEnum ruleValue)
		{
			if (CurrentToken == null)
			{
				return false;
			}

			if (IsPasswordExpired)
			{
				return false;
			}

			if (CurrentUser != null && CurrentUser.IsSuperAdmin)
			{
				return true;
			}

			var permission = GetResourcePermission(resourcePath);
			return (permission != null && permission.RuleValue.HasFlag(ruleValue));
		}

		public bool IsUserInGroup(String groupName)
		{
			return CurrentUserGroups != null && CurrentUserGroups.Any(item => defComparer.Equals(item.Name, groupName));
		}

		public void PreloadUserPermissions()
		{
			if (CurrentToken == null || ProjectID == null)
			{
				return;
			}

			var allPermissions = UserManagementProxy.GetAllResourcesPermissions(CurrentToken.Value, ProjectID);
			if (allPermissions == null)
			{
				return;
			}

			var permsByProjects = allPermissions.ToLookup(n => n.ProjectID);
			foreach (var permsByProjectGrp in permsByProjects)
			{
				var permsByPathsLp = permsByProjectGrp.ToLookup(n => n.ResourcePath);

				foreach (var permsByPathGrp in permsByPathsLp)
				{
					var permsByRuleValue = permsByPathGrp.OrderByDescending(n => n.RuleValue);
					var newPermContract = permsByRuleValue.First();

					var path = String.Format("{0}/{1}", permsByProjectGrp.Key, permsByPathGrp.Key);
					PermissionsCache[path] = newPermContract;
				}
			}
		}

		public void PreloadPermissions(IEnumerable<String> permissions)
		{
			var @set = new HashSet<String>(permissions);
			if (EnabledPermissionsCache && PermissionsCache != null)
				@set.ExceptWith(PermissionsCache.Keys);
		}

		public PasswordChangeResultEnum ChangePassword(String newPassword, String oldPassword)
		{
			if (CurrentToken == null)
			{
				return PasswordChangeResultEnum.TokenNotFound;
			}

			var result = UserManagementProxy.ChangePassword(CurrentToken.Value, newPassword, oldPassword);
			var success = result == PasswordChangeResultEnum.Success;

			LogEvent("ChangePassword", success);

			if (success)
			{
				var currentLoginName = CurrentUser.LoginName;
				var remember = IsCurrentLoginInCookies();

				Logout();
				Login(currentLoginName, newPassword);

				if (remember)
				{
					SaveCurrentLoginCookies();
				}
				else
				{
					ClearCurrentLoginCookies();
				}
			}

			return result;
		}

		public List<UserContract> GetGroupUsers(Guid groupID)
		{
			if (!IsLogged)
			{
				return null;
			}

			if (CurrentToken != null)
			{
				return UserManagementProxy.GetGroupUsers(CurrentToken.Value, groupID);
			}

			return null;
		}

		public List<ProjectContract> GetProjectsByUserToken()
		{
			if (CurrentToken == null)
			{
				return null;
			}

			return UserManagementProxy.GetProjectByUserToken(CurrentToken.Value);
		}

		public PermissionContract GetResourcePermission(String path)
		{
			if (IsPasswordExpired || String.IsNullOrWhiteSpace(path))
			{
				return null;
			}

			path = String.Format("{0}/{1}", ProjectID, path);

			if (CurrentToken == null)
			{
				return null;
			}

			var permission = LoadPermission(path);
			return permission;
		}

		private PermissionContract LoadPermission(String path)
		{
			PermissionContract permission;

			if (EnabledPermissionsCache)
			{
				if (PermissionsCache.TryGetValue(path, out permission))
				{
					return permission;
				}

				if (EnabledHierachycalSearch)
				{
					var pathParts = path.Split('/');

					for (int i = (pathParts.Length - 1); i > 0; i--)
					{
						var parentPath = String.Join("/", pathParts, 0, i);

						if (PermissionsCache.TryGetValue(parentPath, out permission))
						{
							return permission;
						}
					}
				}
			}

			permission = UserManagementProxy.GetResourcePermission(CurrentToken.Value, path);
			if (EnabledPermissionsCache)
			{
				PermissionsCache.Add(path, permission);
			}

			return permission;
		}

		public List<MessageContract> GetMessages()
		{
			if (CurrentToken == null || !IsLogged)
			{
				return null;
			}

			var allMessages = UserManagementProxy.GetMessages(CurrentToken.Value, ProjectID);
			if (allMessages == null)
			{
				return null;
			}

			var notShowedMessages = (from n in allMessages
									 where n != null && displayedMessages.Add(n.ID)
									 select n).ToList();

			return notShowedMessages;
		}

		private bool IsTokenValid(Guid? token)
		{
			if (token == null)
			{
				return false;
			}

			if (Context != null)
			{
				var isActual = Context.Items[isActualRequestKey] as bool?;
				if (isActual == null)
				{
					isActual = UserManagementProxy.IsTokenActual(token.Value);
					Context.Items[isActualRequestKey] = isActual;
				}

				return isActual.GetValueOrDefault();
			}

			return UserManagementProxy.IsTokenActual(token.Value);
		}

		private String GetEncodedUrl(String url)
		{
			var bytes = defEncoding.GetBytes(url);
			return HttpServerUtility.UrlTokenEncode(bytes);
		}

		private LoginInfo ExtractLoginInfo()
		{
			if (Request == null)
			{
				return null;
			}

			var loginInfo = new LoginInfo();

			Guid token;
			if (Guid.TryParse(Request[requestLoginTokenKey], out token))
			{
				currentToken = token;
				loginInfo.LoginToken = token;
				if (IsPasswordExpired)
				{
					currentUser = null;
				}
			}

			var userInfoCookie = Request.Cookies[cookieLoginInfoKey];
			if (userInfoCookie == null)
			{
				return loginInfo;
			}

			loginInfo.LoginName = userInfoCookie[cookieLoginNameKey];
			loginInfo.Password = userInfoCookie[cookiePasswordKey];

			if (loginInfo.LoginToken == null)
			{
				var strToken = userInfoCookie[cookieLoginTokenKey];
				if (Guid.TryParse(strToken, out token))
				{
					loginInfo.LoginToken = token;
				}
			}

			return loginInfo;
		}

		private void SaveCurrentTokenCookies()
		{
			if (!IsLogged)
			{
				return;
			}

			if (Response == null)
			{
				return;
			}

			var loginInfoCookie = Response.Cookies[cookieLoginInfoKey];
			loginInfoCookie = (loginInfoCookie ?? new HttpCookie(cookieLoginInfoKey));

			loginInfoCookie[cookieLoginTokenKey] = Convert.ToString(CurrentToken);

			Response.Cookies.Set(loginInfoCookie);
		}

		private void ClearCurrentTokenCookies()
		{
			if (!IsLogged)
			{
				return;
			}

			if (Response == null)
			{
				return;
			}

			var loginInfoCookie = Response.Cookies[cookieLoginInfoKey];
			loginInfoCookie = (loginInfoCookie ?? new HttpCookie(cookieLoginInfoKey));

			loginInfoCookie.Expires = DateTime.Now.AddDays(15);
			loginInfoCookie[cookieLoginTokenKey] = String.Empty;

			Response.Cookies.Set(loginInfoCookie);
		}

		private void ResetInstanceData()
		{
			currentToken = null;
			currentUser = null;
			currentUserGroups = null;
			currentUserAttributes = null;
			userProjects = null;
			permissionsCache = null;

			displayedMessages.Clear();
		}

		private void LogEvent(String eventName)
		{
			LogEvent(eventName, CurrentUser, CurrentToken, null);
		}
		private void LogEvent(String eventName, Guid? token)
		{
			LogEvent(eventName, CurrentUser, token, null);
		}
		private void LogEvent(String eventName, bool? success)
		{
			LogEvent(eventName, CurrentUser, success);
		}

		private void LogEvent(String eventName, UserContract loginUser)
		{
			LogEvent(eventName, loginUser, CurrentToken);
		}
		private void LogEvent(String eventName, UserContract loginUser, Guid? token)
		{
			LogEvent(eventName, loginUser, token, null);
		}
		private void LogEvent(String eventName, UserContract loginUser, bool? success)
		{
			LogEvent(eventName, loginUser, CurrentToken, success);
		}
		private void LogEvent(String eventName, UserContract loginUser, Guid? token, bool? success)
		{
			if (loginUser != null)
			{
				LogEvent(eventName, loginUser.LoginName, loginUser.Password, token, success);
			}
			else
			{
				LogEvent(eventName, null, null, token, success);
			}
		}

		private void LogEvent(String eventName, String loginName, String password)
		{
			LogEvent(eventName, loginName, password, CurrentToken);
		}
		private void LogEvent(String eventName, String loginName, String password, Guid? token)
		{
			LogEvent(eventName, loginName, password, token, null);
		}
		private void LogEvent(String eventName, String loginName, String password, bool? success)
		{
			LogEvent(eventName, loginName, password, CurrentToken, success);
		}
		private void LogEvent(String eventName, String loginName, String password, Guid? token, bool? success)
		{
			if (Logger != null)
			{
				var clientIP = String.Empty;

				if (Request != null)
				{
					clientIP = String.Format("{0}, {1}", Request.UserHostAddress, Request.UserHostName);
				}

				var sessionID = (Session != null ? Session.SessionID : String.Empty);

				var logText = String.Format("Event: {0}; LoginName: {1}; Password: {2}; ProjectID: {3}; Token: {4}; SessionID: {5}; ClientIP: {6}; Success: {7}",
											eventName, loginName, password, ProjectID, token, sessionID, clientIP, success);

				Logger.Info(logText);
			}
		}

		#endregion

		#region static methods
		private static void ResetSessionIfNeeded()
		{
			var session = Session;
			if (session == null)
			{
				return;
			}

			lock (resetInitedSessions)
			{
				if (!resetInitedSessions.Add(session.SessionID))
				{
					return;
				}
			}

			if (session.Mode == SessionStateMode.SQLServer || session.Mode == SessionStateMode.StateServer)
			{
				var index = 0;
				var count = session.Count;

				while (index < count)
				{
					try
					{
						var tempObj = session[index];

						index++;
					}
					catch (Exception)
					{
						session.RemoveAt(index);

						count--;
					}
				}
			}
		}

		private static void InitAssemblyResolver()
		{
			if (assemblyResolverInited)
			{
				return;
			}

			assemblyResolverInited = true;
		}

		private static ILog LoadLogger()
		{
			return LogUtil.GetLogger("LoginLogger");
		}

		private static UmUtil GetOutOfSessionInstance()
		{
			var process = Process.GetCurrentProcess();

			if (!HostingEnvironment.IsHosted)
			{
				var instance = GetProcessWideInstance(process);
				return instance;
			}
			else
			{
				var instance = GetThreadWideInstance(process);
				return instance;
			}
		}

		private static UmUtil GetProcessWideInstance(Process process)
		{
			if (process == null)
			{
				throw new ArgumentNullException("process");
			}

			CleanInstances(process);

			var instance = CreateOrGetInstance(process.Id);
			return instance;
		}

		private static UmUtil GetThreadWideInstance(Process process)
		{
			if (process == null)
			{
				throw new ArgumentNullException("process");
			}

			CleanInstances(process);

			var threadId = GetCurrentThreadId();

			var instance = CreateOrGetInstance(threadId);
			return instance;
		}

		private static UmUtil CreateOrGetInstance(int instanceID)
		{
			var instance = lazyInstances.GetValueOrDefault(instanceID);
			if (instance == null)
			{
				instance = new UmUtil();
				lazyInstances[instanceID] = instance;
			}

			return instance;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private static void CleanInstances(Process process)
		{
			if (process == null)
			{
				throw new ArgumentNullException("process");
			}

			var threadIds = (from ProcessThread n in process.Threads
							 select n.Id).ToHashSet();

			var dispInstances = from n in lazyInstances
								where !threadIds.Contains(n.Key)
								select n.Key;

			foreach (var threadId in dispInstances)
			{
				var intance = lazyInstances[threadId];
				intance.Dispose();
			}

			CleanInstances();
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private static void CleanInstances()
		{
			var disposeds = from n in lazyInstances
							where n.Value.disposed
							select n.Key;

			foreach (var key in disposeds)
			{
				lazyInstances.Remove(key);
			}
		}

		#endregion

		#region nested classes

		private class LoginInfo
		{
			public Guid? LoginToken { get; set; }

			public String LoginName { get; set; }
			public String Password { get; set; }
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern. 
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			if (disposing)
			{
				// Free any other managed objects here. 
				//
			}

			// Free any unmanaged objects here. 
			//
			disposed = true;
		}

		#endregion

	}

}
