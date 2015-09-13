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
    /// Sell after big Rise
    /// </summary>
    [Description("Sell after big Rise With Small Target")]
    public class SellAfterBigRiseWithSmallTarget : Strategy
    {
        #region Variables
        // Wizard generated variables
        private double riseSize = 7.00; // Default setting for RiseSize
        private int riseTime = 7; // Default setting for RiseTime
        private int unitsToSell = 1; // Default setting for UnitsToSell
        private double target = 0.004; // Default setting for Target
        private double stop = 0.02; // Default setting for Stop
		private int barsToWait = 1; // Number of bars to wait after Rise
		private int mfiLength = 18; // MFI parameter
        // User defined variables (add any user defined variables below)
		int alreadyTradedToday = 0;
		private IOrder entryOrder = null;
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
            if ((High[BarsToWait] > Low[RiseTime+BarsToWait] + RiseSize) 
				&& (GetCurrentAsk() < High[BarsToWait]) 
				&& alreadyTradedToday < 1 
				&& (MFI(MFILength)[0] < 60 || CrossBelow(MFI(MFILength), 80, BarsToWait))
				)
            {
				alreadyTradedToday += 1;
                entryOrder = EnterShort(UnitsToSell, "BigRiseSmallTargets");
            }
			
			if (Position.MarketPosition == MarketPosition.Short && BarsSinceEntry() > 300)
			{
				ExitShort();
			}
        }

		protected override void OnExecution(IExecution execution)
		{
			if (entryOrder != null && entryOrder == execution.Order)
				SendMail("naren.salem+auto@gmail.com", "naren.salem@gmail.com", "BigRiseSmallTargets", execution.ToString());
		}


        #region Properties
        [Description("Rise before Sell")]
        [GridCategory("Parameters")]
        public double RiseSize
        {
            get { return riseSize; }
            set { riseSize = Math.Max(0.100, value); }
        }

        [Description("Rise time")]
        [GridCategory("Parameters")]
        public int RiseTime
        {
            get { return riseTime; }
            set { riseTime = Math.Max(1, value); }
        }

        [Description("How many to Sell")]
        [GridCategory("Parameters")]
        public int UnitsToSell
        {
            get { return unitsToSell; }
            set { unitsToSell = Math.Max(1, value); }
        }

        [Description("Bars to wait before Sell")]
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
