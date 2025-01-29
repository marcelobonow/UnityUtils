using System.Collections.Generic;
using System.Globalization;
using UnityEngine.UI;

public static class StringUtils
{

    public class LinkPosition
    {
        public string text;
        public int startPosition;

        public LinkPosition(string text, int startPosition)
        {
            this.text = text;
            this.startPosition = startPosition;
        }
    }

    public class LinkFromWord
    {
        public int startPosition;
        public int endPosition;

        public LinkFromWord(int startPosition, int endPosition)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
        }
    }

    public static LinkFromWord GetLinkFromWordGetLinkFromWord(string text, int wordIndex)
    {

        if (wordIndex >= text.Length)
            return null;

        Dictionary<int, char> nextWordToHttps = new Dictionary<int, char>(){
            {0, 'h' },
            {1, 't' },
            {2, 't' },
            {3, 'p' },
        };

        HashSet<char> invalidCharacters = new HashSet<char>()
        {
            {' ' },  {'[' }, { ']'}, {'('}, {')'},
        };

        const int minPrefixLength = 7; /// 7 letras em http://

        var startLinkIndex = -1;
        var endLinkIndex = -1;
        for (int i = wordIndex; i >= 0; i--)
        {
            var letter = text[i];

            if (invalidCharacters.Contains(letter))
            {
                startLinkIndex = i + 1;
                break;
            }

            else if (i == 0)
            {
                startLinkIndex = i;
                break;
            }
        }

        for (int i = startLinkIndex; i < text.Length; i++)
        {
            var letter = text[i];

            ///Se o que falta para completar http:// (que são 7 letras) é menor do que falta, então ja quebra o loop;
            if (i + (minPrefixLength - (i - startLinkIndex)) >= text.Length)
                break;

            if ((i - startLinkIndex) < nextWordToHttps.Count)
            {
                if (letter != nextWordToHttps[i - startLinkIndex])
                    break;
            }

            else if ((i - startLinkIndex) == nextWordToHttps.Count)
            {
                if (letter == 's')
                {
                    i++;
                    letter = text[i];
                }

                if (letter == ':' && text[i + 1] == '/' && text[i + 2] == '/')
                    i += 2;
                else
                    break;
            }

            ///Fim da URL
            else if (invalidCharacters.Contains(letter) || i >= text.Length - 1)
            {
                endLinkIndex = i;
                ///Se não tiver no fim, ele vai ir até o espaço, que é um depois
                if (i < text.Length - 1 || invalidCharacters.Contains(letter))
                    endLinkIndex--;

                break;
            }
        }

        if (startLinkIndex >= 0 && endLinkIndex > 0)
            return new LinkFromWord(startLinkIndex, endLinkIndex);
        return null;
    }

    public static List<LinkPosition> GetLinks(this string text)
    {
        var links = new List<LinkPosition>();

        Dictionary<int, char> nextWordToHttps = new Dictionary<int, char>(){
            {0, 'h' },
            {1, 't' },
            {2, 't' },
            {3, 'p' },
        };

        HashSet<char> invalidCharacters = new HashSet<char>()
        {
            {' ' },  {'[' }, { ']'}, {'('}, {')'},
        };

        const int minPrefixLength = 7; /// 7 letras em http://

        string currentWord = "";
        bool isHttp = false;

        var startLinkIndex = 0;

        for (int i = 0; i < text.Length; i++)
        {
            var letter = text[i];

            ///Se o que falta para completar http:// (que são 7 letras) é menor do que falta, então ja quebra o loop;
            if (i + (minPrefixLength - currentWord.Length) >= text.Length)
                break;

            if (currentWord.Length < nextWordToHttps.Count)
            {
                if (letter != nextWordToHttps[currentWord.Length])
                {
                    isHttp = false;
                    currentWord = "";
                }
                else
                    currentWord += letter;
            }
            else if (isHttp && !invalidCharacters.Contains(letter) && i < text.Length - 1)
            {
                currentWord += letter;
            }
            ///É espaço
            else if (isHttp)
            {
                var charsToRemove = i - startLinkIndex + 1;
                ///Se não tiver no fim, ele vai ir até o espaço, que é um depois
                if (i < text.Length - 1 || invalidCharacters.Contains(letter))
                    charsToRemove--;

                var link = text.Substring(startLinkIndex, charsToRemove);
                links.Add(new LinkPosition(link, startLinkIndex));

                currentWord = "";
                isHttp = false;
                startLinkIndex = 0;
            }

            else if (currentWord.Length == nextWordToHttps.Count)
            {
                if (letter == 's')
                {
                    i++;
                    currentWord += letter;
                    letter = text[i];
                }

                if (letter == ':' && text[i + 1] == '/' && text[i + 2] == '/')
                {
                    currentWord += $"{letter}{text[i + 1]}{text[i + 2]}";
                    isHttp = true;
                    ///Como adicionamos 2 letras a frente do i atual sem avançar o i, devemos levar isso em conta
                    startLinkIndex = i + 3 - currentWord.Length;
                }
                else
                {
                    currentWord = "";
                    isHttp = false;
                }

            }
        }

        return links;
    }

    public static string RemoveLinks(this string text)
    {
        var links = text.GetLinks();
        int currentOffset = 0;

        foreach (var link in links)
        {
            text = text.Remove(link.startPosition - currentOffset, link.text.Length);
            currentOffset += link.text.Length;
        }

        return text;
    }

    public static string RemoveSpecialCharacters(this string text)
    {
        HashSet<char> characteresToRemove = new HashSet<char>()
        {
            { ','}, { '.'}, { '!' },
            {'/' }, {'\\'}, {'|'},
            {'`' }, {'~'}, {';' },
            {'{' }, {'}'}, {'_'},
            {'[' }, {']'}, {'^'},
            { '?' }, { '@'}, {'#'},
            { '&' }
        };
        for (int i = 0; i < text.Length; i++)
        {

            if (i >= text.Length - 3)
                break;

            var letter = text[i];
            var nextLetter = text[i + 1];
            var finalLetter = text[i + 2];

            if (letter != ' ')
            {
                if (finalLetter == ' ' || nextLetter != ' ')
                {
                    i++;
                    continue;
                }
                continue;
            }
            else if (characteresToRemove.Contains(nextLetter) && finalLetter == ' ')
            {
                text = text.Remove(i, 2);
                i--;
            }
        }

        return text;
    }

    /// Transforma de R$ 3.000,00 em 3.000,0 Reais
    public static string ExpandCurrency(this string text)
    {
        ///Como no minimo precisa ter R$ ,
        ///então no minimo precisa de 3 a frente
        for (int i = 0; i < text.Length - 1; i++)
        {
            var letter = text[i];
            if (letter == 'R' && text[i + 1] == '$')
            {
                if (i == text.Length - 2)
                    text = text.Replace("R$", "reais");
                else if (text[i + 2] == ' ')
                {
                    if (char.IsNumber(text[i + 3]))
                    {
                        ///Tendo o numero podemos saber como substituir o numero
                        string number = ExtractNumber(text, i + 3);
                        text = text.Remove(i, number.Length + 3);

                        var numberModified = number.Replace(".", ";").Replace(",", ".").Replace(";", ",");
                        if (decimal.TryParse(numberModified, NumberStyles.AllowDecimalPoint | NumberStyles.Number, NumberFormatInfo.CurrentInfo, out var numberAsDecimal))
                            number = NumberToWords(numberAsDecimal, true);

                        text = text.Insert(i, number);
                    }
                    else
                        text = text.Replace("R$", "reais");

                }
                else
                {
                    if (char.IsNumber(text[i + 2]))
                    {
                        ///Tendo o numero podemos saber como substituir o numero
                        string number = ExtractNumber(text, i + 2);
                        text = text.Remove(i, number.Length + 2);

                        var numberModified = number.Replace(".", ";").Replace(",", ".").Replace(";", ",");
                        if (decimal.TryParse(numberModified, NumberStyles.AllowDecimalPoint | NumberStyles.Number, NumberFormatInfo.CurrentInfo, out var numberAsDecimal))
                            number = NumberToWords(numberAsDecimal, true);

                        text = text.Insert(i, number);
                    }
                    else
                        text = text.Replace("R$", "reais");
                }
            }
        }
        return text;
    }

    public static string ExpandAllNumbers(this string text, bool isInBrazilianFormat = true)
    {
        string number = "";
        int lastDigitIndex = 0;
        for (int i = 0; i < text.Length; i++)
        {
            var letter = text[i];
            if (char.IsDigit(letter) || letter == ',' || (letter == '.' && i < text.Length - 1 && char.IsDigit(text[i + 1])) || letter == '-')
            {
                number += letter;
                lastDigitIndex = i;
            }
            else if (number != "")
            {
                ///Teria que verificar de alguma forma se está em portugues ou não para saber se inverte as vigulas com pontos
                string numberAsWords = "";
                if (isInBrazilianFormat)
                {
                    var numberModified = number.Replace(".", ";").Replace(",", ".").Replace(";", ",");
                    if (decimal.TryParse(numberModified, NumberStyles.AllowDecimalPoint | NumberStyles.Number, NumberFormatInfo.CurrentInfo, out var numberAsDecimalModified))
                        numberAsWords = NumberToWords(numberAsDecimalModified, false);
                }

                else if (decimal.TryParse(number, NumberStyles.AllowDecimalPoint | NumberStyles.Number, NumberFormatInfo.CurrentInfo, out var numberAsDecimal))
                    numberAsWords = NumberToWords(numberAsDecimal, false);

                if (numberAsWords != "")
                {
                    text = text.Remove(lastDigitIndex - number.Length + 1, number.Length);
                    text = text.Insert(lastDigitIndex - number.Length + 1, numberAsWords);
                    i += numberAsWords.Length - number.Length;
                    var nextLetter = text[i];
                    if (i < text.Length - 1 && (nextLetter != ' ' && nextLetter != '.' && nextLetter != ','))
                    {
                        text = text.Insert(i, " ");
                    }
                }

                number = "";
                lastDigitIndex = 0;
            }
        }
        return text;
    }

    public static string ExtractNumber(string text, int index)
    {
        string number = "";
        for (int i = index; i < text.Length; i++)
        {
            var letter = text[i];
            if (char.IsDigit(letter) || letter == ',' || (letter == '.' && i < text.Length - 1 && char.IsDigit(text[i + 1])) || letter == '-')
            {
                number += letter;
            }
            else
                return number;
        }
        return number;
    }

    public static string ExpandSymbols(this string text)
    {
        text = text.Replace("m²", "metros quadrados");
        text = text.Replace("   ", " ");
        text = text.Replace("  ", " ");

        return text;
    }

    public static string ReplaceSymbols(this string text)
    {
        text = text.Replace("[Imagem]", "").Replace("[Mais detalhes]", "");
        text = text.Replace("**", "");
        text = text.Replace("()", "");
        text = text.Replace(" - \n", "");
        return text;
    }

    public static string NumberToWords(decimal number, bool currency = false)
    {
        var numbersWord = new Dictionary<int, string>() {
            {1, "um" }, {2, "dois" }, {3, "três" },
            {4, "quatro" }, {5, "cinco" }, {6, "seis" },
            {7, "sete" }, {8, "oito" }, {9, "nove" },
            {10, "dez" }, {11, "onze" }, {12, "doze" },
            {13, "treze" }, {14, "catorze" }, {15, "quinze" },
            {16, "dezesseis" }, {17, "dezessete" }, {18, "dezoito" },
            {19, "dezenove" }, {20, "vinte" }, {30, "trinta" },
            {40, "quarenta" }, {50, "cinquenta" }, {60, "sessenta" },
            {70, "setenta" }, {80, "oitenta" }, {90, "noventa" },

            {100, "cento" }, {200, "duzentos" }, {300, "trezentos" },
            {400, "quatrocentos" }, {500, "quinhentos" }, {600, "seiscentos" },
            {700, "setecentos" }, {800, "oitocentos" }, {900, "novecentos" },
        };


        const int thousand = 1000;
        const int million = 1000000;
        const int billion = 1000000000;
        const long trillion = 1000000000000;
        const long quadrillion = 1000000000000000;

        var remainder = number;
        var numberInWords = "";
        ///Se for mais que 999 trilhões não lidamos
        if (number > quadrillion)
        {
            return numberInWords;
        }

        if (remainder >= trillion)
        {
            long part = ((long)remainder % quadrillion) / trillion;
            if (part > 0)
            {
                if (numberInWords != "")
                    numberInWords += " e ";
                numberInWords += HundredsToWord((int)part, numbersWord) + (part > 1 ? " trilhões" : " trilhão");
                remainder = remainder % trillion;
            }
        }

        if (remainder >= billion)
        {
            long part = ((long)remainder % trillion) / billion;
            if (part > 0)
            {
                if (numberInWords != "")
                    numberInWords += " e ";
                numberInWords += HundredsToWord((int)part, numbersWord) + (part > 1 ? " bilhões" : " bilhão");
                remainder = remainder % billion;
            }
        }

        if (remainder >= million)
        {
            long part = ((long)remainder % billion) / million;
            if (part > 0)
            {
                if (numberInWords != "")
                    numberInWords += " e ";
                numberInWords += HundredsToWord((int)part, numbersWord) + (part > 1 ? " milhões" : " milhão");
                remainder = remainder % million;
            }

        }


        if (remainder >= thousand)
        {
            long part = ((long)remainder % million) / thousand;
            if (part > 0)
            {
                if (numberInWords != "")
                    numberInWords += " e ";

                numberInWords += HundredsToWord((int)part, numbersWord) + " mil";
                remainder = remainder % thousand;
            }
        }

        if (remainder > 0)
        {
            long part = (long)remainder % thousand;
            if (part > 0)
            {
                if (numberInWords != "")
                    numberInWords += " e ";
                numberInWords += HundredsToWord((int)remainder, numbersWord);
            }

            remainder = remainder % 1;
        }

        if (remainder > 0)
        {
            remainder *= 100;
            var cents = HundredsToWord((int)remainder, numbersWord);

            if (currency)
            {
                if (numberInWords != "")
                    numberInWords += " reais";
                numberInWords += $" e {cents} centavos";
            }
            else
                numberInWords += " vírgula " + cents;
        }
        else if (numberInWords != "" && currency)
            numberInWords += " reais";
        ///400.000

        return numberInWords;
    }


    ///Quem chama tem que garantir que não é maior que 999
    private static string HundredsToWord(int number, Dictionary<int, string> numbersInWords)
    {
        var result = "";
        var remainder = number;
        var hundred = remainder / 100;
        if (hundred > 0)
            result += numbersInWords[hundred * 100];

        remainder = remainder % 100;

        var tens = number % 100;
        if (tens >= 20)
        {
            if (result != "")
                result += " e ";
            result += numbersInWords[(tens / 10) * 10];
            remainder = remainder % 10;
        }
        else if (tens > 0)
        {
            if (result != "")
                result += " e ";
            result += numbersInWords[tens];
            remainder = 0;
        }

        var digit = remainder % 10;
        if (digit > 0)
        {
            if (result != "")
                result += " e ";
            result += numbersInWords[digit];
        }

        return result;

    }
}
