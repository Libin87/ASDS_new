using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASDS_dev.Pages.UsrMgmt
{
    public class LoginResult
    {
        public bool IsSuccess { get; set; } = false;
        public bool IsUserNotFound { get; set; } = false;
        public bool IsPasswordIncorrect { get; set; } = false;
        public string Status { get; set; } = string.Empty;
        public bool IsSuspended { get; set; } = false;
    }

}
