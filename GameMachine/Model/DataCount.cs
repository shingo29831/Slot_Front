using System;


namespace Model;

public class DataCount
{
	private static int bigBonusCount = 0;
	private static int regularBonusCount = 0;
	private static int betweenBonus = 0;

	public static void BigBonusCountup(){ bigBonusCount++; }

	public static void RegularBonusCountup(){ regularBonusCount++; }

	public static void BetweenBonusup() { betweenBonus++; }


	public static int GetBigBonusCount() { return bigBonusCount; }

	public static int GetRegularBonusCount() {return regularBonusCount; }

	public static int GetBetweenBonusCount() { return betweenBonus; }


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
