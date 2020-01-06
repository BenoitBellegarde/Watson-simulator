
/***************************************************************************
*                                                                          *
*  Copyright (c) Raphaël Ernaelsten (@RaphErnaelsten)                      *
*  All Rights Reserved.                                                    *
*                                                                          *
*  NOTICE: Aura 2 is a commercial project.                                 * 
*  All information contained herein is, and remains the property of        *
*  Raphaël Ernaelsten.                                                     *
*  The intellectual and technical concepts contained herein are            *
*  proprietary to Raphaël Ernaelsten and are protected by copyright laws.  *
*  Dissemination of this information or reproduction of this material      *
*  is strictly forbidden.                                                  *
*                                                                          *
***************************************************************************/

using UnityEditor;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Collection of custom GuiStyles
    /// </summary>
    public static class GuiStyles
    {
        #region Private Members
        /// <summary>
        /// Name prefix
        /// </summary>
        private const string nameBase = "Aura2.Styles.";
        /// <summary>
        /// The font color
        /// </summary>
        private static readonly Color fontColor = Color.white * 0.85f;
        /// <summary>
        /// The font size of big texts
        /// </summary>
        private const int fontBigSize = 13;
        /// <summary>
        /// The font size of small texts
        /// </summary>
        private const int fontSmallSize = 9;
        /// <summary>
        /// The default margins size
        /// </summary>
        private const int defaultPadding = 8;
        /// <summary>
        /// The default margin rectoffset
        /// </summary>
        private static readonly RectOffset defaultPaddingRectOffset = new RectOffset(defaultPadding, defaultPadding, defaultPadding, defaultPadding);
        #endregion

        #region Properties
        public static GUIStyle Background
        {
            get;
            private set;
        }

        public static GUIStyle BackgroundNoBorder
        {
            get;
            private set;
        }

        public static GUIStyle Label
        {
            get;
            private set;
        }
        public static GUIStyle LabelBold
        {
            get;
            private set;
        }
        public static GUIStyle LabelCentered
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldCentered
        {
            get;
            private set;
        }
        public static GUIStyle LabelBig
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldBig
        {
            get;
            private set;
        }
        public static GUIStyle LabelCenteredBig
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldCenteredBig
        {
            get;
            private set;
        }
        public static GUIStyle LabelSmall
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldSmall
        {
            get;
            private set;
        }
        public static GUIStyle LabelCenteredSmall
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldCenteredSmall
        {
            get;
            private set;
        }
        public static GUIStyle LabelBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelCenteredBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldCenteredBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelBigBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldBigBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelCenteredBigBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldCenteredBigBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelSmallBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldSmallBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelCenteredSmallBackground
        {
            get;
            private set;
        }
        public static GUIStyle LabelBoldCenteredSmallBackground
        {
            get;
            private set;
        }

        public static GUIStyle Checker
        {
            get;
            private set;
        }

        public static GUIStyle Button
        {
            get;
            private set;
        }
        public static GUIStyle ButtonBig
        {
            get;
            private set;
        }
        public static GUIStyle ButtonBold
        {
            get;
            private set;
        }
        public static GUIStyle ButtonBigBold
        {
            get;
            private set;
        }
        public static GUIStyle ButtonImageOnly
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressed
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressedBig
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressedBold
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressedBigBold
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressedImageOnly
        {
            get;
            private set;
        }
        public static GUIStyle ButtonNoHover
        {
            get;
            private set;
        }
        public static GUIStyle ButtonNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonBigNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonBoldNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonBigBoldNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonImageOnlyNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressedNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressedBigNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressedBoldNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressedBigBoldNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonPressedImageOnlyNoBorder
        {
            get;
            private set;
        }
        public static GUIStyle ButtonNoHoverNoBorder
        {
            get;
            private set;
        }


        public static GUIStyle EmptyMiddleAligned
        {
            get;
            private set;
        }
        public static GUIStyle EmptyMiddleAlignedTop
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        static GuiStyles()
        {
            Background = new GUIStyle();
            Background.alignment = TextAnchor.MiddleCenter;
            Background.border = new RectOffset(2, 2, 2, 2);
            Background.imagePosition = ImagePosition.ImageOnly;
            Background.name = nameBase + "Background";
            Background.normal.background = Aura.ResourcesCollection.backgroundStylesTexture;
            Background.normal.textColor = fontColor;
            Background.padding = defaultPaddingRectOffset;

            BackgroundNoBorder = new GUIStyle(Background);
            BackgroundNoBorder.padding = new RectOffset(0, 0, 0, 0);
            BackgroundNoBorder.name = Background.name + "NoBorder";

            Label = new GUIStyle();
            Label.alignment = TextAnchor.MiddleLeft;
            Label.name = nameBase + "Label";
            Label.normal.textColor = fontColor;
            Label.padding = new RectOffset(3,3,3,3);
            Label.richText = true;
            Label.wordWrap = true;

            LabelBold = new GUIStyle(Label);
            LabelBold.fontStyle = FontStyle.Bold;
            LabelBold.name = Label.name + "Bold";

            LabelCentered = new GUIStyle(Label);
            LabelCentered.alignment = TextAnchor.UpperCenter;
            LabelCentered.name = Label.name + "Centered";

            LabelBoldCentered = new GUIStyle(LabelBold);
            LabelBoldCentered.alignment = LabelCentered.alignment;
            LabelBoldCentered.name = Label.name + "BoldCentered";

            LabelBig = new GUIStyle(Label);
            LabelBig.fontSize = fontBigSize;
            LabelBig.name = Label.name + "Big";

            LabelBoldBig = new GUIStyle(LabelBold);
            LabelBoldBig.fontSize = LabelBig.fontSize;
            LabelBoldBig.name = LabelBold.name + "Big";

            LabelCenteredBig = new GUIStyle(LabelCentered);
            LabelCenteredBig.fontSize = LabelBig.fontSize;
            LabelCenteredBig.name = LabelCentered.name + "Big";

            LabelBoldCenteredBig = new GUIStyle(LabelBoldCentered);
            LabelBoldCenteredBig.fontSize = LabelBig.fontSize;
            LabelBoldCenteredBig.name = LabelBoldCentered.name + "Big";

            LabelSmall = new GUIStyle(Label);
            LabelSmall.fontSize = fontSmallSize;
            LabelSmall.name = Label.name + "Small";

            LabelBoldSmall = new GUIStyle(LabelBold);
            LabelBoldSmall.fontSize = LabelSmall.fontSize;
            LabelBoldSmall.name = LabelBold.name + "Small";

            LabelCenteredSmall = new GUIStyle(LabelCentered);
            LabelCenteredSmall.fontSize = LabelSmall.fontSize;
            LabelCenteredSmall.name = LabelCentered.name + "Small";

            LabelBoldCenteredSmall = new GUIStyle(LabelBoldCentered);
            LabelBoldCenteredSmall.fontSize = LabelSmall.fontSize;
            LabelBoldCenteredSmall.name = LabelBoldCentered.name + "Small";

            LabelBackground = new GUIStyle(Label);
            LabelBackground.border = new RectOffset(1, 1, 1, 1);
            LabelBackground.normal.background = Aura.ResourcesCollection.buttonUpStylesTexture;
            LabelBackground.padding = defaultPaddingRectOffset;
            LabelBackground.name = Label.name + "Background";

            LabelBoldBackground = new GUIStyle(LabelBold);
            LabelBoldBackground.border = LabelBackground.border;
            LabelBoldBackground.normal.background = LabelBackground.normal.background;
            LabelBoldBackground.padding = defaultPaddingRectOffset;
            LabelBoldBackground.name = LabelBold.name + "Background";

            LabelCenteredBackground = new GUIStyle(LabelCentered);
            LabelCenteredBackground.border = LabelBackground.border;
            LabelCenteredBackground.normal.background = LabelBackground.normal.background;
            LabelCenteredBackground.padding = defaultPaddingRectOffset;
            LabelCenteredBackground.name = LabelCentered.name + "Background";

            LabelBoldCenteredBackground = new GUIStyle(LabelBoldCentered);
            LabelBoldCenteredBackground.border = LabelBackground.border;
            LabelBoldCenteredBackground.normal.background = LabelBackground.normal.background;
            LabelBoldCenteredBackground.padding = defaultPaddingRectOffset;
            LabelBoldCenteredBackground.name = LabelBoldCentered.name + "Background";

            LabelBigBackground = new GUIStyle(LabelBig);
            LabelBigBackground.border = LabelBackground.border;
            LabelBigBackground.normal.background = LabelBackground.normal.background;
            LabelBigBackground.padding = defaultPaddingRectOffset;
            LabelBigBackground.name = LabelBig.name + "Background";

            LabelBoldBigBackground = new GUIStyle(LabelBoldBig);
            LabelBoldBigBackground.border = LabelBackground.border;
            LabelBoldBigBackground.normal.background = LabelBackground.normal.background;
            LabelBoldBigBackground.padding = defaultPaddingRectOffset;
            LabelBoldBigBackground.name = LabelBoldBig.name + "Background";

            LabelCenteredBigBackground = new GUIStyle(LabelCenteredBig);
            LabelCenteredBigBackground.border = LabelBackground.border;
            LabelCenteredBigBackground.normal.background = LabelBackground.normal.background;
            LabelCenteredBigBackground.padding = defaultPaddingRectOffset;
            LabelCenteredBigBackground.name = LabelCenteredBig.name + "Background";

            LabelBoldCenteredBigBackground = new GUIStyle(LabelBoldCenteredBig);
            LabelBoldCenteredBigBackground.border = LabelBackground.border;
            LabelBoldCenteredBigBackground.normal.background = LabelBackground.normal.background;
            LabelBoldCenteredBigBackground.padding = defaultPaddingRectOffset;
            LabelBoldCenteredBigBackground.name = LabelBoldCenteredBig.name + "Background";

            LabelSmallBackground = new GUIStyle(LabelSmall);
            LabelSmallBackground.border = LabelBackground.border;
            LabelSmallBackground.normal.background = LabelBackground.normal.background;
            LabelSmallBackground.padding = defaultPaddingRectOffset;
            LabelSmallBackground.name = LabelSmall.name + "Background";

            LabelBoldSmallBackground = new GUIStyle(LabelBoldSmall);
            LabelBoldSmallBackground.border = LabelBackground.border;
            LabelBoldSmallBackground.normal.background = LabelBackground.normal.background;
            LabelBoldSmallBackground.padding = defaultPaddingRectOffset;
            LabelBoldSmallBackground.name = LabelBoldSmall.name + "Background";

            LabelCenteredSmallBackground = new GUIStyle(LabelCenteredSmall);
            LabelCenteredSmallBackground.border = LabelBackground.border;
            LabelCenteredSmallBackground.normal.background = LabelBackground.normal.background;
            LabelCenteredSmallBackground.padding = defaultPaddingRectOffset;
            LabelCenteredSmallBackground.name = LabelCenteredSmall.name + "Background";

            LabelBoldCenteredSmallBackground = new GUIStyle(LabelBoldCenteredSmall);
            LabelBoldCenteredSmallBackground.border = LabelBackground.border;
            LabelBoldCenteredSmallBackground.normal.background = LabelBackground.normal.background;
            LabelBoldCenteredSmallBackground.padding = defaultPaddingRectOffset;
            LabelBoldCenteredSmallBackground.name = LabelBoldCenteredSmall.name + "Background";

            Checker = new GUIStyle();
            Checker.alignment = TextAnchor.MiddleCenter;
            Checker.border = new RectOffset(1, 1, 1, 1);
            Checker.name = nameBase + "Checker";
            Checker.normal.background = Aura.ResourcesCollection.checkerUpStylesTexture;
            Checker.onNormal.background = Checker.onNormal.background;
            Checker.active.background = Aura.ResourcesCollection.checkerDownStylesTexture;
            Checker.onActive.background = Checker.active.background;
            Checker.focused.background = Aura.ResourcesCollection.checkerHoverStylesTexture;
            Checker.onFocused.background = Checker.focused.background;
            Checker.hover.background = Aura.ResourcesCollection.checkerHoverStylesTexture;
            Checker.onHover.background = Checker.hover.background;
            Checker.padding = new RectOffset(0, 0, 0, 0);

            Button = new GUIStyle();
            Button.alignment = TextAnchor.MiddleCenter;
            Button.border = new RectOffset(1, 1, 1, 1);
            Button.name = nameBase + "Button";
            Button.normal.background = Aura.ResourcesCollection.buttonUpStylesTexture;
            Button.normal.textColor = fontColor;
            Button.onNormal.background = Button.onNormal.background;
            Button.onNormal.textColor = Button.normal.textColor;
            Button.active.background = Aura.ResourcesCollection.buttonDownStylesTexture;
            Button.active.textColor = Button.normal.textColor;
            Button.onActive.background = Button.active.background;
            Button.onActive.textColor = Button.normal.textColor;
            Button.focused.background = Aura.ResourcesCollection.buttonHoverStylesTexture;
            Button.focused.textColor = Button.normal.textColor;
            Button.onFocused.background = Button.focused.background;
            Button.onFocused.textColor = Button.normal.textColor;
            Button.hover.background = Aura.ResourcesCollection.buttonHoverStylesTexture;
            Button.hover.textColor = Button.normal.textColor;
            Button.onHover.background = Button.hover.background;
            Button.onHover.textColor = Button.normal.textColor;
            Button.padding = defaultPaddingRectOffset;

            ButtonBig = new GUIStyle(Button);
            ButtonBig.fontSize = fontBigSize;
            ButtonBig.name = Button.name + "Big";

            ButtonBold = new GUIStyle(Button);
            ButtonBold.fontStyle = FontStyle.Bold;
            ButtonBold.name = Button.name + "Bold";

            ButtonBigBold = new GUIStyle(Button);
            ButtonBigBold.fontSize = ButtonBig.fontSize;
            ButtonBigBold.fontStyle = ButtonBold.fontStyle;
            ButtonBigBold.name = Button.name + "BigBold";

            ButtonImageOnly = new GUIStyle(Button);
            ButtonImageOnly.imagePosition = ImagePosition.ImageOnly;
            ButtonImageOnly.name = Button.name + "ImageOnly";

            ButtonPressed = new GUIStyle(Button);
            ButtonPressed.normal.background = Aura.ResourcesCollection.buttonDownStylesTexture;
            ButtonPressed.onNormal.background = ButtonPressed.normal.background;
            ButtonPressed.name = Button.name + "Pressed";

            ButtonPressedBig = new GUIStyle(ButtonPressed);
            ButtonPressedBig.fontSize = fontBigSize;
            ButtonPressedBig.name = ButtonPressed.name + "Big";

            ButtonPressedBold = new GUIStyle(ButtonPressed);
            ButtonPressedBold.fontStyle = FontStyle.Bold;
            ButtonPressedBold.name = ButtonPressed.name + "Bold";

            ButtonPressedBigBold = new GUIStyle(ButtonPressed);
            ButtonPressedBigBold.fontSize = ButtonBig.fontSize;
            ButtonPressedBigBold.fontStyle = ButtonBold.fontStyle;
            ButtonPressedBigBold.name = ButtonPressed.name + "BigBold";

            ButtonPressedImageOnly = new GUIStyle(ButtonPressed);
            ButtonPressedImageOnly.imagePosition = ImagePosition.ImageOnly;
            ButtonPressedImageOnly.name = Button.name + "PressedImageOnly";

            ButtonNoHover = new GUIStyle();
            ButtonNoHover.alignment = Button.alignment;
            ButtonNoHover.border = Button.border;
            ButtonNoHover.imagePosition = Button.imagePosition;
            ButtonNoHover.name = Button.name + "NoHover";
            ButtonNoHover.active.background = Button.active.background;
            ButtonNoHover.active.textColor = Button.active.textColor;
            ButtonNoHover.onActive.background = Button.active.background;
            ButtonNoHover.onActive.textColor = Button.active.textColor;
            ButtonNoHover.normal.background = Button.normal.background;
            ButtonNoHover.normal.textColor = Button.normal.textColor;
            ButtonNoHover.onNormal.background = Button.normal.background;
            ButtonNoHover.onNormal.textColor = Button.normal.textColor;
            ButtonNoHover.padding = Button.padding;

            ButtonNoBorder = new GUIStyle(Button);
            ButtonNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonNoBorder.name = Button.name + "NoBorder";

            ButtonBigNoBorder = new GUIStyle(ButtonBig);
            ButtonBigNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonBigNoBorder.name = ButtonBig.name + "NoBorder";

            ButtonBoldNoBorder = new GUIStyle(ButtonBold);
            ButtonBoldNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonBoldNoBorder.name = ButtonBold.name + "NoBorder";

            ButtonBigBoldNoBorder = new GUIStyle(ButtonBigBold);
            ButtonBigBoldNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonBigBoldNoBorder.name = ButtonBigBold.name + "NoBorder";

            ButtonImageOnlyNoBorder = new GUIStyle(ButtonImageOnly);
            ButtonImageOnlyNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonImageOnlyNoBorder.name = ButtonImageOnly.name + "NoBorder";

            ButtonPressedNoBorder = new GUIStyle(ButtonPressed);
            ButtonPressedNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonPressedNoBorder.name = ButtonPressed.name + "NoBorder";

            ButtonPressedBigNoBorder = new GUIStyle(ButtonPressedBig);
            ButtonPressedBigNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonPressedBigNoBorder.name = ButtonPressedBig.name + "NoBorder";

            ButtonPressedBoldNoBorder = new GUIStyle(ButtonPressedBold);
            ButtonPressedBoldNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonPressedBoldNoBorder.name = ButtonPressedBold.name + "NoBorder";

            ButtonPressedBigBoldNoBorder = new GUIStyle(ButtonPressedBigBold);
            ButtonPressedBigBoldNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonPressedBigBoldNoBorder.name = ButtonPressedBigBold.name + "NoBorder";

            ButtonPressedImageOnlyNoBorder = new GUIStyle(ButtonPressedImageOnly);
            ButtonPressedImageOnlyNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonPressedImageOnlyNoBorder.name = ButtonPressedImageOnly.name + "NoBorder";

            ButtonNoHoverNoBorder = new GUIStyle(ButtonNoHover);
            ButtonNoHoverNoBorder.padding = new RectOffset(0, 0, 0, 0);
            ButtonNoHoverNoBorder.name = ButtonNoHover.name + "NoBorder";

            EmptyMiddleAligned = new GUIStyle();
            EmptyMiddleAligned.alignment = TextAnchor.MiddleCenter;
            EmptyMiddleAligned.imagePosition = ImagePosition.ImageOnly;
            EmptyMiddleAligned.name = nameBase + "EmptyMiddleAligned";
            EmptyMiddleAligned.padding = new RectOffset(0, 0, 0, 0);
            
            EmptyMiddleAlignedTop = new GUIStyle(EmptyMiddleAligned);
            EmptyMiddleAlignedTop.alignment = TextAnchor.UpperCenter;
            EmptyMiddleAlignedTop.name = EmptyMiddleAligned.name + "Top";
        }
        #endregion
    }
}