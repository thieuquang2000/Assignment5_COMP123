﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment5
{
    public partial class CalculatorForm : Form
    {
        // CLASS PROPERTIES
        public string outputString { get; set; }
        public bool decimalExists { get; set; }
        public float outputValue { get; set; }

        public Label ActiveLabel { get; set; }

        public Animation animationState;

        /// <summary>
        /// This is the Constructor for the Calculator Form
        /// </summary>
        public CalculatorForm()
        {
            InitializeComponent();

        }

        /// <summary>
        /// This is the event handler that triggers when the form loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculatorForm_Load(object sender, EventArgs e)
        {
            clearNumericKeyboard();

            ActiveLabel = null;
            NumericKeyboardPanel.Visible = false;

            Size = new Size(320, 480);

            animationState = Animation.IDLE;
        }

        /// <summary>
        /// This is the event handler for the CalculatorForm Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculatorForm_Click(object sender, EventArgs e)
        {
            clearNumericKeyboard();

            if(ActiveLabel != null)
            {
                ActiveLabel.BackColor = Color.White;
            }

            ActiveLabel = null;
            animationState = Animation.DOWN;
            AnimationTimer.Enabled = true;
        }


        /// <summary>
        /// This is the shared Event Handler for all the calculator buttons - Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculatorButton_Click(object sender, EventArgs e)
        {
            var TheButton = sender as Button;
            var tag = TheButton.Tag.ToString();

            int buttonValue;
            bool resultCondition = int.TryParse(tag, out buttonValue);

            // If the use pressed a number button
            if(resultCondition)
            {
                int maxSize = 3;
                if(decimalExists)
                {
                    maxSize = 5;
                }
                
                if((outputString != "0") &&  (ResultLabel.Text.Count() < maxSize))
                {
                    outputString += tag;
                    ResultLabel.Text = outputString;
                }
                
            }
           
            // if the user pressed a button that is not a number
            if(!resultCondition)
            {
                switch (tag)
                {
                    case "clear":
                        clearNumericKeyboard();
                        break;
                    case "back":
                        removeLastCharacterFromResultLabel();
                        break;
                    case "done":
                        finalizeOutput();
                        break;
                    case "decimal":
                        addDecimalToResultLabel();
                        break;
                }
            }

            
        }

        /// <summary>
        /// This method adds a decimal to the ResultLabel
        /// </summary>
        private void addDecimalToResultLabel()
        {
            if (!decimalExists)
            {
                outputString += ".";
                decimalExists = true;
            }
        }

        /// <summary>
        /// This method finalizes the calculation for a label
        /// </summary>
        private void finalizeOutput()
        {
            if (outputString == string.Empty)
            {
                outputString = "0";
            }
            outputValue = float.Parse(outputString);
            ActiveLabel.Text = outputValue.ToString();
            clearNumericKeyboard();

            ActiveLabel.BackColor = Color.White;
            ActiveLabel = null;

            animationState = Animation.DOWN;
            AnimationTimer.Enabled = true;
        }


        /// <summary>
        /// This method removes the last character from the ResultLabel
        /// </summary>
        private void removeLastCharacterFromResultLabel()
        {
            if (outputString.Length > 0)
            {

                var lastChar = outputString.Substring(outputString.Length - 1);
                if (lastChar == ".")
                {
                    decimalExists = false;
                }
                outputString = outputString.Remove(outputString.Length - 1);

                if(outputString.Length == 0)
                {
                    outputString = "0";
                }
                ResultLabel.Text = outputString;
            }
        }

        /// <summary>
        /// This method clears the numeric keyboard
        /// </summary>
        private void clearNumericKeyboard()
        {
            ResultLabel.Text = "0";
            outputString = String.Empty;
            decimalExists = false;
            outputValue = 0.0f;
        }

       

        /// <summary>
        /// This is the event handler for the HeightLabel Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActiveLabel_Click(object sender, EventArgs e)
        {
            if(ActiveLabel != null)
            {
                ActiveLabel.BackColor = Color.White;
                ActiveLabel = null;
            }

            ActiveLabel = sender as Label;

            ActiveLabel.BackColor = Color.LightBlue;

            NumericKeyboardPanel.Visible = true;

            if(ActiveLabel.Text != "0")
            {
                ResultLabel.Text = ActiveLabel.Text;
                outputString = ActiveLabel.Text;

            }

            //CalculatorButtonTableLayoutPanel.Location = new Point(12, ActiveLabel.Location.Y + 55);
            NumericKeyboardPanel.BringToFront();

            AnimationTimer.Enabled = true;
            animationState = Animation.UP;
        }

        /// <summary>
        ///  This is the event handler for the AnimationTimer Tick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            switch(animationState)
            {
                case Animation.IDLE:
                    break;
                case Animation.UP:
                    MoveNumericKeyboardUp();
                    break;
                case Animation.DOWN:
                    MoveNumericKeyboardDown();
                    break;
            }
            
        }

        private void MoveNumericKeyboardUp()
        {
            var currentLocation = NumericKeyboardPanel.Location;

            // decrement current location of Numeric Keyboard by 20
            currentLocation = new Point(currentLocation.X, currentLocation.Y - 20);
            NumericKeyboardPanel.Location = currentLocation;

            // compare NumericKeyboard current location with the Active Label
            if (currentLocation.Y <= ActiveLabel.Location.Y + 55)
            {
                NumericKeyboardPanel.Location = new Point(currentLocation.X, ActiveLabel.Location.Y + 55);
                AnimationTimer.Enabled = false;
                animationState = Animation.IDLE;
            }
        }

        private void MoveNumericKeyboardDown()
        {
            var currentLocation = NumericKeyboardPanel.Location;

            // increment current location of Numeric Keyboard by 20
            currentLocation = new Point(currentLocation.X, currentLocation.Y + 20);
            NumericKeyboardPanel.Location = currentLocation;

            // compare NumericKeyboard current location with the Active Label
            if (currentLocation.Y >= 466)
            {
                NumericKeyboardPanel.Location = new Point(currentLocation.X, 466);
                AnimationTimer.Enabled = false;
                animationState = Animation.IDLE;
                NumericKeyboardPanel.Visible = false;
            }
        }

        private void Ca(object sender, EventArgs e)
        {
            if(ImperialButton.Checked)
            {
                int weight;
                int height;
                int Bmi;
                weight = int.Parse(WeightLabel.Text);
                height = int.Parse(HeightLabel.Text);
                Bmi = (weight * 703) / (height * height);
                if (Bmi < 18.5)
                {
                    Result.Text = Bmi + "\n Underweight." + "\n Please eat more food.";
                    Result.BackColor = Color.LightGray;
                }
                else if (Bmi > 18.5 && Bmi < 24.9)
                {
                    Result.Text = Bmi + "\n Normal." + "\n Please maintain your ideal weight";
                    Result.BackColor = Color.Green;
                }
                else if (Bmi > 25 && Bmi < 29.9)
                {
                    Result.Text = Bmi + "\n Overweight." + "\n Please eat less and do more exercise";
                    Result.BackColor = Color.Orange;
                }
                else if (Bmi > 30)
                {
                    Result.Text = Bmi + "\n OBESE." + "\n Please go see a doctor to have some consultation.";
                    Result.BackColor = Color.Red;
                }
            }
            else if(NumericButton.Checked)
            {
                double weight;
                double height;
                double Bmi;
                weight = int.Parse(WeightLabel.Text);
                height = double.Parse(HeightLabel.Text);
                Bmi = weight / (height * height);
                if (Bmi < 18.5)
                {
                    Result.Text = Bmi + "\n Underweight." + "\n Please eat more food.";
                    Result.BackColor = Color.LightGray;
                }
                else if (Bmi > 18.5 && Bmi < 24.9)
                {
                    Result.Text = Bmi + "\n Normal." + "\n Please maintain your ideal weight";
                    Result.BackColor = Color.Green;
                }
                else if (Bmi > 25 && Bmi < 29.9)
                {
                    Result.Text = Bmi + "\n Overweight." + "\n Please eat less and do more exercise";
                    Result.BackColor = Color.Orange;
                }
                else if (Bmi > 30)
                {
                    Result.Text = Bmi + "\n OBESE." + "\n Please go see a doctor to have some consultation.";
                    Result.BackColor = Color.Red;
                }
            }
        }
    }
}
