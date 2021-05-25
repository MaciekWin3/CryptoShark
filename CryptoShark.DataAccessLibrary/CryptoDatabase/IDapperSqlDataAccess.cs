using CryptoShark.DataAccessLibrary.Models;
using System.Collections.Generic;

namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public interface IDapperSqlDataAccess
    {
        UserDataModel AddUserDataModel(UserDataModel user);
        UserDataModel FindUserDataModelByEmail(string Email);
        List<UserDataModel> GetAllLastUserDataModel();
        List<UserDataModel> GetAllUserDataForUser(string Email);
        List<LineChartData> GetChartData(string Email, string currnecy);
        List<CryptocurrencySqlModel> GetLastCryptoRecords();
        UserDataModel GetLastUserDataModel(string Email);
        List<PortfolioData> GetPortfolioData(string Email);
        UserDataModel UpdateCurrencyFromLastUserDataModel(string Email, string currency, double amount, string operation);
    }
}