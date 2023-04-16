using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TextSimilarity.API.Common.DataAccess.Entities;

namespace TextSimilarity.API.Common.Security.Authorization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizeAttribute : Attribute
    {
        public RequestSourse RequestSourse { get; private set; }
        public AuthorizeAttribute(RequestSourse requestSourse)
        {
            RequestSourse = requestSourse;
        }
    }
}
