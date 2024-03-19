using NUnit.Framework;
using System.ComponentModel;

[TestFixture]
public class LiczenieTests
{
    [Test]
    public void CzyLiczbaWiekszaNizDziesiec_LiczbyWiekszeNizDziesiec_ZwracaTrue()
    {
   
        Liczenie liczenie = new Liczenie();

        bool wynik = liczenie.CzyLiczbaWiekszaNizDziesiec(11, 5);

        Assert.IsTrue(wynik);
    }

    [Test]
    public void CzyLiczbaWiekszaNizDziesiec_LiczbyMniejszeLubRowneDziesieciu_ZwracaFalse()
    {
        Liczenie liczenie = new Liczenie();

        bool wynik = liczenie.CzyLiczbaWiekszaNizDziesiec(8, 10);

        Assert.IsFalse(wynik);
    }
}
