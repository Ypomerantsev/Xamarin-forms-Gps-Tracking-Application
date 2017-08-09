using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using App1.Model;

namespace App1.Database
{
    public class DB
    {
        readonly SQLiteAsyncConnection database;

        public DB(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<GpsPodatek>().Wait();
        }

        public Task<List<GpsPodatek>> GetItemsAsync()
        {
            return database.Table<GpsPodatek>().ToListAsync();
        }

        public Task<List<GpsPodatek>> GetItemsNotSent()
        {
            return database.Table<GpsPodatek>().Where(x => x.PoslanNaStreznik == false).OrderBy(x => x.Timestamp).ToListAsync();
//            return database.QueryAsync<GpsPodatek>("SELECT * FROM [GpsPodatek] WHERE [PoslanNaStreznik] = false ORDER BY [TimeStamp] ASC");
        }


        public Task<GpsPodatek> GetItemAsync(DateTimeOffset id)
        {
            return database.Table<GpsPodatek>().Where(i => i.Timestamp == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(GpsPodatek item)
        {
            if (item.Timestamp == null)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(GpsPodatek item)
        {
            return database.DeleteAsync(item);
        }

    }
}
