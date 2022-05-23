using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManagerApp.Models.Api
{
    public class SetAccount
    {
        public string Title { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Service { get; set; }
    }
}
