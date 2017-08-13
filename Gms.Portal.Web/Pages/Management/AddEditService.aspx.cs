using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CITI.EVO.Core.Common;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.Services;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Entities.ServiceStructure;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class AddEditService : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var serviceID = DataConverter.ToNullableGuid(RequestUrl["ServiceID"]);
            if (serviceID != null)
            {
                var entity = HbSession.Query<GM_Service>().FirstOrDefault(n => n.ID == serviceID);
                if (entity != null)
                {
                    var converter = new ServiceEntityModelConverter(HbSession);
                    var model = converter.Convert(entity);

                    serviceControl.Model = model;
                }
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var serviceID = DataConverter.ToNullableGuid(RequestUrl["ServiceID"]);

            var entity = HbSession.Query<GM_Service>().FirstOrDefault(n => n.ID == serviceID);
            if (entity == null)
                entity = EntityFactory.CreateEntity<GM_Service>();

            var converter = new ServiceModelEntityConverter(HbSession);
            var model = serviceControl.Model;

            converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);
        }

        protected void btnRefresh_OnClick(object sender, EventArgs e)
        {
            var model = serviceControl.Model;
            if (String.IsNullOrWhiteSpace(model.Url))
                return;

            var url = new UrlHelper(model.Url);

            var loader = DynamicProxyFactory.Load(url.ToUri());

            var serviceEntity = new ServiceEntity
            {
                ID = Guid.NewGuid(),
                Name = loader.ServiceContract.FullName,
                Classes = new List<ClassEntity>(),
                Methods = new List<MethodEntity>()
            };

            foreach (var dataContract in loader.DataContracts)
            {
                var classEntity = serviceEntity.Classes.FirstOrDefault(n => n.Name == dataContract.FullName);
                if (classEntity == null)
                {
                    classEntity = new ClassEntity
                    {
                        ID = Guid.NewGuid(),
                        Properties = new List<PropertyEntity>()
                    };

                    serviceEntity.Classes.Add(classEntity);
                }

                classEntity.Name = dataContract.FullName;

                foreach (var propertyInfo in dataContract.GetProperties())
                {
                    if (propertyInfo.PropertyType.GetInterface("IExtensibleDataObject") == null && propertyInfo.PropertyType != typeof(ExtensionDataObject))
                    {
                        var propertyEntity = classEntity.Properties.FirstOrDefault(n => n.Name == propertyInfo.Name);
                        if (propertyEntity == null)
                        {
                            propertyEntity = new PropertyEntity
                            {
                                ID = Guid.NewGuid()
                            };

                            classEntity.Properties.Add(propertyEntity);
                        }

                        propertyEntity.Name = propertyInfo.Name;
                        propertyEntity.Type = GetElementaryDataType(propertyInfo.PropertyType);
                        propertyEntity.IsPrimitive = IsPrimitive(propertyInfo.PropertyType);
                    }
                }
            }

            foreach (var methodInfo in loader.Methods)
            {
                var methodEntity = serviceEntity.Methods.FirstOrDefault(n => n.Name == methodInfo.Name);
                if (methodEntity == null)
                {
                    methodEntity = new MethodEntity
                    {
                        ID = Guid.NewGuid(),
                        Parameters = new List<ParameterEntity>()
                    };

                    serviceEntity.Methods.Add(methodEntity);
                }

                methodEntity.Name = methodInfo.Name;

                if (methodInfo.ReturnParameter != null)
                {
                    var returnParameter = methodInfo.ReturnParameter;

                    var returnEntity = new ReturnEntity
                    {
                        Type = GetElementaryDataType(returnParameter.ParameterType),
                        IsPrimitive = IsPrimitive(returnParameter.ParameterType)
                    };

                    methodEntity.Return = returnEntity;
                }

                var parameterInfos = methodInfo.GetParameters();
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    var parameterInfo = parameterInfos[i];

                    var parameterEntity = methodEntity.Parameters.FirstOrDefault(n => n.Name == parameterInfo.Name);
                    if (parameterEntity == null)
                    {
                        parameterEntity = new ParameterEntity
                        {
                            ID = Guid.NewGuid()
                        };

                        methodEntity.Parameters.Add(parameterEntity);
                    }

                    parameterEntity.ID = Guid.NewGuid();
                    parameterEntity.Name = parameterInfo.Name;
                    parameterEntity.IsOut = parameterInfo.IsOut;
                    parameterEntity.OrderIndex = i;
                    parameterEntity.ReturnType = GetElementaryDataType(parameterInfo.ParameterType);
                    parameterEntity.IsPrimitive = IsPrimitive(parameterInfo.ParameterType);
                }
            }

            model.Entity = serviceEntity;

            serviceControl.Model = model;
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            var returnUrl = new UrlHelper("~/Pages/Management/ServicesList.aspx");
            Response.Redirect(returnUrl.ToEncodedUrl());
        }

        private String GetElementaryDataType(Type type)
        {
            if (type.IsArray)
                return GetElementaryDataType(type.GetElementType()); //TODO: take array into account(save this info)

            if (type.IsGenericType)
                return GetElementaryDataType(type.GetGenericArguments()[0]); //TODO: determine what to do with generic types like Nullable<T>

            return type.FullName;
        }

        private bool IsPrimitive(Type type)
        {
            if (type.IsValueType)
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            return type == typeof(String);
        }
    }
}