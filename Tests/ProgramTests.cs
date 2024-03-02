using NUnit.Framework;
using System.ComponentModel;

[TestFixture]
public class LiczenieTests
{
    [Test]
    public void CzyLiczbaWiekszaNizDziesiec_LiczbyWiekszeNizDziesiec_ZwracaTrue()
    {
        // Arrange
        Liczenie liczenie = new Liczenie();

        // Act
        bool wynik = liczenie.CzyLiczbaWiekszaNizDziesiec(11, 5);

        // Assert
        Assert.IsTrue(wynik);
    }

    [Test]
    public void CzyLiczbaWiekszaNizDziesiec_LiczbyMniejszeLubRowneDziesieciu_ZwracaFalse()
    {
        // Arrange
        Liczenie liczenie = new Liczenie();

        // Act
        bool wynik = liczenie.CzyLiczbaWiekszaNizDziesiec(8, 10);

        // Assert
        Assert.IsFalse(wynik);
    }
}
