/*
 * pathfinder-endpoint
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.0.0-beta2
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ClimateCamp.GHG.Calculations.Pathfinder
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PFUpdateEventBody : IEquatable<PFUpdateEventBody>
    { 
        /// <summary>
        /// Gets or Sets PfIds
        /// </summary>
        [Required]

        [DataMember(Name="pfIds")]
        public List<Guid?> PfIds { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PFUpdateEventBody {\n");
            sb.Append("  PfIds: ").Append(PfIds).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((PFUpdateEventBody)obj);
        }

        /// <summary>
        /// Returns true if PFUpdateEventBody instances are equal
        /// </summary>
        /// <param name="other">Instance of PFUpdateEventBody to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PFUpdateEventBody other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    PfIds == other.PfIds ||
                    PfIds != null &&
                    PfIds.SequenceEqual(other.PfIds)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (PfIds != null)
                    hashCode = hashCode * 59 + PfIds.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(PFUpdateEventBody left, PFUpdateEventBody right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PFUpdateEventBody left, PFUpdateEventBody right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}