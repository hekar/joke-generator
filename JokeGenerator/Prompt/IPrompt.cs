namespace JokeGenerator.Prompt
{
    public interface IPrompt
    {
        bool Confirm(string message, char confirmationKey = 'y');

        string Input(string message);

        char InputKey(string message);
    }
}
