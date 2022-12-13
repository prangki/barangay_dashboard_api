using System;
using System.Collections.Generic;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using System.Linq;
using System.Globalization;

namespace webapi.App.Aggregates.Common
{
    public class SubscriberDto
    {
        public static IDictionary<string, object> AccountBalance(STLAccount account, IDictionary<string, object> data){
            dynamic o = Dynamic.Object;
            o.Balance = data["ACT_BAL"].Str().ToDecimalDouble();
            o.CreditBalance = data["ACT_CRDT_BAL"].Str().ToDecimalDouble();
            //if(!account.IsPlayer){
            //    var combal = data["ACT_COM_BAL"].Str().ToDecimalDouble();
            //    o.CommissionBalance = combal;
            //}
            var winbal = data["ACT_WIN_BAL"].Str().ToDecimalDouble();
            if(winbal>0) o.WinningBalance = winbal;
            return o;
        }

        public static IEnumerable<dynamic> SearchSubscribers(STLAccount account, IEnumerable<dynamic> list, int limit = 50){
            if(list==null) return null;
            var items = SearchSubscribers(account, list);
            var count = items.Count();
            if(count>=limit){
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count-1).Concat(new[]{ o });
                filter.BaseFilter = o.AccountID;
            }
            return items;
        }

        public static IEnumerable<dynamic> SearchSubscribers(STLAccount account, IEnumerable<dynamic> list){
            if(list==null) return null;
            return list.Select(e=> SearchSubscriber(account, e));
        }

        public static IDictionary<string, object> SearchSubscriber(STLAccount account,  IDictionary<string, object> data){
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            //o.SubscriberID =  data["CUST_ID"].Str();
            o.AccountID =  data["ACT_ID"].Str();
            o.MobileNumber = data["MOB_NO"].Str();
            o.DisplayName = textInfo.ToTitleCase( data["NCK_NM"].Str());
            o.ImageUrl =  data["IMG_URL"].Str();
            o.Firstname = textInfo.ToTitleCase(data["FRST_NM"].Str().ToUpper());
            o.Lastname = textInfo.ToTitleCase(data["LST_NM"].Str().ToUpper());
            o.Fullname = textInfo.ToTitleCase(data["FLL_NM"].Str().ToUpper());
            o.CreditBalance = data["ACT_CRDT_BAL"].Str().ToDecimalDouble();
            o.CommissionBalance = data["ACT_COM_BAL"].Str().ToDecimalDouble();
            //
            string usertype = data["USR_TYP"].Str();
            o.IsPlayer = (usertype.Equals("5"));
            //if(account.IsGeneralCoordinator){
            //    o.IsCoordinator = (usertype.Equals("4"));
            //    o.IsGeneralCoordinator = (usertype.Equals("3"));
            //}
            o.IsBlocked = (data["S_BLCK"].Str().Equals("1"));
            return o;
        }


