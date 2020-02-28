using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customerAPI.Models
{
    /// <summary>
    /// Kind of Address
    /// </summary>
    public enum AddressKind
    {
        /// <summary>
        /// Mailing
        /// </summary>
        Mailing = 0,
        /// <summary>
        /// Billing
        /// </summary>
        Billing = 1,
        /// <summary>
        /// Other
        /// </summary>    
        Other = 2
    }
}
