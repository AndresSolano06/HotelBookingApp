using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.Enums
{
    /// <summary>
    /// Represents the types of identification documents.
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// Passport document type.
        /// </summary>
        Passport = 1,

        /// <summary>
        /// National ID document type.
        /// </summary>
        NationalId = 2,

        /// <summary>
        /// Driver's license document type.
        /// </summary>
        DriverLicense = 3
    }
}
