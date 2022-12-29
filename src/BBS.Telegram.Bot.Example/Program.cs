using BBS.Telegram.Bot.Example.Forms;
using BBS.Telegram.Bot.Example.Forms.UserInfoForm;
using BBS.Telegram.Bot.Form.Factories;
using BBS.Telegram.Bot.Form.Infra;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

builder.Services.AddHostedService<BotBackgroundService>();
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(builder.Configuration["TelegramBotToken"]));
builder.Services.AddSingleton<IUpdateHandler>(p => new FormHandlerDecorator(null,
    p.GetRequiredService<IFormStatesFactory>(),
    p.GetRequiredService<IFormStatesRepository>()));
builder.Services.AddSingleton<IFormStatesFactory, FormStatesFactory>();
builder.Services.AddSingleton<IFormStatesRepository, FormStatesInMemoryRepository>();

#endregion

var app = builder.Build();

#region Configure app

app.MapGet("/", () => "Hello World!");

#endregion

app.Run();