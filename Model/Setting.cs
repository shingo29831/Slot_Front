using GameMachine;
using System;
using System.Data;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace Model;
public class Setting: BusinessLogic
{
	static String tableID = "info";
    static String userID = "";
    static int expected = 0;
    static int bonusProbability = 50; //最大100%での表記
    static int[] symbolsWeight = {8,5,4,4,1};
    //ベル,リプレイ,スイカ,チェリー,7の順で入れること
  


    public static void setUserID(String userID) { Setting.userID = userID; }
    public static String getUserID() { return Setting.userID; }

    public static void maketableID()
    {
        byte[] hashValue = generateHash(getMacAddress() + getProductID());

        // ハッシュ値を表示
        tableID = BitConverter.ToString(hashValue).Replace("-", "");
    }

    public static void setTableID(String tableID)
	{
        Setting.tableID = tableID;
    }

    public static String getTableID()
    {
        return Setting.tableID;
    }

    public static byte[] generateHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }

    private static string getMacAddress()
    {
        string macAddress = string.Empty;

        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            // 有効であることを確認
            if (nic.OperationalStatus == OperationalStatus.Up)
            {
                macAddress = nic.GetPhysicalAddress().ToString();
                break;
            }
        }

        return macAddress;
    }

    private static string getProductID()
    {
        var processInfo = new ProcessStartInfo("cmd.exe", "/c " + "wmic os get serialnumber")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            WorkingDirectory = @"C:\Windows\System32\"
        };

        StringBuilder sb = new StringBuilder();
        Process p = Process.Start(processInfo);
        p.OutputDataReceived += (sender, args_) => sb.AppendLine(args_.Data);
        p.BeginOutputReadLine();
        p.WaitForExit();

        return sb.ToString();
    }

    public static void setBonusProbability(int bonusProbability) { Setting.bonusProbability = bonusProbability; }
    public static int getBonusProbability() {  return Setting.bonusProbability; }

    public static void weightingSymbol(int expected) //重み付け
    {
        switch (expected)
        {
            case 0:
                symbolsWeight[BELL] = 1;
                symbolsWeight[REPLAY] = 2;
                symbolsWeight[WATERMELON] = 1;
                symbolsWeight[CHERRY] = 1;
                break;

            case 1:
                symbolsWeight[BELL] = 1;
                symbolsWeight[REPLAY] = 2;
                symbolsWeight[WATERMELON] = 1;
                symbolsWeight[CHERRY] = 1;
                break;

            case 2:
                symbolsWeight[BELL] = 1;
                symbolsWeight[REPLAY] = 2;
                symbolsWeight[WATERMELON] = 1;
                symbolsWeight[CHERRY] = 1;
                break;

            case 3:
                symbolsWeight[BELL] = 1;
                symbolsWeight[REPLAY] = 2;
                symbolsWeight[WATERMELON] = 1;
                symbolsWeight[CHERRY] = 1;
                break;

            case 4:
                symbolsWeight[BELL] = 1;
                symbolsWeight[REPLAY] = 2;
                symbolsWeight[WATERMELON] = 1;
                symbolsWeight[CHERRY] = 1;
                break;

            case 5:
                symbolsWeight[BELL] = 1;
                symbolsWeight[REPLAY] = 2;
                symbolsWeight[WATERMELON] = 1;
                symbolsWeight[CHERRY] = 1;
                break;


        }
    } 
}
