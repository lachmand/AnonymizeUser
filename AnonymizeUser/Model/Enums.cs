using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonymizeUser.Model
{
    public static class Enums
    {
        public enum UserLevel
        {
            Any = -1,
            Unknown = 0,
            Admin = 5,
            Parent = 2,
            Child = 1
        }

        public enum DeviceOSType
        {
            Any = -1,
            Android = 1,
            iOS = 2
        }

        public enum Gender
        {
            Other, Male, Female
        }


    }//class
}//ns
