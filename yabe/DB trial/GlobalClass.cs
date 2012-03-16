using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB_trial
{
    static class GlobalClass
    {
        private static int m_globalVar = 101;

        public static int GlobalVar
        {
            get { return m_globalVar; }
            set { m_globalVar = value; }
        }


    }

}
