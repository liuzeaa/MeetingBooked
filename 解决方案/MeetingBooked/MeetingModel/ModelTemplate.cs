  
using System;
namespace MeetingModel
{
    

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Size_SpecialNormal:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Globe { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Size_SpecialNormal))
            {
                Size_SpecialNormal _obj = obj as Size_SpecialNormal;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Size_Sum:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Globe { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Size_Sum))
            {
                Size_Sum _obj = obj as Size_Sum;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Animal_Info:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Globe { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Operation_Year { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Animal_Info))
            {
                Animal_Info _obj = obj as Animal_Info;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class UserInfo:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string LoginName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Salt { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? Money { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Role { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EnableTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Remarks { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(UserInfo))
            {
                UserInfo _obj = obj as UserInfo;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Award_Public:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Year { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string PrizeContent { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? StartTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CloseTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EndTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsCompleate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Award_Public))
            {
                Award_Public _obj = obj as Award_Public;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Wave:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Globe { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Wave))
            {
                Wave _obj = obj as Wave;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Detail:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Globe { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Detail))
            {
                Detail _obj = obj as Detail;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Globe:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Color { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Globe))
            {
                Globe _obj = obj as Globe;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class MoneyLog:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string O_Content { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? O_Money { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string UserID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string UserName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? Type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? OperationType { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(MoneyLog))
            {
                MoneyLog _obj = obj as MoneyLog;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Operation_Record:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? AwardCode { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? ClueCode { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string UserID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Buy_Content { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Buy_Content2 { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? UnitPrice { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string BuyPayReturn { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string BuyPayReturn2 { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? MinPayReturn { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? MaxPayReturn { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? Using_Money { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? Get_Money { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? Araw_ReturnMoney { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Araw_RetrunContent { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? Return_Money { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? IsWin { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Operation_Time { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string PayUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Operation_Record))
            {
                Operation_Record _obj = obj as Operation_Record;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Size_Six:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Globe { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Award_Globe { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Size_Six))
            {
                Size_Six _obj = obj as Size_Six;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Globe_Clue:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Clue { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Pay { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Clue2 { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Pay2 { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Limit_Max { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Limit_Min { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? First_Type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Second_Type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string First_Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Second_Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? Return_Pay { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? Return_Pay2 { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Sort { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Globe_Clue))
            {
                Globe_Clue _obj = obj as Globe_Clue;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Size_Special:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Globe { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }

          public override bool Equals(object obj)
        {
            bool result = false;
            if (obj.GetType() == typeof(Size_Special))
            {
                Size_Special _obj = obj as Size_Special;
                if (_obj.Id == this.Id)
                {
                    result = true;
                }
            }
            return result;
        }

	    public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}