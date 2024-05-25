using Application;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Presentation;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddDomain();
builder.Services.AddInfrastructure();
builder.Services.AddPresentation();

var application = builder.Build();
application.UseMiddleware<ExceptionHandlerMiddleware>();
application.UseMiddleware<UnitOfWorkMiddleware>();
application.UseSwagger();
application.UseSwaggerUI();
application.UseAuthentication();
application.UseAuthorization();
application.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .SetIsOriginAllowed(_ => true));
application.UseHttpsRedirection();
application.MapControllers();
application.MapHealthChecks("/healthcheck/liveness", new HealthCheckOptions { Predicate = _ => false });
application.MapHealthChecks("/healthcheck/readiness");
application.Run();