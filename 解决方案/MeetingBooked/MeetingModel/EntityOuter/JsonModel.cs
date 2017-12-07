using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingModel
{
    public class JsonModel
    {
        public object retData { get; set; }
        public string errMsg { get; set; }
        public int errNum { get; set; }
        public string status { get; set; }

        public static JsonModel GetJsonModel(int errNum, string erMsg, object refdata)
        {
            JsonModel jsonModel = new JsonModel()
                {
                    errNum = errNum,
                    errMsg = erMsg,
                    retData = refdata
                };
            return jsonModel;
        }

        public static JsonModel GetJsonModel(int errNum, string erMsg)
        {
            JsonModel jsonModel = new JsonModel()
            {
                errNum = errNum,
                errMsg = erMsg,
                retData = ""
            };
            return jsonModel;
        }
    }


    public class JsonModelNum : JsonModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }
        public int RowCount { get; set; }



        new public static JsonModelNum GetJsonModel(int errNum, string erMsg, object refdata)
        {
            JsonModelNum jsonModel = new JsonModelNum()
            {
                errNum = errNum,
                errMsg = erMsg,
                retData = refdata
            };
            return jsonModel;
        }

        new public static JsonModelNum GetJsonModel(int errNum, string erMsg)
        {
            JsonModelNum jsonModel = new JsonModelNum()
            {
                errNum = errNum,
                errMsg = erMsg,
                retData = ""
            };
            return jsonModel;
        }
    }


    public class JsonModelNum_Total : JsonModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }
        public int RowCount { get; set; }

        decimal win_MoneyTotal = 0;
        public decimal Win_MoneyTotal
        {
            get { return win_MoneyTotal; }
            set { win_MoneyTotal = value; }
        }
      
        decimal get_MoneyTotal = 0;
        public decimal Get_MoneyTotal
        {
            get { return get_MoneyTotal; }
            set { get_MoneyTotal = value; }
        }

        decimal araw_ReturnMoneyTotal = 0;
        public decimal Araw_ReturnMoneyTotal
        {
            get { return araw_ReturnMoneyTotal; }
            set { araw_ReturnMoneyTotal = value; }
        }

        decimal using_MoneyTotal = 0;
        public decimal Using_MoneyTotal
        {
            get { return using_MoneyTotal; }
            set { using_MoneyTotal = value; }
        }

        decimal return_MoneyTotal = 0;
        public decimal Return_MoneyTotal
        {
            get { return return_MoneyTotal; }
            set { return_MoneyTotal = value; }
        }

        new public static JsonModelNum_Total GetJsonModel(int errNum, string erMsg, object refdata)
        {
            JsonModelNum_Total jsonModel = new JsonModelNum_Total()
            {
                errNum = errNum,
                errMsg = erMsg,
                retData = refdata
            };
            return jsonModel;
        }

        new public static JsonModelNum_Total GetJsonModel(int errNum, string erMsg)
        {
            JsonModelNum_Total jsonModel = new JsonModelNum_Total()
            {
                errNum = errNum,
                errMsg = erMsg,
                retData = ""
            };
            return jsonModel;
        }
    }



    public class JsonModelUser_Total : JsonModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }
        public int RowCount { get; set; }

        decimal personCount = 0;
        public decimal PersonCount
        {
            get { return personCount; }
            set { personCount = value; }
        }

        decimal allMoney = 0;
        public decimal AllMoney
        {
            get { return allMoney; }
            set { allMoney = value; }
        }

        decimal cashMoney = 0;
        public decimal CashMoney
        {
            get { return cashMoney; }
            set { cashMoney = value; }
        }

      
        new public static JsonModelUser_Total GetJsonModel(int errNum, string erMsg, object refdata)
        {
            JsonModelUser_Total jsonModel = new JsonModelUser_Total()
            {
                errNum = errNum,
                errMsg = erMsg,
                retData = refdata
            };
            return jsonModel;
        }

        new public static JsonModelUser_Total GetJsonModel(int errNum, string erMsg)
        {
            JsonModelUser_Total jsonModel = new JsonModelUser_Total()
            {
                errNum = errNum,
                errMsg = erMsg,
                retData = ""
            };
            return jsonModel;
        }
    }
}
