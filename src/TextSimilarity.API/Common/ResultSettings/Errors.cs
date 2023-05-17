using FluentResults;

namespace TextSimilarity.API.Common.ResultSettings
{
    public static class Errors 
    {
        public static UnauthorizedError UnauthorizedError(string message)
        {
            return new UnauthorizedError(message);
        }

        public static NotFoundError NotFoundError(string message)
        {
            return new NotFoundError(message);
        }
    }


    public class UnauthorizedError : Error
    {
        public UnauthorizedError(string message)
            : base(message)
        {

        }
    }

    public class NotFoundError : Error
    {
        public NotFoundError(string message)
            : base(message)
        {

        }
    }

    public class ValidationError : Error
    {
        public string PropertyName { get; set; }
        public ValidationError(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
