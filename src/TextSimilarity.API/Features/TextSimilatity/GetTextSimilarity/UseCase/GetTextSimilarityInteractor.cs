using FluentResults;
using FluentValidation;
using MediatR;
using System.Threading;
using TextSimilarity.API.Common.Security.Authorization;

namespace TextSimilarity.API.Features.TextSimilatity.GetTextSimilarity.UseCase
{
    [Authorize(RequestSourse.API)]
    public record GetTextSimilarityRequest (string Text1, string Text2) : IRequest<Result<GetTextSimilarityResponse>>;
    public class GetTextSimilarityRequestValidator : AbstractValidator<GetTextSimilarityRequest>
    {
        public GetTextSimilarityRequestValidator()
        {
            RuleFor(v => v.Text1)
                .NotEmpty();

            RuleFor(v => v.Text2)
                .NotEmpty();
        }
    }
    public record GetTextSimilarityResponse(double Similarity);

    public class GetTextSimilarityInteractor : IRequestHandler<GetTextSimilarityRequest, Result<GetTextSimilarityResponse>>
    {
        public async Task<Result<GetTextSimilarityResponse>> Handle(GetTextSimilarityRequest request, CancellationToken cancellationToken)
        {
            double maxLength = Math.Max(request.Text1.Length, request.Text1.Length);
            if (maxLength > 0)
            {
                return await Task.Factory.StartNew(() =>
                {
                    var result = (maxLength - GetEditDistance(request.Text1, request.Text1, cancellationToken)) / maxLength;
                    return Result.Ok(new GetTextSimilarityResponse(result));
                    
                }, cancellationToken);
            }

            return Result.Ok(new GetTextSimilarityResponse(1.0));
        }

        private int GetEditDistance(string x, string y, CancellationToken cancellationToken)
        {
            int m = x.Length;
            int n = y.Length;

            int[][] T = new int[m + 1][];
            for (int i = 0; i < m + 1; ++i)
            {
                T[i] = new int[n + 1];
            }

            for (int i = 1; i <= m; i++)
            {
                T[i][0] = i;
            }
            for (int j = 1; j <= n; j++)
            {
                T[0][j] = j;
            }

            int cost;
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    cost = x[i - 1] == y[j - 1] ? 0 : 1;
                    T[i][j] = Math.Min(Math.Min(T[i - 1][j] + 1, T[i][j - 1] + 1),
                            T[i - 1][j - 1] + cost);
                }
            }

            return T[m][n];
        }
    }
}
