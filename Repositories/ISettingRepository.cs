namespace MrMohamedHassan.Repositories;

public interface ISettingRepository
{
    Task<string?> GetValueAsync(string key);
    Task SetValueAsync(string key, string? value, string? description = null);
    Task<Dictionary<string, string?>> GetAllSettingsAsync();
}
