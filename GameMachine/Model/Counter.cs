using System;

namespace Model;

public class Counter
{
	private static int bigBonusCount = 0;
	private static int regularBonusCount = 0;
	private static int betweenBonus = 0;

	public static void UpBigBonusCount(){ bigBonusCount++; }

	public static void UpRegularBonusCount(){ regularBonusCount++; }

	public static void UpBetweenBonusCount() { betweenBonus++; }

	//

	public static int GetBigBonusCount() { return bigBonusCount; }

	public static int GetRegularBonusCount() {return regularBonusCount; }

	public static int GetBetweenBonusCount() { return betweenBonus; }


	//集計リセット

	public static void ResetBigBonusCount() { bigBonusCount = 0; }

	public static void ResetRegularBonusCount() { regularBonusCount = 0; }
	
	public static void ResetBetweenBonusCount() { betweenBonus = 0; }

	public static void ResetCounter()
	{
		ResetBigBonusCount();
		ResetRegularBonusCount();
		ResetBetweenBonusCount();
	}
}
