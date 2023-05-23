using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using BizTalkSubscriptionsExtractor.dsBTSubscriptionTableAdapters;
using BizTalkSubscriptionsExtractor.Models;

namespace BizTalkSubscriptionsExtractor
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            txtMgmtDB.Text = Properties.Settings.Default.BizTalkMgmtDb;
            txtMsgBoxDB.Text = Properties.Settings.Default.BizTalkMsgBoxDb;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string filename = "";
            if (string.IsNullOrEmpty(txtMgmtDB.Text))
            {
                MessageBox.Show("Please provide a valid connection string for BizTalkMgmtDb!",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMgmtDB.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtMsgBoxDB.Text))
            {
                MessageBox.Show("Please provide a valid connection string for BizTalkMsgBoxDb!",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMsgBoxDB.Focus();
                return;
            }
            SaveConfig();
            using (var dlg=new SaveFileDialog
            {
                Filter="Xml File(*.xml)|*.xml",
                Title="Save Subscriptions File",
                RestoreDirectory=true
            })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    filename = dlg.FileName;
            }
            if (!string.IsNullOrEmpty(filename))
            {               
                try
                {
                    FillMsgBoxTables();
                    FillMgmtDbTables();
                    CreateSubscriptions(filename);
                    MessageBox.Show("Extracting BizTalk subscriptions has been completed successfully!",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected Error: {ex.GetType().Name}\n{ex.Message}",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void FillMsgBoxTables()
        {
            //var cnxn = new SqlConnection(txtMsgBoxDB.Text);
            SubscriptionTableAdapter daSubscription = new SubscriptionTableAdapter();// { Connection = cnxn };
            PredicateGroupTableAdapter daPredicateGroup = new PredicateGroupTableAdapter();// { Connection = cnxn };
            ConditionsTableAdapter daConditions = new ConditionsTableAdapter();// { Connection = cnxn };
            daSubscription.Fill(dsBTSubscription1.Subscription);
            daPredicateGroup.Fill(dsBTSubscription1.PredicateGroup);
            daConditions.Fill(dsBTSubscription1.Conditions);
            //cnxn.Close();
        }
        private void FillMgmtDbTables() 
        {
            //var cnxn = new SqlConnection(txtMgmtDB.Text);
            bts_sendportgroupTableAdapter daBts_Sendportgroup = new bts_sendportgroupTableAdapter();// { Connection = cnxn };
            bts_sendportTableAdapter daBts_Sendport = new bts_sendportTableAdapter();//{ Connection = cnxn };
            bts_sendport_transportTableAdapter daBts_Sendport_Transport = new bts_sendport_transportTableAdapter();//{ Connection = cnxn };
            bts_receiveportTableAdapter daBts_Receiveport = new bts_receiveportTableAdapter();//{ Connection = cnxn };
            bt_DocumentSpecTableAdapter daBt_DocumentSpec = new bt_DocumentSpecTableAdapter();//{ Connection = cnxn };
            daBts_Sendport.Fill(dsBTSubscription1.bts_sendport);
            daBts_Sendport_Transport.Fill(dsBTSubscription1.bts_sendport_transport);
            daBts_Sendportgroup.Fill(dsBTSubscription1.bts_sendportgroup);
            daBts_Receiveport.Fill(dsBTSubscription1.bts_receiveport);
            daBt_DocumentSpec.Fill(dsBTSubscription1.bt_DocumentSpec);            
        }

        private void CreateSubscriptions(string filename)
        {
            var subscriptions = new List<Subscription>();

            for (int i = 0; i < dsBTSubscription1.Subscription.Count; i++)
            {

                var subscription = new Subscription
                {
                    SubscriptionId = dsBTSubscription1.Subscription[i].uidSubID,
                    BTAppName = dsBTSubscription1.Subscription[i].BTAppName,
                    Name = dsBTSubscription1.Subscription[i].Name,
                    HostName = dsBTSubscription1.Subscription[i].HostName,
                    PortId = dsBTSubscription1.Subscription[i].IsuidPortIDNull() ? Guid.Empty : dsBTSubscription1.Subscription[i].uidPortID,
                    Enabled = dsBTSubscription1.Subscription[i].fEnabled,
                    Paused = dsBTSubscription1.Subscription[i].fPaused,
                    Conditions = new List<ConditionGroup>()
                };
                var grpRows = dsBTSubscription1.Subscription[i].GetPredicateGroupRows();
                for (int j = 0; j < grpRows.Length; j++)
                {
                    var condGrp = new ConditionGroup
                    {
                        Id = grpRows[j].uidPredicateORGroupID,
                        Filters = new List<Filter>()
                    };
                    var filters = grpRows[j].GetConditionsRows();
                    for (int k = 0; k < filters.Length; k++)
                    {
                        var filter = new Filter
                        {
                            FilterGroupId = filters[k].uidPredicateGroupID,
                            FilterId = filters[k].nID,
                            Property = GetMessageProperty(filters[k].uidPropID),
                            Operator = filters[k].PredType,                            
                        };
                        filter.Value = GetValue(filter.Property, filters[k].vtValue);
                        condGrp.Filters.Add(filter);
                    }
                    subscription.Conditions.Add(condGrp);
                }
                subscriptions.Add(subscription);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<Subscription>),new XmlRootAttribute("Conditions"));

            var fs = new FileStream(filename, FileMode.Create);
            serializer.Serialize(fs, subscriptions);
            fs.Flush();
            fs.Close();
        }

        private string GetMessageProperty(Guid propertyId)
        {
            string ret = "";
            var dr = dsBTSubscription1.bt_DocumentSpec.FirstOrDefault(r => r.id == propertyId);
            ret = dr?.msgtype ?? propertyId.ToString();
            return ret;
        }

        private string GetValue(string msgType, object value)
        {
            string ret = value.ToString();
            if (msgType == "http://schemas.microsoft.com/BizTalk/2003/system-properties#SPID")
            {
                var dr = dsBTSubscription1.bts_sendport.FindByuidGUID(Guid.Parse((string)value));
                ret = dr?.nvcName;
            }
            else if (msgType == "http://schemas.microsoft.com/BizTalk/2003/system-properties#SPTransportID")
            {
                var dr = dsBTSubscription1.bts_sendport_transport.FindByuidGUID(Guid.Parse((string)value));
                ret = dr?.bts_sendportRow.nvcName;
            }
            else if (msgType == "http://schemas.microsoft.com/BizTalk/2003/system-properties#ReceivePortID")
            {
                var dr = dsBTSubscription1.bts_receiveport.FindByuidGUID(Guid.Parse((string)value));
                ret = dr?.nvcName;
            }
            else if (msgType == "http://schemas.microsoft.com/BizTalk/2003/system-properties#SPGroupID")
            {
                var dr = dsBTSubscription1.bts_sendportgroup.FindByuidGUID(Guid.Parse((string)value));
                ret = dr?.nvcName;
            }
            return ret;
        }

        private void SaveConfig()
        {
            
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            UpdateConnectionString(connectionStringsSection, "BizTalkSubscriptionsExtractor.Properties.Settings.BizTalkMgmtDb", txtMgmtDB.Text);
            UpdateConnectionString(connectionStringsSection, "BizTalkSubscriptionsExtractor.Properties.Settings.BizTalkMsgBoxDb", txtMsgBoxDB.Text);
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
            Properties.Settings.Default.Reload();
        }

        private void UpdateConnectionString(ConnectionStringsSection section, string connectionKey, string connectionString)
        {
            if (section.ConnectionStrings[connectionKey] == null)
            {
                section.ConnectionStrings.Add(new ConnectionStringSettings(connectionKey , connectionString, "System.Data.SqlClient"));
            }
            else
            {
                section.ConnectionStrings[connectionKey].ConnectionString = connectionString;
            }
        }
    }
}
