// Services/DatabaseService.cs
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<UserTotal>().Wait();
        _database.CreateTableAsync<ModificationRecord>().Wait();
    }

    // UserTotal methods
    public Task<List<UserTotal>> GetUserTotalsAsync()
    {
        return _database.Table<UserTotal>().ToListAsync();
    }

    // ModificationRecord methods
    public Task<List<ModificationRecord>> GetModificationRecordsAsync()
    {
        return _database.Table<ModificationRecord>().ToListAsync();
    }

    public Task<int> SaveModificationRecordAsync(ModificationRecord modificationRecord)
    {
        return _database.InsertAsync(modificationRecord);
    }

    // Add methods to handle transactions
    public async Task SaveDataWithTransactionAsync(IEnumerable<UserTotal> userTotals, IEnumerable<ModificationRecord> modificationRecords)
    {
        await _database.RunInTransactionAsync(async transaction =>
        {
            try
            {
                foreach (var record in modificationRecords)
                {
                    await _database.InsertAsync(record);
                }

                foreach (var userTotal in userTotals)
                {
                    await _database.InsertOrReplaceAsync(userTotal);
                }
                
                // Commit the transaction
                transaction.Commit();
            }
            catch
            {
                // Rollback the transaction if an error occurs
                transaction.Rollback();
                throw;
            }
        });
    }

    public async Task<int> DeleteUserTotalAsync(UserTotal user)
    {
        return await _database.DeleteAsync(user);
    }   

    public async Task<List<DailyConsumption>> GetTodayDailyConsumptionsAsync()
    {
        var query = @$"
            SELECT Username, SUM(Settlement) AS Consumption
                FROM ModificationRecord
                WHERE datetime(ModifiedAt/10000000 - 62135596800, 'unixepoch') > date('now') and Order <> 0
                GROUP BY Username;
            ";

        return await _database.QueryAsync<DailyConsumption>(query);
    }

    public async Task<List<OrderSummary>> GetTodayOrderSummariesAsync()
    {
        var query = @"
            SELECT 
                CASE
                    WHEN `Order` IN (1, 3, 5) THEN '1-3-5'
                    WHEN `Order` IN (2, 4, 6) THEN '2-4-6'
                    ELSE 'Other'
                END AS OrderGroup,
                SUM(Amount) AS TotalAmount
            FROM ModificationRecord
            WHERE datetime(ModifiedAt/10000000 - 62135596800, 'unixepoch') > date('now')
            GROUP BY 
                CASE
                    WHEN `Order` IN (1, 3, 5) THEN '1-3-5'
                    WHEN `Order` IN (2, 4, 6) THEN '2-4-6'
                    ELSE 'Other'
                END";

            var results = await _database.QueryAsync<OrderSummary>(query);
            return results;

    }


}
