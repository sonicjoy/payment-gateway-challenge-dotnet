using System.Text.Json;
using System.Text.Json.Serialization;

using FluentValidation;

using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Helpers;
using PaymentGateway.Api.Services.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add CORS policy for React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPaymentsRepository, PaymentsRepository>();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddValidatorsFromAssemblyContaining<PaymentRequestValidator>();

builder.Services.AddHttpClient<IAcquiringBankClient, AcquiringBankClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AcquiringBank:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS before other middleware
app.UseCors("ReactAppPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
