using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;

public class StringUtilsTest
{
    [Test]
    public void TestRemoveLinks()
    {
        Assert.AreEqual("teste ", "teste https://beupse.com".RemoveLinks());
        Assert.AreEqual(
            "teste http mas não com formato",
            "teste http mas não com formato".RemoveLinks()
        );
        Assert.AreEqual(
            "Testando links [] e acesse no link ()",
            "Testando links [https://logos.flamingtext.com/Word-Logos/exemplo-design-sketch-name.png] e acesse no link (https://www.brognoli.com.br/imovel/comprar-apartamentos-com-55m-2-quartos-no-bairro-campeche-leste-em-florianopolis_PU102zidi/)".RemoveLinks()
        );
        Assert.AreEqual(
            "Testando links [] e acesse no link ()",
            "Testando links [http://logos.flamingtext.com/Word-Logos/exemplo-design-sketch-name.png] e acesse no link (http://www.brognoli.com.br/imovel/comprar-apartamentos-com-55m-2-quartos-no-bairro-campeche-leste-em-florianopolis_PU102zidi/)".RemoveLinks()
        );

        Assert.AreEqual(
            "Testando link que nem parece link, tipo  e texto depois",
            "Testando link que nem parece link, tipo http://linkaleatorio,semponto;comcaracteres&82 e texto depois".RemoveLinks()
        );
        Assert.AreEqual(
            "Testando link que nem parece link, tipo  e texto depois",
            "Testando link que nem parece link, tipo https://linkaleatorio,semponto;comcaracteres&82 e texto depois".RemoveLinks()
        );
    }

    [Test]
    public void TestGetLinkFromWord()
    {
        var test1 = StringUtils.GetLinkFromWordGetLinkFromWord("teste https://beupse.com", 14);
        Assert.AreEqual(6, test1.startPosition);
        Assert.AreEqual(23, test1.endPosition);

        var test2 = StringUtils.GetLinkFromWordGetLinkFromWord("teste http mas não com formato", 55);
        Assert.IsNull(test2);

        var test3 = StringUtils.GetLinkFromWordGetLinkFromWord("Testando links[https://logos.flamingtext.com/Word-Logos/exemplo-design-sketch-name.png] e acesse no link (https://www.brognoli.com.br/imovel/comprar-apartamentos-com-55m-2-quartos-no-bairro-campeche-leste-em-florianopolis_PU102zidi/)", 115);
        Assert.AreEqual(106, test3.startPosition);
        Assert.AreEqual(231, test3.endPosition);

        var test4 = StringUtils.GetLinkFromWordGetLinkFromWord("Testando links[https://logos.flamingtext.com/Word-Logos/exemplo-design-sketch-name.png] e acesse no link (https://www.brognoli.com.br/imovel/comprar-apartamentos-com-55m-2-quartos-no-bairro-campeche-leste-em-florianopolis_PU102zidi/)", 87);
        Assert.IsNull(test4);
    }

    [Test]
    public void GetLinks()
    {
        var test1 = "teste https://beupse.com".GetLinks();
        Assert.AreEqual(1, test1.Count);
        Assert.AreEqual("https://beupse.com", test1[0].text);
        Assert.AreEqual(6, test1[0].startPosition);

        var test2 = "Teste sem links".GetLinks();
        Assert.AreEqual(0, test2.Count);

        var test3 = "Testando links[https://logos.flamingtext.com/Word-Logos/exemplo-design-sketch-name.png] e acesse no link (https://www.brognoli.com.br/imovel/comprar-apartamentos-com-55m-2-quartos-no-bairro-campeche-leste-em-florianopolis_PU102zidi/)".GetLinks();
        Assert.AreEqual(2, test3.Count);
        Assert.AreEqual("https://logos.flamingtext.com/Word-Logos/exemplo-design-sketch-name.png", test3[0].text);
        Assert.AreEqual(15, test3[0].startPosition);

        var text3 = "Testando links[https://logos.flamingtext.com/Word-Logos/exemplo-design-sketch-name.png] e acesse no link (https://www.brognoli.com.br/imovel/comprar-apartamentos-com-55m-2-quartos-no-bairro-campeche-leste-em-florianopolis_PU102zidi/)";

        Debug.Log(text3.Substring(106, 10));
        Assert.AreEqual("https://www.brognoli.com.br/imovel/comprar-apartamentos-com-55m-2-quartos-no-bairro-campeche-leste-em-florianopolis_PU102zidi/", test3[1].text);
        Assert.AreEqual(106, test3[1].startPosition);

    }

    [Test]
    public void TestRemoveSpecialCharacteres()
    {
        Assert.AreEqual("teste abcdefgh", "teste , . abcdefgh".RemoveSpecialCharacters());
        Assert.AreEqual(
            "teste com, vários caracteres. No meio, então verificando, se não sai sem querer! tem que ter tirado esses",
            "teste com, vários caracteres. No meio, então verificando, se não sai sem querer! , , . ! @ tem que ter tirado esses".RemoveSpecialCharacters()
        );
    }

