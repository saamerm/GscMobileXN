using System;
using Android.Widget;
using Android.Graphics;
using MvvmCross.Platforms.Android.Binding.Target;

namespace MobileClaims.Droid.Binding
{
    class StrikedTextViewBinding : MvxTextViewTextTargetBinding
    {
        private readonly TextView _textView;
        private bool _currentValue;

        public StrikedTextViewBinding(TextView textView) : base(textView)
        {
            _textView = textView;
        }

        public override void SetValue(object value)
        {
            var boolValue = (bool)value;
            _currentValue = boolValue;
            SetStrikeOffText();
        }

        private void SetStrikeOffText()
        {
            if (_currentValue)
            {
                _textView.PaintFlags = PaintFlags.StrikeThruText;
            }
            else
            {

                _textView.PaintFlags = (_textView.PaintFlags & (~PaintFlags.StrikeThruText));
            }
        }
        public override Type TargetType
        {
            get { return typeof(bool); }
        }

        //public override MvxBindingMode DefaultMode
        //{
        //    get { return MvxBindingMode.OneWay; }
        //}
    }
}
