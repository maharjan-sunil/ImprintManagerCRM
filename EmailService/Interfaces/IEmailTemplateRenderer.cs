namespace EmailService.Interfaces
{
    public interface IEmailTemplateRenderer
    {
        Task<string> RenderAsync<TModel>(string templateName, TModel data);
    }
}
