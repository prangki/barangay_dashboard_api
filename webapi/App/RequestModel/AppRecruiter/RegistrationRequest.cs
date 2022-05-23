using System;
using System.ComponentModel.DataAnnotations;
namespace webapi.App.RequestModel.AppRecruiter
{
    public class RegistrationRequest
    {
        public string RegisterID;
        public string Type;
        public string Role;
        public string GeneralCoordinator;
        public string Coordinator;

        public string Firstname;
        public string Middlename;
        public string Lastname;
        public string Fullname;

        public string BirthDate;
        public string Gender;
        public string BloodType;
        public string Nationality;
        public string Citizenship;
        public string MaritalStatus;

        public string MobileNumber;
        public string EmailAddress;
        public string Address;
        public string Occupation;
        public string Skills;

        public string Region;
        public string Province;
        public string Municipality;
        public string Barangay;

        //
        public string Img;
        public string ImageUrl;
    }
    public class STLMembership
    {
        public string PLID;
        public string PGRPID;
        public string Userid;
        public string Acctid;

        public string Firstname;
        public string Lastname;
        public string Middlename;
        public string Nickname;

        public string BirthDate;
        public string Gender;
        public string BloodType;
        public string Nationality;
        public string Citizenship;
        public string MaritalStatus;

        public string MobileNumber;
        public string EmailAddress;
        public string PrecentNumber;
        public string ClusterNumber;
        public string SequnceNumber;
        public string HomeAddress;
        public string PresentAddress;
        public string Occupation;
        public string Skills;

        public string Region;
        public string Province;
        public string Municipality;
        public string Barangay;
        public string Sitio;
        public string LocationSite;

        public int Leadertype;
        public string AccountType;
        public string Username;
        public string Userpassword;


        public string Img;
        public string ImageUrl;
        public string SignatureURL;
        //public string psncd;

        public string RefGroupID;
        public string RefLDRID;
        public int isEmployed;
    }
    public class DateRangeModel
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
    public class ValidityAccount : DateRangeModel
    {
        public string iUSR_ID { get; set; }
    }
    public class ResetGroupPassword
    {
        public string PL_ID;
        public string PGRP_ID;
        public string isAgent;
        public string NewPassword;
        public string ACT_ID;
        public string USR_ID;

    }
    public class ResetPassword
    {
        public string PL_ID;
        public string PGRP_ID;
        public string MobileNumber;
        public string ACT_ID;
        public string USR_ID;
    }
    public class TransferMember
    {
        public string PL_ID;
        public string PGRP_ID;
        public string Ref_Group_ID;
        public string Ref_LDR_ID;
        public string USR_ID;
        public string ACT_ID;
    }
    public class BlockAccount
    {
        public string PL_ID;
        public string PGRP_ID;
        public string ACT_ID;
    }
    public class ChangeAccountType
    {
        public string PL_ID;
        public string PGRP_ID;
        public string USR_ID;
        public string USR_NM;
        public int AccountType;
    }
    public class TotalRegisterVoter
    {
        public string PLID;
        public string PGRPID;
        public string Reg;
        public string Prov;
        public string Mun;
        public string Brgy;
        public string BrgyProfID;
        public string TotalVoter;
    }
    public class APK
    {
        public string PL_ID;
        public string PGRP_ID;
        public string TRNNo;
        public string APKVerno;
        public string APKName;
        public string APKPath;
        public string APKPathCBA;
    }
    public class GroupLeader
    {
        public string plid;
        public string pgrpid;
        public string usrid;
    }
    public class MasterListRequest
    {
        public string num_row;
        public string search;
        public string reg;
        public string prov;
        public string mun;
        public string brgy;
    }
    public class BarangayList
    {
        public string num_row;
        public string islandcode;
        public string reg;
        public string prov;
        public string mun;
        public string brgy;
    }
    public class BRGYProfileRequest
    {
        public string plid;
        public string pgrpid;
        public string num_row;
        public string reg;
        public string prov;
        public string mun;
        public string brgy;
    }
    public class LicenseKey
    {
        public string licensekey;
        public string licenseval;
        public string registereddevice;
        public string licenseexpiry;
    }
}