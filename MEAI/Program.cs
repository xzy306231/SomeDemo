using Azure.AI.OpenAI;
using Azure.Core;
using MEAI.Helper;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Net.Http;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

var endpoint = "http://175.178.155.193/v1";

var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-4o";

OpenAIClientOptions clientOptions = new OpenAIClientOptions();
clientOptions.Endpoint = new Uri(Keys.AzureOpenAIEndpoint);

OpenAIClient aiClient = new(new ApiKeyCredential(Keys.AzureOpenAIApiKey), clientOptions);

var chatClient = aiClient.GetChatClient("gpt-4o").AsIChatClient();

var agent = chatClient.CreateAIAgent(name: "VisionAgent", instructions: "你是一个分析图片内容的智能代理，请根据图片内容回答用户的问题。");

byte[] imageBytes = await File.ReadAllBytesAsync("C:\\Users\\admin\\Desktop\\test.png");

ChatMessage message = new(ChatRole.User, [
 new TextContent("这个群有几个人，有几个人聊天，在聊什么"),
 new DataContent(imageBytes, "image/jpeg")
]);

var thread = agent.GetNewThread();

await foreach (var update in agent.RunStreamingAsync(message, thread))
{
    await Task.Delay(100);
    Console.Write(update);
}

Console.ReadKey();