
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingDLL;
using MeetingModel;
using MeetingBLL;



namespace MeetingBLL
{

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
	
}