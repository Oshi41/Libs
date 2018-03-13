using System.Drawing;
using System.Windows.Forms;

namespace XamlWindowService
{
    public class BubblePopupProvider
    {
        public static void ShowBubbleSuccess(string text, 
            string title = "Success",
            int duration = 2000)
        {
            var bubble = new NotifyIcon
            {
                Visible = true,
                BalloonTipText = text,
                BalloonTipTitle = title,
                BalloonTipIcon = ToolTipIcon.Info,
                Icon = SystemIcons.Information,
                Text = title
            };

            bubble.ShowBalloonTip(duration);
        }

        public static void ShowError(string text, 
            string title = "Error",
            int duration = 2000)
        {
            var bubble = new NotifyIcon
            {
                Visible = true,
                BalloonTipText = text,
                BalloonTipTitle = title,
                BalloonTipIcon = ToolTipIcon.Error,
                Icon = SystemIcons.Warning,
                Text = title
            };

            bubble.ShowBalloonTip(duration);
        }
    }
}
