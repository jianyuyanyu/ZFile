using Leadshine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ZTAppFramework.Card.Model;
using ZTAppFramework.Card.Utils;

namespace ZTAppFramework.Card.Card
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  zt
    /// 创建时间    ：  2022/10/10 8:42:11 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class Smc304Card
    {

        #region 默认配置文件路劲
        public static string LogsFolder => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderDirName);

        public const string FolderDirName = "Config";
        public static string ConfigPath
        {
            get
            {
                string fileName = string.Format("WH-Axis.xml", DateTime.Now);
                return Path.Combine(LogsFolder, fileName);
            }
        }

        public static string DefaultCardConfig
        {
            get
            {
                string fileName = string.Format("WH-Default.xml", DateTime.Now);
                return Path.Combine(LogsFolder, fileName);
            }
        }


        #endregion
        #region 属性
        AxisParm axisParm;
        SMCDefaultModel Config;//默认配置参数
        public CardInfo cardInfo;

        ushort _ConnectNo = 0;//控制卡号
        #endregion

        #region 状态
        bool WorkState = false;
        #endregion

        public Smc304Card(ushort ConnectNo =0)
        {
            _ConnectNo = ConnectNo;
            cardInfo = new CardInfo();
            Task.Run(UpdateInfo);
        }

        ~Smc304Card()
        {
            WorkState = false;
        }
        #region 更新信息

        /// <summary>
        /// 读取轴当前速度
        /// </summary>
        double[] GetAxisPostionsSpeed()
        {
            double[] PostionsSpeed = new double[6];
            for (int i = 0; i < 6; i++)
                LTSMC.smc_read_current_speed_unit(_ConnectNo, (ushort)i, ref PostionsSpeed[i]);
            return PostionsSpeed;
        }
        /// <summary>
        /// 读取轴的坐标系
        /// </summary>
        /// <returns></returns>
        double[] GetAxisPostions()
        {
            double[] AxisPostions = new double[6];
            for (int i = 0; i < 6; i++)
                LTSMC.smc_get_position_unit(_ConnectNo, (ushort)i, ref AxisPostions[i]);
            return AxisPostions;
        }
        /// <summary>
        /// 查询轴的状态
        /// </summary>
        /// <returns></returns>
        int[] GetaxisPosStatus()
        {
            int[] AxisPosStatus = new int[6];
            for (int i = 0; i < 6; i++)
                AxisPosStatus[i] = LTSMC.smc_check_done(_ConnectNo, (ushort)i);
            return AxisPosStatus;
        }
        #endregion
        void UpdateInfo()
        {
            WorkState = true;
            while (WorkState)
            {
                cardInfo.m_CurPos = GetAxisPostions();
                cardInfo.m_CurPosStatus = GetaxisPosStatus();
                cardInfo.PostionsSpeed = GetAxisPostionsSpeed();
            }
        }
        public void ConnectCard()
        {
            LTSMC.smc_board_close(0);
            var st = LTSMC.smc_board_init(0, 2, "192.168.5.11", 0);
            if (st != 0)
                cardInfo.CardConnectStatus = false;
            else
                cardInfo.CardConnectStatus = true;
        }
        #region 查询
        public AxisParm GetAxisParamInfo() => DeepCopy<AxisParm>(axisParm);
        public SMCDefaultModel GetSMCDefaultParamInfo() => DeepCopy<SMCDefaultModel>(Config);
        #endregion


        #region 操作
        //点动
        public void InchingMove(AxisEnum Axis, int AddOrBackMove, SMCDefaultModel config)
        {
            if (config == null) config = this.Config;
            if (AddOrBackMove == 0)
                config.DisplacementThreshold = -config.DisplacementThreshold;
            else
                config.DisplacementThreshold = config.DisplacementThreshold;

            int st = LTSMC.smc_set_profile_unit(_ConnectNo, (ushort)Axis, config.StartSpeed, config.RunSpeed, config.AccelTime, config.DecelerationTime, config.StopSpeed);//设置起始速度、运行速度、停止速度、加速时间、减速时间
            st = LTSMC.smc_set_dec_stop_time(_ConnectNo, (ushort)Axis, config.DecelerationTime);
            st = LTSMC.smc_pmove_unit(_ConnectNo, (ushort)Axis, config.DisplacementThreshold, 0);//定长运动
        }
        #endregion


        #region 配置导入
        public bool LoadAxisParamInfo()
        {
            try
            {
                axisParm = XMLHelper.SerializerXMLToObject<AxisParm>(ConfigPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool LoadCardConfigParamInfo()
        {
            try
            {
                Config = XMLHelper.SerializerXMLToObject<SMCDefaultModel>(DefaultCardConfig);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 深拷贝
        public T DeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        #endregion


    }
}
