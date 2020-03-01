using System.Text.Json.Serialization;

namespace CustomerAPI3.Models
{
    /// <summary>
    /// Address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Primary Delivery Address
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// Appt #, etc/
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// State of Provence
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Zip Code
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Address Kind
        /// </summary>
        public AddressKind Kind { get; set; }

        /// <summary>
        /// Test if the address is effectively empty
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty
        {
            get
            {
                return
                    string.IsNullOrWhiteSpace(this.Address1) &&
                    string.IsNullOrWhiteSpace(this.City) &&
                    string.IsNullOrWhiteSpace(this.State) &&
                    string.IsNullOrWhiteSpace(this.Zip);
            }
        }
    }
}
