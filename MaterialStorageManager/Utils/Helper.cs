using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace MaterialStorageManager.Utils
{
    public class Helper
    {
        static BlurEffect myBlur = new BlurEffect() { Radius = 0 };
        public static void ShowBlurEffectAllWindow()
        {
            myBlur.Radius = 10;
            foreach (Window item in Application.Current.Windows)
                item.Effect = myBlur;
        }

        public static void StopBlurEffectAllWindow()
        {
            myBlur.Radius = 0;
            foreach (Window item in Application.Current.Windows)
                item.Effect = myBlur;
        }

    }

    public static class ExtensionMethods
    {
        public static T ToEnum<T>(this string value)
        {
            if (!System.Enum.IsDefined(typeof(T), value)) return default(T);
            return (T)System.Enum.Parse(typeof(T), value, true);
        }
    }

    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }
}
