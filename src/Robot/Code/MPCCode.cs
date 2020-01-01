using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.Robot.Code
{
    public class MPCCode
    {
        /// <summary>
        /// 获取 @指定QQ 的操作代码
        /// </summary>
        /// <param name="qq">指定的QQ号码</param>
        /// <returns>MPQ @操作代码</returns>
        public static string MPQCode_At(string qq)
        {
            return "[@" + qq + "]";
        }

    }
}
