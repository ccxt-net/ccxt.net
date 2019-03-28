using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Globalization;

#pragma warning disable 1591

namespace OdinSdk.BaseLib.Serialize
{
    /// <summary>
    /// Default JSON serializer for request bodies
    /// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
    /// </summary>
    public class RestSharpJsonNetSerializer : ISerializer
    {
        /// <summary>
        /// Default serializer
        /// </summary>
        public RestSharpJsonNetSerializer()
        {
            ContentType = "application/json";
        }

        /// <summary>
        /// Serialize the object as JSON
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>JSON as String</returns>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, new RestSerializerSettings(DateFormat));
        }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string DateFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string RootElement
        {
            get;
            set;
        }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string Namespace
        {
            get;
            set;
        }

        /// <summary>
        /// Content type for serialized content
        /// </summary>
        public string ContentType
        {
            get;
            set;
        }
    }
    
    /// <summary>
         /// Default JSON serializer for request bodies
         /// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
         /// </summary>
    public class RestSharpJsonNetDeserializer : IDeserializer
    {
        /// <summary>
        /// Default deserializer
        /// </summary>
        public RestSharpJsonNetDeserializer()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content, new RestSerializerSettings(DateFormat));
        }

        public string RootElement
        {
            get;
            set;
        }

        public string Namespace
        {
            get;
            set;
        }

        public string DateFormat
        {
            get;
            set;
        }
    }

    internal class DecimalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(decimal?));
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var _result = (object)null;

            if (reader.TokenType != JsonToken.Null)
            {
                var _token = JToken.Load(reader);

                if (_token.Type == JTokenType.Float || _token.Type == JTokenType.Integer)
                {
                    _result = _token.ToObject<decimal>();
                }
                else if (_token.Type == JTokenType.String)
                {
                    _result = Decimal.Parse(_token.ToString(), NumberStyles.Number | NumberStyles.AllowExponent);
                }
            }

            return _result;
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RestSerializerSettings : JsonSerializerSettings
    {
        public RestSerializerSettings(string date_format = "")
        {
            NullValueHandling = NullValueHandling.Ignore;
            MissingMemberHandling = MissingMemberHandling.Ignore;

            Formatting = Formatting.None;

            DateFormatString = String.IsNullOrEmpty(date_format) == true ? "yyyy-MM-dd HH:mm:ss" : date_format;
            FloatParseHandling = FloatParseHandling.Decimal;

            DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            Converters = new List<JsonConverter> { new DecimalConverter() };
        }
    }
}