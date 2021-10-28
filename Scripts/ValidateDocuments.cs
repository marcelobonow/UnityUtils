using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ValidateDocuments
{
    public bool IsCPFValid(string cpf)
    {
        if (cpf.Length != 11)
            return false;

        var digits = cpf.Substring(0, 9);
        var verifyDigits = cpf.Substring(9);
        var sum = cpf.ToCharArray().Take(9).Select((digit, index) => (10 - index) * digit).Sum();
        var remainder = (sum * 10) / 11;
        if (remainder > 10)
            remainder = 0;


        Debug.Log(sum);
        for (int i = 0; i < 9; i++)
        {
            sum += (10 - i) * cpf[i];
        }

        return true;
    }
    //function TestaCPF(strCPF)
    //{
    //    var Soma;
    //    var Resto;
    //    Soma = 0;
    //    if (strCPF == "00000000000") return false;

    //    for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
    //    Resto = (Soma * 10) % 11;

    //    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    //    if (Resto != parseInt(strCPF.substring(9, 10))) return false;

    //    Soma = 0;
    //    for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
    //    Resto = (Soma * 10) % 11;

    //    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    //    if (Resto != parseInt(strCPF.substring(10, 11))) return false;
    //    return true;
    //}
}
