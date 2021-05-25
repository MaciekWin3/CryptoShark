using CryptoShark.DataAccessLibrary.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public class DapperSqlDataAccess : IDapperSqlDataAccess
    {
        private IDbConnection db;
        public DapperSqlDataAccess(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        //UserData

        public UserDataModel AddUserDataModel(UserDataModel user)
        {
            throw new NotImplementedException();
        }

        public UserDataModel FindUserDataModelByEmail(string Email)
        {
            var sql = "SELECT top 1 * FROM UserData WHERE Email = @Email order by id desc";
            return db.Query<UserDataModel>(sql, new { @Email = Email }).Single();
        }

        public List<UserDataModel> GetAllUserDataForUser(string Email)
        {
            var sql = "SELECT * FROM UserData WHERE Email = @Email";
            return db.Query<UserDataModel>(sql, new { @Email = Email }).ToList();
        }

        public List<UserDataModel> GetAllLastUserDataModel()
        {
            var sql = "SELECT * FROM UserData";
            return db.Query<UserDataModel>(sql).ToList();
        }

        public UserDataModel UpdateCurrencyFromLastUserDataModel(string Email, string currency, double amount, string operation) //do zrobienia
        {

            var sql = "";
            currency = currency.Replace(" ", String.Empty);
            string stringAmount = amount.ToString();
            stringAmount = stringAmount.Replace(",", ".");
            if (operation == "Add")
            {
                sql = "Update UserData set " + currency + " = " + currency + " + " + stringAmount + " select top 1 * FROM UserData where Email = @Email ORDER BY id";
            }
            else
            {
                sql = "Update UserData set " + currency + " = " + currency + " - " + stringAmount + " select top 1 * FROM UserData where Email = @Email ORDER BY id";
            }

            return db.Query<UserDataModel>(sql, new { @Email = Email }).Single();
        }

        public UserDataModel GetLastUserDataModel(string Email)
        {
            var sql = @"select top 1  * from UserData
                        where[save] = (select max([save]) from UserData) and Email = @Email
                        order by id desc";
            return db.Query<UserDataModel>(sql, new { @Email = Email }).Single();
        }



        public List<CryptocurrencySqlModel> GetLastCryptoRecords()
        {
            var sql = "select * from CryptocurrenciesCoinLore where id IN (SELECT MAX(id) FROM CryptocurrenciesCoinLore GROUP BY [name]) order by [name] ";
            return db.Query<CryptocurrencySqlModel>(sql).ToList();
        }

        public List<PortfolioData> GetPortfolioData(string Email)
        {
            var sql = @"select avg(bitcoin) as 'bitcoin',avg(ethereum) as 'ethereum',avg(binancecoin) as 'binancecoin',
                avg(polkadot) as 'polkadot',avg(chainlink) as 'chainlink', avg(monero) as 'monero',
                avg(dash) as 'dash', avg(zilliqa) as 'zilliqa',avg(ravencoin) as 'ravencoin',avg(uniswap) as 'uniswap', LEFT([save],11) as 'date'
                from UserData
                WHERE Email = @Email
                group by LEFT([save],11)";
            return db.Query<PortfolioData>(sql, new { @Email = Email }).ToList();
        }

        public List<LineChartData> GetChartData(string Email, string Crypto)
        {
            var sql = @"select max(LEFT(c.[datetime],11)) as 'date', max(bitcoin) * max(price_usd) as 'value'
                        from UserData u, CryptocurrenciesCoinLore c 
                        where email=@Email and LEFT(u.[save],11) = LEFT(c.[datetime],11) and c.name = @Crypto
                        group by LEFT(c.[datetime],11)";

            return db.Query<LineChartData>(sql, new { @Email = Email, @Crypto = Crypto}).ToList();
        }


    }
}
