using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiNibu.Helpers
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParameters = context.ApiDescription.ParameterDescriptions
                .Where(p => p.Type == typeof(IFormFile) || p.Type == typeof(IFormFile[])
                            || (p.Type.IsGenericType && p.Type.GetGenericArguments().FirstOrDefault() == typeof(IFormFile)))
                .ToList();

            if (!fileParameters.Any()) return;

            operation.Parameters?.Clear();

            var schema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>()
            };

            var required = new HashSet<string>();

            foreach (var param in context.ApiDescription.ParameterDescriptions)
            {
                var name = param.Name;
                var paramType = param.Type;

                if (paramType == typeof(IFormFile) || paramType == typeof(IFormFile[]) ||
                    (paramType.IsGenericType && paramType.GetGenericArguments().FirstOrDefault() == typeof(IFormFile)))
                {
                    schema.Properties[name] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    };

                    if (param.IsRequired)
                        required.Add(name);

                    continue;
                }

                schema.Properties[name] = MapTypeToSchema(paramType);

                if (param.IsRequired)
                    required.Add(name);
            }

            if (required.Any())
                schema.Required = new SortedSet<string>(required);

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = schema
                    }
                }
            };
        }

        private OpenApiSchema MapTypeToSchema(Type type)
        {
            if (type == typeof(string))
                return new OpenApiSchema { Type = "string" };
            if (type == typeof(int) || type == typeof(long) || type == typeof(short))
                return new OpenApiSchema { Type = "integer", Format = "int32" };
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                return new OpenApiSchema { Type = "number", Format = "double" };
            if (type == typeof(bool))
                return new OpenApiSchema { Type = "boolean" };
            if (type == typeof(DateTime))
                return new OpenApiSchema { Type = "string", Format = "date-time" };
            return new OpenApiSchema { Type = "string" };
        }
    }
}
