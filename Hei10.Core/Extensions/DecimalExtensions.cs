using System;

namespace Hei10.Core.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToMoney(this decimal money)
        {
            const int limit = 10000;
            if (money > limit)
            {
                money = Math.Round(money / limit, 0);
                return string.Format("{0}万", money.ToString("0"));
            }
            return money.ToString("0");
        }

        /// <summary>
        /// 显示为美金
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string ToDollar(this decimal money)
        {
            return money.ToString("#,#");
        }

        /// <summary>
        /// 显示为美金
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string ToDollarDigit(this decimal money)
        {
            return money.ToString("#,0.00");
        }

    }
}