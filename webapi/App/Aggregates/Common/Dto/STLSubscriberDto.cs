using Comm.Commons.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Model.User;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.STLDashboardModel;

namespace webapi.App.Aggregates.Common.Dto
{
    public class STLSubscriberDto
    {
        public static IDictionary<string, object> GetPartyList(IDictionary<string,object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PL_NM = data["PL_NM"].Str();
            o.PL_ADD = data["PL_ADD"].Str();
            o.PL_TEL_NO = data["PL_TEL_NO"].Str();
            o.URL_STRMNG_NM = data["URL_STRMNG_NM"].Str();
            o.URL_STRMNG = data["URL_STRMNG"].Str();
            return o;
        }
        public static STLParty GetSTLParty(IDictionary<string,object> data)
        {
            STLParty o = new STLParty();
            o.PL_ID = data["PL_ID"].Str();
            return o;
        }
        public static IDictionary<string, object> GetGroup(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            //o.PSN_CD = data["PSN_CD"].Str();
            o.PLTCL_NM = data["PLTCL_NM"].Str();
            return o;
        }
        public static IDictionary<string, object> GetGroupSA(IDictionary<string, object> data, string plid, string pgrpid, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.PL_ID = plid;
            o.PGRP_ID = pgrpid;
            o.PSN_CD = data["PSN_CD"].Str();
            o.PLTCL_NM = data["PLTCL_NM"].Str();
            return o;
        }
        public static IDictionary<string, object> GetMenuPage(IDictionary<string,object> data, string accttype)
        {
            string menubar = string.Empty;
            if (data!=null)
                menubar = (data["PGS"].Str().IsEmpty()) ? "" : data["PGS"].Str();
            dynamic o = Dynamic.Object;
            o.MenuPage = (accttype== "1" || accttype=="2") ? menubar : data["PGS"].Str();
            return o;
        }
        public static STLAccount STLUpdateMember(STLMembership data)
        {
            STLAccount o = new STLAccount();
            o.PL_ID = data.PLID;
            o.PGRP_ID = data.PGRPID;
            o.USR_ID = data.Userid;
            o.ACT_TYP = data.AccountType;

            o.FRST_NM = data.Firstname;
            o.LST_NM = data.Lastname;
            o.MDL_NM = data.Middlename;
            o.FLL_NM = $"{data.Lastname}, {data.Firstname} {data.Middlename}";
            o.NCK_NM = data.Nickname;

            o.MOB_NO = data.MobileNumber;
            o.EML_ADD = data.EmailAddress;
            o.HM_ADDR = data.HomeAddress;
            o.PRSNT_ADDR = data.PresentAddress;
            o.LOC_REG = data.Region;
            o.LOC_PROV = data.Province;
            o.LOC_MUN = data.Municipality;
            o.LOC_BRGY = data.Barangay;

            o.GNDR = data.Gender;
            o.MRTL_STAT = data.MaritalStatus;
            o.CTZNSHP = data.Citizenship;
            o.ImageUrl = data.ImageUrl;
            o.BRT_DT = data.BirthDate;
            o.BLD_TYP = data.BloodType;
            o.NATNLTY = data.Nationality;
            o.OCCPTN = data.Occupation;
            o.SKLLS = data.Skills;
            return o;
        }

