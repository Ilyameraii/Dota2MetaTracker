using Services.Contracts.Models;

namespace Repository.Contracts;

public interface IUserPreferencesCache
{
    UserPreferences GetOrCreate(long userId);
    UserPreferences Update(long userId, Action<UserPreferences> update);
}