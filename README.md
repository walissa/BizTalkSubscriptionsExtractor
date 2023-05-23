# BizTalk Subscriptions Extractor
[![Build status](https://waal.visualstudio.com/BizTalk%20Components/_apis/build/status/BizTalk%20Components/BizTalkSubscriptionsExtractor?branchName=main)](https://waal.visualstudio.com/BizTalk%20Components/_apis/build/status/BizTalk%20Components/BizTalkSubscriptionsExtractor?branchName=main)

BizTalk Subscriptions Extractor is a tool helps you extract all BizTalk subscriptions to one XML file for simplifying the analysis of the flows in BizTalk.
If you are seeking for information how to contribute and to make further development on this tool, you may continue reading this document, if you are seeking for more information about how to use this tool, please visit this [blog](https://simplify-it.info/2023/05/22/how-to-extract-biztalk-subscriptions)

## BizTalk MessageBox database and subscriptions
All subscribers are created in BizTalkMsgBoxDb (subscription database in case of multiple message box databases), this tool extracts subscriptions' information from MsgBox tables, those tables can be devided into two groups:
### Group1: subscriber's information
this group contains information such name, BTApplication, Hostname, service class, etc...
- Subscription
- Modules
- Services
- ServiceClasses

### Group2: Predicates and conditions
In BizTalk, a filter is a set of OR groups, each OR group contain several AND conditions, a simple filter as an example can be on the receive port name, that means that the OR set contains only one OR group underwhich there is only one AND condition
```
<OR>
  <AND Property="http://schemas.microsoft.com/BizTalk/2003/system-properties#ReceivePortName" Operator="==">ReceivePort1</AND>
</OR>
```
Another condition is added by BizTalk to handle messages correctly, this condition refers to the subscriber itself
```
<OR>
  <AND Property="http://schemas.microsoft.com/BizTalk/2003/system-properties#SPTransportID" Operator="==">SendPort1</AND>
</OR>
```
And the final view for this subscriber will be
```
<Subscription>
  <Name>SendPort1: {3197AEC7-6760-482A-A6B4-500ADE234012}</Name>
  <BTAppName>Application1</BTAppName>
  <HostName>SendHost</HostName>
  <PortId>3197aec7-6760-482a-a6b4-500ade234012</PortId>
  <Enabled>1</Enabled>
  <Paused>0</Paused>
  <Conditions>
    <OR>
      <AND Property="http://schemas.microsoft.com/BizTalk/2003/system-properties#SPTransportID" Operator="==">SendPort1</AND>
    </OR>
    <OR>
      <AND Property="http://schemas.microsoft.com/BizTalk/2003/system-properties#ReceivePortName" Operator="==">ReceivePort1</AND>
    </OR>
  </Conditions>
</Subscription>
````
So, PredicateGroup rows reflect OR groups, all the AND conditions are related to their group by a field called uidPredicateANDGroupID, but where these conditions are located?
Well, they are located in different tables based on the comparison type,so the tables in Group2 tables will be then:
- PredicateGroup
- BitwiseANDPredicates
- EqualsPredicates
- EqualsPredicates2ndPass
- ExistsPredicates
- GreaterThanOrEqualsPredicates
- GreaterThanPredicates
- LessThanOrEqualsPredicates
- LessThanPredicates
- NotEqualsPredicates

One more point that is worth to pay attention for, all conditions in BizTalk rely on context properties, for that you need to lookup for uidPropId in BizTalkMgmtDb.dbo.bt_DocumentSpec to extract the context properties in a readable text.

## BizTalk Management Database and related definitions
Many conditions for almost all the subscribers are referred to by their Ids, therefore we need to lookup for these Ids in BizTalkMgmtDb to be able to extract readable information.
In the previous example, we saw the SendPort was referred to by its Id ``http://schemas.microsoft.com/BizTalk/2003/system-properties#SPTransportID``, BizTalk Subscriptions Extractor replaces these Ids with their corresponding names, the function responsible for this is called ``GetValue``
The following tables are used to extract the port names:
- bts_receiveport
- bts_sendportgroup
- bts_sendport
- bts_sendport_transport
- bts_orchestration
- bts_orchestration_port

I hope this breif explanation about the tool and its concept will be helpful, you may find out more info through the code, if you have any question about the tool, please do not histate to reach me on my blog [Simplify-IT.info](https://simplify-it.info/2023/05/how-to-extract-biztalk-subscriptions)