        public static IEnumerable<dynamic> GetAttachementIssuesConcernList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAttachementIssuesConcern_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAttachementIssuesConcern_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AttachementIssuesConcern_List(e));
        }
        public static IDictionary<string, object> Get_AttachementIssuesConcern_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["CompID"].Str();
            o.GroupID = data["GroupId"].Str();
            o.SequenceNo = data["SequenceNo"].Str();
            o.AttachmentNo = data["AttachmentNo"].Str();
            o.Attachment = data["Attachment"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetAllIssuesConcernList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllIssuesConcern_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllIssuesConcern_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllIssuesConcern_List(e));
        }
        public static IDictionary<string, object> Get_AllIssuesConcern_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.Userid = data["USR_ID"].Str();
            o.Firstname = textInfo.ToTitleCase(data["FRST_NM"].Str());
            o.Lastname = textInfo.ToTitleCase(data["LST_NM"].Str());
            o.Middlename = textInfo.ToTitleCase(data["MDL_NM"].Str());
            o.Fullname = textInfo.ToTitleCase(data["FLL_NM"].Str());
            o.MobileNumber = data["MOB_NO"].Str();
            o.Sitio = data["LOC_SIT"].Str();
            o.SitioName = textInfo.ToTitleCase(data["SIT_NM"].Str());
            o.ImageUrl = data["IMG_URL"].Str();
            o.TransactionNo = data["TRN_NO"].Str();
            o.TicketNo = data["TCKT_NO"].Str();
            o.Subject = data["SBJCT"].Str();
            o.Body = data["BODY"].Str();
            o.STAT = data["STAT"].Str();
            o.CorrectiveAction = data["COR_ACTION"].Str();
            o.IssuedDate = (data["RGS_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["RGS_TRN_TS"].Str()).ToString("MMM, dd, yyyy");
            o.ProcessDate = (data["PRCS_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["PRCS_TRN_TS"].Str()).ToString("MMM dd, yyyy");
            o.ActionDate = (data["FXD_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["FXD_TRN_TS"].Str()).ToString("MMM, dd, yyyy");
            o.TotalAttachment = (data["TTL_ATTCHMNT"].Str()=="0") ? "" : data["TTL_ATTCHMNT"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }


        public static IEnumerable<dynamic> GetAllMemoList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllMemo_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllMemo_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllMemo_List(e));
        }
        public static IDictionary<string, object> Get_AllMemo_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.BrgyCode = data["LOC_BRGY"].Str();
            o.Brgy = data["BRGY"].Str();
            o.MemoID = data["MEMO_ID"].Str();
            o.MemorandumNo = data["MEMO_NO"].Str();
            o.Subject = textInfo.ToTitleCase(data["MEMO_SBJCT"].Str());
            o.MemoURL = data["MEMO_PATH"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }



        public static IEnumerable<dynamic> GetAllDocTypeList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllDocType_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllDocType_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllDocType_List(e));
        }
        public static IDictionary<string, object> Get_AllDocType_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.BrgyCode = data["LOC_BRGY"].Str();
            o.Brgy = data["BRGY"].Str();
            o.DocTypeID = data["DOC_TYP_ID"].Str();
            o.DocTypeNM = data["DOC_TYPE"].Str();
            o.Category = data["CATEGORY"].Str();
            o.CategoryNM = data["CATEGORY_NM"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }

        public static IEnumerable<dynamic> GetAllBrgyOfficialList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllBrgyOfficial_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllBrgyOfficial_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllBrgyOfficial_List(e));
        }
        public static IDictionary<string, object> Get_AllBrgyOfficial_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.BrgyOfficialID = data["BRGY_OFL_ID"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.BrgyCode = data["LOC_BRGY"].Str();
            o.Brgy = data["BRGY"].Str();
            o.SitioCode = data["LOC_SIT"].Str();
            o.Sitio = data["SIT_NM"].Str();
            o.BarangayPositionID = data["BRGY_PSTN_ID"].Str();
            o.BarangayPosition = data["POSITION"].Str();
            o.ElectedOfficial = data["FLL_NM"].Str();
            o.Userid = data["USR_ID"].Str();
            o.RankNo = data["RNK_NO"].Str();
            o.Committee = data["CMTE"].Str();
            o.TermStart = Convert.ToDateTime(data["EF_DATE"].Str()).ToString("MMM dd, yyyy");
            o.TermEnd = Convert.ToDateTime(data["EOT_DATE"].Str()).ToString("MMM dd, yyyy");
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }


        public static IEnumerable<dynamic> GetAllRequestDocumentList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllRequestDocument_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllRequestDocument_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllRequestDocument_List(e));
        }
        public static IDictionary<string, object> Get_AllRequestDocument_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.ReqDocID = data["REQ_DOC_ID"].Str();
            o.DoctypeID = data["DOCTYP_ID"].Str();
            o.DoctypeNM = data["DOCTYP_NM"].Str();
            o.CategoryID = data["CATEGORY"].Str();
            o.Category_Doc_ID = data["CAT_DOC"].Str();
            o.Category_Document = data["CAT_DOC_NM"].Str();
            o.ApplicationDate = (data["APP_DATE"].Str()=="") ? "" : Convert.ToDateTime(data["APP_DATE"].Str()).ToString("MMM dd, yyyy");
            o.BusinessName = data["BIZ_NM"].Str();
            o.BusinessAddress = data["BIZ_ADDRESS"].Str();
            o.BusinessOwnerName = data["BIZ_OWNER"].Str();
            o.BusinessOwnerAddress = data["BIZ_OWNER_ADDRESS"].Str();
            o.Type = data["BIZ_TYP"].Str();
            o.RequestorID = data["REQTR_ID"].Str();
            o.RequestorNM = data["REQTR_NM"].Str();
            o.STATUS = data["STATUS"].Str();
            o.STATUS_NM = data["STATUS_NM"].Str();
            o.CategoryID = data["CATEGORY"].Str();
            o.CategoryNM = data["CATEGORY_NM"].Str();
            o.Purpose = data["PURPOSE"].Str();
            o.ORNO = data["OR_NO"].Str();
            o.CTCNo = data["CTC_NO"].Str();
            o.IssuedDate = (data["DATE_ISSUED"].Str()=="") ? "" : Convert.ToDateTime(data["DATE_ISSUED"].Str()).ToString("MMM dd, yyyy");
            o.Amount = data["AMOUNT"].Str();
            o.BrgyCaptain = data["BRGY_CAPTAIN"].Str();
            o.URL_DocPath = data["URL_DOCPATH"].Str();
            o.ControlNo = data["CNTRL_NO"].Str();
            o.Gender = data["GNDR"].Str();
            o.Gender_NM = data["GNDR_NM"].Str();
            o.MaritalStatus = data["MRTL_STAT"].Str();
            o.MaritalStatus_NM = data["MRTL_STAT_NM"].Str();
            o.Birthdate = data["BRT_DT"].Str();
            o.PurokSitio = data["SIT_NM"].Str();
            o.ProfilePicture = data["PRF_PIC"].Str();
            o.PresentAddress = textInfo.ToUpper(data["PRSNT_ADDR"].Str());
            o.URLAttachment = data["URL_ATTACHMENT"].Str();

            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }

        public static IEnumerable<dynamic> GetAllDocRequirementList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllDocRequirement_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllDocRequirement_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllDocRequirement_List(e));
        }
        public static IDictionary<string, object> Get_AllDocRequirement_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["RW_NO"].Str();
            o.SEQ_NO = data["SEQ_NO"].Str();
            o.DOC_REQRD = data["REQ_NM"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }


        public static IEnumerable<dynamic> Notifications(IDictionary<string, object> data)
        {
            List<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            list.Add(Notification(data));
            return list;
        }
        public static IDictionary<string, object> Notification(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            o.NotificationID = ((int)data["NOTIF_ID"].Str().ToDecimalDouble()).ToString("X");
            o.DateTransaction = data["RGS_TRN_TS"];
            o.Title = data["NOTIF_TTL"].Str();
            o.Description = data["NOTIF_DESC"].Str();
            o.IsCompany = data["S_COMP"].To<bool>(false);
            o.IsOpen = data["S_OPN"].To<bool>(false);
            string type = data["TYP"].Str();
            if (!type.IsEmpty()) o.Type = type;

            try
            {
                DateTime datetime = data["RGS_TRN_TS"].To<DateTime>();
                o.DateDisplay = datetime.ToString("MMM dd, yyyy");
                o.TimeDisplay = datetime.ToString("hh:mm:ss tt");
                o.FulldateDisplay = $"{o.DateDisplay} {o.TimeDisplay}";
            }
            catch { }
            return o;
        }
    }
}
