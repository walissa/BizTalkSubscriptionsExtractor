using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BizTalkSubscriptionsExtractor.Models
{
    public class Subscription
    {
        [XmlIgnore]
        public Guid SubscriptionId { get; set; }
        public string Name { get; set; }
        public string BTAppName { get; set; }
        public string HostName { get; set; }
        public Guid PortId { get; set; }
        public int Enabled { get; set; }
        public int Paused { get; set; }
        [XmlArray("Conditions"), XmlArrayItem("OR")]
        public List<ConditionGroup> Conditions { get; set; }
    }
}
