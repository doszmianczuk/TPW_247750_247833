using System;

public class Liczenie
{
    public bool CzyLiczbaWiekszaNizDziesiec(int liczba1, int liczba2)
    {
        if (liczba1 > 10 || liczba2 > 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Podaj dwie liczby od 1 do 10:");

        Console.Write("Pierwsza liczba: ");
        int liczba1 = Convert.ToInt32(Console.ReadLine());

        Console.Write("Druga liczba: ");
        int liczba2 = Convert.ToInt32(Console.ReadLine());

        Liczenie liczenie = new Liczenie();
        bool wynik = liczenie.CzyLiczbaWiekszaNizDziesiec(liczba1, liczba2);

        if (wynik)
        {
            Console.WriteLine("Przynajmniej jedna z podanych liczb jest większa niż 10.");
        }
        else
        {
            Console.WriteLine("Żadna z podanych liczb nie jest większa niż 10.");
        }
    }
}
