using System.Windows.Controls;
using System.Windows;

namespace LabAllianceTest.Helpers
{
    public class UIHelper
    {
        public static void TextBox_Style_0(TextBox textBox, string defaultText) // Для очистки textbox при дефолтном значение
        {
            if (textBox.Text == defaultText)
            {
                textBox.Clear();
            }
        }

        public static void TextBox_Style_1(TextBox textBox, string defaultText) // Установка дефолтноготекста при пустом textbox
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = defaultText;
            }
        }

        public static void Label_Style_1(Label label, TextBox textBox, string defaultText) // Видимость метки при знеачении textbox
        {
            label.Visibility = (textBox.Text != defaultText) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