        public static IEnumerable<dynamic> SearchSubscribers(IEnumerable<dynamic> list, int limit = 50){
            if(list==null) return null;
            var items = SearchSubscribers(list);
            var count = items.Count();
            if(count>=limit){
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count-1).Concat(new[]{ o });
                filter.BaseFilter = o.AccountID;
            }
            return items;
        }

        public static IEnumerable<dynamic> SearchSubscribers(IEnumerable<dynamic> list){
            if(list==null) return null;
            return list.Select(e=> SearchSubscriber(e));
        }

        public static IDictionary<string, object> SearchSubscriber(IDictionary<string, object> data){
            dynamic o = Dynamic.Object;
            //o.SubscriberID =  data["CUST_ID"].Str();
            o.AccountID =  data["ACT_ID"].Str();
            o.MobileNumber = data["MOB_NO"].Str().Replace("+639","09");
            //o.DisplayName =  data["NCK_NM"].Str();
            o.Fullname = data["FLL_NM"].Str().ToUpper();
            o.ImageUrl =  data["IMG_URL"].Str();
            /*o.Firstname = data["FRST_NM"].Str().ToUpper();
            o.Lastname = data["LST_NM"].Str().ToUpper();
            o.Fullname = data["FLL_NM"].Str().ToUpper();
            o.CreditBalance = data["ACT_CRDT_BAL"].Str().ToDecimalDouble();
            o.CommissionBalance = data["ACT_COM_BAL"].Str().ToDecimalDouble();
            //
            string usertype = data["USR_TYP"].Str();
            o.IsPlayer = (usertype.Equals("5"));
            if(account.IsGeneralCoordinator){
                o.IsCoordinator = (usertype.Equals("4"));
                o.IsGeneralCoordinator = (usertype.Equals("3"));
            }
            o.IsPlayer = (usertype.Equals("5"));
            o.IsCoordinator = (usertype.Equals("4"));
            o.IsGeneralCoordinator = (usertype.Equals("3"));
            o.IsBlocked = (data["S_BLCK"].Str().Equals("1"));*/
            return o;
        }


        
        public static IEnumerable<dynamic> SearchRegisters(IEnumerable<dynamic> list, int limit = 50){
            if(list==null) return null;
            var items = SearchRegisters(list);
            var count = items.Count();
            if(count>=limit){
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count-1).Concat(new[]{ o });
                filter.BaseFilter = o.RegisterID;
            }
            return items;
        }

        public static IEnumerable<dynamic> SearchRegisters(IEnumerable<dynamic> list){
            if(list==null) return null;
            return list.Select(e=> SearchRegister(e));
        }

        public static IDictionary<string, object> SearchRegister(IDictionary<string, object> data){
            dynamic o = Dynamic.Object;
            //o.SubscriberID =  data["CUST_ID"].Str();
            o.RegisterID =  data["RGS_ID"].Str();
            o.MobileNumber = data["MOB_NO"].Str().Replace("+639","09");
            //o.DisplayName =  data["NCK_NM"].Str();
            o.ImageUrl =  data["IMG_URL"].Str();
            //
            o.Type =  (int)data["USR_TYP"].Str().ToDecimalDouble();
            o.Role =  (int)data["GRP_CD"].Str().ToDecimalDouble();
            //o.RoleName =  data["NCK_NM"].Str();
            //
            o.Fullname =  data["FLL_NM"].Str();
            o.Lastname =  data["LST_NM"].Str();
            o.Middlename =  data["MDL_NM"].Str();
            o.Firstname =  data["FRST_NM"].Str();

            o.BirthDate =  data["BRT_DT"].Str();
            o.Gender =  data["GNDR"].Str();
            o.BloodType =  data["BLD_TYP"].Str();
            o.Nationality =  data["NATNLTY"].Str();
            o.MaritalStatus =  data["MRTL_STAT"].Str();

            //o.MobileNumber =  data["MOB_NO"].Str();
            o.EmailAddress =  data["EML_ADD"].Str();
            o.Address =  data["PRSNT_ADDR"].Str();
            o.Occupation =  data["OCCPTN"].Str();
            o.Skills =  data["SKLLS"].Str();
            o.GeneralCoordinator =  data["REF_ACT_ID"].Str();
            o.Coordinator =  data["REF_ACT_ID2"].Str();
            o.GeneralCoordinatorName =  data["REF_ACT_NM"].Str();
            o.CoordinatorName =  data["REF_ACT_NM2"].Str();

            o.Region =  data["LOC_REG"].Str();
            o.Province =  data["LOC_PROV"].Str();
            o.Municipality =  data["LOC_MUN"].Str();
            o.Barangay =  data["LOC_BRGY"].Str();

            return o;
        }

        public static IEnumerable<dynamic> GetMemberAccountList(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetMemberAccount_List(data);
            return items;
        }
        public static IEnumerable<dynamic> GetMemberAccount_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_MemberAccount_List(e));
        }
        public static IDictionary<string,object> Get_MemberAccount_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.GRP_NM = textInfo.ToTitleCase(data["PGRP_NM"].Str());
            o.LDR_NM = textInfo.ToTitleCase(data["LDR_NM"].Str());
            o.FLL_NM = textInfo.ToTitleCase(data["FLL_NM"].Str());
            o.USR_ID = data["USR_ID"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.S_BLCK = data["S_BLCK"].Str();
            o.isEmployed_NM = data["isEmployed_NM"].Str();
            return o;
        }
        public static IEnumerable<dynamic> GetAPKList(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAPK_List(data);
            return items;
        }
        public static IEnumerable<dynamic> GetAPK_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_APK_List(e));
        }
        public static IDictionary<string, object> Get_APK_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.APK_TRN = data["APK_TRN"].Str();
            o.APK_VER = data["APK_VER"].Str();
            o.APK_NM = data["APK_NM"].Str();
            o.APK_PATH_CBA = data["APK_PATH_CBA"].Str();
            o.APK_PATH = data["APK_PATH"].Str();
            o.RGS_TRN_DT = data["RGS_TRN_DT"].Str();
            o.RGS_TRN_TM = data["RGS_TRN_TM"].Str();
            o.RGS_TRN_TS = data["RGS_TRN_TS"].Str();
            o.APK_PRIMARY = Convert.ToBoolean((data["APK_PRIMARY"].Str()=="0") ? 0 : 1);
            o.UPD_TRN_TS = data["UPD_TRN_TS"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetBrgyList(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetBrgyLists(data);
            return items;
        }
        public static IEnumerable<dynamic> GetBrgyLists(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => GetBrgy_List(e));
        }
        public static IDictionary<string, object> GetBrgy_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.Code = data["BRGY_CODE"].Str();
            o.Name = data["BRGY"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetGroupLeaderAccount(IEnumerable<dynamic> data)
        {
            if (data == null) return null;
            var items = Get_GroupLeaderAccount(data);
            var count = items.Count();
            return items;

        }
        public static IEnumerable<dynamic> Get_GroupLeaderAccount(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_GroupLeader_Account(e));
        }
        public static IDictionary<string, object> Get_GroupLeader_Account(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.Num_Row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.SUBSCR_TYP = data["SUBSCR_TYP"].Str();
            o.S_BLCK = Convert.ToBoolean(data["S_BLCK"].Str());
            o.REF_GRP_ID = data["REF_GRP_ID"].Str();

            o.REF_LDR_ID = data["REF_LDR_ID"].Str();
            o.LDR_TYP = data["LDR_TYP"].Str();
            o.SUBSCR_ID = data["SUBSCR_ID"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.LOC_REG = data["LOC_REG"].Str();

            o.LOC_REG_NM = data["LOC_REG_NM"].Str();
            o.LOC_PROV = data["LOC_PROV"].Str();
            o.LOC_PROV_NM = data["LOC_PROV_NM"].Str();
            o.LOC_MUN = data["LOC_MUN"].Str();
            o.LOC_MUN_NM = data["LOC_MUN_NM"].Str();

            o.LOC_BRGY = data["LOC_BRGY"].Str();
            o.LOC_BRGY_NM = data["LOC_BRGY_NM"].Str();
            o.LOC_SIT = data["LOC_SIT"].Str();
            o.SIT_NM = textInfo.ToUpper(textInfo.ToLower(data["SIT_NM"].Str()));
            o.TTL_USR = Convert.ToInt32(data["TTL_USR"].Str());

            o.LDR_TYP_NM = data["LDR_TYP_NM"].Str();
            o.FRST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FRST_NM"].Str()));
            o.LST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LST_NM"].Str()));
            o.FLL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FLL_NM"].Str()));
            o.NCK_NM = textInfo.ToTitleCase(textInfo.ToLower(data["NCK_NM"].Str()));

            o.MDL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["MDL_NM"].Str()));
            o.PRSNT_ADDR = textInfo.ToUpper(textInfo.ToLower(data["PRSNT_ADDR"].Str()));
            o.HM_ADDR = textInfo.ToUpper(textInfo.ToLower(data["HM_ADDR"].Str())); 
            o.EML_ADD = data["EML_ADD"].Str();
            o.MOB_NO = data["MOB_NO"].Str();

            o.PRCNT_NO = textInfo.ToTitleCase(textInfo.ToLower(data["PRCNT_NO"].Str()));
            o.CLSTR_NO = textInfo.ToTitleCase(textInfo.ToLower(data["CLSTR_NO"].Str()));
            o.BRT_DT = data["BRT_DT"].Str();
            o.BLD_TYP = data["BLD_TYP"].Str();
            o.GNDR = data["GNDR"].Str();

            o.GNDR_NM = data["GNDR_NM"].Str();
            o.MRTL_STAT = data["MRTL_STAT"].Str();
            o.MRTL_STAT_NM = data["MRTL_STAT_NM"].Str();
            o.PRF_PIC = data["PRF_PIC"].Str();
            o.SIGNATUREID = data["SIGNATUREID"].Str();

            o.NATNLTY = data["NATNLTY"].Str();
            o.CTZNSHP = data["CTZNSHP"].Str();
            o.OCCPTN = data["OCCPTN"].Str();
            o.SKLLS = data["SKLLS"].Str();
            o.USR_NM = data["USR_NM"].Str();
            o.Age = data["AGE"].Str();
            o.WDisability = (data["WDISABILITY"].Str()=="") ? 0 : Convert.ToInt32(data["WDISABILITY"].Str());
            o.TypeDisability = data["TYPE_DISABILITY"].Str();
            o.RegisterVoter = (data["REGISTERVOTER"].Str()=="") ? 0 : Convert.ToInt32(data["REGISTERVOTER"].Str());

            return o;
        }


        public static IEnumerable<dynamic> GetLeaderAccount(IEnumerable<dynamic> data)
        {
            if (data == null) return null;
            var items = Get_LeaderAccount(data);
            var count = items.Count();
            return items;

        }
        public static IEnumerable<dynamic> Get_LeaderAccount(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Leader_Account(e));
        }
        public static IDictionary<string, object> Get_Leader_Account(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.USR_ID = data["USR_ID"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.FRST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FRST_NM"].Str()));
            o.MDL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["MDL_NM"].Str()));
            o.LST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LST_NM"].Str()));
            o.FLL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FLL_NM"].Str()));
            o.REF_GRP_ID = data["REF_GRP_ID"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetMobileMemberAccount(IEnumerable<dynamic> data)
        {
            if (data == null) return null;
            var items = Get_MobileMemberAccount(data);
            var count = items.Count();
            return items;

        }
        public static IEnumerable<dynamic> Get_MobileMemberAccount(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_MobileMember_Account(e));
        }
        public static IDictionary<string, object> Get_MobileMember_Account(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.Num_Row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.USR_ID = data["USR_ID"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.SUBSCR_TYP = data["SUBSCR_TYP"].Str();
            o.S_BLCK = data["S_BLCK"].Str();
            o.REF_GRP_ID = data["REF_GRP_ID"].Str();
            o.REF_LDR_ID = data["REF_LDR_ID"].Str();
            o.FRST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FRST_NM"].Str()));
            o.MDL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["MDL_NM"].Str()));
            o.LST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LST_NM"].Str()));
            o.FLL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FLL_NM"].Str()));
            o.MOB_NO = data["MOB_NO"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetMasterList(IEnumerable<dynamic> data, int limit=100,bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetMaster_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetMaster_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Master_List(e));
        }
        public static IDictionary<string, object> Get_Master_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PSN_CD = data["PSN_CD"].Str();
            o.PSN_NM = data["PSN_NM"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.PGRP_NM = data["PGRP_NM"].Str();
            o.PLTCL_NM = data["PLTCL_NM"].Str();
            o.PSN_NM = data["PSN_NM"].Str();
            o.USR_ID = data["USR_ID"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.ACT_TYP = data["ACT_TYP"].Str();
            o.FRST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FRST_NM"].Str()));
            o.MDL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["MDL_NM"].Str()));
            o.LST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LST_NM"].Str()));
            o.NCK_NM = textInfo.ToTitleCase(textInfo.ToLower(data["NCK_NM"].Str()));
            o.FLL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FLL_NM"].Str()));
            o.MOB_NO = data["MOB_NO"].Str();
            o.EML_ADD = data["EML_ADD"].Str();
            o.USR_NM = data["USR_NM"].Str();
            o.PRCNT_NO = data["PRCNT_NO"].Str();
            o.CLSTR_NO = data["CLSTR_NO"].Str();
            o.SEQUENCE_NO = data["SEQUENCE_NO"].Str();
            o.HM_ADDR = textInfo.ToTitleCase(textInfo.ToLower(data["HM_ADDR"].Str()));
            o.PRSNT_ADDR = textInfo.ToTitleCase(textInfo.ToLower(data["PRSNT_ADDR"].Str()));
            o.LOC_REG = data["LOC_REG"].Str();
            o.LOC_REG_NM = data["LOC_REG_NM"].Str();
            o.LOC_PROV = data["LOC_PROV"].Str();
            o.LOC_PROV_NM = data["LOC_PROV_NM"].Str();
            o.LOC_MUN = data["LOC_MUN"].Str();
            o.LOC_MUN_NM = data["LOC_MUN_NM"].Str();
            o.LOC_BRGY = data["LOC_BRGY"].Str();
            o.LOC_BRGY_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LOC_BRGY_NM"].Str()));
            o.LOC_SIT = data["LOC_SIT"].Str();
            o.LOC_SIT_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LOC_SIT_NM"].Str()));
            o.GNDR = data["GNDR"].Str();
            o.GNDR_NM = data["GNDR_NM"].Str();
            o.MRTL_STAT = data["MRTL_STAT"].Str();
            o.MRTL_STAT_NM = data["MRTL_STAT_NM"].Str();
            o.CTZNSHP = data["CTZNSHP"].Str();
            o.IMG_URL = data["IMG_URL"].Str();
            o.BRT_DT = data["BRT_DT"].Str();
            o.BLD_TYP = data["BLD_TYP"].Str();
            o.NATNLTY = textInfo.ToTitleCase(textInfo.ToLower(data["NATNLTY"].Str()));
            o.OCCPTN = textInfo.ToTitleCase(textInfo.ToLower(data["OCCPTN"].Str()));
            o.SKLLS = textInfo.ToTitleCase(textInfo.ToLower(data["SKLLS"].Str()));
            o.PRF_PIC = data["PRF_PIC"].Str();
            o.isEmployed = data["isEmployed"].Str();
            o.SIGNATUREID = data["SIGNATUREID"].Str();
            o.S_BLCK = Convert.ToBoolean((data["S_BLCK"].Str() == "0") ? 0 : 1);
            return o;
        }



        public static IEnumerable<dynamic> GetMasterList1(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetMaster_List1(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetMaster_List1(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Master_List1(e));
        }
        public static IDictionary<string, object> Get_Master_List1(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.Num_Row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            //o.PSN_CD = data["PSN_CD"].Str();
            //o.PSN_NM = data["PSN_NM"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.PGRP_NM = data["PGRP_NM"].Str();
            o.PLTCL_NM = data["PLTCL_NM"].Str();
            //o.PSN_NM = data["PSN_NM"].Str();
            o.USR_ID = data["USR_ID"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.ACT_TYP = data["ACT_TYP"].Str();
            o.HouseNo = data["HSENO_ID"].Str();
            o.HouseClassification = data["HSE_CLSFCTN"].Str();
            o.HouseOwner = data["HSE_OWNR"].Str();
            o.HouseholdNo = data["HHLD_ID"].Str();
            o.HouseholdNM = data["HHLD_NM"].Str();
            o.Family = data["FAM_ID"].Str();
            o.FamilyNM = data["FAM_NM"].Str();
            o.TitleName = data["NM_TTL"].Str();
            o.FRST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FRST_NM"].Str()));
            o.MDL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["MDL_NM"].Str()));
            o.LST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LST_NM"].Str()));
            o.NCK_NM = textInfo.ToTitleCase(textInfo.ToLower(data["NCK_NM"].Str()));
            o.FLL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FLL_NM"].Str()));
            o.Religion = textInfo.ToTitleCase(textInfo.ToLower(data["REL"].Str()));
            o.MOB_NO = data["MOB_NO"].Str();
            o.EML_ADD = data["EML_ADD"].Str();
            o.USR_NM = data["USR_NM"].Str();
            o.Height = data["HEIGHT"].Str();
            o.Weight = data["WEIGHT"].Str();
            o.PRCNT_NO = data["PRCNT_NO"].Str();
            o.CLSTR_NO = data["CLSTR_NO"].Str();
            o.SEQUENCE_NO = data["SEQUENCE_NO"].Str();
            o.HM_ADDR = textInfo.ToUpper(textInfo.ToLower(data["HM_ADDR"].Str()));
            o.PRSNT_ADDR = textInfo.ToUpper(textInfo.ToLower(data["PRSNT_ADDR"].Str()));
            o.PartnerID = data["PTNR_ID"].Str();
            o.LOC_REG = data["LOC_REG"].Str();
            o.LOC_REG_NM = data["LOC_REG_NM"].Str();
            o.LOC_PROV = data["LOC_PROV"].Str();
            o.LOC_PROV_NM = data["LOC_PROV_NM"].Str();
            o.LOC_MUN = data["LOC_MUN"].Str();
            o.LOC_MUN_NM = data["LOC_MUN_NM"].Str();
            o.LOC_BRGY = data["LOC_BRGY"].Str();
            o.LOC_BRGY_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LOC_BRGY_NM"].Str()));
            o.LOC_SIT = data["LOC_SIT"].Str();
            o.LOC_SIT_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LOC_SIT_NM"].Str()));
            o.GNDR = data["GNDR"].Str();
            o.GNDR_NM = data["GNDR_NM"].Str();
            o.MRTL_STAT = data["MRTL_STAT"].Str();
            o.MRTL_STAT_NM = data["MRTL_STAT_NM"].Str();
            o.CTZNSHP = data["CTZNSHP"].Str();
            o.Religion = data["REL"].Str();
            o.IMG_URL = data["IMG_URL"].Str();
            o.BRT_DT = data["BRT_DT"].Str();
            o.PlaceOfBirth = data["PLC_BRT"].Str();
            o.BLD_TYP = data["BLD_TYP"].Str();
            o.NATNLTY = textInfo.ToTitleCase(textInfo.ToLower(data["NATNLTY"].Str()));
            o.Profession = data["PROFESSION"].Str();
            o.OCCPTN = textInfo.ToTitleCase(textInfo.ToLower(data["OCCPTN"].Str()));
            o.SKLLS = textInfo.ToTitleCase(textInfo.ToLower(data["SKLLS"].Str()));
            o.PRF_PIC = data["PRF_PIC"].Str();
            o.LeftThumb = "";
            o.RightThumb = "";
            o.Taken_ProfPic = data["TAKEN_PIC"].Str();
            o.FR_ID = data["FR_ID"].Str();
            o.MO_ID = data["MO_ID"].Str();
            o.FrFirstName = data["FR_FRST_NM"].Str();
            o.FrMiddleInitial = data["FR_MI_NM"].Str();
            o.FrLastname = data["FR_LST_NM"].Str();
            o.FrFullName = data["FR_FLL_NM"].Str();
            o.FrContactNo = data["FR_MOB_NO"].Str();
            o.FrEmail = data["FR_EML_ADD"].Str();
            o.FrAddress = data["FR_PRSNT_ADDRESS"].Str();
            o.MoFirstname = data["MOM_FRST_NM"].Str();
            o.MoMiddleInitial = data["MOM_MI_NM"].Str();
            o.MoLastname = data["MOM_LST_NM"].Str();
            o.MoFullName = data["MOM_FLL_NM"].Str();
            o.MoContactNo = data["MOM_MOB_NO"].Str();
            o.MoEmail = data["MOM_EML_ADD"].Str();
            o.MoAddress = data["MOM_PRSNT_ADDRESS"].Str();
            //o.isEmployed = data["isEmployed"].Str();
            o.SIGNATUREID = data["SIGNATUREID"].Str();
            o.S_BLCK = Convert.ToBoolean((data["S_BLCK"].Str() == "0") ? 0 : 1);
            o.Age = data["AGE"].Str();
            o.WDisability = (data["WDISABILITY"].Str() == "") ? 0 : Convert.ToInt32(data["WDISABILITY"].Str());
            o.TypeDisability = data["TYPE_DISABILITY"].Str();
            o.RegisterVoter = (data["REGISTERVOTER"].Str() == "") ? 0 : Convert.ToInt32(data["REGISTERVOTER"].Str());
            o.WChildren = (data["WDependent"].Str() == "") ? 0 : Convert.ToInt32(data["WDependent"].Str());
            o.LivingWParent = (data["LivingWParent"].Str() == "") ? 0 : Convert.ToInt32(data["LivingWParent"].Str());
            o.ParentResideBrgy = (data["Parent_Reside_Brgy"].Str() == "") ? 0 : Convert.ToInt32(data["Parent_Reside_Brgy"].Str());
            o.TotalRequest = (data["TTL_REQ"].Str() == "") ? 0 : Convert.ToInt32(data["TTL_REQ"].Str());
            o.TotalReceived = (data["TTL_REQ"].Str() == "") ? 0 : Convert.ToInt32(data["TTL_REC"].Str());
            o.TotalSummon = (data["TTL_SUMMON"].Str() == "") ? 0 : Convert.ToInt32(data["TTL_SUMMON"].Str());
            o.CauseofDeath = data["CAUSEOFDEATH"].Str();
            o.DiedDate = data["DCDATE"].Str();
            o.DiedTime = data["DCTIME"].Str();
            //o.PermanentResidence = (data["PERM_RES"].Str() == "") ? 0 : Convert.ToInt32(data["PERM_RES"].Str());

            o.PermanentResidence = (data["PERM_RES"].Str() == "") ? 0 : Convert.ToInt32(data["PERM_RES"]);
            o.MonthlyIncome = data["MON_INC"].Str();
            o.ResidentType = data["TYP_RES"].Str();
            o.DateReside = data["DT_RES"].Str();
            o.CompleteAddress = data["CMPLT_ADDRESS"].Str();
            o.SeniorCitizenMember = (data["SR_CIT"].Str() == "") ? 0 : Convert.ToInt32(data["SR_CIT"]);
            o.SingleParent = (data["SGL_PAR"].Str() == "") ? 0 : Convert.ToInt32(data["SGL_PAR"]);
            o.IndigentFamily = (data["IND_FAM"].Str() == "") ? 0 : Convert.ToInt32(data["IND_FAM"]);

            return o;
        }


        public static IEnumerable<dynamic> GetParentList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetParent_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetParent_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Parent_List(e));
        }
        public static IDictionary<string, object> Get_Parent_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.USR_ID = data["USR_ID"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.ACT_TYP = data["ACT_TYP"].Str();
            o.HouseNo = data["HSENO_ID"].Str();
            o.HouseHold = data["HHLD_ID"].Str();
            o.Family = data["FAM_ID"].Str();
            o.FRST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FRST_NM"].Str()));
            o.MDL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["MDL_NM"].Str()));
            o.LST_NM = textInfo.ToTitleCase(textInfo.ToLower(data["LST_NM"].Str()));
            o.NCK_NM = textInfo.ToTitleCase(textInfo.ToLower(data["NCK_NM"].Str()));
            o.FLL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FLL_NM"].Str()));
            o.BRT_DT = data["BRT_DT"].Str();
            o.Age = data["AGE"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetBrygClearanceList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetBrygClearance_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetBrygClearance_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_BrygClearance_List(e));
        }
        public static IDictionary<string, object> Get_BrygClearance_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.ClearanceNo = data["BRGYCLR_ID"].Str();
            o.ControlNo = data["CNTRL_NO"].Str();
            o.UserID = data["USR_ID"].Str();
            o.ResidentIDNo = data["RESIDENT_ID_NO"].Str();
            o.Requestor = textInfo.ToUpper(textInfo.ToLower(data["FLL_NM"].Str()));
            o.TypeofClearance = data["CERTTYP_ID"].Str();
            o.ClearanceType = textInfo.ToUpper(textInfo.ToLower(data["CERTTYP_NM"].Str()));
            o.Purpose = data["PURP_ID"].Str();
            o.PurposeNM = textInfo.ToUpper(textInfo.ToLower(data["PURP_NM"].Str()));

            o.ORNumber = textInfo.ToUpper(textInfo.ToLower(data["OR_NO"].Str()));
            //o.AmountPaid = Convert.ToDecimal(string.Format("{0:#.0}", data["AMOUNT_PAID"].Str()));
            o.AmountPaid = (data["AMOUNT_PAID"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDouble(data["AMOUNT_PAID"].Str()).ToString("n2");
            o.DocStamp = (data["DOC_STAMP"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDecimal(data["DOC_STAMP"].Str()).ToString("n2");
            o.TotalAmount = (data["TTL_AMNT"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDecimal(data["TTL_AMNT"].Str()).ToString("n2");

            o.EnableCommunityTax = Convert.ToBoolean(data["ENABLECTC"].Str());
            o.CTCNo = textInfo.ToUpper(textInfo.ToLower(data["CTC"].Str()));
            o.CTCIssuedAt = textInfo.ToUpper(textInfo.ToLower(data["CTCISSUEDAT"].Str()));
            o.CTCIssuedOn = (data["CTCISSUEDON"].Str() == "") ? "" : Convert.ToDateTime(data["CTCISSUEDON"].Str()).ToString("MMM dd, yyyy");

            o.VerifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["VERIFIEDBY"].Str()));
            o.CertifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["CERTIFIEDBY"].Str()));

            o.Release = (data["RELEASED"].Str() == "") ? false : Convert.ToBoolean(data["RELEASED"].Str());
            o.MosValidity = (data["MOS_VAL"].Str() == "") ? 0 : Convert.ToInt32(data["MOS_VAL"].Str());
            o.IssuedDate = (data["DATEPROCESS"].Str() == "") ? "" : Convert.ToDateTime(data["DATEPROCESS"].Str()).ToString("MMM dd, yyyy");
            o.DateRelease = (data["DATERELEASED"].Str() == "") ? "" : Convert.ToDateTime(data["DATERELEASED"].Str()).ToString("MMM dd, yyyy");
            o.ExpiryDate = (data["VALIDITYDATE"].Str() == "" ) ? "" : Convert.ToDateTime(data["VALIDITYDATE"].Str()).ToString("MMM dd, yyyy");
            o.ReleasedBy = textInfo.ToUpper(textInfo.ToLower(data["RELEASEDBY"].Str()));
            o.Cancelled = (data["CANCELLED"].Str() == "") ? false : Convert.ToBoolean(data["CANCELLED"].Str());
            o.CancelledBy = textInfo.ToUpper(textInfo.ToLower(data["CANCELLEDBY"].Str()));
            o.CancelledDate = (data["DATECANCELLED"].Str() == "") ? "" : Convert.ToDateTime(data["DATECANCELLED"].Str()).ToString("MMM dd, yyyy");
            o.AppointmentDate = (data["APP_DATE"].Str() == "") ? "" : Convert.ToDateTime(data["APP_DATE"].Str()).ToString("MMM dd, yyyy");
            return o;
        }

        public static IEnumerable<dynamic> GetLegalDocumentList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetLegalDocument_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetLegalDocument_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Ge_LegalDocument_List(e));
        }
        public static IDictionary<string, object> Ge_LegalDocument_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.LegalDocID = data["LGLDOC_ID"].Str();
            o.ControlNo = data["CONTROLNO"].Str();
            o.RequestorID = data["REQUESTORID"].Str();
            o.RequestorName = data["REQUESTORNM"].Str();
            o.TypeofTemplateID = data["TPLTYP_ID"].Str();
            o.TypeofTemplateNM = data["TPLTTYP_NM"].Str();
            o.FormsCertificateID = data["TPL_ID"].Str();
            o.FormsCertificateNM = data["TPL_NM"].Str();
            o.ReportContent = data["TPL_CONT"].Str();

            o.ORNumber = data["OR_NO"].Str();
            o.AmountPaid = (data["AMOUNT_PAID"].Str() == "") ? Convert.ToDouble("0").ToString("n2") : Convert.ToDouble(data["AMOUNT_PAID"].Str()).ToString("n2");

            o.VerifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["VERIFIEDBY"].Str()));
            o.CertifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["CERTIFIEDBY"].Str()));

            o.ProcessDate = (data["DATEPROCESS"].Str() == "") ? "" : Convert.ToDateTime(data["DATEPROCESS"].Str()).ToString("MMM dd, yyyy");
            o.Release = (data["RELEASED"].Str() == "") ? false : Convert.ToBoolean(data["RELEASED"].Str());
            o.DateRelease = (data["DATERELEASED"].Str() == "") ? "" : Convert.ToDateTime(data["DATERELEASED"].Str()).ToString("MMM dd, yyyy");
            o.ReleasedBy = textInfo.ToUpper(textInfo.ToLower(data["RELEASEDBY"].Str()));
            o.Cancelled = (data["CANCELLED"].Str() == "") ? false : Convert.ToBoolean(data["CANCELLED"].Str());
            o.CancelledBy = textInfo.ToUpper(textInfo.ToLower(data["CANCELLEDBY"].Str()));
            o.CancelledDate = (data["DATECANCELLED"].Str() == "") ? "" : Convert.ToDateTime(data["DATECANCELLED"].Str()).ToString("MMM dd, yyyy");
            o.StatusRequest = (data["STAT_REQ"].Str() == "") ? 0 : Convert.ToInt32(data["STAT_REQ"].Str());
            o.AppointmentDate = (data["APP_DATE"].Str() == "") ? "" : Convert.ToDateTime(data["APP_DATE"].Str()).ToString("MMM dd, yyyy");
            return o;
        }


        public static IEnumerable<dynamic> GetLegalDocumentDetailsList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetLegalDocumentDetails_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetLegalDocumentDetails_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_LegalDocumentDetails_List(e));
        }
        public static IDictionary<string, object> Get_LegalDocumentDetails_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.FormsCertificateID = data["TPL_ID"].Str();
            o.LegalDocID = data["LGLDOC_ID"].Str();
            o.TagDescription = data["TAG_DESCR"].Str();
            o.Tagline = data["TAGLINE"].Str();
            o.TaglineInputType = data["TAG_IPTYP"].Str();
            o.Value = data["Value"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetBrygBusinessClearanceList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetBrygBusinessClearance_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetBrygBusinessClearance_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_BrygBusinessClearance_List(e));
        }
        public static IDictionary<string, object> Get_BrygBusinessClearance_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.BusinessClearanceID = data["BIZCLR_ID"].Str();
            o.ControlNo = data["CTNRL_NO"].Str();
            o.BusinessID = data["BIZ_ID"].Str();
            o.BusinessNM = textInfo.ToUpper(textInfo.ToLower(data["BIZ_NM"].Str()));
            o.BusinessAddress = textInfo.ToUpper(textInfo.ToLower(data["BIZ_ADDR"].Str()));
            o.DateOperate = data["DATE_OPRT"].Str();
            o.OwnerID = data["OWNER_ID"].Str();
            o.OwnerNM = textInfo.ToUpper(textInfo.ToLower(data["OWNER_NM"].Str()));
            o.OwnerAddress = textInfo.ToUpper(textInfo.ToLower(data["OWNR_ADDRESS"].Str()));
            o.DateIssued = data["DATEISSUED"].Str();
            o.ExpiryDate = data["EXPIREDDATE"].Str();

            o.ORNumber = textInfo.ToUpper(textInfo.ToLower(data["OR_NO"].Str()));
            //o.AmountPaid = Convert.ToDecimal(string.Format("{0:#.0}", data["AMOUNT_PAID"].Str()));
            o.AmountPaid = (data["AMOUNT_PAID"].Str() == "") ? Convert.ToDouble(data["AMOUNT_PAID"].Str()).ToString("n2") : Convert.ToDouble(data["AMOUNT_PAID"].Str()).ToString("n2");
            o.DocStamp = (data["DOC_STAMP"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDecimal(data["DOC_STAMP"].Str()).ToString("n2");
            o.TotalAmount = (data["TTL_AMNT"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDecimal(data["TTL_AMNT"].Str()).ToString("n2");

            o.EnableCommunityTax = (data["ENABLECTC"].Str() == "") ? false : Convert.ToBoolean(data["ENABLECTC"].Str());
            o.CTCNo = textInfo.ToUpper(textInfo.ToLower(data["CTC"].Str()));
            o.CTCIssuedAt = textInfo.ToUpper(textInfo.ToLower(data["CTCISSUEDAT"].Str()));
            o.CTCIssuedOn = (data["CTCISSUEDON"].Str() == "") ? "" : Convert.ToDateTime(data["CTCISSUEDON"].Str()).ToString("MMM dd, yyyy");

            o.VerifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["VERIFIEDBY"].Str()));
            o.CertifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["CERTIFIEDBY"].Str()));

            o.Release = (data["RELEASED"].Str() == "") ? false : Convert.ToBoolean(data["RELEASED"].Str());
            o.MosValidity = (data["MOS_VAL"].Str() == "") ? 0 : Convert.ToInt32(data["MOS_VAL"].Str());
            o.IssuedDate = (data["DATEPROCESS"].Str() == "") ? "" : Convert.ToDateTime(data["DATEPROCESS"].Str()).ToString("MMM dd, yyyy");
            o.DateRelease = (data["DATERELEASED"].Str() == "") ? "" : Convert.ToDateTime(data["DATERELEASED"].Str()).ToString("MMM dd, yyyy");
            o.ExpiryDate = (data["VALIDITYDATE"].Str() == "") ? "" : Convert.ToDateTime(data["VALIDITYDATE"].Str()).ToString("MMM dd, yyyy");
            o.ReleasedBy = textInfo.ToUpper(textInfo.ToLower(data["RELEASEDBY"].Str()));
            o.Cancelled = (data["CANCELLED"].Str() ==  "") ? false : Convert.ToBoolean(data["CANCELLED"].Str());
            o.CancelledBy = textInfo.ToUpper(textInfo.ToLower(data["CANCELLEDBY"].Str()));
            o.CancelledDate = (data["DATECANCELLED"].Str() == "") ? "" : Convert.ToDateTime(data["DATECANCELLED"].Str()).ToString("MMM dd, yyyy");
            o.StatusRequest = (data["STAT_REQ"].Str() == "") ? 0 : Convert.ToInt32(data["STAT_REQ"].Str());
            o.AppointmentDate = (data["APP_DATE"].Str() == "") ? "" : Convert.ToDateTime(data["APP_DATE"].Str()).ToString("MMM dd, yyyyy");
            return o;
        }

        public static IEnumerable<dynamic> GetSignatoryList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetSignatory_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetSignatory_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Signatory_List(e));
        }
        public static IDictionary<string, object> Get_Signatory_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.Name = data["NAME"].Str();
            o.SignatureURL = data["SIGNATURE"].Str();
            o.Position = textInfo.ToTitleCase(textInfo.ToLower(data["POSITION"].Str()));
            return o;
        }


        public static IDictionary<string, object> EventNofitication(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            o.NotifcationID = ((int)data["NOTIF_ID"].Str().ToDecimalDouble()).ToString("X");
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


        public static IEnumerable<dynamic> GetAllGovenmentValidIDList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllGovenmentValidID_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //    filter.Userid = userid;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetAllGovenmentValidID_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllGovenmentValidID_List(e));
        }
        public static IDictionary<string, object> Get_AllGovenmentValidID_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.ID = data["ID"].Str();
            o.GoventmentID_NM = data["GOVERNMENTID"].Str();
            o.UserID = data["USR_ID"].Str();
            o.SEQ_NO = data["SEQ_NO"].Str();
            o.GovValID = data["GOVVAL_ID"].Str();
            o.GovernmentID_NO = data["GOVVAL_ID_NO"].Str();
            o.Base64StringAttachment = data["Base64StringAttachment"].Str();
            o.Attachment = data["ATTACHMENT"].Str();
            o.NewUpload = data["NewUpload"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetAllGovenmentIDList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllGovenmentID_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //    filter.Userid = userid;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetAllGovenmentID_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllGovenmentID_List(e));
        }
        public static IDictionary<string, object> Get_AllGovenmentID_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.Num_Row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.ID = data["ID"].Str();
            o.GovernmentID_NM = data["GOVERNMENTID"].Str();
            return o;
        }
    }

}