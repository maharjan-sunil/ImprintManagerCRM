using EmailService.Interfaces;
using EmailService.Options;
using EmailService.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
.ReadFrom.Configuration(ctx.Configuration)
.WriteTo.Console());

builder.Services.Configure<SmtpSettingOptions>(builder.Configuration.GetSection(SmtpSettingOptions.SectionName));

builder.Services.AddGrpc();
builder.Services.AddGrpcHealthChecks();

builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
builder.Services.AddSingleton<IEmailTemplateRenderer, EmailTemplateRenderer>();

// Configure Kestrel for gRPC (HTTP/2)
builder.WebHost.ConfigureKestrel(k =>
{
    k.ListenAnyIP(8080, o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
});

var app = builder.Build();

app.MapGrpcService<EmailGrpcService>();
app.MapGrpcHealthChecksService();

// Health + root endpoints
app.MapHealthChecks("/healthz");
app.MapGet("/", () => "Email gRPC Service is running.");


app.Run();
