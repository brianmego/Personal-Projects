using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ArcaneDefense
{
    internal class SpellButton: Sprite
    {
        private Bitmap DefaultImage;
        private Bitmap SelectedImage;
        private Bitmap HiddenImage;
        private bool hidden = false;
        /// <summary>
        /// The assigned spell corresponding to this button
        /// </summary>
        internal Spell Spell;
        internal char Hotkey;
        private string spellName;

        internal SpellButton(GameState gameState, string spellName, float x, float y, List<string> images, string selectedFilename, char hotkey, Spell spell) :
            base(gameState, x, y, images)
        {
            this.DefaultImage = new Bitmap(images[0]);
            this.SelectedImage = new Bitmap(selectedFilename);
            this.HiddenImage = new Bitmap(@"images\Spells\QuestionMark.png");
            this.Hotkey = hotkey;
            this.Spell = spell;
            this.spellName = spellName;
            this.Size.Height = this.Size.Width = gameState.GameArea.Width / 10;
        }

        internal void Select()
        {
            this._frames[0] = new Bitmap(SelectedImage);
            this.Spell.Select();
            _gameState.selectedSpell = this.Spell;
        }

        internal void Deselect()
        {
            this._frames[0] = new Bitmap(DefaultImage);
            this.Spell.Deselect();
            this.hidden = false;
        }

        internal void Hide()
        {
            this._frames[0] = new Bitmap(HiddenImage);
            hidden = true;
        }

        internal void ShowInformation(Graphics graphics)
        {
            Rectangle rectOutline = new Rectangle((int)(_gameState.TotalWindowArea.Width/2-_gameState.GameArea.Width/2+2), 
                                                  (int)(_gameState.TotalWindowArea.Height/2 + _gameState.GameArea.Height/2 - _gameState.GameArea.Height/3), 
                                                  150, 
                                                  40);
            graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), rectOutline);            
            
            Rectangle rectBackground = new Rectangle(rectOutline.X + 1, rectOutline.Y + 1, 
                                                     rectOutline.Width - 1, rectOutline.Height - 1);
            graphics.FillRectangle(new SolidBrush(Color.Gray), rectBackground);

            //Spell Name
            if (this.hidden != true)
            {
                graphics.DrawString(spellName, new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.BlanchedAlmond),
                                   rectBackground.X, rectBackground.Y + 5);
            }
            else
                graphics.DrawString("???", new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.BlanchedAlmond),
                                   rectBackground.X, rectBackground.Y + 5);
            //Spell Cost
            graphics.DrawString(Spell.GetManaCost().ToString(), new Font("Arial", 8), new SolidBrush(Color.BlanchedAlmond),
                                rectBackground.X, rectBackground.Y + 20);
        }
        
    }
}
