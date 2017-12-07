
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLC_DAL;
using TLC_Model;
using TLC_BLL;



namespace TLC_BLL
{

	/// </summary>
	///	
	/// </summary>
    public partial class Size_SpecialNormalService:BaseService<Size_SpecialNormal>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetSize_SpecialNormalDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class Size_SumService:BaseService<Size_Sum>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetSize_SumDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class Animal_InfoService:BaseService<Animal_Info>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetAnimal_InfoDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class UserInfoService:BaseService<UserInfo>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetUserInfoDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class Award_PublicService:BaseService<Award_Public>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetAward_PublicDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class WaveService:BaseService<Wave>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetWaveDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class DetailService:BaseService<Detail>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetDetailDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class GlobeService:BaseService<Globe>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetGlobeDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class Globe_ClueService:BaseService<Globe_Clue>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetGlobe_ClueDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class MoneyLogService:BaseService<MoneyLog>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetMoneyLogDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class Operation_RecordService:BaseService<Operation_Record>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetOperation_RecordDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class Size_SixService:BaseService<Size_Six>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetSize_SixDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class Size_SpecialService:BaseService<Size_Special>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetSize_SpecialDal();
        }
		
		 
    }
	
}