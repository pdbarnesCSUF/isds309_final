// Patrick Barnes
// ISDS309_Fa18_S80_Final_CreditCard_Barnes_Patrick
// Final
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;

namespace isdsfinal
{
    class ISDS309_Fa18_S80_Final_CreditCard_Barnes_Patrick
    {
        static void Main(string[] args)
        {
            //-------------------------var decs
            //file reading
            string dataPath = "CreditCardData.txt";
            string[,] inData = new string[150,6]; // 150, just to be safe (:
            int inDataRows = 0;
            string inLine = "";
            string[] inLineSplit;

            //control / input
            bool exit = false;
            bool status = false; //multi-purpose boolean i guess
            string choice = "";
            string strRedactCC = "";

            //bank reporting
            string[] reportBank = { "Bank of America", "Chase", "Citibank", "Wells Fargo" };
            int[,] reportNumbers = new int[4, 3];
            //card,limit,average

            //logging
            string logPath = "CreditCardLog.txt";
            FileStream logFile = new FileStream(logPath, FileMode.Append, FileAccess.Write);
            StreamWriter logWriter = new StreamWriter(logFile);
            //---------------------------read file
            //WriteLine("!!!!!DBG: READ FILE");
            FileStream inFile = new FileStream(dataPath, FileMode.Open, FileAccess.Read);
            StreamReader inStream = new StreamReader(inFile);
            //WriteLine("!!!!!DBG: READ FILE - 2");
            inLine = inStream.ReadLine(); //dump first line headers
            inLine = inStream.ReadLine(); //primed input
            while (inLine != null)
            {
                //WriteLine("!!!!!DBG: READ FILE - i - " + inDataRows);
                inLineSplit = inLine.Split(',');
                //WriteLine("!!!!!DBG:" + inLine);
                //WriteLine("!!!!!DBG:" + inLineSplit);
                if (inLineSplit[0] != "")
                {
                    for (int i = 0; i < 6; ++i)
                        inData[inDataRows, i] = inLineSplit[i];

                    ++inDataRows;
                }
                inLine = inStream.ReadLine(); //go for next line!
            }
            inStream.Close();
            inFile.Close();
            WriteLine("File Loaded");
            //test output
            //WriteLine("!!!!!DBG: OUT DATA");
            /*
            for (int i = 0; i < inDataRows; ++i)
            {
                for (int j = 0; j < 6; ++j)
                    Write(inData[i, j] + "|");
                WriteLine();
            }
            */
            // ******************
            // *      MENU      *
            // ******************
            while (!exit)
            {
                //------RESET VARS---------
                choice = "";
                status = false;


                WriteLine();
                WriteLine("--------MENU--------");
                WriteLine("ALL  : List All Info");
                WriteLine("CUST : Customer Info");
                WriteLine("CARD : Card Info");
                WriteLine("BANK : Bank Info");
                WriteLine("QUIT or Q : Exit Program");
                Write("Menu:");
                choice = ReadLine().ToUpper();
                switch (choice)
                {
                    //--------------------------
                    //  ALL   All Info
                    //--------------------------
                    case "ALL":
                        //all info
                        WriteLine("{0,3}{1,28}{2,20}{3,18}{4,25}{5,10}", "CC", "Card Type", "Bank Name", "Card Number", "Customer Name", "Limit");
                        for (int i = 0; i < inDataRows; ++i)
                        {
                            WriteLine("{0,3}{1,28}{2,20}{3,18}{4,25}{5,10}", 
                                inData[i, 0], 
                                inData[i, 1], 
                                inData[i, 2], 
                                inData[i, 3], 
                                inData[i, 4], 
                                inData[i, 5]);
                        }
                        //log
                        logWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd_hh:mm:ss") + ",ALL");
                        break;
                    //--------------------------
                    //  CUST  Customer Info
                    //--------------------------
                    case "CUST":
                        // customer info
                        status = false; //didnt find yet
                        Write("Enter Customer Name:");
                        choice = ReadLine();
                        for (int i = 0; i < inDataRows; ++i)
                        {
                            if (choice == inData[i,4])
                            {
                                //found him!
                                status = true;
                                //redact cc num
                                strRedactCC = inData[i, 3].Substring(inData[i, 3].Length-4);
                                WriteLine("{0,25}{1,8}{2,10}{3,20}{4,3}{5,28}",
                                    "Customer Name",
                                    "CC#",
                                    "Limit",
                                    "Bank Name",
                                    "CC",
                                    "Card Type");
                                WriteLine("{0,25}{1,8}{2,10}{3,20}{4,3}{5,28}", 
                                    inData[i, 4], 
                                    "**" + strRedactCC, //inData[i,3]
                                    inData[i, 5], 
                                    inData[i, 2], 
                                    inData[i, 0], 
                                    inData[i, 1]);
                            }//if match
                        }//for data
                        if (!status)
                        {
                            WriteLine("Customer Not Found!");
                        }
                        //log
                        logWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd_hh:mm:ss") + ",CUST");
                        break;//case cust
                    //--------------------------
                    //  CARD  Card Info
                    //--------------------------
                    case "CARD":
                        status = false; //didnt find yet
                        Write("Input Last 4 of card:");
                        choice = ReadLine();
                        for (int i = 0; i < inDataRows; ++i)
                        {
                            strRedactCC = inData[i, 3].Substring(inData[i, 3].Length - 4);
                            if (choice == strRedactCC)
                            {
                                //found it!
                                status = true;
                                WriteLine("{0,6}{1,10}{2,20}{3,3}{4,28}{5,25}",
                                    "CC#",
                                    "Limit",
                                    "Bank Name",
                                    "CC",
                                    "Card Type",
                                    "Customer Name");
                                WriteLine("{0,6}{1,10}{2,20}{3,3}{4,28}{5,25}",
                                    strRedactCC,
                                    inData[i, 5],
                                    inData[i, 2],
                                    inData[i, 0],
                                    inData[i, 1],
                                    inData[i, 4]);
                            }//if match
                        }//for data
                        if (!status)
                        {
                            WriteLine("Card Not Found!");
                        }
                        //log
                        logWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd_hh:mm:ss") + ",CARD");
                        break;//case card
                    //--------------------------
                    //  BANK  Bank Report
                    //--------------------------
                    case "BANK":
                        status = false; //valid bank choice
                        // Bank report
                        WriteLine("---Bank Report---");
                        //reset vars
                        reportNumbers = new int[4, 3]; //quick but very dirty way to 0 it out
                            
                        //search / calc
                        for (int i = 0; i < inDataRows; ++i)
                        {
                            switch (inData[i, 2])
                            {
                                case "Bank of America":
                                    ++reportNumbers[0, 0];
                                    reportNumbers[0, 1] += int.Parse(inData[i, 5]);
                                    break;
                                case "Chase":
                                    ++reportNumbers[1, 0];
                                    reportNumbers[1, 1] += int.Parse(inData[i, 5]);
                                    break;
                                case "Citibank":
                                    ++reportNumbers[2, 0];
                                    reportNumbers[2, 1] += int.Parse(inData[i, 5]);
                                    break;
                                case "Wells Fargo":
                                    ++reportNumbers[3, 0];
                                    reportNumbers[3, 1] += int.Parse(inData[i, 5]);
                                    break;
                                default:
                                    //nothing, skip
                                    break;
                            }
                        }
                        //calc average
                        for (int i = 0; i < 4; ++i)
                            reportNumbers[i, 2] = reportNumbers[i, 1] / reportNumbers[i, 0];
                        
                        //prep file...
                        string reportPath = "BankReport_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt";
                        FileStream reportFile = new FileStream(reportPath, FileMode.Create, FileAccess.Write);
                        StreamWriter reportStream = new StreamWriter(reportFile);

                        //output to screen/File
                        //headers first
                        WriteLine("{0,17}{1,10}{2,12}{3,12}", "Bank Name", "Ttl Cards", "Ttl Limit", "Avg Limit");
                        reportStream.WriteLine("{0,17}{1,10}{2,12}{3,12}", "Bank Name", "Ttl Cards", "Ttl Limit", "Avg Limit");
                        //rows
                        for (int i = 0; i < 4; ++i)
                        {
                            WriteLine("{0,17}{1,10}{2,12}{3,12}", reportBank[i], reportNumbers[i,0], reportNumbers[i,1], reportNumbers[i, 2]);
                            reportStream.WriteLine("{0,17}{1,10}{2,12}{3,12}", reportBank[i], reportNumbers[i, 0], reportNumbers[i, 1], reportNumbers[i, 2]);
                        }
                        //done with file
                        reportStream.Close();
                        reportFile.Close();
                            
                        //log
                        logWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd_hh:mm:ss") + ",BANK");
                        break;//case bank
                    //--------------------------
                    //  QUIT    exiting
                    //--------------------------
                    case "QUIT":
                    case "Q":
                    case "EXIT": //...just because
                        exit = true;
                        break;
                    default:
                        WriteLine("Invalid Choice");
                        break;
                }//switch choice
                
            }//while !exit
            logWriter.Close();
            logFile.Close();
            WriteLine("---END---");
        }
    }
}
