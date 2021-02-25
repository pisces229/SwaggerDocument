using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using SwaggerDocument.OperationProcessor.Attributes;
using System.Linq;

namespace SwaggerDocument.OperationProcessor
{
    public class AddRequiredHeaderParameter : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            if ("PostJsonResponseValue".Equals(context.MethodInfo.GetBaseDefinition().Name))
            {
                context.OperationDescription.Operation.Parameters.Add(
                    new OpenApiParameter()
                    {
                        Name = "Token",
                        Kind = OpenApiParameterKind.Header,
                        Type = JsonObjectType.String,
                        IsRequired = true,
                        Description = "Token Description",
                        Default = "",
                    });
            }
            //if (context.MethodInfo.CustomAttributes
            //    .Where(w => w.AttributeType == typeof(OperationHeaderToken))
            //    .Any())
            //{
            //    context.OperationDescription.Operation.Parameters.Add(
            //        new OpenApiParameter()
            //        {
            //            Name = "Token",
            //            Kind = OpenApiParameterKind.Header,
            //            Type = JsonObjectType.String,
            //            IsRequired = true,
            //            Description = "Token Description",
            //            Default = "",
            //        });
            //}
            return true;
        }
    }
}