    [Test]
    public void TestExpandCurrency()
    {
        Assert.AreEqual(
            "Teste quinhentos mil reais e cinquenta centavos",
            "Teste R$ 500.000,50".ExpandCurrency()
        );

        Assert.AreEqual(
            "trezentos e vinte e sete reais e cinquenta e dois centavos",
            "R$ 327,52".ExpandCurrency()
        );

        Assert.AreEqual(
            "Esse ímovel custa quinhentos mil reais",
            "Esse ímovel custa R$ 500.000,00".ExpandCurrency()
        );
        Assert.AreEqual(
            "E esse outro custa trezentos mil reais",
            "E esse outro custa R$300.000,00".ExpandCurrency()
        );
        Assert.AreEqual(
            "Eu não tenho como te dar uma resposta em reais",
            "Eu não tenho como te dar uma resposta em R$".ExpandCurrency()
        );
    }

    [Test]
    public void CompleteTest()
    {
        var message =
            "Aqui estão algumas opções de apartamentos à venda em Florianópolis por até 800 mil reais:\n\n1. **Ingleses**  \n   - **Valor:** R$ 429.564,72  \n   - **Área:** 62 m²  \n   - **Quartos:** 2 (1 suíte)  \n   - [Mais detalhes](https://www.brognoli.com.br/imovel/comprar-apartamentos-com-62m-2-quartos-no-bairro-ingleses-em-florianopolis_PUk2z488/)  \n   - ![Imagem](https://pui.loft.com.br/listings/ae7a4023-a748-4dec-ad50-e682677384b0/photos/000140c4?preset=thumbnail)\n\n2. **Ingleses**  \n   - **Valor:** R$ 455.800,00  \n   - **Área:** 92 m²  \n   - **Quartos:** 3 (1 suíte)  \n   - [Mais detalhes](https://www.brognoli.com.br/imovel/comprar-apartamentos-com-92m-3-quartos-no-bairro-ingleses-em-florianopolis_PUmic342/)  \n   - ![Imagem](https://pui.loft.com.br/listings/b577978d-bb7f-427b-bf5e-cb48f6ed84f6/photos/acaef7ca?preset=thumbnail)\n\n3. **Ingleses**  \n   - **Valor:** R$ 797.533,05  \n   - **Área:** 77 m²  \n   - **Quartos:** 2 (1 suíte)  \n   - [Mais detalhes](https://www.brognoli.com.br/imovel/comprar-apartamentos-com-77m-2-quartos-no-bairro-ingleses-em-florianopolis_PUdbfh21/)  \n   - ![Imagem](https://pui.loft.com.br/listings/dba5910b-b742-497c-b1b4-63ec0e4951c3/photos/ac52c1fb?preset=thumbnail)\n\n4. **Vargem Grande**  \n   - **Valor:** R$ 508.000,00  \n   - **Área:** 52 m²  \n   - **Quartos:** 2 (1 suíte)  \n   - [Mais detalhes](https://www.brognoli.com.br/imovel/comprar-apartamentos-com-52m-2-quartos-no-bairro-vargem-grande-em-florianopolis_PU1n1wnvs/)  \n   - ![Imagem](https://pui.loft.com.br/listings/dc300927-e7df-4094-96d6-086e064e2f5b/photos/e7c6fdf8?preset=thumbnail)\n\n5. **Itaguaçu (Abraão)**  \n   - **Valor:** R$ 700.000,00  \n   - **Área:** 93 m²  \n   - **Quartos:** 3  \n   - [Mais detalhes](https://www.brognoli.com.br/imovel/comprar-apartamentos-com-93m-3-quartos-no-bairro-itaguacu-em-florianopolis_PU1lpzurg/)  \n   - ![Imagem](https://pui.loft.com.br/listings/d67b01a9-0805-4dfc-8d18-d4c09d410f11/photos/22d6f164?preset=thumbnail)\n\n6. **Trindade**  \n   - **Valor:** R$ 669.000,00  \n   - **Área:** 34 m²  \n   - **Quartos:** 1  \n   - [Mais detalhes](https://www.brognoli.com.br/imovel/comprar-apartamentos-com-34m-1-quarto-no-bairro-trindade-em-florianopolis_PU1pbfn13/)  \n   - ![Imagem](https://pui.loft.com.br/listings/84198be7-e488-41ba-896d-0a93220194be/photos/7b91337a?preset=thumbnail)\n\nSe precisar de mais informações ou quiser ajustar os filtros, estou à disposição!";
        var expectedMessage =
            "Aqui estão algumas opções de apartamentos à venda em Florianópolis por até oitocentos mil reais:\n\num. **Ingleses** \n - **Valor:** quatrocentos e vinte e nove mil e quinhentos e sessenta e quatro reais \n - **Área:** sessenta e dois metros quadrados \n - **Quartos:** dois (um suíte) \n - [Mais detalhes]() \n - ![Imagem]()\n\ndois. **Ingleses** \n - **Valor:** quatrocentos e cinquenta e cinco mil e oitocentos reais \n - **Área:** noventa e dois metros quadrados \n - **Quartos:** três (um suíte) \n - [Mais detalhes]() \n - ![Imagem]()\n\ntrês. **Ingleses** \n - **Valor:** setecentos e noventa e sete mil e quinhentos e trinta e três reais \n - **Área:** setenta e sete metros quadrados \n - **Quartos:** dois (um suíte) \n - [Mais detalhes]() \n - ![Imagem]()\n\nquatro. **Vargem Grande** \n - **Valor:** quinhentos e oito mil reais \n - **Área:** cinquenta e dois metros quadrados \n - **Quartos:** dois (um suíte) \n - [Mais detalhes]() \n - ![Imagem]()\n\ncinco. **Itaguaçu (Abraão)** \n - **Valor:** setecentos mil reais \n - **Área:** noventa e três metros quadrados \n - **Quartos:** três \n - [Mais detalhes]() \n - ![Imagem]()\n\nseis. **Trindade** \n - **Valor:** seiscentos e sessenta e nove mil reais \n - **Área:** trinta e quatro metros quadrados \n - **Quartos:** um \n - [Mais detalhes]() \n - ![Imagem]()\n\nSe precisar de mais informações ou quiser ajustar os filtros, estou à disposição!";

        ///Testar aqui
        string messageProcessed = message
            .RemoveLinks()
            .RemoveSpecialCharacters()
            .ExpandCurrency()
            .ExpandSymbols()
            .ExpandAllNumbers();
        Debug.Log(messageProcessed);
        Assert.AreEqual(expectedMessage, messageProcessed);
    }

