namespace JokeGenerator
{
    public interface IPrinter
    {
        IPrinter Write(string msg);

        IPrinter WriteLine(string msg);

        IPrinter WriteLine(string msg, object[] args);
    }
}
