using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BizTalkSubscriptionsExtractor.Models
{
    public class Filter
    {
        [XmlIgnore]
        public Guid FilterGroupId { get; set; }
        [XmlIgnore]
        public long FilterId {get;set;}
        [XmlAttribute]
        public string Property { get; set; }
        [XmlAttribute] 
        public string Operator { get; set; }
        [XmlText()]
        public string Value { get; set; }
    }
}
