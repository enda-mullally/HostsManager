//
// Copyright © 2021-2023 Enda Mullally.
//
// Based on: legacy SplitButton control found at: http://wyday.com/splitbutton (circa 2007)
//

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

// ReSharper disable UselessBinaryOperation
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault
// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace EM.HostsManager.App.UI
{
    /// <summary>
    /// SplitButton control. 
    /// </summary>
    public class SplitButton : Button
    {
        private PushButtonState _state;

        private const int SplitSectionWidth = 18;

        private static readonly int BorderSize = SystemInformation.Border3DSize.Width * 2;
        private bool _skipNextOpen;
        private Rectangle _dropDownRectangle;
        private bool _showSplit;

        private bool _isSplitMenuVisible;

        private ContextMenuStrip _mSplitMenuStrip = null!;
        private ContextMenuStrip _mSplitMenu = null!;

        private TextFormatFlags _textFormatFlags = TextFormatFlags.Default |
                                                  TextFormatFlags.WordBreak |
                                                  TextFormatFlags.HorizontalCenter; // allow newlines and middle align

        #region Properties

        [Browsable(false)]
        public override ContextMenuStrip ContextMenuStrip
        {
            get => SplitMenuStrip;
            set => SplitMenuStrip = value;
        }

        [DefaultValue(null)]
        public ContextMenuStrip SplitMenu
        {
            get => _mSplitMenu;
            set
            {
                // remove the event handlers for the old SplitMenu
                if (_mSplitMenu != null)
                {
                    _mSplitMenu.Opened -= SplitMenu_Popup!;
                }

                // add the event handlers for the new SplitMenu
                if (value != null)
                {
                    ShowSplit = true;

                    value.Opened += SplitMenu_Popup!;
                }
                else
                {
                    ShowSplit = false;
                }

                _mSplitMenu = value!;
            }
        }

        [DefaultValue(null)]
        public ContextMenuStrip SplitMenuStrip
        {
            get => _mSplitMenuStrip;
            set
            {
                // remove the event handlers for the old SplitMenuStrip
                if (_mSplitMenuStrip != null)
                {
                    _mSplitMenuStrip.Closing -= SplitMenuStrip_Closing!;
                    _mSplitMenuStrip.Opening -= SplitMenuStrip_Opening!;
                }

                //add the event handlers for the new SplitMenuStrip
                if (value != null)
                {
                    ShowSplit = true;
                    value.Closing += SplitMenuStrip_Closing!;
                    value.Opening += SplitMenuStrip_Opening!;
                }
                else
                {
                    ShowSplit = false;
                }

                _mSplitMenuStrip = value!;
            }
        }

        [DefaultValue(false)]
        public bool ShowSplit
        {
            set
            {
                if (value == _showSplit)
                {
                    return;
                }

                _showSplit = value;
                Invalidate();

                Parent?.PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        private PushButtonState State
        {
            get => _state;
            set
            {
                if (_state.Equals(value))
                {
                    return;
                }

                _state = value;
                Invalidate();
            }
        }

        #endregion Properties

        /// <summary>
        /// Determines whether the specified key is a regular input key or a special key that requires preprocessing.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"/> values.</param>
        /// <returns>
        /// true if the specified key is a regular input key; otherwise, false.
        /// </returns>
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData.Equals(Keys.Down) && _showSplit)
            {
                return true;
            }

            return base.IsInputKey(keyData);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            if (!_showSplit)
            {
                base.OnGotFocus(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Default;
            }
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnKeyUp(System.Windows.Forms.KeyEventArgs)"/> event.
        /// </summary>
        /// <param name="kevent">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (_showSplit)
            {
                switch (kevent.KeyCode)
                {
                    case Keys.Down when !_isSplitMenuVisible:
                        ShowContextMenuStrip();
                        break;
                    case Keys.Space when kevent.Modifiers == Keys.None:
                        State = PushButtonState.Pressed;
                        break;
                }
            }

            base.OnKeyDown(kevent);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnKeyUp(System.Windows.Forms.KeyEventArgs)"/> event.
        /// </summary>
        /// <param name="kevent">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            switch (kevent.KeyCode)
            {
                case Keys.Space:
                {
                    if (MouseButtons == MouseButtons.None)
                    {
                        State = PushButtonState.Normal;
                    }

                    break;
                }
                case Keys.Apps:
                {
                    if (MouseButtons == MouseButtons.None && !_isSplitMenuVisible)
                    {
                        ShowContextMenuStrip();
                    }

                    break;
                }
            }

            base.OnKeyUp(kevent);
        }

        /// <summary>
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            State = Enabled ? PushButtonState.Normal : PushButtonState.Disabled;

            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnLostFocus(System.EventArgs)"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            if (!_showSplit)
            {
                base.OnLostFocus(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Normal;
            }
        }

        private bool _isMouseEntered;

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            if (!_showSplit)
            {
                base.OnMouseEnter(e);

                return;
            }

            _isMouseEntered = true;

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Hot;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (!_showSplit)
            {
                base.OnMouseLeave(e);

                return;
            }

            _isMouseEntered = false;

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = Focused ? PushButtonState.Default : PushButtonState.Normal;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!_showSplit)
            {
                base.OnMouseDown(e);

                return;
            }

            // handle ContextMenu re-clicking the drop-down region to close the menu
            if (_mSplitMenu != null && e.Button == MouseButtons.Left && !_isMouseEntered)
            {
                _skipNextOpen = true;
            }

            if (_dropDownRectangle.Contains(e.Location) && !_isSplitMenuVisible && e.Button == MouseButtons.Left)
            {
                ShowContextMenuStrip();
            }
            else
            {
                State = PushButtonState.Pressed;
            }
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnMouseUp(System.Windows.Forms.MouseEventArgs)"/> event.
        /// </summary>
        /// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (!_showSplit)
            {
                base.OnMouseUp(mevent);

                return;
            }

            // if the right button was released inside the button
            if (mevent.Button == MouseButtons.Right && ClientRectangle.Contains(mevent.Location) && !_isSplitMenuVisible)
            {
                ShowContextMenuStrip();
            }
            else if (_mSplitMenuStrip == null && _mSplitMenu == null || !_isSplitMenuVisible)
            {
                SetButtonDrawState();

                if (ClientRectangle.Contains(mevent.Location) && !_dropDownRectangle.Contains(mevent.Location))
                {
                    OnClick(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)"/> event.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (!_showSplit)
            {
                return;
            }

            var g = pevent.Graphics;
            var bounds = ClientRectangle;

            // draw the button background as according to the current state.
            if (State != PushButtonState.Pressed && IsDefault && !Application.RenderWithVisualStyles)
            {
                var backgroundBounds = bounds;
                backgroundBounds.Inflate(-1, -1);
                ButtonRenderer.DrawButton(g, backgroundBounds, State);

                // button renderer doesn't draw the black frame when themes are off
                g.DrawRectangle(SystemPens.WindowFrame, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }
            else
            {
                ButtonRenderer.DrawButton(g, bounds, State);
            }

            // calculate the current dropdown rectangle.
            _dropDownRectangle = new Rectangle(bounds.Right - SplitSectionWidth, 0, SplitSectionWidth, bounds.Height);

            var internalBorder = BorderSize;
            var focusRect =
                new Rectangle(internalBorder - 1,
                              internalBorder - 1,
                              bounds.Width - _dropDownRectangle.Width - internalBorder,
                              bounds.Height - (internalBorder * 2) + 2);

            //bool drawSplitLine = (State == PushButtonState.Hot || State == PushButtonState.Pressed || !Application.RenderWithVisualStyles);
            var drawSplitLine = true; // Fix

            if (RightToLeft == RightToLeft.Yes)
            {
                _dropDownRectangle.X = bounds.Left + 1;
                focusRect.X = _dropDownRectangle.Right;

                if (drawSplitLine)
                {
                    // draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Left + SplitSectionWidth, BorderSize, bounds.Left + SplitSectionWidth, bounds.Bottom - BorderSize);
                    g.DrawLine(SystemPens.ButtonFace, bounds.Left + SplitSectionWidth + 1, BorderSize, bounds.Left + SplitSectionWidth + 1, bounds.Bottom - BorderSize);
                }
            }
            else
            {
                if (drawSplitLine)
                {
                    // draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Right - SplitSectionWidth, BorderSize, bounds.Right - SplitSectionWidth, bounds.Bottom - BorderSize);
                    g.DrawLine(SystemPens.ButtonFace, bounds.Right - SplitSectionWidth - 1, BorderSize, bounds.Right - SplitSectionWidth - 1, bounds.Bottom - BorderSize);
                }
            }

            // Draw an arrow in the correct location
            PaintArrow(g, _dropDownRectangle);

            //paint the image and text in the "button" part of the splitButton
            PaintTextAndImage(g, new Rectangle(0, 0, ClientRectangle.Width - SplitSectionWidth, ClientRectangle.Height));

            // draw the focus rectangle.
            if (State != PushButtonState.Pressed && Focused && ShowFocusCues)
            {
                ControlPaint.DrawFocusRectangle(g, focusRect);
            }
        }

        /// <summary>
        /// Paints the text and image.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="bounds">The bounds.</param>
        private void PaintTextAndImage(Graphics g, Rectangle bounds)
        {
            // Figure out where our text and image should go
            CalculateButtonTextAndImageLayout(ref bounds, out var textRectangle, out var imageRectangle);

            // Draw the image
            if (Image != null)
            {
                if (Enabled)
                {
                    g.DrawImage(Image, imageRectangle.X, imageRectangle.Y, Image.Width, Image.Height);
                }
                else
                {
                    ControlPaint.DrawImageDisabled(g, Image, imageRectangle.X, imageRectangle.Y, BackColor);
                }
            }

            // If we don't use mnemonic, set formatFlag to NoPrefix as this will show ampersand.
            if (!UseMnemonic)
            {
                _textFormatFlags |= TextFormatFlags.NoPrefix;
            }
            else
            if (!ShowKeyboardCues)
            {
                _textFormatFlags |= TextFormatFlags.HidePrefix;
            }
            else
            {
                _textFormatFlags &= ~TextFormatFlags.HidePrefix; // Fix
            }

            // Draw the text
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            if (Enabled)
            {
                TextRenderer.DrawText(g, Text, Font, textRectangle, SystemColors.ControlText, _textFormatFlags);
            }
            else
            {
                ControlPaint.DrawStringDisabled(g, Text, Font, BackColor, textRectangle, _textFormatFlags);
            }
        }

        /// <summary>
        /// Paints the arrow.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="dropDownRect">The drop down rect.</param>
        private void PaintArrow(Graphics g, Rectangle dropDownRect)
        {
            var middle = new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2),
                Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));

            // If the width is odd - favor pushing it over one pixel right.
            middle.X += (dropDownRect.Width % 2);

            var arrow = new[]
            {
                new Point(middle.X - 2, middle.Y - 1), new Point(middle.X + 3, middle.Y - 1),
                new Point(middle.X, middle.Y + 2)
            };

            switch (Enabled)
            {
                case true:
                    g.FillPolygon(SystemBrushes.ControlText, arrow);
                    break;
                default:
                    g.FillPolygon(SystemBrushes.ButtonShadow, arrow);
                    break;
            }
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>
        /// An ordered pair of type <see cref="T:System.Drawing.Size"/> representing the width and height of a rectangle.
        /// </returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            var preferredSize = base.GetPreferredSize(proposedSize);

            if (!_showSplit)
            {
                return preferredSize;
            }

            if (!string.IsNullOrEmpty(Text) && TextRenderer.MeasureText(Text, Font).Width + SplitSectionWidth > preferredSize.Width)
            {
                return preferredSize + new Size(SplitSectionWidth + BorderSize * 2, 0);
            }

            return preferredSize;
        }
        
        #region Button Layout Calculations

        // The following layout functions were taken from Mono's Windows.Forms 
        // implementation, specifically "ThemeWin32Classic.cs", 
        // then modified to fit the context of this splitButton

        /// <summary>
        /// Calculates the button text and image layout.
        /// </summary>
        /// <param name="contentRect">The content_rect.</param>
        /// <param name="textRectangle">The text rectangle.</param>
        /// <param name="imageRectangle">The image rectangle.</param>
        private void CalculateButtonTextAndImageLayout(ref Rectangle contentRect, out Rectangle textRectangle, out Rectangle imageRectangle)
        {
            var textSize = TextRenderer.MeasureText(Text, Font, contentRect.Size, _textFormatFlags);
            var imageSize = Image?.Size ?? Size.Empty;

            textRectangle = Rectangle.Empty;
            imageRectangle = Rectangle.Empty;

            switch (TextImageRelation)
            {
                case TextImageRelation.Overlay:
                    // Overlay is easy, text always goes here
                    textRectangle = OverlayObjectRect(ref contentRect, ref textSize, TextAlign); // Rectangle.Inflate(content_rect, -4, -4);

                    //Offset on Windows 98 style when button is pressed
                    if (_state == PushButtonState.Pressed && !Application.RenderWithVisualStyles)
                    {
                        textRectangle.Offset(1, 1);
                    }

                    // Image is dependent on ImageAlign
                    if (Image != null)
                    {
                        imageRectangle = OverlayObjectRect(ref contentRect, ref imageSize, ImageAlign);
                    }

                    break;
                case TextImageRelation.ImageAboveText:
                    contentRect.Inflate(-4, -4);
                    LayoutTextAboveOrBelowImage(contentRect, false, textSize, imageSize, out textRectangle, out imageRectangle);
                    break;
                case TextImageRelation.TextAboveImage:
                    contentRect.Inflate(-4, -4);
                    LayoutTextAboveOrBelowImage(contentRect, true, textSize, imageSize, out textRectangle, out imageRectangle);
                    break;
                case TextImageRelation.ImageBeforeText:
                    contentRect.Inflate(-4, -4);
                    LayoutTextBeforeOrAfterImage(contentRect, false, textSize, imageSize, out textRectangle, out imageRectangle);
                    break;
                case TextImageRelation.TextBeforeImage:
                    contentRect.Inflate(-4, -4);
                    LayoutTextBeforeOrAfterImage(contentRect, true, textSize, imageSize, out textRectangle, out imageRectangle);
                    break;
            }
        }

        /// <summary>
        /// Overlays the object rect.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="sizeOfObject">The size of object.</param>
        /// <param name="alignment">The alignment.</param>
        /// <returns></returns>
        private static Rectangle OverlayObjectRect(ref Rectangle container, ref Size sizeOfObject, System.Drawing.ContentAlignment alignment)
        {
            int x, y;

            switch (alignment)
            {
                case System.Drawing.ContentAlignment.TopLeft:
                    x = 4;
                    y = 4;
                    break;
                case System.Drawing.ContentAlignment.TopCenter:
                    x = (container.Width - sizeOfObject.Width) / 2;
                    y = 4;
                    break;
                case System.Drawing.ContentAlignment.TopRight:
                    x = container.Width - sizeOfObject.Width - 4;
                    y = 4;
                    break;
                case System.Drawing.ContentAlignment.MiddleLeft:
                    x = 4;
                    y = (container.Height - sizeOfObject.Height) / 2;
                    break;
                case System.Drawing.ContentAlignment.MiddleCenter:
                    x = (container.Width - sizeOfObject.Width) / 2;
                    y = (container.Height - sizeOfObject.Height) / 2;
                    break;
                case System.Drawing.ContentAlignment.MiddleRight:
                    x = container.Width - sizeOfObject.Width - 4;
                    y = (container.Height - sizeOfObject.Height) / 2;
                    break;
                case System.Drawing.ContentAlignment.BottomLeft:
                    x = 4;
                    y = container.Height - sizeOfObject.Height - 4;
                    break;
                case System.Drawing.ContentAlignment.BottomCenter:
                    x = (container.Width - sizeOfObject.Width) / 2;
                    y = container.Height - sizeOfObject.Height - 4;
                    break;
                case System.Drawing.ContentAlignment.BottomRight:
                    x = container.Width - sizeOfObject.Width - 4;
                    y = container.Height - sizeOfObject.Height - 4;
                    break;
                default:
                    x = 4;
                    y = 4;
                    break;
            }

            return new Rectangle(x, y, sizeOfObject.Width, sizeOfObject.Height);
        }

        /// <summary>
        /// Layouts the text before or after image.
        /// </summary>
        /// <param name="totalArea">The total area.</param>
        /// <param name="textFirst">if set to <c>true</c> [text first].</param>
        /// <param name="textSize">Size of the text.</param>
        /// <param name="imageSize">Size of the image.</param>
        /// <param name="textRect">The text rect.</param>
        /// <param name="imageRect">The image rect.</param>
        private void LayoutTextBeforeOrAfterImage(Rectangle totalArea, bool textFirst, Size textSize, Size imageSize, out Rectangle textRect, out Rectangle imageRect)
        {
            var elementSpacing = 0;	// Spacing between the Text and the Image
            var totalWidth = textSize.Width + elementSpacing + imageSize.Width;

            if (!textFirst)
            {
                elementSpacing += 2;
            }

            // If the text is too big, chop it down to the size we have available to it
            if (totalWidth > totalArea.Width)
            {
                textSize.Width = totalArea.Width - elementSpacing - imageSize.Width;
                totalWidth = totalArea.Width;
            }

            var excessWidth = totalArea.Width - totalWidth;
            var offset = 0;

            Rectangle finalTextRect;
            Rectangle finalImageRect;

            var hText = GetHorizontalAlignment(TextAlign);
            var hImage = GetHorizontalAlignment(ImageAlign);

            if (hImage == HorizontalAlignment.Left)
            {
                offset = 0;
            }
            else if (hImage == HorizontalAlignment.Right && hText == HorizontalAlignment.Right)
            {
                offset = excessWidth;
            }
            else if (hImage == HorizontalAlignment.Center && hText is HorizontalAlignment.Left or HorizontalAlignment.Center)
            {
                offset += excessWidth / 3;
            }
            else
            {
                offset += 2 * (excessWidth / 3);
            }

            if (textFirst)
            {
                finalTextRect = new Rectangle(totalArea.Left + offset, AlignInRectangle(totalArea, textSize, TextAlign).Top, textSize.Width, textSize.Height);
                finalImageRect = new Rectangle(finalTextRect.Right + elementSpacing, AlignInRectangle(totalArea, imageSize, ImageAlign).Top, imageSize.Width, imageSize.Height);
            }
            else
            {
                finalImageRect = new Rectangle(totalArea.Left + offset, AlignInRectangle(totalArea, imageSize, ImageAlign).Top, imageSize.Width, imageSize.Height);
                finalTextRect = new Rectangle(finalImageRect.Right + elementSpacing, AlignInRectangle(totalArea, textSize, TextAlign).Top, textSize.Width, textSize.Height);
            }

            textRect = finalTextRect;
            imageRect = finalImageRect;
        }

        /// <summary>
        /// Layouts the text above or below image.
        /// </summary>
        /// <param name="totalArea">The total area.</param>
        /// <param name="textFirst">if set to <c>true</c> [text first].</param>
        /// <param name="textSize">Size of the text.</param>
        /// <param name="imageSize">Size of the image.</param>
        /// <param name="textRect">The text rect.</param>
        /// <param name="imageRect">The image rect.</param>
        private void LayoutTextAboveOrBelowImage(Rectangle totalArea, bool textFirst, Size textSize, Size imageSize, out Rectangle textRect, out Rectangle imageRect)
        {
            var elementSpacing = 0;	// Spacing between the Text and the Image
            var totalHeight = textSize.Height + elementSpacing + imageSize.Height;

            if (textFirst)
            {
                elementSpacing += 2;
            }

            if (textSize.Width > totalArea.Width)
            {
                textSize.Width = totalArea.Width;
            }

            // If the there isn't enough room and we're text first, cut out the image
            if (totalHeight > totalArea.Height && textFirst)
            {
                imageSize = Size.Empty;
                totalHeight = totalArea.Height;
            }

            var excessHeight = totalArea.Height - totalHeight;
            var offset = 0;

            Rectangle finalTextRect;
            Rectangle finalImageRect;

            var vText = GetVerticalAlignment(TextAlign);
            var vImage = GetVerticalAlignment(ImageAlign);

            if (vImage == VerticalAlignment.Top)
            {
                offset = 0;
            }
            else if (vImage == VerticalAlignment.Bottom && vText == VerticalAlignment.Bottom)
            {
                offset = excessHeight;
            }
            else if (vImage == VerticalAlignment.Center && (vText == VerticalAlignment.Top || vText == VerticalAlignment.Center))
            {
                offset += excessHeight / 3;
            }
            else
            {
                offset += 2 * (excessHeight / 3);
            }

            if (textFirst)
            {
                finalTextRect = new Rectangle(AlignInRectangle(totalArea, textSize, TextAlign).Left, totalArea.Top + offset, textSize.Width, textSize.Height);
                finalImageRect = new Rectangle(AlignInRectangle(totalArea, imageSize, ImageAlign).Left, finalTextRect.Bottom + elementSpacing, imageSize.Width, imageSize.Height);
            }
            else
            {
                finalImageRect = new Rectangle(AlignInRectangle(totalArea, imageSize, ImageAlign).Left, totalArea.Top + offset, imageSize.Width, imageSize.Height);
                finalTextRect = new Rectangle(AlignInRectangle(totalArea, textSize, TextAlign).Left, finalImageRect.Bottom + elementSpacing, textSize.Width, textSize.Height);

                if (finalTextRect.Bottom > totalArea.Bottom)
                {
                    finalTextRect.Y = totalArea.Top;
                }
            }

            textRect = finalTextRect;
            imageRect = finalImageRect;
        }

        /// <summary>
        /// Gets the horizontal alignment.
        /// </summary>
        /// <param name="align">The align.</param>
        /// <returns></returns>
        private static HorizontalAlignment GetHorizontalAlignment(System.Drawing.ContentAlignment align)
        {
            switch (align)
            {
                case System.Drawing.ContentAlignment.BottomLeft:
                case System.Drawing.ContentAlignment.MiddleLeft:
                case System.Drawing.ContentAlignment.TopLeft:
                    return HorizontalAlignment.Left;
                case System.Drawing.ContentAlignment.BottomCenter:
                case System.Drawing.ContentAlignment.MiddleCenter:
                case System.Drawing.ContentAlignment.TopCenter:
                    return HorizontalAlignment.Center;
                case System.Drawing.ContentAlignment.BottomRight:
                case System.Drawing.ContentAlignment.MiddleRight:
                case System.Drawing.ContentAlignment.TopRight:
                    return HorizontalAlignment.Right;
            }

            return HorizontalAlignment.Left;
        }

        /// <summary>
        /// Gets the vertical alignment.
        /// </summary>
        /// <param name="align">The align.</param>
        /// <returns></returns>
        private static VerticalAlignment GetVerticalAlignment(System.Drawing.ContentAlignment align)
        {
            switch (align)
            {
                case System.Drawing.ContentAlignment.TopLeft:
                case System.Drawing.ContentAlignment.TopCenter:
                case System.Drawing.ContentAlignment.TopRight:
                    return VerticalAlignment.Top;
                case System.Drawing.ContentAlignment.MiddleLeft:
                case System.Drawing.ContentAlignment.MiddleCenter:
                case System.Drawing.ContentAlignment.MiddleRight:
                    return VerticalAlignment.Center;
                case System.Drawing.ContentAlignment.BottomLeft:
                case System.Drawing.ContentAlignment.BottomCenter:
                case System.Drawing.ContentAlignment.BottomRight:
                    return VerticalAlignment.Bottom;
            }

            return VerticalAlignment.Top;
        }

        /// <summary>
        /// Aligns the inner rectangle.
        /// </summary>
        /// <param name="outer">The outer.</param>
        /// <param name="inner">The inner.</param>
        /// <param name="align">The align.</param>
        /// <returns></returns>
        internal static Rectangle AlignInRectangle(Rectangle outer, Size inner, System.Drawing.ContentAlignment align)
        {
            var x = 0;
            var y = 0;

            switch (align)
            {
                case System.Drawing.ContentAlignment.BottomLeft:
                case System.Drawing.ContentAlignment.MiddleLeft:
                case System.Drawing.ContentAlignment.TopLeft:
                    x = outer.X;
                    break;
                case System.Drawing.ContentAlignment.BottomCenter:
                case System.Drawing.ContentAlignment.MiddleCenter:
                case System.Drawing.ContentAlignment.TopCenter:
                    x = Math.Max(outer.X + ((outer.Width - inner.Width) / 2), outer.Left);
                    break;
                case System.Drawing.ContentAlignment.BottomRight:
                case System.Drawing.ContentAlignment.MiddleRight:
                case System.Drawing.ContentAlignment.TopRight:
                    x = outer.Right - inner.Width;
                    break;
            }

            switch (align)
            {
                case System.Drawing.ContentAlignment.TopCenter:
                case System.Drawing.ContentAlignment.TopLeft:
                case System.Drawing.ContentAlignment.TopRight:
                    y = outer.Y;
                    break;
                case System.Drawing.ContentAlignment.MiddleCenter:
                case System.Drawing.ContentAlignment.MiddleLeft:
                case System.Drawing.ContentAlignment.MiddleRight:
                    y = outer.Y + (outer.Height - inner.Height) / 2;
                    break;
                case System.Drawing.ContentAlignment.BottomCenter:
                case System.Drawing.ContentAlignment.BottomRight:
                case System.Drawing.ContentAlignment.BottomLeft:
                    y = outer.Bottom - inner.Height;
                    break;
            }

            return new Rectangle(x, y, Math.Min(inner.Width, outer.Width), Math.Min(inner.Height, outer.Height));
        }

        #endregion Button Layout Calculations


        /// <summary>
        /// Shows the context menu strip.
        /// </summary>
        private void ShowContextMenuStrip()
        {
            if (_skipNextOpen)
            {
                // we were called because we're closing the context menu strip
                // when clicking the dropdown button.
                _skipNextOpen = false;

                return;
            }

            State = PushButtonState.Pressed;

            if (_mSplitMenu != null)
            {
                _mSplitMenu.Show(this, new Point(0, Height));
            }
            else
            {
                _mSplitMenuStrip.Show(this, new Point(0, Height), ToolStripDropDownDirection.BelowRight);
            }
        }

        /// <summary>
        /// Handles the Opening event of the SplitMenuStrip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void SplitMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            _isSplitMenuVisible = true;
        }

        /// <summary>
        /// Handles the Closing event of the SplitMenuStrip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ToolStripDropDownClosingEventArgs"/> instance containing the event data.</param>
        private void SplitMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _isSplitMenuVisible = false;

            SetButtonDrawState();

            if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
            {
                _skipNextOpen = (_dropDownRectangle.Contains(PointToClient(Cursor.Position))) && MouseButtons == MouseButtons.Left;
            }
        }

        /// <summary>
        /// Handles the Popup event of the SplitMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SplitMenu_Popup(object sender, EventArgs e)
        {
            _isSplitMenuVisible = true;
        }

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message"/> to process.</param>
        protected override void WndProc(ref Message m)
        {
            // 0x0212 == WM_EXITMENULOOP
            if (m.Msg == 0x0212)
            {
                // this message is only sent when a ContextMenu is closed (not a ContextMenuStrip)
                _isSplitMenuVisible = false;

                SetButtonDrawState();
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Sets the the button draw state.
        /// </summary>
        private void SetButtonDrawState()
        {
            if (Bounds.Contains(Parent!.PointToClient(Cursor.Position)))
            {
                State = PushButtonState.Hot;
            }
            else if (Focused)
            {
                State = PushButtonState.Default;
            }
            else if (!Enabled)
            {
                State = PushButtonState.Disabled;
            }
            else
            {
                State = PushButtonState.Normal;
            }
        }
    }
}