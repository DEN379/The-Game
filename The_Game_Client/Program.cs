using System;
using The_Game_Client.Utility;

namespace The_Game_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string logo = @" _____ _            _____                      
|_   _| |          |  __ \                     
  | | | |__   ___  | |  \/ __ _ _ __ ___   ___ 
  | | | '_ \ / _ \ | | __ / _` | '_ ` _ \ / _ \
  | | | | | |  __/ | |_\ \ (_| | | | | | |  __/
  \_/ |_| |_|\___|  \____/\__,_|_| |_| |_|\___|
                                               
                                               ";
            string[] options = new string[] { "Login", "Registration", "Exit" };
            Menu menu = new Menu(logo, options);
            menu.Run();
        }
    }
}
