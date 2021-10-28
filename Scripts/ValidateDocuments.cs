using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ValidateDocuments
{
    public static bool IsCPFValid(string cpf)
    {
        var cpfNumbers = cpf.Where(char.IsDigit).ToArray();
        if (cpfNumbers.Length != 11)
            return false;

        ///O CPF verifica se é valido usando os dois digitos de verificação separadamente
        ///para o primeiro digito se multiplica cada digito por um numero, começando em 10 e decrescendo
        ///No fim, soma todos os digitos, multiplica por 10 e pega o resto da divisão por 11

        var firstVerifyDigits = cpfNumbers.Take(9);
        var firstSum = firstVerifyDigits.Select((digit, index) => (10 - index) * int.Parse(digit.ToString())).Sum();
        var firstRemainder = (firstSum * 10) % 11;
        if (firstRemainder > 10)
            firstRemainder = 0;

        if (int.Parse(cpfNumbers[9].ToString()) != firstRemainder)
        {
            Debug.Log("Primeiro digito verificador diferente");
            return false;
        }

        ///Para o segundo digito se verifica de forma similar
        ///a diferença é que agora multiplicamos começando do 11, e incluimos o primeiro digito verificador

        var secondVerifyDigits = cpfNumbers.Take(10);
        var secondSum = secondVerifyDigits.Select((digit, index) => (11 - index) * int.Parse(digit.ToString())).Sum();
        var secondRemainder = (secondSum * 10) % 11;
        if (secondRemainder > 10)
            secondRemainder = 0;

        if (int.Parse(cpfNumbers[10].ToString()) != secondRemainder)
        {
            Debug.Log("Segundo digito verificador diferente");
            return false;
        }

        return true;
    }
}
