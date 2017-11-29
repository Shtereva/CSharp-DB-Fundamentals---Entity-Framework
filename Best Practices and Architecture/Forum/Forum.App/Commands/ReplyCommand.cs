﻿using Forum.App.Commands.Contracts;
using Forum.Services.Contracts;

namespace Forum.App.Commands
{
    public class ReplyCommand : ICommand
    {
        private readonly IReplyService replyService;

        public ReplyCommand(IReplyService replyService)
        {
            this.replyService = replyService;
        }

        public string Execute(params string[] arguments)
        {
            int postId = int.Parse(arguments[0]);
            var content = arguments[1];

            if (Session.User == null)
            {
                return "You are not logged in";
            }

            replyService.Create(content, postId, Session.User.Id);

            return "Reply created successfully";
        }
    }
}
