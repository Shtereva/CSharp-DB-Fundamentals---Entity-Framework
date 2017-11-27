using System.Linq;
using PhotoShare.Client.Core.Commands;

namespace PhotoShare.Client.Core
{
    using System;

    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParameters)
        {
            string commandName = commandParameters[0];

            var commandArg = commandParameters.Skip(1).ToArray();
            string result = string.Empty;

            switch (commandName)
            {
                case "RegisterUser":
                    var registerUser = new RegisterUserCommand();
                    result = registerUser.Execute(commandArg);
                    break;
                case "AddTown":
                    var addTown = new AddTownCommand();
                    result = addTown.Execute(commandArg);
                    break;
                case "ModifyUser":
                    var modifyUser = new ModifyUserCommand();
                    result = modifyUser.Execute(commandArg);
                    break;
                case "DeleteUser":
                    var deleteUser = new DeleteUser();
                    result = deleteUser.Execute(commandArg);
                    break;
                case "AddTag":
                    var tag = new AddTagCommand();
                    result = tag.Execute(commandArg);
                    break;
                case "CreateAlbum":
                    var album = new CreateAlbumCommand();
                    result = album.Execute(commandArg);
                    break;
                case "AddTagTo":
                    var tagTo = new AddTagToCommand();
                    result = tagTo.Execute(commandArg);
                    break;
                case "AddFriend":
                    var addFriend = new AddFriendCommand();
                    result = addFriend.Execute(commandArg);
                    break;
                case "AcceptFriend":
                    var acceptFriend = new AcceptFriendCommand();
                    result = acceptFriend.Execute(commandArg);
                    break;
                case "ListFriends":
                    var listFriend = new PrintFriendsListCommand();
                    result = listFriend.Execute(commandArg);
                    break;
                case "ShareAlbum":
                    var shareAlbum = new ShareAlbumCommand();
                    result = shareAlbum.Execute(commandArg);
                    break;
                case "UploadPicture":
                    var uploadPicture = new UploadPictureCommand();
                    result = uploadPicture.Execute(commandArg);
                    break;
                case "Exit":
                    ExitCommand.Execute();
                    break;
                case "Login":
                    var login = new LoginCommand();
                    result = login.Execute(commandArg);
                    break;
                case "Logout":
                    var logout = new LogoutCommand();
                    result = logout.Execute();
                    break;
                default:
                    throw new InvalidOperationException($"Command {commandName} not valid!");
            }

            return result;
        }
    }
}
