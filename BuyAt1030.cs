#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Indicator;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Strategy;
#endregion

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    /// <summary>
    /// Just buy at 1030 every day!
    /// </summary>
    [Description("Just buy at 1030 every day!")]
    public class BuyAt1030 : Strategy
    {
        #region Variables
        // Wizard generated variables
        private double stopLossPercent = 1.8; // Default setting for StopLossPercent
        private double profitTargetPercent = 2.2; // Default setting for ProfitTargetPercent
        // User defined variables (add any user defined variables below)
		private int purchaseTime = 103000; 
		private int quantity = 1000;
        #endregion

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
            SetProfitTarget("", CalculationMode.Percent, ProfitTargetPercent/100);
            SetStopLoss("", CalculationMode.Percent, StopLossPercent/100, false);

            CalculateOnBarClose = true;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
            // Condition set 1
            if (ToTime(Time[0]) == PurchaseTime)
            {
                EnterLong(quantity, "");
            }
        }

        #region Properties
        [Description("")]
        [GridCategory("Parameters")]
        public double StopLossPercent
        {
            get { return stopLossPercent; }
            set { stopLossPercent = Math.Max(0.5, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public double ProfitTargetPercent
        {
            get { return profitTargetPercent; }
            set { profitTargetPercent = Math.Max(0.5, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int PurchaseTime
        {
            get { return purchaseTime; }
            set { purchaseTime = Math.Max(093000, value); }
        }
		#endregion
    }
}
