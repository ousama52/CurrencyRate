using System;
using System.Data;

namespace MyProject.Models
{
    public static class UserData
    {
        public static DataTable Users { get; private set; }

        static UserData()
        {
            // Initialize DataTable with Username and Password columns
            Users = new DataTable();
            Users.Columns.Add("Username", typeof(string));
            Users.Columns.Add("Password", typeof(string));

            // Add sample user data (for demo purposes)
            Users.Rows.Add("admin", "password"); // Note: Passwords should be hashed in real applications
        }

        public static bool IsValidUser(string username, string password)
        {
            // Check if the username and password match in the DataTable (for demo purposes)
            foreach (DataRow row in Users.Rows)
            {
                if (row["Username"].ToString() == username && row["Password"].ToString() == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
