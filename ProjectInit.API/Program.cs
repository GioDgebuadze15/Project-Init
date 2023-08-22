using System.Reflection;
using ProjectInit.Application.Services.Language;
using ProjectInit.Infrastructure.ServiceRegistrations;
using Transmogrify.DependencyInjection.Newtonsoft;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiCors(builder.Environment);

builder.Services.AddHttpContextAccessor();

builder.Services.AddNewtonsoftTransmogrify(config =>
{
    config.DefaultLanguage = "en";
    config.LanguagePath = Path.Combine(builder.Environment.ContentRootPath, "Languages");
    config.AddResolver(typeof(DefaultLanguageResolver));
});

//Todo:pass iLoggerFactory here
builder.Services
    .AddApiServices()
    .AddDatabase(builder.Configuration,null,builder.Environment)
    .AddMediatR(
        configuration =>
            configuration.RegisterServicesFromAssemblies(
                Assembly.GetExecutingAssembly()
            ))
    .AddHttpClient("default",
        configureClient =>
        {
            configureClient.DefaultRequestHeaders.Accept
                .Add(new("application/json"));
        });


var app = builder.Build();
app.UseApiCors(app.Environment);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    var iLogger = app.Services.GetRequiredService<ILoggerFactory>();
    app.UseLoggerFactory(iLogger);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();