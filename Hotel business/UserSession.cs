using Hotel_business.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_business
{
    public static class UserSession
    {
        public static Users CurrentUser { get; set; }
    }
}
