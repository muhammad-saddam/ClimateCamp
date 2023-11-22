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
    public partial class ProductOrSectorSpecificRuleSetInner : IEquatable<ProductOrSectorSpecificRuleSetInner>
    { 
        /// <summary>
        /// Gets or Sets _Operator
        /// </summary>
        [Required]

        [DataMember(Name="operator")]
        public ProductOrSectorSpecificRuleOperator _Operator { get; set; }

        /// <summary>
        /// Gets or Sets RuleNames
        /// </summary>
        [Required]

        [DataMember(Name="ruleNames")]
        public NonEmptyStringVec RuleNames { get; set; }

        /// <summary>
        /// Gets or Sets OtherOperatorName
        /// </summary>

        [DataMember(Name="otherOperatorName")]
        public AllOfProductOrSectorSpecificRuleSetInnerOtherOperatorName OtherOperatorName { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ProductOrSectorSpecificRuleSetInner {\n");
            sb.Append("  _Operator: ").Append(_Operator).Append("\n");
            sb.Append("  RuleNames: ").Append(RuleNames).Append("\n");
            sb.Append("  OtherOperatorName: ").Append(OtherOperatorName).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ProductOrSectorSpecificRuleSetInner)obj);
        }

        /// <summary>
        /// Returns true if ProductOrSectorSpecificRuleSetInner instances are equal
        /// </summary>
        /// <param name="other">Instance of ProductOrSectorSpecificRuleSetInner to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ProductOrSectorSpecificRuleSetInner other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    _Operator == other._Operator ||
                    _Operator != null &&
                    _Operator.Equals(other._Operator)
                ) && 
                (
                    RuleNames == other.RuleNames ||
                    RuleNames != null &&
                    RuleNames.Equals(other.RuleNames)
                ) && 
                (
                    OtherOperatorName == other.OtherOperatorName ||
                    OtherOperatorName != null &&
                    OtherOperatorName.Equals(other.OtherOperatorName)
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
                    if (_Operator != null)
                    hashCode = hashCode * 59 + _Operator.GetHashCode();
                    if (RuleNames != null)
                    hashCode = hashCode * 59 + RuleNames.GetHashCode();
                    if (OtherOperatorName != null)
                    hashCode = hashCode * 59 + OtherOperatorName.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ProductOrSectorSpecificRuleSetInner left, ProductOrSectorSpecificRuleSetInner right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProductOrSectorSpecificRuleSetInner left, ProductOrSectorSpecificRuleSetInner right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
