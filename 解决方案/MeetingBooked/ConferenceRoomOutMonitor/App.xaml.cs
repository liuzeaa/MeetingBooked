/*
* ----------------------------------------------------------------             
Copyright (C#) 2015 北京圣邦天麒科技有限公司 版权所有      
文件名： 应用程序入口
文件功能描述： 会议室跟踪平台
创建标识： 2015-8-26
修改标识： 
修改描述：     
* ----------------------------------------------------------------
*/

using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace ConferenceRoomOutMonitor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
