using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Text;

namespace CITI.EVO.Tools.Web.Services
{
    [Serializable]
    public class DynamicProxyException : ApplicationException
    {
        public DynamicProxyException(string message)
            : base(message)
        {
            MetadataImportErrors = null;
            CodeGenerationErrors = null;
            CompilationErrors = null;
        }

        public DynamicProxyException(string message, Exception innerException)
            : base(message, innerException)
        {
            MetadataImportErrors = null;
            CodeGenerationErrors = null;
            CompilationErrors = null;
        }

        public IEnumerable<MetadataConversionError> MetadataImportErrors { get; internal set; }
        public IEnumerable<MetadataConversionError> CodeGenerationErrors { get; internal set; }
        public IEnumerable<CompilerError> CompilationErrors { get; internal set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.ToString());

            if (MetadataImportErrors != null)
            {
                stringBuilder.AppendLine("Metadata Import Errors:");
                stringBuilder.AppendLine(DynamicProxyLoader.ToString(MetadataImportErrors));
            }

            if (CodeGenerationErrors != null)
            {
                stringBuilder.AppendLine("Code Generation Errors:");
                stringBuilder.AppendLine(DynamicProxyLoader.ToString(CodeGenerationErrors));
            }

            if (CompilationErrors != null)
            {
                stringBuilder.AppendLine("Compilation Errors:");
                stringBuilder.AppendLine(DynamicProxyLoader.ToString(CompilationErrors));
            }

            return stringBuilder.ToString();
        }
    }
}