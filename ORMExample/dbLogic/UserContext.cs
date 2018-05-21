﻿using ORMExample.dbLogic;
using ORMExample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMExample
{
    public class UserContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public Repository<User> UsersContext = new Repository<User>(connectionString);
    }
}
