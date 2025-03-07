using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.Enums
{
    /// <summary>
    /// Represents gender options for guests.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        /// <summary>
        /// Male gender.
        /// </summary>
        [Display(Name = "Male")]
        Male = 1,

        /// <summary>
        /// Female gender.
        /// </summary>
        [Display(Name = "Female")]
        Female = 2,

        /// <summary>
        /// Other gender or non-binary option.
        /// </summary>
        [Display(Name = "Other")]
        Other = 3
    }
}
