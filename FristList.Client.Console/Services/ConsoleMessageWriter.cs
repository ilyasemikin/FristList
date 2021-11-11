using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FristList.Client.Console.Message;
using FristList.Client.Console.Services.Static;

namespace FristList.Client.Console.Services
{
    public class ConsoleMessageWriter : IMessageWriter
    {
        private Regex _startColorRegex;
        private Regex _endColorRegex;

        public ConsoleMessageWriter()
        {
            _startColorRegex = new Regex(@"\[color:(?<color>.+?)\]");
            _endColorRegex = new Regex(@"\[\/color]");
        }
        
        private void WriteColored(string message, ConsoleColor color)
        {
            var oldColor  = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = oldColor;
        }
        
        public void WriteText(string text)
        {
            System.Console.Write(text);
        }

        public void WriteMessage(MessageNodeBase messageNode)
        {
            var input = messageNode.Construct();
            
            var startMatches = _startColorRegex.Matches(input);
            var endMatches = _endColorRegex.Matches(input);

            var matches = startMatches.Concat(endMatches)
                .OrderBy(m => m.Index);
            
            var startIndex = 0;
            var colors = new Stack<ConsoleColor>();
            colors.Push(System.Console.ForegroundColor);
            
            foreach (var match in matches)
            {
                var substr = input.Substring(startIndex, match.Index - startIndex);
                System.Console.ForegroundColor = colors.Peek();
                System.Console.Write(substr);

                var newColorName = match.Groups["color"].Value;
                if (newColorName != string.Empty)
                {
                    ConsoleColors.TryGetColor(newColorName, out var newColor);
                    colors.Push(newColor);
                }
                else
                    colors.Pop();
                
                startIndex = match.Index + match.Length;
            }

            var lastSubstr = input.Substring(startIndex, input.Length - startIndex);
            System.Console.ForegroundColor = colors.Peek();
            System.Console.WriteLine(lastSubstr);
        }

        public void WriteMessage(string message)
        {
            WriteColored(message, ConsoleColor.White);
        }

        public void WriteWarning(string warning)
        {
            WriteColored(warning, ConsoleColor.Yellow);
        }

        public void WriteError(string error)
        {
            WriteColored(error, ConsoleColor.Red);
        }
    }
}