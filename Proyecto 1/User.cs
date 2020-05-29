using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_1
{
    [Serializable]
    public class User
    {
        public string Email{ get; set; }

        public User(string email)
        {
            Email = email;
        }

    }
}
