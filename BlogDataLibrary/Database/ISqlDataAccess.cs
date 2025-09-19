using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogDataLibrary.Database
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName, bool isStoredProcedure);
        Task SaveData<T>(string sqlStatement, T parameters, string connectionStringName, bool isStoredProcedure);
    }
}
