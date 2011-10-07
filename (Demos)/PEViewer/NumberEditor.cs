using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Controls;

namespace PEViewer
{
    public class NumberEditor : Control
    {
        bool skipCoercion;

        public NumberEditor()
        {
            this.DefaultStyleKey = typeof(NumberEditor);
        }

        public object Number { get { return (object)GetValue(NumberProperty); } set { SetValue(NumberProperty, value); } }
        #region NumberProperty = DependencyProperty.Register(...)
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
            "Number",
            typeof(object),
            typeof(NumberEditor),
            new PropertyMetadata((sender, e) => ((NumberEditor)sender).OnNumberChanged((object)e.OldValue)));
        #endregion

        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }
        #region TextProperty = DependencyProperty.Register(...)
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(NumberEditor),
            new PropertyMetadata((sender, e) => ((NumberEditor)sender).OnTextChanged((string)e.OldValue)));
        #endregion

        public TextAlignment TextAlignment { get { return (TextAlignment)GetValue(TextAlignmentProperty); } set { SetValue(TextAlignmentProperty, value); } }
        #region TextAlignmentProperty = DependencyProperty.Register(...)
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            "TextAlignment",
            typeof(TextAlignment),
            typeof(NumberEditor),
            new PropertyMetadata((sender, e) => { }));
        #endregion

        private void OnNumberChanged(object oldNumber)
        {
            if (skipCoercion)
                return;

            skipCoercion = true;
            try
            {
                UpdateTextFromNumber();
            }
            finally
            {
                skipCoercion = false;
            }
        }

        private void OnTextChanged(string oldText)
        {
            if (skipCoercion)
                return;

            skipCoercion = true;
            try
            {
                UpdateNumberFromText();

                this.Dispatcher.BeginInvoke(delegate
                {
                    skipCoercion = true;
                    try
                    {
                        UpdateTextFromNumber();
                    }
                    finally
                    {
                        skipCoercion = false;
                    }
                });
            }
            catch
            {
                string newText = this.Text;
                this.Dispatcher.BeginInvoke(delegate
                {
                    if (this.Text != newText)
                        return;

                    skipCoercion = true;
                    try
                    {
                        this.Text = oldText;
                    }
                    finally
                    {
                        skipCoercion = false;
                    }
                });
            }
            finally
            {
                skipCoercion = false;
            }
        }

        protected virtual void UpdateTextFromNumber()
        {
            string newText = this.Number == null ? null : this.Number.ToString();
            this.Text = newText;
        }

        protected virtual void UpdateNumberFromText()
        {
            this.Number = Convert.ChangeType(this.Text, this.Number.GetType(), CultureInfo.CurrentCulture);
        }
    }
}