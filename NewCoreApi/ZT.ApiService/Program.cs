using System.Text.Json.Serialization;
using ZT.ApiService.Configure.Filters;
using ZT.Common.Extensions;
using ZT.Common.Utils;
using ZT.Sugar.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ZTCors", policy =>
    {
        policy.WithOrigins("http://localhost:2800")
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
