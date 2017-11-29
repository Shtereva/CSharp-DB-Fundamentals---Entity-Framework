using System;
using System.Linq;
using BusTicketSystem.Client.Core.Commands;

namespace BusTicketSystem.Client.Core
{
    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParams)
        {
            string commandName = commandParams[0];

            var commandArg = commandParams.Skip(1).ToArray();

            string result = string.Empty;

            switch (commandName)
            {
                case "print-info":
                    var printInfo = new PrintInfoCommand();
                    result = printInfo.Execute(commandArg);
                    break;

                case "buy-ticket":
                    var buyTicket = new BuyTicketCommand();
                    result = buyTicket.Execute(commandArg);
                    break;
                case "publish-review":
                    var publishReview = new PublishReviewCommand();
                    result = publishReview.Execute(commandArg);
                    break;
                case "print-reviews":
                    var printReview = new PrintReviewsCommand();
                    result = printReview.Execute(commandArg);
                    break;
                    default: throw new InvalidOperationException("Invalid command!");
            }
            return result;
        }
    }
}
