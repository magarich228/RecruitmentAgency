using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Application.Bundle;
using RecruitmentAgency.RelationalData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRecruitmentAgency(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RecruitmentDbContext>();

    await context.Database.MigrateAsync();
}

app.Run();