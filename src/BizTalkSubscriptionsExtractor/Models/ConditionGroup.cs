using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BizTalkSubscriptionsExtractor.Models
{
    public class ConditionGroup
    {
        [XmlIgnore]
        public Guid SubscriptionId { get; set; }
        [XmlIgnore]
        public Guid Id { get; set; }
        [XmlElement("AND")]
        public List<Filter> Filters { get; set; }
    }
}
