using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optano.Modeling.Demo
{
    public interface INode
    {
        /// <summary>
        /// Gets the demand of this node. A negative demand is supply. 
        /// </summary>
        public double Demand { get; }
        public string Name { get; }
    }
}
