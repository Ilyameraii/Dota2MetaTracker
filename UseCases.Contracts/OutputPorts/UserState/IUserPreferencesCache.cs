using Services.Contracts.Models;

namespace UseCases.Contracts.OutputPorts.UserState;

public interface IUserPreferencesCache
{
    UserPreferences GetOrCreate(long userId);
    UserPreferences Update(long userId, Action<UserPreferences> update);
}