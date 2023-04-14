using firstapi.Helpter;
using firstapi.Repos;
using firstapi.Repos.Models;
using firstapi.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddDbContext<dbfirstcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("constring"));
});

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

app.MapGet("/basicget", () => "Welcome to Nihira Techiees");

app.MapGet("/basicget1", (string channel) => "Welcome to " + channel).WithOpenApi(options =>
{
    var parameter = options.Parameters[0];
    parameter.Description = "Provide channel name";
    return options;
}).RequireRateLimiting("slidingpolicy");

app.MapGet("/getallcustomer",async (dbfirstcontext db) =>
{
    return await db.TblCustomers.ToListAsync();
});

app.MapGet("/getcustomerbycode/{id}", async (int id,dbfirstcontext db) =>
{
    return await db.TblCustomers.FindAsync(id);
});

app.MapPost("/createcustomer", async (TblCustomer obj, dbfirstcontext db) =>
{
     await db.TblCustomers.AddAsync(obj);
    await db.SaveChangesAsync();
});
app.MapPut("/updatecustomer/{id}", async (int id,TblCustomer obj, dbfirstcontext db) =>
{
    var existdata = await db.TblCustomers.FindAsync(id);
    if (existdata != null)
    {
        existdata.Name = obj.Name;
        existdata.Email = obj.Email;
        existdata.Phone= obj.Phone;
    }
    await db.SaveChangesAsync();
});
app.MapDelete("/deletecustomer/{id}", async (int id, dbfirstcontext db) =>
{
    var existdata = await db.TblCustomers.FindAsync(id);
    if (existdata != null)
    {
        db.TblCustomers.Remove(existdata);
    }
    await db.SaveChangesAsync();
});

app.MapPost("/upload", async (IFormFile file) =>
{
    string filepath = "upload/" + file.FileName;
    using var stream = File.OpenWrite(filepath);
    await file.CopyToAsync(stream);
});

app.MapPost("/multiupload", async (IFormFileCollection collection) =>
{
    foreach (var file in collection)
    {
        string filepath = "upload/" + file.FileName;
        using var stream = File.OpenWrite(filepath);
        await file.CopyToAsync(stream);
    }
});

app.UseRateLimiter();

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
