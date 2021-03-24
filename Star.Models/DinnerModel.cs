using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Star.Models
{
    [BsonIgnoreExtraElements]
    public class DinnerModel
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Chef { get; set; }
        public string Address { get; set; }
        public DateTime Start { get; set; }
        public string Time { get; set; }
        public int Duration { get; set; }
        public List<string> Meals { get; set; }
    }
}
