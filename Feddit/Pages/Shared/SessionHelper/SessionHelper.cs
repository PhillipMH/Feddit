﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feddit.Pages.Shared.SessionHelper
{
    public static class SessionHelper
    {
        public static void SetSessionString(this ISession session, string value, string key)
         => session.Set(key, Encoding.UTF8.GetBytes(value));
    }
}