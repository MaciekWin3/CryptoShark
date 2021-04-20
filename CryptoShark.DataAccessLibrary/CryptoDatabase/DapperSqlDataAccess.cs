using CryptoShark.DataAccessLibrary.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    class DapperSqlDataAccess
    {
        private IDbConnection db;
        public DapperSqlDataAccess(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        //UserData

        public UserDataModel Find(string Email)
        {
            var sql = "SELECT * FROM UserData WHERE Email = @Email";
            return db.Query<UserDataModel>(sql, new { @Email = Email }).Single();
        }

        public List<UserDataModel> GetAll()
        {
            var sql = "SELECT * FROM UserData";
            return db.Query<UserDataModel>(sql).ToList();
        }


    }
}
