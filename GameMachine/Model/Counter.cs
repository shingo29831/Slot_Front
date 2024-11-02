using System;
using static Constants;

namespace GameMachine.Model
{


	public class Counter
	{
		private static int bigCount = 0;
		private static int regCount = 0;
		private static int betweenBonusCount = 0;
		private static bool isGetOutBonus = false;
		private static bool isGetInBonus = false;

        //カウントアップ
        public static void CountUpCounterData(Roles establishedRole)
		{
			switch (establishedRole)
			{
				case Roles.BIG:
					CountUpBigCnt();
					isGetInBonus = true;
					break;

				case Roles.REGULAR:
					CountUpRegCnt();
					isGetInBonus = true;
					break;

				default:
					if (Game.GetInBonus() == false && isGetInBonus)
					{
						isGetInBonus = false;
						isGetOutBonus = true;
                        ResetBetweenBonusCnt();
                    }
					else if(Game.GetInBonus() == false)
					{
						CountUpBetweenBonusCnt();
					}
                    break;

            }
		}

		public static void CountUpBigCnt() { bigCount++; } //ボーナスカウントを増やす時ボーナス間のカウンターを0にする

		public static void CountUpRegCnt() { regCount++; } //ボーナスカウントを増やす時ボーナス間のカウンターを0にする

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
