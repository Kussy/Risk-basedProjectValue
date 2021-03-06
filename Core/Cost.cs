﻿using System.Diagnostics.Contracts;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>支出</summary>
    public class Cost : Money
    {
        /// <summary>プライベートコンストラクタ</summary>
        protected Cost() : base() { }

        /// <summary>基底クラスからのキャスト用のコンストラクタ</summary>
        /// <param name="money">金銭</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        protected static Cost Of(Money money)
        {
            return new Cost()
            {
                Value = money.Value,
            };
        }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <param name="value">値</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public new static Cost Of(decimal value = 0m)
        {
            Contract.Requires(value >= 0);

            return Of(Money.Of(value));
        }
    }
}