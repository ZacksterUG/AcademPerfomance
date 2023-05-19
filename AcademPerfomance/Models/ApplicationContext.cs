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
using System.Data.SqlTypes;

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
        private static readonly string connectionString;
        public static string ConnectionString
        {
            get => string.Format(connectionString, Login ?? "guest", Password ?? "guest");
        }
        public DbSet<Institute> Institute { get; set; } = null!;
        public DbSet<Department> Department { get; set; } = null!;
        public DbSet<GroupView> GroupViews { get; set; } = null!;
        public DbSet<SubjectView> SubjectViews { get; set; } = null!;
        public DbSet<EventTypeMarksView> EventTypeMarks { get; set; } = null!;
        public DbSet<Direction> Directions { get; set; } = null!;
        static ApplicationContext()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFio>().HasNoKey();
            modelBuilder.Entity<CurriculumElement>().HasNoKey();
            modelBuilder.Entity<Department>()
                .HasOne(d => d.institute)
                .WithMany(i => i.departments);
            modelBuilder.Entity<Direction>()
                .ToTable("direction")
                .HasOne(d => d.department)
                .WithMany(d => d.directions);

            modelBuilder.HasDbFunction(() => GetGroupStudentList(default, default));
            modelBuilder.HasDbFunction(() => GetUsersCurriculum(default));
            modelBuilder.HasDbFunction(() => GetGroupsCurriculum(default, default));
            modelBuilder.Entity<GroupView>(g =>
            {
                g.HasNoKey();
                g.ToView("v_group");
            });
            modelBuilder.Entity<EventTypeMarksView>(g =>
            {
                g.HasNoKey();
                g.ToView("v_event_type_marks");
            });
            modelBuilder.Entity<SubjectView>(s =>
            {
                s.HasNoKey();
                s.ToView("v_subject");
            });
        }
        public void CreateDirection(string direction_name, int department_id, byte semester_count, string code)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pDirectionName = new SqlParameter("@direction_name", direction_name);
            SqlParameter pDepartmentId = new SqlParameter("@department_id", department_id);
            SqlParameter pSemesterCount = new SqlParameter("@semester_count", semester_count);
            SqlParameter pCode = new SqlParameter("@code", code);

            Database.ExecuteSqlRaw($@"exec CreateDirection
	                                    @user_identifier,
	                                    @direction_name,
	                                    @department_id,
	                                    @semester_count,
	                                    @code", 
                                        pUserIdentifier,
                                        pDirectionName,
                                        pDepartmentId,
                                        pSemesterCount,
                                        pCode
            );
        }
        public void EditDirection(int direction_id, string direction_name, int department_id, byte semester_count, string code)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pDirectionId = new SqlParameter("@direction_id", direction_id);
            SqlParameter pDirectionName = new SqlParameter("@direction_name", direction_name);
            SqlParameter pDepartmentId = new SqlParameter("@department_id", department_id);
            SqlParameter pSemesterCount = new SqlParameter("@semester_count", semester_count);
            SqlParameter pCode = new SqlParameter("@code", code);

            Database.ExecuteSqlRaw($@"exec EditDirection
	                                    @user_identifier,
                                        @direction_id,
	                                    @direction_name,
	                                    @department_id,
	                                    @semester_count,
	                                    @code",
                                        pUserIdentifier,
                                        pDirectionId,
                                        pDirectionName,
                                        pDepartmentId,
                                        pSemesterCount,
                                        pCode
            );
        }
        public void DeleteDirection(int direction_id)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pDirectionId = new SqlParameter("@direction_id", direction_id);

            Database.ExecuteSqlRaw($@"exec DeleteDirection
	                                    @user_identifier,
	                                    @direction_id",
                                        pUserIdentifier,
                                        pDirectionId
            );
        }
        public void EditSubject(int dep_subject_id, int department_id, string subject_name)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pDepSubjectId = new SqlParameter("@dep_subject_id", dep_subject_id);
            SqlParameter pDepartmentId = new SqlParameter("@department_id", department_id);
            SqlParameter pSubjectName = new SqlParameter("@subject_name", subject_name);

            Database.ExecuteSqlRaw($@"
                exec EditSubject
                    @user_identifier,
	                @dep_subject_id,
	                @department_id,
	                @subject_name",
                    pUserIdentifier,
                    pDepSubjectId,
                    pDepartmentId,
                    pSubjectName
            );
        }
        public void AddSubject(int department_id, string subject_name)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pDepartmentId = new SqlParameter("@department_id", department_id);
            SqlParameter pSubjectName = new SqlParameter("@subject_name", subject_name);

            Database.ExecuteSqlRaw($@"
                exec AddSubject
                    @user_identifier,
	                @department_id,
	                @subject_name",
                    pUserIdentifier,
                    pDepartmentId,
                    pSubjectName
            );
        }
        public void AppendCurriculumSubject(int group_id, int dep_subject_id, int semester, int? control_event_id, int? course_event_id)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pGroupId = new SqlParameter("@group_id", group_id);
            SqlParameter pDepSubjectId = new SqlParameter("@dep_subject_id", dep_subject_id);
            SqlParameter pSemester = new SqlParameter("@semester", semester);
            SqlParameter pControlEventId = new SqlParameter("@control_event_id", control_event_id ?? SqlInt32.Null);
            SqlParameter pCourseEventId = new SqlParameter("@course_event_id", course_event_id ?? SqlInt32.Null);

            Database.ExecuteSqlRaw($@"
                exec AppendCurriculumSubject
                     @user_identifier,
	                 @group_id,
	                 @dep_subject_id,
	                 @semester,
	                 @control_event_id,
	                 @course_event_id",
                     pUserIdentifier,
                     pGroupId,
                     pDepSubjectId,
                     pSemester,
                     pControlEventId,
                     pCourseEventId
            );
        }
        public void EditCurriculumElement(int curriculum_element_id, int semester)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pCurriculumElementId = new SqlParameter("@curriculum_element_id", curriculum_element_id);
            SqlParameter pSemester = new SqlParameter("@semester", semester);

            Database.ExecuteSqlRaw($@"
                exec EditCurriculumElement
                     @user_identifier,
	                 @curriculum_element_id,
	                 @semester",
                     pUserIdentifier,
                     pCurriculumElementId,
                     pSemester
            );
        }
        public void EditControlEvent(int control_event_id, int control_event_type)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pControlEventId = new SqlParameter("@control_event_id", control_event_id);
            SqlParameter pControlEventType = new SqlParameter("@control_event_type", control_event_type);

            Database.ExecuteSqlRaw($@"
                exec EditControlEvent
                     @user_identifier,
	                 @control_event_id,
                     @control_event_type",
                     pUserIdentifier,
                     pControlEventId,
                     pControlEventType
            );
        }
        public void DeleteControlEvent(int control_event_id)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pControlEventId = new SqlParameter("@control_event_id", control_event_id);

            Database.ExecuteSqlRaw($@"
                exec DeleteControlEvent
                     @user_identifier,
	                 @control_event_id",
                     pUserIdentifier,
                     pControlEventId
            );
        }
        public void AddControlEvent(int curriculum_element_id, int control_event_type)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pCurriculumElementId = new SqlParameter("@curriculum_element_id", curriculum_element_id);
            SqlParameter pControlEventType = new SqlParameter("@control_event_type", control_event_type);
            
            Database.ExecuteSqlRaw($@"
                exec AddControlEvent
                     @user_identifier,
	                 @curriculum_element_id,
                     @control_event_type",
                     pUserIdentifier,
                     pCurriculumElementId,
                     pControlEventType
            );
        }
        public void DeleteCurriculumElement(int curriculum_element_id)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pCurriculumElementId = new SqlParameter("@curriculum_element_id", curriculum_element_id);

            Database.ExecuteSqlRaw($@"
                exec DeleteCurriculumElement
                     @user_identifier,
	                 @curriculum_element_id",
                     pUserIdentifier,
                     pCurriculumElementId
            );
        }
        public void SetStudentMark(int control_event_id, int student_id, DateTime date, int mark_type_id)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pControlEventId = new SqlParameter("@control_event_id", control_event_id);
            SqlParameter pStudentId = new SqlParameter("@student_id", student_id);
            SqlParameter pDate = new SqlParameter("@date", date);
            SqlParameter pMarkTypeId = new SqlParameter("@mark_type_id", mark_type_id);

            Database.ExecuteSqlRaw($@"
            exec SetStudentMark
	             @user_identifier,
	             @control_event_id,
	             @student_id,
	             @date,
	             @mark_type_id", 
                 pUserIdentifier,
                 pControlEventId,
                 pStudentId,
                 pDate,
                 pMarkTypeId);
        }
        public void GenerateGroup(int direction_id, byte group_number, int admission_year)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pDirectionId = new SqlParameter("@direction_id", direction_id);
            SqlParameter pGroupNumber = new SqlParameter("@group_number", group_number);
            SqlParameter pAdmissionYear = new SqlParameter("@admission_year", admission_year);

            Database.ExecuteSqlRaw($@"
                exec GenerateGroup
                     @user_identifier,
	                 @direction_id,
	                 @group_number,
	                 @admission_year",
                     pUserIdentifier,
                     pDirectionId,
                     pGroupNumber,
                     pAdmissionYear
            );
        }
        public void DeleteGroup(int group_id)
        {
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", User.CurrentUser?.unique_id);
            SqlParameter pGroupId = new SqlParameter("@group_id", group_id);

            Database.ExecuteSqlRaw($@"
                exec DeleteGroup
                     @user_identifier,
	                 @group_id",
                     pUserIdentifier,
                     pGroupId
            );
        }
        public StudentControlMark? GetStudentControlMark(int? control_event_id, string? unique_id = null)
        {
            if (control_event_id == null) return null;
            var mark = new StudentControlMark();
            SqlParameter pUserIdentifier = new SqlParameter("@user_identifier", unique_id ?? User.CurrentUser?.unique_id);
            SqlParameter pControlEventId = new SqlParameter("@control_event_id", control_event_id);
            SqlParameter pEventType = new()
            {
                ParameterName = "@event_type",
                SqlDbType = SqlDbType.VarChar,
                Size = 24,
                Direction = ParameterDirection.Output
            };
            SqlParameter pSubjectName = new()
            {
                ParameterName = "@subject_name",
                SqlDbType = SqlDbType.VarChar,
                Size = 128,
                Direction = ParameterDirection.Output
            };
            SqlParameter pDepartmentName = new()
            {
                ParameterName = "@department_name",
                SqlDbType = SqlDbType.VarChar,
                Size = 256,
                Direction = ParameterDirection.Output
            };
            SqlParameter pMarkValue = new()
            {
                ParameterName = "@mark_value",
                SqlDbType = SqlDbType.VarChar,
                Size = 8,
                Direction = ParameterDirection.Output
            };
            SqlParameter pTeacherFio = new()
            {
                ParameterName = "@teacher_fio",
                SqlDbType = SqlDbType.VarChar,
                Size = 128,
                Direction = ParameterDirection.Output
            };
            SqlParameter pDate = new()
            {
                ParameterName = "@date",
                SqlDbType = SqlDbType.VarChar,
                Size = 12,
                Direction = ParameterDirection.Output
            };

            Database.ExecuteSqlRaw(
                $@"exec GetStudentControlMark
                    @user_identifier,
	                @control_event_id,
	                @event_type out,
	                @subject_name out,
	                @department_name out,
	                @mark_value out,
	                @teacher_fio out,
	                @date out
                ", pUserIdentifier,
                   pControlEventId,
                   pEventType,
                   pSubjectName,
                   pDepartmentName,
                   pMarkValue,
                   pTeacherFio,
                   pDate);

            mark.control_event_id = (int)pControlEventId.Value;
            mark.event_type = (string)pEventType.Value;
            mark.subject_name = (string)pSubjectName.Value;
            mark.department_name = (string)pDepartmentName.Value;
            mark.mark_value = (string)pMarkValue.Value;
            mark.teacher_fio = (string)pTeacherFio.Value;
            mark.date = (string)pDate.Value;
            return mark;
        }
        public IQueryable<UserFio> GetGroupStudentList(string? uid, int? group_id) => FromExpression(() => GetGroupStudentList(uid, group_id));
        public IQueryable<CurriculumElement> GetUsersCurriculum(string? uid) => FromExpression(() => GetUsersCurriculum(uid));
        public IQueryable<CurriculumElement> GetGroupsCurriculum(string? uid, int? group_id) => FromExpression(() => GetGroupsCurriculum(uid, group_id));
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

            User.InitCurrentUser(
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
