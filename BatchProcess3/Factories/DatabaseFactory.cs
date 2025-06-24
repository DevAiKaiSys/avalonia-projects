using System;
using BatchProcess3.Services;

namespace BatchProcess3.Factories;

public class DatabaseFactory(Func<DatabaseService> factory)
{
    public DatabaseService GetDatabaseService(Action<DatabaseService>? afterCreation = null)
    {
        var databaseService = factory();

        afterCreation?.Invoke(databaseService);

        return databaseService;
    }
}