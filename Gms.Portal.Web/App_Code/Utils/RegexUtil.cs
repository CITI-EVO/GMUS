using System.Text.RegularExpressions;

namespace Gms.Portal.Web.Utils
{
    public static class RegexUtil
    {
        public static readonly Regex CommandRx = new Regex(@"(?<ownerID>.*)/(?<recordID>.*)/(?<containerID>.*)", RegexOptions.Compiled);
        public static readonly Regex EmailCheckRx = new Regex(@"(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$)", RegexOptions.Compiled);
        public static readonly Regex DataGridFuncParserRx = new Regex(@"^(?<form>.+?)\.(?<grid>.+?)($|\.(?<col>.+?))$", RegexOptions.Compiled);
        public static readonly Regex FilterElementParserRx = new Regex(@"(?<type>\w+)_(?<index>\d+)_(?<elemID>.+)", RegexOptions.Compiled);
        public static readonly Regex SortingFieldsParserRx = new Regex(@"(?<name>.+?)(?<type>(asc|desc)?)($|,)", RegexOptions.Compiled);
        public static readonly Regex FillingValidationRx = new Regex(@"^{(?<form>.+?)\.(?<field>.+?)}$", RegexOptions.Compiled);
        public static readonly Regex DataSourceParserRx = new Regex(@"(?<parentID>.*)/(?<childID>.*)", RegexOptions.Compiled);
        public static readonly Regex FormValueParserRx = new Regex(@"(\$|^)(?<type>\w+)_(?<elemID>.+)$", RegexOptions.Compiled);
    }
}