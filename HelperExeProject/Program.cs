using System;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace VRUniverse_Helper
{
    public class ProcessHelper
    {

        //takes 3 arguments: first application's datapath, second application's datapath, and a delay before process starting (in milliseconds)
        static void Main(string[] args)
        {
            foreach(string arg in args)
            {
                if(arg.StartsWith("-h"))
                {
                    Console.WriteLine("Usage: VRUniverse_Helper.exe [Target Executable] [Return Executable] [Sleep Time]");
                    Console.WriteLine("Summary: VRUniverse_Helper waits a short period of time, then starts up one executable, waits for that to finish, then starts a second executable.");
                    Console.WriteLine("Target Executable: Path to the first executable to start.");
                    Console.WriteLine("Return Executable: Path to the second executable to start.");
                    Console.WriteLine("Sleep Time: Amount of time (in milliseconds) to wait before starting the first executable and to wait after the first executable finishes before starting the second.");
                    Console.WriteLine("Note: This executable is intended for use by the Virtual Reality application VR Universe. While this executable is designed to be as robust as possible, its usage for other purposes is not supported.");
                    return;
                }
            }

            if(args.Length < 3)
            {
                try
                {
                    string vruniverse_exe_name;
                    if (File.Exists("VRUniverse_Vive.exe")) vruniverse_exe_name = "VRUniverse_Vive.exe";
                    else if (File.Exists("VRUniverse_Oculus.exe")) vruniverse_exe_name = "VRUniverse_Oculus.exe";
                    else if (File.Exists("VRUniverse.exe")) vruniverse_exe_name = "VRUniverse.exe";
                    else
                    {
                        Console.WriteLine("Error: Could not find any VRUniverse executable");
                        WaitForInput();
                        return;
                    }

                    Console.WriteLine("Too few arguments, starting " + vruniverse_exe_name);
                    Process.Start(vruniverse_exe_name);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error executing VRUniverse.exe: \n" + e.Message);
                    WaitForInput();
                    return;
                }
            }

            int sleepTime = 0;
            try
            {
                sleepTime = (args.Length >= 3) ? Int32.Parse(args[2]) : 500; //sleep time, measured in milliseconds; defaults to 500 ms
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in third parameter, \"Sleep Time\": \n" + args[2] + "\n" + e.Message);
                WaitForInput();
                return;
            }

            try
            {
                Thread.Sleep(sleepTime); //waits before starting the first application
                Process.Start(args[0]).WaitForExit(); //starts up the first application, then waits for it to close
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in first parameter, \"Target Executable\": \n" + args[0] + "\n" + e.Message);
                WaitForInput();
                return;
            }

            try
            {
                Thread.Sleep(sleepTime); //waits before starting the second application
                Process.Start(args[1]); //starts the second application
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in second parameter, \"Return Executable\": \n" + args[1] + "\n" +  e.Message);
                WaitForInput();
                return;
            }
        }


        static void WaitForInput()
        {
            Console.WriteLine("\n\nPress the 'Enter' or 'Return' key to close VRUniverse_Helper");
            Console.ReadKey();
            return;
        }
    }
}
