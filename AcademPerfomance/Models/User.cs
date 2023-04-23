using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademPerfomance.Models
{
    public record UserFio
    {
        public string student_fio { get; set; }
    }
    public record User
    {
        static public User? CurrentUser { get; private set; } = null;
        public string unique_id { get; private set; } = null!;
        public string user_fio { get; private set; } = null!;
        public string? student_id_numb { get; private set; }
        public int? department_id { get; private set; }
        public string? department_name { get; private set; }
        public int? group_id { get; private set; }
        public string? group_number { get; private set; }
        public int? admission_year { get; private set; }
        public string role_name { get; private set; } = null!;
        public static User InitCurrentUser(
                    string unique_id,
                    string user_fio,
                    string role_name,
                    int? admission_year,
                    int? department_id,
                    string? department_name,
                    int? group_id,
                    string? group_number,
                    string? student_id_numb)
        {
            if (CurrentUser != null) throw new Exception("Пользователь уже инициализован");

            CurrentUser = new()
            {
                group_id = group_id,
                group_number = group_number,
                unique_id = unique_id,
                user_fio = user_fio,
                admission_year = admission_year,
                role_name = role_name,
                student_id_numb = student_id_numb,
                department_id = department_id,
                department_name = department_name
            };

            return CurrentUser;
        }
        public override string ToString()
        {
            string sUser = string.Empty;
            sUser += $"ФИО: {user_fio}\n";
            sUser += $"Роль: {role_name}\n";
            sUser += group_number != null? $"Группа: {group_number}\n" : "";
            sUser += student_id_numb != null ? $"Номер студ. билета: {student_id_numb}\n" : "";
            sUser += admission_year != null ? $"Год начала обучения: {admission_year}\n" : "";
            sUser += department_name != null ? $"Название кафедры: {department_name}\n" : "";
            return sUser;
        }
    }
}
