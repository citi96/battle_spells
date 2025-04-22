using System;
using Battle_Spells.Api.Data;
using Battle_Spells.Api.Hubs;
using Battle_Spells.Api.Services;
using Battle_Spells.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurazione di Entity Framework:
// In sviluppo usiamo un database InMemory; in produzione configurare PostgreSQL (es. UseNpgsql)
builder.Services.AddDbContext<BattleSpellsDbContext>(options =>
    options.UseSqlite("Data Source=BattleSpells.db"));

// Registrazione dei servizi
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ISyncService, SyncService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrazione di SignalR per aggiornamenti in tempo reale
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Mappa i controller REST
app.MapControllers();

// Mappa l'Hub SignalR su "/matchhub"
app.MapHub<MatchHub>("/matchhub");

app.Run();
