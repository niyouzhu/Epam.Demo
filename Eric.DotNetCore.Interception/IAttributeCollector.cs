using System;
using System.Collections.Generic;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    ///
    /// </summary>
    public interface IAttributeCollector
    {
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        IEnumerable<Attribute> Attributes { get; set; }
    }
}