    [Test]
    public void TestExpandAllNumbers()
    {
        Assert.AreEqual(
            "Esse apartamento tem sessenta e dois m², dois banheiros e três quartos",
            "Esse apartamento tem 62 m², 2 banheiros e 3 quartos".ExpandAllNumbers()
        );
        Assert.AreEqual(
            "Teste quatrocentos e vinte e nove vírgula trinta e cinco OK",
            "Teste 429,35 OK".ExpandAllNumbers()
        );
        Assert.AreEqual("Tenho apartir de trezentos.", "Tenho apartir de 300.".ExpandAllNumbers());
        Assert.AreEqual(
            "Esse apartamento tem sessenta e dois m², dois banheiros e três quartos",
            "Esse apartamento tem 62m², 2 banheiros e 3 quartos".ExpandAllNumbers()
        );
    }

    [Test]
    public void TestNumberToWords()
    {
        Assert.AreEqual("quatrocentos mil", StringUtils.NumberToWords(400000));
        Assert.AreEqual("duzentos e cinquenta e três mil", StringUtils.NumberToWords(253000));
        Assert.AreEqual(
            "seiscentos e oitenta e seis mil e cinquenta e três",
            StringUtils.NumberToWords(686053)
        );
        Assert.AreEqual("vinte e cinco", StringUtils.NumberToWords(25));
        Assert.AreEqual("doze", StringUtils.NumberToWords(12));
        Assert.AreEqual("dezoito", StringUtils.NumberToWords(18));
        Assert.AreEqual("dois", StringUtils.NumberToWords(2));
        Assert.AreEqual(
            "um milhão e quinhentos e vinte e quatro mil e novecentos e vinte",
            StringUtils.NumberToWords(1524920)
        );
        Assert.AreEqual(
            "dois milhões e quinhentos e vinte e quatro mil e novecentos e vinte",
            StringUtils.NumberToWords(2524920)
        );
        Assert.AreEqual("um bilhão", StringUtils.NumberToWords(1000000000));
        Assert.AreEqual(
            "dois bilhões e quinhentos e vinte e quatro mil",
            StringUtils.NumberToWords(2000524000)
        );
        Assert.AreEqual(
            "cinco trilhões e dois bilhões e quinhentos e vinte e quatro mil",
            StringUtils.NumberToWords(5002000524000)
        );
    }

    [Test]
    public void TestWav()
    {
        var fileFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var data = File.ReadAllBytes(fileFolder + "/new.wav");
        var clip = WavUtils.FromPcmBytes(data);

        var result = WavUtils.TrimSilence(clip, 0.01f);

        FileUtils.SaveToFile(fileFolder + "/new2.wav", WavUtils.Convert(result));
        Assert.IsTrue(clip.samples > result.samples);
    }
}
