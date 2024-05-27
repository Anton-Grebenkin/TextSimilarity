using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using QueryFilter.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Drawing;

namespace TextSimilarity.API.Common.Swagger
{
    public class QueryFilterSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(LogicalOperatorType) || context.Type == typeof(ExpressionOperatorType)) // Укажите тут ваш тип Enum
            {
                schema.Enum.Clear();
                foreach (var enumName in Enum.GetNames(context.Type))
                {
                    schema.Enum.Add(new OpenApiString(enumName));
                }
                schema.Type = "string";
            }

        }

    }
}
