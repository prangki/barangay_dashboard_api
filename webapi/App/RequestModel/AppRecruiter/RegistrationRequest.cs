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
        public string PlaceOfBirth;
        public string Gender;
        public string BloodType;
        public string Nationality;
        public string Citizenship;
        public string MaritalStatus;
        public string PartnerID;

        public string MobileNumber;
        public string EmailAddress;
        public string PrecentNumber;
        public string ClusterNumber;
        public string SequnceNumber;
        public string HomeAddress;
        public string PresentAddress;
        public string Occupation;
        public string Skills;
        public string Height;
        public string Weight;
        public string LivingWParent;


        public string HouseNo;
        public string HouseholdNo;
        public string Family;

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
        public int WDisability;
        public string TypeDisability;
        public int RegisterVoter;
        public int WChildren;
        public int ParentResideBrgy;

        //Parent
        public string FR_ID;
        public string MO_ID;
        public string FrFirstName;
        public string FrMiddleInitial;
        public string FrLastname;
        public string FrFullName;
        public string FrAddress;
        public string FrContactNo;
        public string FrEmail;
        public string MoFirstname;
        public string MoMiddleInitial;
        public string MoLastname;
        public string MoFullName;
        public string MoAddress;
        public string MoContactNo;
        public string MoEmail;

        public string iChildren;
        public string iValidGovernmentID;
        public string iOrganization;
        public string iEducationalAttainment;
        public string iEmployement;
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
    public class BrgySignatory
    {
        public string Brgy_Captain;
        public string Brgy_Captain_sig;
        public string Brgy_Captain_sig_URL;
        public string Capt_Events;
        public string Brgy_FirstCouncilor;
        public string Brgy_FirstCouncilor_sig;
        public string Brgy_FirstCouncilor_sig_URL;
        public string First_Events;
        public string Brgy_SecondCouncilor;
        public string Brgy_SecondCouncilor_sig;
        public string Brgy_SecondCouncilor_sig_URL;
        public string Second_Events;
        public string Brgy_ThirdCouncilor;
        public string Brgy_ThirdCouncilor_sig;
        public string Brgy_ThirdCouncilor_sig_URL;
        public string Third_Events;
        public string Brgy_FourthCouncilor;
        public string Brgy_FourthCouncilor_sig;
        public string Brgy_FourthCouncilor_sig_URL;
        public string Fourth_Events;
        public string Brgy_FifthCouncilor;
        public string Brgy_FifthCouncilor_sig;
        public string Brgy_FifthCouncilor_sig_URL;
        public string Fifth_Events;
        public string Brgy_SixthCouncilor;
        public string Brgy_SixthCouncilor_sig;
        public string Brgy_SixthCouncilor_sig_URL;
        public string Sixth_Events;
        public string Brgy_SeventhCouncilor;
        public string Brgy_SeventhCouncilor_sig;
        public string Brgy_SeventhCouncilor_sig_URL;
        public string Seventh_Events;
        public string SK_Chairman;
        public string SK_Chairman_sig;
        public string SK_Chairman_sig_URL;
        public string SKChairman_Events;
        public string Brgy_Secretary;
        public string Brgy_Secretary_sig;
        public string Brgy_Secretary_sig_URL;
        public string Secretary_Events;
        public string Brgy_Treasurer;
        public string Brgy_Treasurer_sig;
        public string Brgy_Treasurer_sig_URL;
        public string Treasurer_Events;
        public string Brgy_Chief_Tanod;
        public string Brgy_Chief_Tanod_sig;
        public string Brgy_Chief_Tanod_sig_URL;
        public string Tanod_Events;
    }
    public class BusinesseRequest
    {
        public string BussinessID;
        public string ControlNo;
        public string CTCNo;
        public string InitialCapital;
        public string RegisteredNo;
        public string Type;
        public string BusinessName;
        public string Email;
        public string ContactNo;
        public string BusinessAddress;
        public string FirstName;
        public string MiddleInitial;
        public string LastName;
        public string FullName;
        public string OwnerAddress;
        public string OwnerEmail;
        public string OwnerContactNo;

    }
    public class LicenseKey
    {
        public string licensekey;
        public string licenseval;
        public string registereddevice;
        public string licenseexpiry;
    }
    public class OfficialHeader
    {
        public string iBrgyOfficialLogo;
        public string iMunicipalLogo;
        public int BrgyLogoChange;
        public string BrgyOfficialLogo;
        public int MunLogoChange;
        public string MunicipalLogo;
        public string Country;
        public string Province;
        public string Municipality;
        public string Barangay;
        public string IssuedLocation;
    }
}