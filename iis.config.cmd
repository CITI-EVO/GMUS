echo off

%systemdrive%

START /WAIT DISM /Online /Enable-Feature /FeatureName:IIS-ApplicationDevelopment /FeatureName:IIS-ASP /FeatureName:IIS-ASPNET /FeatureName:IIS-BasicAuthentication /FeatureName:IIS-CGI /FeatureName:IIS-ClientCertificateMappingAuthentication /FeatureName:IIS-CommonHttpFeatures /FeatureName:IIS-CustomLogging /FeatureName:IIS-DefaultDocument /FeatureName:IIS-DigestAuthentication /FeatureName:IIS-DirectoryBrowsing /FeatureName:IIS-FTPExtensibility /FeatureName:IIS-FTPServer /FeatureName:IIS-FTPSvc /FeatureName:IIS-HealthAndDiagnostics /FeatureName:IIS-HostableWebCore /FeatureName:IIS-HttpCompressionDynamic /FeatureName:IIS-HttpCompressionStatic /FeatureName:IIS-HttpErrors /FeatureName:IIS-HttpLogging /FeatureName:IIS-HttpRedirect /FeatureName:IIS-HttpTracing /FeatureName:IIS-IIS6ManagementCompatibility /FeatureName:IIS-IISCertificateMappingAuthentication /FeatureName:IIS-IPSecurity /FeatureName:IIS-ISAPIExtensions /FeatureName:IIS-ISAPIFilter /FeatureName:IIS-LegacyScripts /FeatureName:IIS-LegacySnapIn /FeatureName:IIS-LoggingLibraries /FeatureName:IIS-ManagementConsole /FeatureName:IIS-ManagementScriptingTools /FeatureName:IIS-ManagementService /FeatureName:IIS-Metabase /FeatureName:IIS-NetFxExtensibility /FeatureName:IIS-ODBCLogging /FeatureName:IIS-Performance /FeatureName:IIS-RequestFiltering /FeatureName:IIS-RequestMonitor /FeatureName:IIS-Security /FeatureName:IIS-ServerSideIncludes /FeatureName:IIS-StaticContent /FeatureName:IIS-URLAuthorization /FeatureName:IIS-WebDAV /FeatureName:IIS-WebServer /FeatureName:IIS-WebServerManagementTools /FeatureName:IIS-WebServerRole /FeatureName:IIS-WindowsAuthentication /FeatureName:IIS-WMICompatibility /FeatureName:WAS-ConfigurationAPI /FeatureName:WAS-NetFxEnvironment /FeatureName:WAS-ProcessModel /FeatureName:WAS-WindowsActivationService

set sourcesPath=%~dp0

%windir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -i
%windir%\Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe -i

cd %windir%\system32\inetsrv

appcmd set apppool /apppool.name:DefaultAppPool /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated

appcmd set app /app.name:"Default Web Site/" /[path='/'].physicalPath:"C:\inetpub\wwwroot"
appcmd set app /app.name:"Default Web Site/" /applicationPool:DefaultAppPool

appcmd add apppool /name:Moe.Base /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd add apppool /name:Gms.Base /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd add apppool /name:Gms.Portal.Web /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd add apppool /name:CITI.EVO.RpcHub.Web /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd add apppool /name:CITI.EVO.CommonData.Web /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd add apppool /name:CITI.EVO.UserManagement.Web /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated

appcmd set apppool /apppool.name:Moe.Base /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd set apppool /apppool.name:Gms.Base /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd set apppool /apppool.name:Gms.Portal.Web /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd set apppool /apppool.name:CITI.EVO.RpcHub.Web /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd set apppool /apppool.name:CITI.EVO.CommonData.Web /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
appcmd set apppool /apppool.name:CITI.EVO.UserManagement.Web /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated

appcmd add app /site.name:"Default Web Site" /path:"/Rnsf" /physicalPath:"C:\inetpub\wwwroot\Base"
appcmd add app /site.name:"Default Web Site" /path:"/Rnsf/Gms" /physicalPath:"C:\inetpub\wwwroot\Base"
appcmd add app /site.name:"Default Web Site" /path:"/Rnsf/Gms/Gms.Portal.Web" /physicalPath:"%sourcesPath%Gms.Portal.Web"
appcmd add app /site.name:"Default Web Site" /path:"/Rnsf/Gms/CITI.EVO.RpcHub.Web" /physicalPath:"%sourcesPath%CITI.EVO.RpcHub.Web"
appcmd add app /site.name:"Default Web Site" /path:"/Rnsf/Gms/CITI.EVO.CommonData.Web" /physicalPath:"%sourcesPath%CITI.EVO.CommonData.Web"
appcmd add app /site.name:"Default Web Site" /path:"/Rnsf/Gms/CITI.EVO.UserManagement.Web" /physicalPath:"%sourcesPath%CITI.EVO.UserManagement.Web"

appcmd set app /app.name:"Default Web Site/Rnsf" /[path='/'].physicalPath:"C:\inetpub\wwwroot\Base"
appcmd set app /app.name:"Default Web Site/Rnsf/Gms" /[path='/'].physicalPath:"C:\inetpub\wwwroot\Base"
appcmd set app /app.name:"Default Web Site/Rnsf/Gms/Gms.Portal.Web" /[path='/'].physicalPath:"%sourcesPath%Gms.Portal.Web"
appcmd set app /app.name:"Default Web Site/Rnsf/Gms/CITI.EVO.RpcHub.Web" /[path='/'].physicalPath:"%sourcesPath%CITI.EVO.RpcHub.Web"
appcmd set app /app.name:"Default Web Site/Rnsf/Gms/CITI.EVO.CommonData.Web" /[path='/'].physicalPath:"%sourcesPath%CITI.EVO.CommonData.Web"
appcmd set app /app.name:"Default Web Site/Rnsf/Gms/CITI.EVO.UserManagement.Web" /[path='/'].physicalPath:"%sourcesPath%CITI.EVO.UserManagement.Web"

appcmd set app /app.name:"Default Web Site/Rnsf" /[path='/'].physicalPath:"C:\inetpub\wwwroot\Base"
appcmd set app /app.name:"Default Web Site/Rnsf/Gms" /[path='/'].physicalPath:"C:\inetpub\wwwroot\Base"
appcmd set app /app.name:"Default Web Site/Rnsf/Gms/Gms.Portal.Web" /applicationPool:Gms.Portal.Web
appcmd set app /app.name:"Default Web Site/Rnsf/Gms/CITI.EVO.RpcHub.Web" /applicationPool:CITI.EVO.RpcHub.Web
appcmd set app /app.name:"Default Web Site/Rnsf/Gms/CITI.EVO.CommonData.Web" /applicationPool:CITI.EVO.CommonData.Web
appcmd set app /app.name:"Default Web Site/Rnsf/Gms/CITI.EVO.UserManagement.Web" /applicationPool:CITI.EVO.UserManagement.Web

pause