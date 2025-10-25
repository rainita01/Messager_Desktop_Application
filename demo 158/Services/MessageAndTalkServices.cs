
using demo_158.MVVM.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;


namespace demo_158.Services
{
   public class MessageAndTalkServices
    {
   

        public HorizontalAlignment SetHorizontalAlignment(string username,string senderUsername)
        {
            if (username != senderUsername )
            {
                return HorizontalAlignment.Left;
            }

            return HorizontalAlignment.Right;
        }

         public FlowDirection SetFlowDirectionMessage(string username, string senderUsername)
        {

            if (username != senderUsername)
            {
                return  FlowDirection.LeftToRight;
            }
            return FlowDirection.RightToLeft;
        }

        public bool SetFirstMessage(string? username, string? senderUsername)
        {
            if (username == senderUsername)
            {
                return false;
            }

            return true;

        }

        public SolidColorBrush SetBackGroundBrush(string? username, string? senderUsername)
        {
            if (username  != senderUsername)
            {
                return Brushes.LightGray;
            }

            return Brushes.LightSkyBlue;
        }

    }
}
