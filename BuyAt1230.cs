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
    /// Just buy at a specific time every day!
    /// </summary>
    [Description("Just buy at specific time every day!")]
    public class BuyAtSpecificTime : Strategy
    {
        #region Variables
        // Wizard generated variables
        private int stopLossAmount = 50; // Default setting for StopLoss
        private int profitTargetAmount = 100; // Default setting for ProfitTarget
        // User defined variables (add any user defined variables below)
		private int purchaseTime = 135900; // Testing email - better time is 12:30 pm
		private int quantity = 1;
		private IOrder entryOrder = null;
		#endregion

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
			SetProfitTarget("BuyAtSpecificTime", CalculationMode.Ticks, ProfitTargetAmount);
			SetStopLoss("BuyAtSpecificTime", CalculationMode.Ticks, StopLossAmount, false);

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
                entryOrder = EnterLong(Quantity, "BuyAtSpecificTime");
			}
        }

		protected override void OnExecution(IExecution execution)
		{
			if (entryOrder != null && entryOrder == execution.Order)
				SendMail("naren.salem+auto@gmail.com", "naren.salem@gmail.com", "BuyAtSpecificTime", execution.ToString());
		}

		

        #region Properties
        [Description("")]
        [GridCategory("Parameters")]
        public int StopLossAmount
        {
            get { return stopLossAmount; }
            set { stopLossAmount = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int ProfitTargetAmount
        {
            get { return profitTargetAmount; }
            set { profitTargetAmount = Math.Max(1, value); }
        }

        [Description("")]
        [GridCategory("Parameters")]
        public int PurchaseTime
        {
            get { return purchaseTime; }
            set { purchaseTime = Math.Max(093000, value); }
        }
		
		[Description("")]
		[GridCategory("Parameters")]
		public int Quantity
		{
			get { return quantity; }
			set { quantity = Math.Max(1, value); }
		}
		#endregion
    }
}
