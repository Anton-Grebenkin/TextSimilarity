using TextSimilarity.API.Common.DataAccess;
using TextSimilarity.API.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using TextSimilarity.API.Common.Security.Authentication;
using Microsoft.OpenApi.Models;
using FluentResults.Extensions.AspNetCore;
using TextSimilarity.API.Common.ResultSettings;

var builder = WebApplication.CreateBuilder(args);

AspNetCoreResult.Setup(config => config.DefaultProfile = new ResultEndpointProfile());

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection(nameof(JWTSettings)));
builder.Services.AddDbContext<AppDataContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddApplicationServices();
builder.Services.AddMappingConfiguration();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
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
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
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
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthenticationMiddleware();

app.MapControllers();

app.Run();
