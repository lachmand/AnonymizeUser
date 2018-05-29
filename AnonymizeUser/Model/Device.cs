using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AnonymizeUser.Model.Enums;

namespace AnonymizeUser.Model
{
    public class Device
    {

        public long ID { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string OS { get; set; }
        public DeviceOSType OSType { get; set; }
        public string Created { get; set; }
        public string LastUsed { get; set; }
        public long UserID { get; set; }
        public string UserFullName { get; set; }
        public long UserParentID { get; set; }
        public string UDID { get; set; }
        public string UniqueID { get; set; }
        public string MagicToken { get; set; }
        public string PushToken { get; set; }
        public string UnlockToken { get; set; }
        public int Status { get; set; }             // SMP: My guess is 0=inactive, 1=active
        public string StnVersionName { get; set; }
        public int DeviceUsagePushSent { get; set; }
        public string UninstallDate { get; set; }

    }   // Device

}
