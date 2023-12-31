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
        /// Data Type \"DeclaredUnit\" of Spec Version 1
        /// </summary>
        /// <value>Data Type \"DeclaredUnit\" of Spec Version 1</value>
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum DeclaredUnit
        {
            /// <summary>
            /// Enum LiterEnum for liter
            /// </summary>
            [EnumMember(Value = "liter")]
            LiterEnum = 0,
            /// <summary>
            /// Enum KilogramEnum for kilogram
            /// </summary>
            [EnumMember(Value = "kilogram")]
            KilogramEnum = 1,
            /// <summary>
            /// Enum CubicMeterEnum for cubic meter
            /// </summary>
            [EnumMember(Value = "cubic meter")]
            CubicMeterEnum = 2,
            /// <summary>
            /// Enum KilowattHourEnum for kilowatt hour
            /// </summary>
            [EnumMember(Value = "kilowatt hour")]
            KilowattHourEnum = 3,
            /// <summary>
            /// Enum MegajouleEnum for megajoule
            /// </summary>
            [EnumMember(Value = "megajoule")]
            MegajouleEnum = 4,
            /// <summary>
            /// Enum TonKilometerEnum for ton kilometer
            /// </summary>
            [EnumMember(Value = "ton kilometer")]
            TonKilometerEnum = 5,
            /// <summary>
            /// Enum SquareMeterEnum for square meter
            /// </summary>
            [EnumMember(Value = "square meter")]
            SquareMeterEnum = 6        }
}
