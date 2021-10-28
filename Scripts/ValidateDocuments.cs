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

    public static bool IsCNPJValid(string cnpj)
    {
        var cnpjDigits = cnpj.Where(char.IsDigit).ToArray();
        if (cnpjDigits.Length != 14)
            return false;

        ///O CPF verifica se é valido usando os dois digitos de verificação separadamente
        ///para o primeiro digito se multiplica cada digito por um numero, começando em 10 e decrescendo
        ///No fim, soma todos os digitos, multiplica por 10 e pega o resto da divisão por 11

        var firstWeights = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var firstDigits = cnpj.Take(12);
        var firstSum = firstDigits.Select((digit, index) => int.Parse(digit.ToString()) * firstWeights[index]).Sum();
        var firstRemainder = firstSum % 11;
        var firstVerifyingDigit = firstRemainder < 2 ? 0 : 11 - firstRemainder;

        if (firstVerifyingDigit != cnpj[12])
        {
            Debug.Log("Primeiro digito verificador diferente");
            return false;
        }

        var secondWeights = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var secondDigits = cnpj.Take(13);
        var secondSum = secondDigits.Select((digit, index) => int.Parse(digit.ToString()) * secondWeights[index]).Sum();
        var secondRemainder = secondSum % 11;
        var secondVerifyingDigit = secondRemainder < 2 ? 0 : 11 - secondRemainder;

        if (secondVerifyingDigit != cnpj[13])
        {
            Debug.Log("Segundo digito verificador diferente");
            return false;
        }

        return true;
    }
}
