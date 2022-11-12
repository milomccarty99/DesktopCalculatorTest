using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopCalculator
{
    class Token
    {
        protected TokenTypes tokenType;
        public Token(TokenTypes tokenT)
        {
            this.tokenType = tokenT;
        }

        public TokenTypes GetTokenType()
        {
            return this.tokenType;
        }
        public bool IsErr()
        {
            return tokenType.Equals(TokenTypes.err);
        }
        public bool IsNumber()
        {
            return tokenType.Equals(TokenTypes.number);
        }
        public bool IsFunction()
        {
            switch(tokenType)
            {
                case TokenTypes.sin:
                    return true;
                case TokenTypes.cos:
                    return true;
                case TokenTypes.tan:
                    return true;
                case TokenTypes.cot:
                    return true;
                case TokenTypes.ln:
                    return true;
                case TokenTypes.log10:
                    return true;
                default:
                    return false;
            }
        }
        public bool IsOperator()
        {
            return !(tokenType == TokenTypes.openparen || tokenType == TokenTypes.closedparen 
                || tokenType == TokenTypes.openbracket || tokenType == TokenTypes.closedbracket);
        }
        public bool IsParenBracket()
        {
            switch(tokenType)
            {
                case TokenTypes.openparen:
                    return true;
                case TokenTypes.openbracket:
                    return true;
                case TokenTypes.closedparen:
                    return true;
                case TokenTypes.closedbracket:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsLeftAssociative()
        {
            switch (tokenType)
            {
                case TokenTypes.multiply:
                    return true;
                case TokenTypes.divide:
                    return true;
                case TokenTypes.plus:
                    return true;
                case TokenTypes.minus:
                    return true;
                default:
                    return false;
            }

        }
        public int GetPrecedence()
        {
            switch(tokenType)
            {
                case TokenTypes.plus:
                    return 2;
                case TokenTypes.minus:
                    return 2;
                case TokenTypes.multiply:
                    return 3;
                case TokenTypes.divide:
                    return 3;
                case TokenTypes.power:
                    return 4;
                default:
                    return int.MaxValue;

            }
        }
        public string ConvertToString()
        {
            switch(tokenType)
            {
                case TokenTypes.number:
                    throw new NotImplementedException("this code should not have been reached");
                    break;
                case TokenTypes.openparen:
                    return "(";
                    break;
                case TokenTypes.closedparen:
                    return ")";
                    break;
                case TokenTypes.openbracket:
                    return "{";
                    break;
                case TokenTypes.closedbracket:
                    return "}";
                    break;
                case TokenTypes.plus:
                    return "+";
                    break;
                case TokenTypes.minus:
                    return "-";
                    break;
                case TokenTypes.multiply:
                    return "*";
                    break;
                case TokenTypes.divide:
                    return "/";
                    break;
                case TokenTypes.power:
                    return "^";
                    break;
                case TokenTypes.sin:
                    return "sin";
                    break;
                case TokenTypes.cos:
                    return "cos";
                    break;
                case TokenTypes.tan:
                    return "tan";
                    break;
                case TokenTypes.cot:
                    return "cot";
                    break;
                case TokenTypes.ln:
                    return "ln";
                    break;
                case TokenTypes.log10:
                    return "log10"; // so far, i don't know how to make the 10 a subscript, but this should be fiiiiiine.
                    break;
                   
            }
            throw new NotImplementedException("Unexpected token reached");
        }
        public enum TokenTypes
        {
            number,
            openparen,
            closedparen,
            openbracket,
            closedbracket,
            plus,
            minus,
            multiply,
            divide,
            power,
            sin,
            cos,
            tan,
            cot,
            ln,
            log10,
            err
        }
    }
    class Err:Token
    {
        
        public Err(): base(TokenTypes.err)
        {

        }
        public string Message()
        {
            return "err";
        }
    }
    class Number:Token
    {
        List<Digits> numb = new List<Digits>();
        bool hasDecimal = false;
        public Number() : base(TokenTypes.number)
        {

        }
        public Number (Digits dig) : base (TokenTypes.number)
        {
            numb.Add(dig);
        }
        public Number (decimal deci) : base(TokenTypes.number)
        {
            string parseNumber = deci + ""; // convert decimal to string
            for(int i = 0; i < parseNumber.Length; i++)
            {
                Digits digi = ConvertCharToDigit(parseNumber[i]);
                numb.Add(digi);
            }
        }
        public enum Digits
        {
            one,
            two,
            three,
            four,
            five,
            six,
            seven,
            eight,
            nine,
            zero,
            decimalpoint,
            negative
        }
        private Digits ConvertCharToDigit(char c)    
        {
            switch (c)
            {
                case '1':
                    return Digits.one;
                case '2':
                    return Digits.two;
                case '3':
                    return Digits.three;
                case '4':
                    return Digits.four;
                case '5':
                    return Digits.five;
                case '6':
                    return Digits.six;
                case '7':
                    return Digits.seven;
                case '8':
                    return Digits.eight;
                case '9':
                    return Digits.nine;
                case '0':
                    return Digits.zero;
                case '.':
                    return Digits.decimalpoint;
                case '-':
                    return Digits.negative;
            }

            throw new NotImplementedException();
        }
        private char ConvertDigitsToChar(Digits dig)
        {
            switch(dig)
            {
                case Digits.one:
                    return '1';
                    break;
                case Digits.two:
                    return '2';
                    break;
                case Digits.three:
                    return '3';
                    break;
                case Digits.four:
                    return '4';
                    break;
                case Digits.five:
                    return '5';
                    break;
                case Digits.six:
                    return '6';
                    break;
                case Digits.seven:
                    return '7';
                    break;
                case Digits.eight:
                    return '8';
                    break;
                case Digits.nine:
                    return '9';
                    break;
                case Digits.zero:
                    return '0';
                    break;
                case Digits.decimalpoint:
                    return '.';
                    break;
                case Digits.negative:
                    return '-';
                    break;
                
            }

            throw new NotImplementedException("Unexpected digit was passed");

        }

        public decimal ConvertNumberToDecimal()
        {
            string parsenumb = ConvertNumberToString();
            decimal result;
            
            bool success = Decimal.TryParse(parsenumb, out result);
            if(!success)
            {
                throw new Exception("Number could not be parsed into a decimal");
            }
            return result;
        }
        public string ConvertNumberToString()
        {
            string parsenumb = "";
            for (int i = 0; i < numb.Count; i++)
            {
                parsenumb += ConvertDigitsToChar(numb[i]);
            }
            return parsenumb;

        }
        public bool AddDigitToNumber(Digits dig)
        {
            if (hasDecimal && dig.Equals(Digits.decimalpoint))
            {
                // if there is already a decimal point, we do not want to add another one to our number we are building
                return false;
            }

            numb.Add(dig);
            if(dig.Equals(Digits.decimalpoint))
            {
                hasDecimal = true;
            }
            //hasDecimal = dig.Equals(Digits.decimalpoint); // we can write this line this way since there is only one decimal point
            return true;
        }

        public bool AddDigitToNumber(Number num)
        {
            bool canAdd = true;
            if(!num.hasDecimal || !this.hasDecimal)
            {
                if(num.hasDecimal)
                {
                    this.hasDecimal = true; // making sure the decimal points are accounted for.
                }
                for(int i = 0; i < num.numb.Count; i++)    
                {
                    Digits digToAdd = num.numb[i];
                    AddDigitToNumber(digToAdd);
                    
                }
            }
            else
            {
                return false;
            }
            return canAdd; 
        }

    }
}
