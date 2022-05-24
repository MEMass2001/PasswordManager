using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManagerApp.Models
{
    public class Setting
    {
        public string SettingCode { get; set; }
        public string UserEmail { get; set; }
        public bool Value { get; set; }
    }
}
