using System;
using System.Collections.Generic;
using System.Text;

namespace ExTra.DTO
{
    /// <summary>
    /// Represents the name of a property.
    /// Used to distinguish property names from regular string variables.
    /// </summary>
    public class PropertyName
    {
        public string Name { get; set; }

        public PropertyName(string name)
        {
            Name = name;
        }
    }
}
