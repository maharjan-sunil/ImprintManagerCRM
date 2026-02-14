using EmailService.Interfaces;
using RazorLight;

namespace EmailService.Services
{
    public class EmailTemplateRenderer : IEmailTemplateRenderer
    {
        private readonly RazorLightEngine _engine;

        public EmailTemplateRenderer()
        {
            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates/Emails"))
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> RenderAsync<TModel>(string templateName, TModel data)
        {
            return await _engine.CompileRenderAsync(templateName, data);
        }
    }
}
