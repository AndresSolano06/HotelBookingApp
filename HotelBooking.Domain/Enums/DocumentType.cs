using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.Enums
{
    /// <summary>
    /// Represents the types of identification documents.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DocumentType
    {
        /// <summary>
        /// Passport document type.
        /// </summary>
        [Display(Name = "Passport")]
        Passport = 1,

        /// <summary>
        /// National ID document type.
        /// </summary>
        [Display(Name = "CC")]
        CC = 2,

        /// <summary>
        /// Driver's license document type.
        /// </summary>
        [Display(Name = "Driver License")]
        DriverLicense = 3
    }
}
