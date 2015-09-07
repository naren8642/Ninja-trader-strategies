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
    /// Buy after big drop
    /// </summary>
    [Description("Buy after big drop")]
    public class BuyAfterBigDrop : Strategy
    {
        #region Variables
        // Wizard generated variables
        private double dropSize = 8.00; // Default setting for DropSize
        private int dropTime = 25; // Default setting for DropTime
        private int unitsToBuy = 1; // Default setting for UnitsToBuy
        private double target = 0.009; // Default setting for Target
        private double stop = 0.006; // Default setting for Stop
		private int barsToWait = 5; // Number of bars to wait after drop
		private int mfiLength = 14; // MFI parameter
        // User defined variables (add any user defined variables below)
		int alreadyTradedToday = 0;
        #endregion

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
            SetStopLoss("", CalculationMode.Percent, Stop, false);
            SetProfitTarget("", CalculationMode.Percent, Target);

            CalculateOnBarClose = false;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
			if (CurrentBar < 2) return;
			if (Time[0].DayOfWeek != Time[1].DayOfWeek) // new day, reset bool flag
			{   alreadyTradedToday = 0; }
			
            // Condition set 1
            if ((Low[BarsToWait] < High[dropTime+BarsToWait] - dropSize) 
				&& (GetCurrentAsk() > Low[BarsToWait]) 
				&& alreadyTradedToday < 1 
				&& (MFI(MFILength)[0] > 40 || CrossAbove(MFI(MFILength), 20, BarsToWait)))
            {
				alreadyTradedToday += 1;
                EnterLong(UnitsToBuy, "");
            }
        }

        #region Properties
        [Description("Drop before buy")]
        [GridCategory("Parameters")]
        public double DropSize
        {
            get { return dropSize; }
            set { dropSize = Math.Max(0.100, value); }
        }

        [Description("Drop time")]
        [GridCategory("Parameters")]
        public int DropTime
        {
            get { return dropTime; }
            set { dropTime = Math.Max(1, value); }
        }

        [Description("How many to buy")]
        [GridCategory("Parameters")]
        public int UnitsToBuy
        {
            get { return unitsToBuy; }
            set { unitsToBuy = Math.Max(1, value); }
        }

        [Description("Bars to wait before buy")]
        [GridCategory("Parameters")]
        public int BarsToWait
        {
            get { return barsToWait; }
            set { barsToWait = Math.Max(1, value); }
        }

        [Description("MFI Param")]
        [GridCategory("Parameters")]
        public int MFILength
        {
            get { return mfiLength; }
            set { mfiLength = Math.Max(1, value); }
        }

        [Description("Profit target percent")]
        [GridCategory("Parameters")]
        public double Target
        {
            get { return target; }
            set { target = Math.Max(0.002, value); }
        }

        [Description("Stop loss percent")]
        [GridCategory("Parameters")]
        public double Stop
        {
            get { return stop; }
            set { stop = Math.Max(0.001, value); }
        }
        #endregion
    }
}
