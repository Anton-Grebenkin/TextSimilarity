using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using TextSimilarity.API.Features.TextSimilatity;

namespace TextSimilarity.API.Common.Swagger
{
    public class ApiExplorerTextSimilarityControllerOnlyConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            action.ApiExplorer.IsVisible = action.Controller.ControllerType == typeof(TextSimilarityController);
        }
    }
}
