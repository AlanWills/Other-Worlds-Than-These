using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxyGameLibrary
{
    public class Text : UIElement
    {
        private SpriteFont Font
        {
            get;
            set;
        }

        private string FontAsset
        {
            get;
            set;
        }

        private UIElement ParentUIElement
        {
            get;
            set;
        }

        public Vector2 ParentOffset
        {
            get;
            private set;
        }

        public Vector2 TextOrigin
        {
            get
            {
                if (Font != null)
                {
                    return new Vector2(MaxLineLength / 2, LinesOfStrings.Count * Font.LineSpacing / 2);
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        // Individual lines of strings to allow wrapping over more than one line
        public List<string> LinesOfStrings
        {
            get;
            private set;
        }

        private float MaxLineLength
        {
            get
            {
                if (LinesOfStrings.Count > 0)
                {
                    float maxLength = Font.MeasureString(LinesOfStrings[0]).X;
                    foreach (string line in LinesOfStrings)
                    {
                        if (Font.MeasureString(line).X > maxLength)
                        {
                            maxLength = Font.MeasureString(line).X;
                        }
                    }

                    return maxLength;
                }

                return 0;
            }
        }

        private float MaxWidth
        {
            get;
            set;
        }

        private bool Wrapped
        {
            get;
            set;
        }

        public Text(string text, Vector2 position, Color colour, string name, string spriteFont = "Fonts/TextFont")
            : base("", position, name)
        {
            LinesOfStrings = new List<string>() { text };
            Position = position;
            FontAsset = spriteFont;
            Colour = colour;
        }

        public Text(string text, Vector2 position, float maxWidth, Color colour, string name, string spriteFont = "Fonts/TextFont")
            : base("", position, name)
        {
            LinesOfStrings = new List<string>() { text };
            Position = position;
            FontAsset = spriteFont;
            Colour = colour;
            MaxWidth = maxWidth;
        }

        // Useful for texts that is linked with buttons
        public Text(string text, UIElement parentUIElement, Vector2 offset, Color colour, string name, string spriteFont = "Fonts/TextFont")
            : base("", parentUIElement.Position, name)
        {
            LinesOfStrings = new List<string>() { text };
            FontAsset = spriteFont;
            ParentUIElement = parentUIElement;
            ParentOffset = offset;
            Colour = colour;
        }

        public override void LoadContent(ContentManager content)
        {
            Font = content.Load<SpriteFont>(FontAsset);

            WrapText();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ParentUIElement != null)
            {
                Position = ParentUIElement.Position + ParentOffset;
            }
        }

        public override void CheckClicked(InGameMouse mouse)
        {
            Rectangle bounds = new Rectangle((int)(Position.X - TextOrigin.X), (int)(Position.Y - TextOrigin.Y), (int)(2 * TextOrigin.X), (int)(2 * TextOrigin.Y));

            if (HoverInfo != null)
            {
                if (IsActive())
                {
                    if (bounds.Contains(new Point((int)InGameMouse.ScreenPosition.X, (int)InGameMouse.ScreenPosition.Y)))
                    {
                        HoverInfo.Activate();
                    }
                    else
                    {
                        HoverInfo.DisableAndHide();
                    }
                }
            }

            if (mouse.IsLeftClicked && mouse.PreviousMouseState.LeftButton == ButtonState.Released && clickDelay > 0.3f)
            {
                CheckForInteraction(mouse.LastLeftClickedPosition);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Font != null)
            {
                if (IsActive())
                {
                    for (int i = 0; i < LinesOfStrings.Count; i++)
                    {
                        spriteBatch.DrawString(Font, LinesOfStrings[i], Position + i * new Vector2(0, Font.LineSpacing), Colour, 0, TextOrigin, 1f, SpriteEffects.None, 0);
                    }

                    if (HoverInfo != null)
                    {
                        HoverInfo.Draw(spriteBatch);
                    }
                }
            }
        }

        public void ChangeText(string newText)
        {
            Wrapped = false;
            LinesOfStrings.Clear();
            LinesOfStrings.Add(newText);

            WrapText();
        }

        public void WrapText()
        {
            if (MaxWidth > 0 && !Wrapped)
            {
                string[] strings = LinesOfStrings[0].Split(' ');
                if (strings.Length > 0)
                {
                    LinesOfStrings.Clear();
                    string line = strings[0];

                    if (strings.Length > 1)
                    {
                        for (int i = 1; i < strings.Length; i++)
                        {
                            Vector2 dimensions = Font.MeasureString(line + strings[i]);
                            if (dimensions.X > MaxWidth)
                            {
                                line += "\n";
                                LinesOfStrings.Add(line);
                                line = strings[i];
                            }
                            else
                            {
                                line += (" " + strings[i]);
                            }

                            if (i == strings.Length - 1 && line != "")
                            {
                                LinesOfStrings.Add(line);
                            }
                        }
                    }
                    else
                    {
                        LinesOfStrings.Add(line);
                    }
                }

                Wrapped = true;
            }
        }

        public void ChangeColour(Color colour)
        {
            Colour = colour;
        }
    }
}
