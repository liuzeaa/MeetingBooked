

using MeetingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingDLL
{



	     /// </summary>
	     ///	
	     /// </summary>
		 public partial class UserInfoDal:BaseDal<UserInfo>
         {


         }	

        public partial class DalFactory
        {
            public static UserInfoDal GetUserInfoDal()
            {
                return new UserInfoDal();
            }
	    }
}