using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Primitives;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter(policyName: "ratepolicy", options =>
{
    options.Window = TimeSpan.FromSeconds(5);
    options.PermitLimit = 1;
    options.QueueLimit = 0;
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}).RejectionStatusCode=401);

builder.Services.AddRateLimiter(_ => _.AddSlidingWindowLimiter(policyName: "slidingpolicy", options =>
{
    options.Window = TimeSpan.FromSeconds(5);
    options.PermitLimit = 1;
    options.SegmentsPerWindow = 2;
    options.QueueLimit = 0;
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}).RejectionStatusCode = 401);

builder.Services.AddRateLimiter(_ => _.AddConcurrencyLimiter(policyName: "concurrencypolicy", options =>
{
    options.PermitLimit = 1;
    options.QueueLimit = 0;
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}).RejectionStatusCode = 401);

builder.Services.AddRateLimiter(_ => _.AddTokenBucketLimiter(policyName: "tokenpolicy", options =>
{
    options.TokenLimit = 1;
    options.QueueLimit = 1;
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    options.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
    options.TokensPerPeriod = 2;
}).RejectionStatusCode = 401);



var app = builder.Build();

app.UseRateLimiter();

app.MapGet("/testing",()=> DateTime.Now.ToString()).RequireRateLimiting("tokenpolicy");

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
