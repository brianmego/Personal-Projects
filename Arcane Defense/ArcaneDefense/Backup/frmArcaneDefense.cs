using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ArcaneDefense
{
    internal partial class frmArcaneDefense : Form
    {
        private GameState _gameState;
        private Random randnum = new Random();
        
        internal frmArcaneDefense()
        {
            InitializeComponent();

            //Startup the game state
            _gameState = new GameState(this);
        }

        /// <summary>
        /// Handles inputs related to exiting (need to add pausing and instructions)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmArcaneDefense_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void frmArcaneDefense_KeyPress(object sender, KeyPressEventArgs e)
        {
            _gameState.KeyPress(sender, e);
        }

        private void frmArcaneDefense_MouseClick(object sender, MouseEventArgs e)
        {
            _gameState.MouseClick(sender, e);
        }

        private void frmArcaneDefense_Paint(object sender, PaintEventArgs e)
        {
            //Perform any animation and updates
            _gameState.Update();

            //Draw everything
            _gameState.Draw(e.Graphics);

            //Force the next Paint()
            this.Invalidate();
        }

        private void frmArcaneDefense_MouseMove(object sender, MouseEventArgs e)
        {
            _gameState.MouseMove(sender, e);
        }

        private void frmArcaneDefense_Resize(object sender, EventArgs e)
        {
            _gameState.Resize();
        }

    }
}
