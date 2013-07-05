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
       // static DynamicSQL.DynamicSQL db = new DynamicSQL.DynamicSQL();


        static void Main(string[] args)
        {
            using (DynamicSQL.DynamicSQL db = new DynamicSQL.DynamicSQL())
            {
                //for (var a = 0; a < 4000; a++)
                //{
                //    var db2 = new DynamicSQL.DynamicSQL();
                //    db2.Exec("SELECT * FROM Users");
                //    db2.Dispose();
                //}
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

                var result = db.Exec("SELECT * FROM Widgets");
                foreach (var item in result)
                {
                    Console.WriteLine("ID: {0} - Name: {1}", item.WidgetId, item.Name);
                }
                //db.ExecProc("proc_Test");
                //db.ExecProc("proc_Test", new {id=1 });
                //db.Dispose();
            };

            Console.WriteLine("Done! Press any key to exit.");

            Console.ReadLine();
        }
    }
}
