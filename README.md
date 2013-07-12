sql helper dynamicsql 
==========
DynamicSQL for inline sql helper (ADO.NET).
See demo in source code to see how to use it.

一个直接执行SQL语句的帮助类，使用dynamic特性，使用方法可以查看源码中的Demo项目

Four main tips:

    new DynamicSQL.DynamicSQL();
     db.Exec
     db.ExecProc
     db.Dispose() 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    class Program
    {
        //Update this to use your connection string.

        // static DynamicSQL.DynamicSQL db = new DynamicSQL.DynamicSQL("Data Source=.; Initial Catalog=WeBlog; User Id=sa; Password=123456;");
        static DynamicSQL.DynamicSQL db = new DynamicSQL.DynamicSQL();


        static void Main(string[] args)
        {
            //select
            foreach (var person in db.Exec("SELECT * FROM Users"))
            {
                Console.WriteLine("ID: {0} - Email: {1}", person.UserId, person.Email);
            }

            //insert
            db.Exec(@"INSERT INTO [Users]
           (FirstName
           ,LastName
           ,PasswordHash
           ,Email
           ,DateCreated
           ,LastLoginTime
           ,Activated
           ,RoleFlag)
     VALUES
          (@FirstName
           ,@LastName
           ,@PasswordHash
           ,@Email
           ,@DateCreated
           ,@LastLoginTime
           ,@Activated
           ,@RoleFlag)", new { FirstName = "HI", LastName = "hi'2%'ee", PasswordHash = "sss", Email = "we@3.com", DateCreated = DateTime.Now, LastLoginTime = DateTime.Now, Activated = 1, RoleFlag = 1 });

            //update
            db.Exec("UPDATE [Users] SET [FirstName] = @FirstName WHERE Email=@Email", new { FirstName = "Test2", Email = "we@3.com" });

            //delete
            db.Exec("DELETE [Users] WHERE [FirstName] LIKE '%'+@FirstName+'%'", new { FirstName = "Test" });

            //db.ExecProc("proc_Test");
            //db.ExecProc("proc_Test", new {id=1 });
            db.Dispose();
            Console.WriteLine("Done! Press any key to exit.");

            Console.ReadLine();
        }
    }
}
