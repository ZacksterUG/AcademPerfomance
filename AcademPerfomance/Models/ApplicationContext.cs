using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AcademPerfomance.Views;
using System.Configuration;

namespace AcademPerfomance.Models
{
    public class ApplicationContext : DbContext
    {
        public static string? Login
        {
            get; private set;
        } = null;
        private static string? Password
        {
            get; set;
        } = null;
        private static string connectionString;
        public static string ConnectionString
        {
            get => string.Format(connectionString, Login ?? "guest", Password ?? "guest");
        }
        public DbSet<Institute> Institute { get; set; } = null!;
        static ApplicationContext()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Debug.WriteLine("use");
            optionsBuilder.UseSqlServer(ConnectionString);
        }
        public static void Auth(string login, string password)
        {
            if(Login is not null)
            {
                throw new Exception($"Вы уже авторизованы в системе, {User.CurrentUser?.user_fio}!");
            }
            using ApplicationContext context = new ApplicationContext();
            SqlParameter pLogin = new("@login", login);
            SqlParameter pPassword = new("@password", password);
            SqlParameter pDBlogin = new()
            {
                ParameterName = "@DBlogin",
                SqlDbType = SqlDbType.VarChar,
                Size = 32,
                Direction = ParameterDirection.Output
            };
            SqlParameter pDBpassword = new()
            {
                ParameterName = "@DBpassword",
                SqlDbType = SqlDbType.VarChar,
                Size = 32,
                Direction = ParameterDirection.Output
            }; 
            SqlParameter pUnique_id = new()
            {
                ParameterName = "@unique_id",
                SqlDbType = SqlDbType.VarChar,
                Size = 36,
                Direction = ParameterDirection.Output
            }; 
            SqlParameter pUser_fio = new()
            {
                ParameterName = "@user_fio",
                SqlDbType = SqlDbType.VarChar,
                Size = 128,
                Direction = ParameterDirection.Output
            }; 
            SqlParameter pStudent_id_numb = new()
            {
                ParameterName = "@student_id_numb",
                SqlDbType = SqlDbType.VarChar,
                Size = 24,
                IsNullable = true,
                Direction = ParameterDirection.Output
            };
            SqlParameter pDepartment_id = new()
            {
                ParameterName = "@department_id",
                SqlDbType = SqlDbType.Int,
                IsNullable = true,
                Direction = ParameterDirection.Output
            }; 
            SqlParameter pDepartment_name = new()
            {
                ParameterName = "@department_name",
                SqlDbType = SqlDbType.VarChar,
                Size = 256,
                IsNullable = true,
                Direction = ParameterDirection.Output
            };
            SqlParameter pGroup_id = new()
            {
                ParameterName = "@group_id",
                SqlDbType = SqlDbType.Int,
                IsNullable = true,
                Direction = ParameterDirection.Output
            }; 
            SqlParameter pGroup_number = new()
            {
                ParameterName = "@group_number",
                SqlDbType = SqlDbType.VarChar,
                Size = 10,
                IsNullable = true,
                Direction = ParameterDirection.Output
            }; 
            SqlParameter pAdmission_year = new()
            {
                ParameterName = "@admission_year",
                SqlDbType = SqlDbType.Int,
                IsNullable = true,
                Direction = ParameterDirection.Output
            };
            SqlParameter pRole_name = new()
            {
                ParameterName = "@role_name",
                SqlDbType = SqlDbType.VarChar,
                Size = 32,
                Direction = ParameterDirection.Output
            };

            context.Database.ExecuteSqlRaw(
                $@"Exec AuthUser 
	                @login,
	                @password,
	                @DBlogin out,
	                @DBpassword out,
	                @unique_id out,
	                @user_fio out,
	                @student_id_numb out,
	                @department_id out,
	                @department_name out,
	                @group_id out,
	                @group_number out,
	                @admission_year out,
	                @role_name out",
                pLogin,
                pPassword,
                pDBlogin,
                pDBpassword,
                pUnique_id,
                pUser_fio,
                pStudent_id_numb,
                pDepartment_id,
                pDepartment_name,
                pGroup_id,
                pGroup_number,
                pAdmission_year,
                pRole_name);

            Login = (string)pDBlogin.Value;
            Password = (string)pDBpassword.Value;

            var CurrentUser = User.InitCurrentUser(
                unique_id: (string)pUnique_id.Value,
                user_fio: (string)pUser_fio.Value,
                role_name: (string)pRole_name.Value,
                admission_year: (pAdmission_year.Value != DBNull.Value) ? (int?)pAdmission_year.Value : null,
                department_id: (pDepartment_id.Value != DBNull.Value) ? (int?)pDepartment_id.Value : null,
                department_name: (pDepartment_name.Value != DBNull.Value) ? (string?)pDepartment_name.Value : null,
                group_id: (pGroup_id.Value != DBNull.Value) ? (int?)pGroup_id.Value : null,
                group_number: (pGroup_number.Value != DBNull.Value) ? (string?)pGroup_number.Value : null,
                student_id_numb: (pStudent_id_numb.Value != DBNull.Value) ? (string?)pStudent_id_numb.Value : null
            );

        } 
    }
}
