using Battle_Spells.Api.Data;
using Battle_Spells.Api.Hubs;
using Battle_Spells.Api.Repositories;
using Battle_Spells.Api.Repositories.Interfaces;
using Battle_Spells.Api.Services;
using Battle_Spells.Api.Services.Interfaces;
using Battle_Spells.Api.Singletons;
using Battle_Spells.Api.Singletons.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<BattleSpellsDbContext>(options =>
    options.UseSqlite(connString));

// Registrazione dei servizi
builder.Services.AddScoped<IDeckService, DeckService>();
builder.Services.AddScoped<IMatchmakingService, MatchmakingService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISyncService, SyncService>();

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IEffectDefinitionRepository, EffectDefinitionRepository>();
builder.Services.AddScoped<IHeroRepository, HeroRepository>();
builder.Services.AddScoped<IMatchPlayerCardRepository, MatchPlayerCardRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IPlayerCardRepository, PlayerCardRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();

builder.Services.AddSingleton<IPlayerConnectionTracker, PlayerConnectionTracker>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrazione di SignalR per aggiornamenti in tempo reale
builder.Services.AddSignalR();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BattleSpellsDbContext>();
    db.Database.Migrate();
}

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Mappa i controller REST
app.MapControllers();

// Mappa l'Hub SignalR su "/matchhub"
app.MapHub<MatchHub>("/matchhub");

app.Run();
