namespace PhotoShare.Client.Core.Commands
{
    using System;

    public static class ExitCommand
    {
        public static void Execute()
        {
            Console.WriteLine("Good Bye!");
            Environment.Exit(0);
        }
    }
}
