using Component;

using System;
using System.Collections.Generic;
using System.Text;

namespace ZFileComponent.Internal
{
    public class Notice
    {
        public static void Show(string message, string title)
        {
            CallNoticeWindow(message, title, null, MessageBoxIcon.None);
        }

        public static void Show(string message, string title, MessageBoxIcon noticeIcon)
        {
            CallNoticeWindow(message, title, null, noticeIcon);
        }

        public static void Show(string message, string title, double durationSeconds = 3, MessageBoxIcon noticeIcon = MessageBoxIcon.None)
        {
            CallNoticeWindow(message, title, durationSeconds, noticeIcon);
        }

        private static void CallNoticeWindow(string message, string title, double? durationSeconds, MessageBoxIcon noticeIcon)
        {
            if (NoticeWin.Instance == null)
            {
                var window = new NoticeWin();
                window.Show();
            }
            NoticeWin.Instance.AddNotice(message, title, durationSeconds, noticeIcon);
        }


    }
}
