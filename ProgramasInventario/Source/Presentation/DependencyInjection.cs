using System.Collections.Immutable;
using System.Reflection;
using Application.Services.Users.Security;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddJwt();
        services.AddSwagger();
    }

    private static void AddJwt(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = Token.GetSecurityKey(),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidAudience = GlobalConfigurations.JwtAudience,
                ValidIssuer = GlobalConfigurations.JwtIssuer
            };
        });
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Description = "JWT authorization header using the bearer scheme.",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "Bearer",
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    ImmutableArray<string>.Empty
                }
            });
            options.ExampleFilters();
            var filePath =
                Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            options.IncludeXmlComments(filePath);
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Contact = new OpenApiContact
                {
                    Email = "example@template.com"
                },
                Description = "API to manage the Template application.",
                License = new OpenApiLicense
                {
                    Name = "© All rights reserved."
                },
                Title = "Template",
                Version = "v1"
            });
        });
    }
}