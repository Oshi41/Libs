using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfControls
{
    public class Helper
    {
        /// <summary>
        /// Возвращает char из нажатой клавишы
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            KeyDecoder.GetKeyboardState(keyboardState);

            uint scanCode = KeyDecoder.MapVirtualKey((uint)virtualKey, KeyDecoder.MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = KeyDecoder.ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                {
                    ch = stringBuilder[0];
                    break;
                }
                default:
                {
                    ch = stringBuilder[0];
                    break;
                }
            }
            return ch;
        }

        /// <summary>
        /// Возвращает первого родителя заданного типа в дереве контролов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T GetParent<T>(DependencyObject element)
            where T : DependencyObject
        {
            DependencyObject parent = element;

            while (parent != null)
            {
                if (parent is T find)
                {
                    return find;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        #region Nested WinAPI class

        private class KeyDecoder
        {
            public enum MapType : uint
            {
                MAPVK_VK_TO_VSC = 0x0,
                MAPVK_VSC_TO_VK = 0x1,
                MAPVK_VK_TO_CHAR = 0x2,
                MAPVK_VSC_TO_VK_EX = 0x3,
            }

            [DllImport("user32.dll")]
            public static extern int ToUnicode(
                uint wVirtKey,
                uint wScanCode,
                byte[] lpKeyState,
                [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
                StringBuilder pwszBuff,
                int cchBuff,
                uint wFlags);

            [DllImport("user32.dll")]
            public static extern bool GetKeyboardState(byte[] lpKeyState);

            [DllImport("user32.dll")]
            public static extern uint MapVirtualKey(uint uCode, MapType uMapType);
        }

        #endregion
    }
}
