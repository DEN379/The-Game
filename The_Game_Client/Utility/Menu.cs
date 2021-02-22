using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace The_Game_Client.Utility
{
    class Menu
    {
        
        private int selectedIndex;
        private string[] options;
        private string prompt;

        public Menu(string prompt, string[] options)
        {
            this.prompt = prompt;
            this.options = options;
            selectedIndex = 0;
        }

        public void DisplayOptions()
        {
            string prefix;
            WriteLine(prompt);
            for (int i = 0; i < options.Length; i++)
            {
                string current = options[i];

                if (i == selectedIndex)
                {
                    prefix = "*";
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = " ";
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }
                WriteLine($"{prefix} << {current} >>");
            }
            ResetColor();
        }


        public int Run()
        {
            ConsoleKey key;
            do
            {
                Clear();
                //if (isScene) WriteLine(scene.PrintScene());
                DisplayOptions();

                ConsoleKeyInfo info = ReadKey(true);
                key = info.Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex == -1) selectedIndex = options.Length - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex == options.Length) selectedIndex = 0;
                }

            } while (key != ConsoleKey.Enter);
            return selectedIndex;
        }
    
    }
}
