using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using MongoDB.Driver;
using NHibernate.Linq;

namespace Gms.Portal.Web.Utils
{

    public static class RecipientsGroupUtil
    {
        public static IEnumerable<RecipientModel> GetRecipients(Guid? groupID)
        {
            if (groupID == null)
                return null;

            var session = Hb8Factory.InitSession();

            var group = session.Query<GM_RecipientGroup>().FirstOrDefault(n => n.ID == groupID);
            if (group == null)
                return null;

            return GetRecipients(group);
        }

        public static IEnumerable<RecipientModel> GetRecipients(GM_RecipientGroup group)
        {
            var session = Hb8Factory.InitSession();

            if (group.Type == "Expression")
            {
                if (group.FormID == null)
                    return null;


                var dbForm = session.Query<GM_Form>().FirstOrDefault(n => n.ID == group.FormID);
                if (dbForm == null)
                    return null;

                var collection = MongoDbUtil.GetCollection(group.FormID);

                var query = (from n in collection.AsQueryable()
                             where n[FormDataConstants.DateDeletedField] == (DateTime?)null
                             select n);

                if (String.IsNullOrWhiteSpace(group.Expression))
                {
                    var @set = query.Select(n => n[FormDataConstants.UserIDField].AsNullableGuid).ToHashSet();
                    return ConvertUsersToRecipients(@set, group.ID);
                }
                else
                {
                    var formConverter = new FormEntityModelConverter(session);
                    var formModel = formConverter.Convert(dbForm);

                    var allControls = FormStructureUtil.PreOrderFirstLevelTraversal(formModel.Entity);

                    var compitablityDict = allControls.ToDictionary(n => Convert.ToString(n.ID), n => n.Name);

                    foreach (var defaultField in FormDataBase.DefaultFields)
                        compitablityDict[defaultField] = defaultField;

                    var @set = new HashSet<Guid?>();
                    var node = ExpressionParser.GetOrParse(group.Expression);

                    foreach (var document in query)
                    {
                        var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
                        var adpFormData = FormDataUtil.Transform(formData, compitablityDict);

                        var expGlobals = new ExpressionGlobalsUtil(adpFormData);

                        Object value;
                        if (ExpressionEvaluator.TryEval(node, expGlobals.Eval, out value))
                        {
                            var @bool = DataConverter.ToNullableBool(value);
                            if (@bool.GetValueOrDefault())
                            {
                                if (formData.UserID != null)
                                    @set.Add(formData.UserID);
                            }
                        }
                    }

                    return ConvertUsersToRecipients(@set, group.ID);
                }
            }

            var entities = (from n in @group.Recipients
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new RecipientEntityModelConverter(session);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            return models;
        }

        private static IEnumerable<RecipientModel> ConvertUsersToRecipients(IEnumerable<Guid?> usersID, Guid groupID)
        {
            foreach (var userID in usersID)
            {
                var user = UmUsersCache.GetUser(userID);
                if (user == null)
                    continue;

                var model = new RecipientModel
                {
                    ID = user.ID,
                    UserID = user.ID,
                    GroupID = groupID,
                    Email = user.Email,
                    UserName = user.LoginName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                };

                yield return model;
            }
        }
    }
}