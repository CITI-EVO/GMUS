using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Transform;

namespace CITI.EVO.Tools.Helpers.Hibern8
{
    public class DictionaryTransformer : IResultTransformer
    {
        public static readonly DictionaryTransformer Transformer = new DictionaryTransformer();

        private DictionaryTransformer()
        {
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            var result = new Dictionary<String, Object>(aliases.Length);

            for (int i = 0; i < aliases.Length; i++)
            {
                var key = aliases[i];
                var value = tuple[i];

                result[key] = value;
            }

            return result;
        }

        public IList TransformList(IList collection)
        {
            return collection;
        }
    }
}
