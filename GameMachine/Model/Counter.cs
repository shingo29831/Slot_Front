using System;
using static Constants;

namespace GameMachine.Model
{


	public class Counter
	{
		private static int bigCount = 0;
		private static int regCount = 0;
		private static int betweenBonusCount = 0;

        //カウントアップ
        public static void CountUpCounterData(Roles establishedRole)
		{
			switch (establishedRole)
			{
				case Roles.BIG:
					CountUpBigCnt();
					break;

				case Roles.REGULAR:
					CountUpRegCnt();
					break;

				default:
					CountUpBetweenBonusCnt();
                    break;

            }
		}

		public static void CountUpBigCnt() { bigCount++; ResetBetweenBonusCnt(); } //ボーナスカウントを増やす時ボーナス間のカウンターを0にする

		public static void CountUpRegCnt() { regCount++; ResetBetweenBonusCnt(); } //ボーナスカウントを増やす時ボーナス間のカウンターを0にする

        public static void CountUpBetweenBonusCnt() { betweenBonusCount++; }

		//取得

		public static int GetBigCnt() { return bigCount; }

		public static int GetRegCnt() { return regCount; }

		public static int GetBetweenBonusCnt() { return betweenBonusCount; }


		//集計リセット

		public static void ResetBigCnt() { bigCount = 0; }

		public static void ResetRegCnt() { regCount = 0; }

		public static void ResetBetweenBonusCnt() { betweenBonusCount = 0; }

		public static void ResetCounter()
		{
			ResetBigCnt();
			ResetRegCnt();
			ResetBetweenBonusCnt();
		}
	}

}
