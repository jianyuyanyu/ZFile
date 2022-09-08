using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using ZT.ApiService.Configure.Filters;
using ZT.ApiService.Configure.Middleware;
using ZT.ApiService.Hubs;
using ZT.ApiService.Swagger;
using ZT.Application.Mapper;
using ZT.Common.Extensions;
using ZT.Common.Utils;
using ZT.CrossCutting;
using ZT.Sugar.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ZTCors", policy =>
    {
        policy.WithOrigins("any")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials()
                     .WithExposedHeaders("X-Refresh-Token");
    });
});


// Add services to the container.
AppUtils.InitConfig(builder.Configuration);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<AopActionFilter>();
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<UnitOfWorkFilter>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.AllowTrailingCommas = false;
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new LongJsonConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

// Register DI
builder.Services.RegisterServices();

// Mapper
builder.Services.AddMapperProfile();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}
app.UseSwaggerSetup();

app.UseStaticFiles();


//app.UseFileServer(new FileServerOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "upload")),
//    RequestPath = "/upload",
//});

app.UseSetup();

app.MapControllers().RequireAuthorization();
app.MapHub<ChatHub>("/chathub");

app.Run();