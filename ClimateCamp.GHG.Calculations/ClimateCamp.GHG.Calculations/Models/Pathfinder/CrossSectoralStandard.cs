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
        /// Data Type \"CrossSectoralStandard\" of Spec Version 1
        /// </summary>
        /// <value>Data Type \"CrossSectoralStandard\" of Spec Version 1</value>
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum CrossSectoralStandard
        {
            /// <summary>
            /// Enum GHGProtocolProductStandardEnum for GHG Protocol Product standard
            /// </summary>
            [EnumMember(Value = "GHG Protocol Product standard")]
            GHGProtocolProductStandardEnum = 0,
            /// <summary>
            /// Enum ISOStandard14067Enum for ISO Standard 14067
            /// </summary>
            [EnumMember(Value = "ISO Standard 14067")]
            ISOStandard14067Enum = 1,
            /// <summary>
            /// Enum ISOStandard14044Enum for ISO Standard 14044
            /// </summary>
            [EnumMember(Value = "ISO Standard 14044")]
            ISOStandard14044Enum = 2        }
}