using Leadshine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Card.Card;
using ZTAppFramework.Card.Model;

namespace ZTAppFramework.Card.Service
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/10/10 9:40:57 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class CardOperationService
    {

        #region Service
        private readonly CardServer _cardServer;
        Smc304Card _SmcCard;
        private CardInfo _CardInfo;

        public Action<string> ErrorMessageEvent;
        #endregion
        public CardOperationService(CardServer cardServer)
        {
            _cardServer = cardServer;
            if (IsConnect())
            {
                _SmcCard = _cardServer.GetCard();
                if (_SmcCard == null) return;
                _CardInfo = _SmcCard.cardInfo;
            }         
        }

        bool IsConnect()
        {
            if (_SmcCard == null) return false;
            return true;
        }

        public void InchingMove(AxisEnum Axis, int AddOrBackMove, SMCDefaultModel config)
        {
            try
            {
                if (!IsConnect()) throw new Exception("控制卡未链接");
                if (_CardInfo.RunStatus != RunStatus.Normal)
                    throw new Exception("空闲状态才能执行操纵");
                _SmcCard.InchingMove(Axis, AddOrBackMove, config);
            }
            catch (Exception ex)
            {
                ErrorMessageEvent?.Invoke(ex.Message);
            }   
        }

        public void StartCardMove(AxisEnum axis, int AddOrBackMove, SMCDefaultModel config)
        {
            try
            {
                if (!IsConnect()) throw new Exception("控制卡未链接");
                _SmcCard.StartCardMove(axis, AddOrBackMove, config);
            }
            catch (Exception ex)
            {
                ErrorMessageEvent?.Invoke(ex.Message);
            }
          
        }

        public void StopMove(AxisEnum axis, int StopType = 0)
        {
            try
            {
                if (!IsConnect()) throw new Exception("控制卡未链接");
                _SmcCard.StopMove(axis, StopType);
            }
            catch (Exception ex)
            {
                ErrorMessageEvent?.Invoke(ex.Message);
            }
           
        }



    }
}
