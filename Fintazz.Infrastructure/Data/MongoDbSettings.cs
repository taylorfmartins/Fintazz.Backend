namespace Fintazz.Infrastructure.Data;

public class MongoDbSettings
{
    public const string SectionName = "MongoDbSettings";
    
    // Propriedades separadas para conexão
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AppName { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    
    // Propriedade calculada para retornar a string completa dependendo de onde vem a configuração
    public string ConnectionString 
    {
        get
        {
            // Se houver usuário e senha, montamos no formato de autenticação (Padrão Atlas / Prod)
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                var credentials = $"{Uri.EscapeDataString(Username)}:{Uri.EscapeDataString(Password)}";
                var options = string.IsNullOrEmpty(AppName) ? "" : $"/?appName={Uri.EscapeDataString(AppName)}";
                
                return $"mongodb+srv://{credentials}@{Host}{options}";
            }
            
            // Fallback: Formato simples sem autentição (Padrão localhost / Dev)
            return $"mongodb://{Host}";
        }
    }
}
