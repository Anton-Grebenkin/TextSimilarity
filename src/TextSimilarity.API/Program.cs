using TextSimilarity.API.Common.Extensions;
using Microsoft.OpenApi.Models;
using FluentResults.Extensions.AspNetCore;
using TextSimilarity.API.Common.ResultSettings;
using TextSimilarity.API.Common.Swagger;

var builder = WebApplication.CreateBuilder(args);

AspNetCoreResult.Setup(config => config.DefaultProfile = new ResultEndpointProfile());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyMethod()
             .AllowAnyHeader()
             .SetIsOriginAllowed(origin => true)
             .AllowCredentials();
        });
});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddMappingConfiguration();

//builder.Services.AddMvc(c =>
//        c.Conventions.Add(new ApiExplorerTextSimilarityControllerOnlyConvention())
//    );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = @"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] { }
        }
    });
    c.SchemaFilter<QueryFilterSchemaFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthenticationMiddleware();

app.UseRequestResponseLoggingMiddleware();

app.MapControllers();

app.Run();
