public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Person()
    {
        Name = " Tidak Diketahui";
        Age = 0;
    }

    public Person(string name)
    {
        Name = name;
        Age = 0;
    }
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Info()
    {
        Console.WriteLine($"Nama {Name} Umur {Age}");
    }
}