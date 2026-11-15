using RegexSearch.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexSearch.Core.Services
{
    public class RecognitionService
    {
        public const string SnilsPattern = @"\b\d{3}-\d{3}-\d{3}\s\d{2}\b";
        public List<MatchResult> SearchByRegex(string text)
        {
            var results = new List<MatchResult>();
            if (string.IsNullOrEmpty(text))
                return results;
            MatchCollection matches = Regex.Matches(text, SnilsPattern);
            int counter = 1;
            foreach (Match match in matches)
            {
                results.Add(new MatchResult
                {
                    Number = counter++,
                    Value = match.Value,
                    Position = match.Index,
                    Length = match.Length
                });
            }
            return results;
        }
        public List<MatchResult> SearchByAutomation(string text)
        {
            var results = new List<MatchResult>();
            if (string.IsNullOrEmpty(text))
                return results;
            int i = 0;
            int counter = 1;
            while (i < text.Length)
            {
                if (i > 0 && IsWordChar(text[i - 1]))
                {
                    i++;
                    continue;
                }
                State currentState = State.Start;
                int j = i;
                while (j < text.Length && currentState != State.Accept && currentState != State.Error)
                {
                    currentState = GetNextState(currentState, text[j]);
                    j++;
                }
                if (currentState == State.Accept)
                {
                    bool isRightBoundaryValid = (j >= text.Length) || !IsWordChar(text[j]);
                    if (isRightBoundaryValid)
                    {
                        int length = j - i;
                        results.Add(new MatchResult
                        {
                            Number = counter++,
                            Value = text.Substring(i, length),
                            Position = i,
                            Length = length
                        });
                        i = j;
                        continue;
                    }
                }
                i++;
            }
            return results;
        }
        private State GetNextState(State currentState, char currentChar)
        {
            switch (currentState)
            {
                case State.Start:
                    if (char.IsDigit(currentChar))
                        return State.Digits;
                    break;
                case State.Digits:
                    if (char.IsDigit(currentChar))
                        return State.Digits;
                    else if (currentChar == '-')
                        return State.Hyphen;
                    break;
                case State.Hyphen:
                    if (char.IsDigit(currentChar))
                        return State.Digits1;
                    break;
                case State.Digits1:
                    if (char.IsDigit(currentChar))
                        return State.Digits1;
                    else if (currentChar == '-')
                        return State.Hyphen1;
                    break;
                case State.Hyphen1:
                    if (char.IsDigit(currentChar))
                        return State.Digits2;
                    break;
                case State.Digits2:
                    if (char.IsDigit(currentChar))
                        return State.Digits2;
                    else if (currentChar == ' ')
                        return State.Space;
                    break;
                case State.Space:
                    if (char.IsDigit(currentChar))
                        return State.Digits3;
                    break;
                case State.Digits3:
                    if (char.IsDigit(currentChar))
                        return State.Accept;
                    break;
            }
            return State.Error;
        }
        private bool IsAsciiDigit(char c) => c >= '0' && c <= '9';
        private bool IsWordChar(char c) => char.IsLetterOrDigit(c) || c == '_';
    }
}
