using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [Serializable]
    public class User
    {
        #region Fields
        public string Email{ get; set; }
        #endregion
        #region Methods
        public User(string email)
        {
            Email = email;
        }

        public override string ToString()
        {
            return Email;
        }
        #endregion
    }
}
