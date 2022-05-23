using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.App.STLDashboardModel
{
    public class STLDashboard
    {
    }
    public class STLParty
    {
        public String PL_ID;
        public String PL_NM;
        public String PL_ADD;
        public String PL_TEL_NO;
        public String URL_STRMNG_NM;
        public String URL_STRMNG;
        public String APP_SHRING_DESC;
    }
    public class STLRequestValidator
    {
        public string Host;
        public string Username;
        public string Password;
    }

    public class HeadOffice
    {
        //Party List Information
        public string parmplid { get; set; }
        public string parmplnm { get; set; }
        public string parmpladd { get; set; }
        public string parmtelno { get; set; }

        public string parmplemladd { get; set; }

        //Group Information
        public string parmpgrpid { get; set; }
        public string parmpsncd { get; set; }
        public string parmpltclid { get; set; }


    }
    public class AgentHeadOffice : HeadOffice
    {
        
        //User Information
        public string parmusrfnm { get; set; }
        public string parmusrlnm { get; set; }
        public string parmusrmnm { get; set; }
        public string parmusrnm { get; set; }
        public string parmusrpsswrd { get; set; }
        public string parmaddrss { get; set; }
        //6
        public string parmusrmobno { get; set; }
        public string parmemladd { get; set; }
        public string parmuserid { get; set; }
    }
    public class PCSCO : AgentHeadOffice
    {
        public string parmurlstrmngnm { get; set; }
        public string parmurlstrmng { get; set; }
        public string parmapkurl { get; set; }
        public string parmapkupdturl { get; set; }
    }
    public class GROUPEVENT
    {
        public string PL_ID;
        public string PGRP_ID;
        public string Title;
        public string Description;
        public string Date;
        public string Time;
        public string Location;
        public string Process;
        public string Event;
    }

    public class LiveStreaming
    {
        public string LiveStreamUrl;
        public string LiveStreamDescription;
        public string Group;
        public string PL_ID;
        public string PGRP_ID;
    }
    public class GroupList
    {
        public string PL_ID;
        public string PGRP_ID;
    }

    public class Donation
    {
        public string PL_ID;
        public string PGRP_ID;
        public double Amount;
        public string Purpose;
        public string DateReceived;
        public string iUSR_ID;
        public string DonoID;
        public string OTP;
        public string MobileNo;
    }
    public class Sitio
    {
        public string Region;
        public string Province;
        public string Municipality;
        public string Brgy;
        public string SitioID;
        public string SitioName;
    }
    public class Barangay
    {
        public string IslandCode;
        public string Region;
        public string Province;
        public string Municipality;
        public string BarangayCode;
        public string BarangayName;
        public string MunicipalityCode;
        public string Code;
        public string Name;
        public string ID;
    }
    public class AcctStatistic
    {
        public string PGRP_ID;
        public string Reg;
        public string Prov;
        public string Mun;
        public string Brgy;
        public string Sitio;

    }
    public class TextBlasting
    {
        public string MobileNumber;
        public string TextMessage;
    }
    public class IndividualText
    {
        public string MobileNumber;
        public string TextMessage;
    }
    public class GenerateBlasting
    {
        public string Prefix;
        public string TextMessage;
        public int FromNo;
        public int ToNo;
    }
    public class TextBlastingMemberAccount
    {
        public string AccntID;
        public string Row_Num;
        public string Row_Num_To;
        public string Search;
    }
}
