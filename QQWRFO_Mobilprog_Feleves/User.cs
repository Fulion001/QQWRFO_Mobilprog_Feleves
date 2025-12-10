using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace QQWRFO_Mobilprog_Feleves
{
    
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string Username { get; set; }

        public string Password { get; set; }

    }
}
