using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power
{
    /// <summary>
    /// 别名
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AliasNameAttribute : Attribute
    {
        /// <summary>
        /// 服务别名
        /// </summary>
        public string Name { get; set; }

        public AliasNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
