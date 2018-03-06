using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace betAppCasino
{
    public partial class Default : System.Web.UI.Page
    {
        Random random = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
           if (!Page.IsPostBack)
            {
                string[] reel = new string[] { spinReel(), spinReel(), spinReel() };
                displayImage(reel);
                ViewState.Add("playersMoney", 100);
                displayPlayersMoney();
            }
        }

        protected void pullButton_Click(object sender, EventArgs e)
        {
            // When someone clicks the button, display a random
            //image in Image1

            checkZeroAcctBeforeExecutePullButton();

        }

        private void displayPlayersMoney()
        {
            moneyLabel.Text = string.Format("Player Money:{0:C}", ViewState["playersMoney"]);
        }

        private void adjustPlayersMoney(int bet, int winnings)
        {
                int PlayersMoney = (int)ViewState["playersMoney"];
                PlayersMoney -= bet;
                PlayersMoney += winnings;
                ViewState["playersMoney"] = PlayersMoney;
       }

        private void displayResult(int bet, int winnings)
        {
            if (winnings > bet)
                resultLabel.Text = string.Format("You  bet {0} and win {1}", bet , winnings);
            else
                resultLabel.Text = "You lost";
        }

        private string spinReel()
        {
            string[] images = new string[] { "Strawberry", "Bar", "Lemon", "Bell", "Clover", "Cherry", "Diamond", "Orange", "Seven", "HorseShoe", "Plum", "Watermelon" };
             return images[random.Next(11)];

        }
        private int  pullLever(int bet)
        {
            string[] reel = new string[] {spinReel(), spinReel(), spinReel()};
            displayImage(reel);
           int mutiplier = evaluateSpin(reel);
            return bet* mutiplier;
        }
        private void displayImage(string[] reel)
        {
            Image1.ImageUrl = "/Images/" + reel[0] + ".png";
            Image2.ImageUrl = "/Images/" + reel[1] + ".png";
            Image3.ImageUrl = string.Format("Images/{0}.png", reel[2]);
        }
        // Determine the value of the pull
        private int evaluateSpin(string[] reel)
        {
            if (isBar(reel)) return 0;
            if (isJackpot(reel)) return 100;
            int multiplier = 0;
            if (isWinner(reel, out multiplier)) return multiplier;
            return 0;
        }

        private bool isWinner(string[] reel, out int multiplier)
        {
            multiplier = determineCherryMultiplier(reel);
            if (multiplier > 0) return true;
            return false;
        }

        private int determineCherryMultiplier(string[] reel)
        {
            int cherryCount = determineCherry(reel);
            if (cherryCount == 1) return 2;
            if (cherryCount == 2) return 3;
            if (cherryCount == 3) return 4;
            return 0;
        }

        private int determineCherry(string[] reel)
        {
            int cherryCount = 0;
            if (reel[0] == "Cherry") cherryCount++;
            if (reel[1] == "Cherry") cherryCount++;
           if (reel[2] == "Cherry") cherryCount++;
            return cherryCount;


        }

        private bool isJackpot(string[] reel)
        {
            if (reel[0] == "Seven" && reel[1] == "Seven" && reel[2] == "Seven" )
                return true;
            return false;
        }

        private bool isBar(string[] reel)
        {
            if (reel[0] == "Bar" || reel[1] == "Bar" || reel[2] == "Bar")
                return true;
            return false;
        }
        private void ExecutePullButton()
        {
            int bet = 0;
            if (!int.TryParse(betTextBox.Text, out bet)) return;
            int winnings = pullLever(bet);
            //In the pullButton_Click, we’ll initialize an int bet that’s equal to 0. Then, directly underneath, we’ll setup a conditional to ensure that the value passed in to the betTextBox can be converted to an int and, if so, set bet equal to that value. To do that, we’ll use int.TryParse(), passing in the TextBox and the bet variable. Then we’ll modify the pullLever() call, passing in the bet variable:
            displayResult(bet, winnings);
            adjustPlayersMoney(bet, winnings);
            displayPlayersMoney();
        }
        private void checkZeroAcctBeforeExecutePullButton()
        {
            if ((int)ViewState["playersMoney"] != 0)
            ExecutePullButton();
            else
            resultLabel.Text = "Please add money to your Acct";
            
        }
    }
}
