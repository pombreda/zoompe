using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace PEViewer
{
    public class HexEditor : NumberEditor
    {
        protected override void UpdateTextFromNumber()
        {
            if (this.Number == null)
            {
                this.Text = null;
                return;
            }

            ulong extendedNumber = (ulong)Convert.ChangeType(this.Number, typeof(ulong), CultureInfo.CurrentCulture);

            string newText = extendedNumber.ToString("X") + "h";

            this.Text = newText;
        }

        protected override void UpdateNumberFromText()
        {
            string text = this.Text;
            if (text == null)
            {
                this.Number = null;
                return;
            }

            text = text.Trim();

            if (text.EndsWith("H", StringComparison.OrdinalIgnoreCase))
                text = text.Substring(0, text.Length-1);

            ulong extendedNumber = ulong.Parse(text, NumberStyles.HexNumber);

            this.Number = Convert.ChangeType(extendedNumber, this.Number.GetType(), CultureInfo.CurrentCulture);
        }
    }
}