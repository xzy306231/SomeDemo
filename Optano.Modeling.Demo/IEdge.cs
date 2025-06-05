using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optano.Modeling.Demo
{
    public interface IEdge
    {
        /// <summary>
        /// Gets the design cost of this edge, which are applied if the edge is used at all. 
        /// </summary>
        public double DesignCost { get; }

        /// <summary>
        /// Gets the cost per flow unit on this edge
        /// </summary>
        public double CostPerFlowUnit { get; }

        /// <summary>
        /// Gets the capacity of the edge
        /// </summary>
        public double? Capacity { get; }

        /// <summary>
        /// Gets the end node of the edge
        /// </summary>
        public INode ToNode { get; }

        /// <summary>
        /// Gets the start node of the edge
        /// </summary>
        public INode FromNode { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the edge is used in the solution
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// Gets or sets the flow of the optimal solution
        /// </summary>
        public double Flow { get; set; }

    }
}
