using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopCalculator
{
    class ExpressionHandler
    {
        private List<Token> expression = new List<Token>();
        private Token lastToken = null; // null as placeholder for the placeholder
        
        public ExpressionHandler()
        {

        }

        public bool IsLastTokenANumber()
        {
            return lastToken.Equals(Token.TokenTypes.number);

        }

        public string PrintExpression()
        {
            string result = "";
            for(int i = 0; i < expression.Count; i++)
            {
                if(expression[i].IsNumber())
                {
                    Number num = (Number)expression[i];
                    //result += num.ConvertNumberToDecimal();
                    result += num.ConvertNumberToString();
                }
                
                else if(expression[i].IsErr())
                {
                    result += ((Err)expression[i]).Message();
                }
                else
                {
                    result += expression[i].ConvertToString();
                }

            }
            return result;
        }

        public bool AddToken(Token t)
        {
            int lastIndex = expression.Count - 1;
            bool lastTokenIsNum = (expression.Count > 0) && expression[lastIndex].IsNumber();
            if (t.IsNumber() && lastTokenIsNum)
            {
                
                return ((Number)expression[lastIndex]).AddDigitToNumber((Number)(t));
                
            }
            else if (t.IsNumber())
            {
                expression.Add((Number)t);
                return true;
            }
            else
            {
                
                expression.Add(t);
                return true;
            }
        }
        
        public Number EvalOperation(Token.TokenTypes op, Number num1, Number num2)
        {
            decimal output = 0;
            switch(op)
            {
                case Token.TokenTypes.power:
                    output = (decimal) Math.Pow( (double)num1.ConvertNumberToDecimal(), (double)num2.ConvertNumberToDecimal());
                    break;
                case Token.TokenTypes.plus:
                    output = num1.ConvertNumberToDecimal() + num2.ConvertNumberToDecimal();
                    break;
                case Token.TokenTypes.minus:
                    output = num1.ConvertNumberToDecimal() - num2.ConvertNumberToDecimal();
                    break;
                case Token.TokenTypes.multiply:
                    output = num1.ConvertNumberToDecimal() * num2.ConvertNumberToDecimal();
                    break;
                case Token.TokenTypes.divide:
                    output = num1.ConvertNumberToDecimal() / num2.ConvertNumberToDecimal();
                    break;
                
            }
            return new Number(output);
            throw new NotImplementedException("get back to work");

        }
        public Number EvalOperation(Token.TokenTypes op, Number num1)
        {
            decimal output = 0;
            switch (op)
            {
                case Token.TokenTypes.minus:
                    output = -num1.ConvertNumberToDecimal();
                    break;
                case Token.TokenTypes.sin:
                    output = (decimal)Math.Sin((double)num1.ConvertNumberToDecimal());
                    break;
                case Token.TokenTypes.cos:
                    output = (decimal)Math.Cos((double)num1.ConvertNumberToDecimal());
                    break;
                case Token.TokenTypes.tan:
                    output = (decimal)Math.Tan((double)num1.ConvertNumberToDecimal());
                    break;
                case Token.TokenTypes.cot:
                    output = 1/(decimal)Math.Tan((double)num1.ConvertNumberToDecimal());
                    break;
                case Token.TokenTypes.ln:
                    output = (decimal)Math.Log((double)num1.ConvertNumberToDecimal());
                    break;
                case Token.TokenTypes.log10:
                    output = (decimal)Math.Log10((double)num1.ConvertNumberToDecimal());
                break;
            }
            return new Number(output);
        }
        public decimal EvaluateExpression()
        {
            try
            {
                Queue<Token> output = new Queue<Token>();
                Stack<Token> operatorStack = new Stack<Token>();
                decimal ret = (decimal)0.0;
                int len = expression.Count;
                int i = 0;
                while (i < len)
                {

                    if (expression[i].IsNumber())
                    {
                        output.Enqueue(expression[i]);
                    }
                    //if(expression[i].)
                    else if (expression[i].IsFunction())
                    {
                        operatorStack.Push(expression[i]);
                    }
                    else if (expression[i].IsOperator())
                    {
                        while (operatorStack.Count != 0 && operatorStack.Peek().GetTokenType() != Token.TokenTypes.openparen && operatorStack.Peek().GetTokenType() != Token.TokenTypes.openbracket)
                        {
                            if (operatorStack.Peek() == null)
                            {
                                throw new Exception("mismatch paren/brackets");
                            }
                            output.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Push(expression[i]);
                        // left paren
                        if (operatorStack.Count != 0)
                        {
                            if (operatorStack.Peek().GetTokenType() == Token.TokenTypes.openparen)
                            {
                                operatorStack.Pop(); // discard paren/bracket
                            }
                            if (operatorStack.Peek().IsFunction())
                            {
                                output.Enqueue(operatorStack.Pop());
                            }
                        }
                    }
                    i++;
                }
                while (operatorStack.Count != 0)
                {
                    if (operatorStack.Peek().IsParenBracket())
                    {
                        throw new Exception("mismatch paren/brackets");
                    }
                    output.Enqueue(operatorStack.Pop());
                }
                expression = new List<Token>();
                //expression.Add(new Number(ret));
                Stack<Token> evaluation = new Stack<Token>();
                expression = output.ToList<Token>();
                for (i = 0; i < expression.Count; i++)
                {
                    if (expression[i].IsNumber())
                    {
                        evaluation.Push(expression[i]);
                    }
                    else if(expression[i].IsFunction() ) 
                    {
                        Number r = (Number)evaluation.Pop();
                        evaluation.Push(EvalOperation(expression[i].GetTokenType(), r));
                    }
                    else
                    {
                        Number r = (Number)evaluation.Pop();
                        Number l = (Number)evaluation.Pop();
                        evaluation.Push(EvalOperation(expression[i].GetTokenType(), l, r));
                    }
                }
                expression = evaluation.ToList<Token>();
                return ret;
            }
            catch (Exception e)
            {
                expression = new List<Token>();
                expression.Add(new Err());
                return 0;
            }
            
            }

    }
}
