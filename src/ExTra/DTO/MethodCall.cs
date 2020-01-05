using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExTra.DTO
{
    /// <summary>
    /// Represents a method call.
    /// </summary>
    public class MethodCall
    {
        /// <summary>
        /// The name of the method being called.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// The name of the caller.
        /// </summary>
        public string Caller { get; set; }

        /// <summary>
        /// The arguments of the method.
        /// </summary>
        public List<object> Arguments { get; set; }

        public MethodCall(string method, string caller, List<object> arguments)
        {
            Method = method;
            Arguments = arguments;
            Caller = caller;
        }
    }
}
