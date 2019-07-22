using System;
using Eto.Drawing;
using s = SharpDX;
using sd = SharpDX.Direct2D1;
using sw = SharpDX.DirectWrite;

namespace Eto.Direct2D.Drawing
{
	public class FormattedTextHandler : WidgetHandler<sw.TextFormat, FormattedText, FormattedText.ICallback>, FormattedText.IHandler
	{
		FormattedTextTrimming _trimming;
		FormattedTextAlignment _alignment;
		FormattedTextWrapMode _wrap;
		sw.TextLayout _layout;
		SizeF _maximumSize = SizeF.MaxValue;
		string _text;
		protected override sw.TextFormat CreateControl() => new sw.TextFormat(SDFactory.DirectWriteFactory, SystemFonts.Default().FamilyName, SystemFonts.Default().Size);

		public FormattedTextWrapMode Wrap
		{
			get => _wrap;
			set
			{
				_wrap = value;
			}
		}
		public FormattedTextTrimming Trimming
		{
			get => _trimming;
			set
			{
				_trimming = value;
				switch (value)
				{
					case FormattedTextTrimming.None:
						Control.SetTrimming(new sw.Trimming(), null);
						break;
					case FormattedTextTrimming.CharacterEllipsis:
						Control.SetTrimming(new sw.Trimming(), new sw.EllipsisTrimming(SDFactory.DirectWriteFactory, Control));
						break;
					case FormattedTextTrimming.WordEllipsis:
						Control.SetTrimming(new sw.Trimming(), new sw.EllipsisTrimming(SDFactory.DirectWriteFactory, Control));
						break;
					default:
						break;
				}
			}
		}
		public FormattedTextAlignment Alignment
		{
			get => _alignment;
			set
			{
				_alignment = value;
				switch (value)
				{
					case FormattedTextAlignment.Left:
						Control.TextAlignment = sw.TextAlignment.Leading;
						break;
					case FormattedTextAlignment.Right:
						Control.TextAlignment = sw.TextAlignment.Trailing;
						break;
					case FormattedTextAlignment.Center:
						Control.TextAlignment = sw.TextAlignment.Center;
						break;
					case FormattedTextAlignment.Justify:
						Control.TextAlignment = sw.TextAlignment.Justified;
						break;
					default:
						break;
				}
			}
		}
		public Font Font { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				var max = MaximumSize;
				_layout = new sw.TextLayout(SDFactory.DirectWriteFactory, value, Control, max.Width, max.Height);
			}
		}
		public SizeF MaximumSize
		{
			get => _maximumSize;
			set
			{
				_maximumSize = value;
				if (_layout != null)
				{
					_layout.MaxWidth = value.Width;
					_layout.MaxHeight = value.Height;
				}
		}
		public Brush ForegroundBrush { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public SizeF Measure()
		{
			if (_layout == null)
				return SizeF.Empty;

			var metrics = _layout.Metrics;
			return new SizeF(metrics.WidthIncludingTrailingWhitespace, metrics.Height);
		}
	}
}
