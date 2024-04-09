using System.Net.Http.Headers;
using System.Reflection;
using ProjectInit.API.Middlewares;
using ProjectInit.Application.Behaviors;
using ProjectInit.Application.Features.FileFeatures.Commands.Create;
using ProjectInit.Infrastructure.ServiceRegistrations;
using ProjectInit.Infrastructure.Services.Language;
using ProjectInit.Persistence.DatabaseRegistrations;
using ProjectInit.Shared.Constants;
using ProjectInit.Shared.Helpers;
using Transmogrify.DependencyInjection.Newtonsoft;
using Wolverine;
using Wolverine.FluentValidation;

var builder = WebApplication.CreateBuilder(args);

var loggerFactory = builder.Services.AddLoggerFactory(builder.Environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiCors(builder.Environment);

builder.Services.AddHttpContextAccessor();

builder.Services.AddNewtonsoftTransmogrify(config =>
{
    config.DefaultLanguage = LanguageConstants.DefaultLanguageCode;
    config.LanguagePath = LanguageHelper.GetLanguageFolderFullPath();
    config.AddResolver(typeof(DefaultLanguageResolver));
});

builder.Host.UseWolverine(options =>
{
    options.Discovery.IncludeAssembly(typeof(CreateFile).GetTypeInfo().Assembly);

    options.UseFluentValidation();
    options.Services.AddSingleton(typeof(IFailureAction<>), typeof(FluentValidationBehavior<>));
});

builder.Services
    .AddApiServices()
    .AddDatabase(builder.Configuration, loggerFactory, builder.Environment)
    .AddHttpClient(ApiConstants.HttpClientName,
        configureClient =>
        {
            configureClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(ApiConstants.DefaultContentType));
        });

var app = builder.Build();

app.UseApiCors(app.Environment);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<DatabaseMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();