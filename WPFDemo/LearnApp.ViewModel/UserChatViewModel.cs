using CommunityToolkit.Mvvm.Input;
using LearnApp.Control;
using LearnApp.Shared;
using LearnApp.Shared.Base;
using Microsoft.Extensions.AI;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace LearnApp.ViewModel
{
    public class UserChatViewModel : BaseBindable
    {
        public ICommand SendCommand { get; set; }
        public ObservableCollection<ChatContent> List { get; set; } = new ObservableCollection<ChatContent>();
        IChatClient chatService;
        public UserChatViewModel()
        {
            OpenAIClientOptions clientOptions = new OpenAIClientOptions();
            clientOptions.Endpoint = new Uri(Keys.AzureOpenAIEndpoint);

            OpenAIClient aiClient = new(new ApiKeyCredential(Keys.AzureOpenAIApiKey), clientOptions);

            var chatClient = aiClient.GetChatClient("gpt-4o");

            chatService = chatClient.AsIChatClient();

            SendCommand = new AsyncRelayCommand(Send);
        }
        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                SetProperty(ref message, value);
            }
        }

        private string typedText;
        public string TypedText
        {
            get { return typedText; }
            set { SetProperty(ref typedText, value); }
        }


        private async Task Send()
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            //List.Add(new ChatContent { Status = 1, Content = Message, DateTime = DateTime.Now, Type = 2 });
            //var rep = await chatService.GetResponseAsync(message);
            //Message = "";

            var repo = chatService.GetStreamingResponseAsync(message);
            Message = "";
            typedText = "";
            await foreach (var item in repo)
            {

                TypedText += item.Text;
                await Task.Delay(100);
            }

        }
    }
}
