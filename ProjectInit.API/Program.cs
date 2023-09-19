using System.Net.Http.Headers;
using System.Reflection;
using ProjectInit.API.Middlewares;
using ProjectInit.Application.Constants;
using ProjectInit.Application.Services.Language;
using ProjectInit.Infrastructure.ServiceRegistrations;
using Transmogrify.DependencyInjection.Newtonsoft;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var loggerFactory = builder.Services.AddLoggerFactory(builder.Environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiCors(builder.Environment);

builder.Services.AddHttpContextAccessor();

builder.Services.AddNewtonsoftTransmogrify(config =>
{
    config.DefaultLanguage = LanguageConstants.DefaultLanguageCode;
    config.LanguagePath = Path.Combine(builder.Environment.ContentRootPath, LanguageConstants.LanguageFolder);
    config.AddResolver(typeof(DefaultLanguageResolver));
});

builder.Services
    .AddApiServices()
    .AddDatabase(builder.Configuration, loggerFactory, builder.Environment)
    .AddMediatR(
        configuration =>
            configuration.RegisterServicesFromAssemblies(
                Assembly.GetExecutingAssembly()
            ))
    .AddHttpClient(ProjectInitConstants.HttpClientName,
        configureClient =>
        {
            configureClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(ProjectInitConstants.DefaultContentType));
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

app.UseAuthorization();

app.MapControllers();

app.Run();