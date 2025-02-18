internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Program obj = new Program();
        Program obj2  = new Program();
    }
    private Program() {
        Console.WriteLine("I am from the constructor.......");
    }
    ~Program()
    {
        Console.WriteLine("Destructor invoked....");
    }
}