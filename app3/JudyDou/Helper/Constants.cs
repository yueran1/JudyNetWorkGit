using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudyDou.Helper
{
    public static class Constants
    {
        public enum ListingStatus
        {
            Expired = -1,
            Disabled = 0,
            Active = 1,
            Sold = 2
        }

        public enum UserStatus
        {
            Disabled = 0,
            Normal = 1
        }

        public static string[] AcceptedImageTypes = { ".bmp", ".jpeg", ".jpg", ".png", ".gif" };

        public static string[] PropertyFactors = { "价位和贷款比例",
                                                   "购房目的",
                                                   "地段",
                                                   "房型",
                                                   "房龄",
                                                   "占地面积",
                                                   "户型",
                                                   "房间数",
                                                   "环境要求",
                                                   "景观要求",
                                                   "朝向要求,光照要求",
                                                   "房屋外型喜好",
                                                   "室内设计喜好",
                                                   "花园及植物喜好",
                                                   "介意因素" };
    }
}
