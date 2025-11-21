using MEAI.Helper;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ScheduleProgram
{
    public static async Task Main()
    {

        var chatClient = AIClientHelper.GetDefaultChatClient(enableLogging: true);

        var tool = AIFunctionFactory.Create(InitStock);

        var imageUrl = @"C:/Users/admin/Desktop/gantt.png";
        var userQuestion = "简单描述这张图片的内容？";

        // 创建一条包含文本和图像内容的消息
        var multiModalMessage = new ChatMessage(
            ChatRole.User,
            contents: new AIContent[]
            {
          new TextContent(userQuestion),
          new UriContent(imageUrl, "image/png")
            }
        );

        var agent = chatClient.CreateAIAgent(instructions: "你是一个能够处理多模态输入的AI助手", name: "multiModalAgent", tools: [tool]);

        var msg = await agent.RunAsync(multiModalMessage);
        Console.WriteLine(msg);

        //var agent = chatService.CreateAIAgent(instructions: "你是石化行业的调度排产专家", name: "schedule", tools: [tool]);
        //var response = agent.RunStreamingAsync("请根据排产期初数据，生成未来30天的排产计划,期初数据中包含了工艺关系，管线连接的罐区，能够输出储罐油种的走向，从哪到哪，什么时间点，多少量");
        //await foreach (var chunk in response)
        //{
        //    Console.Write(chunk);
        //    await Task.Delay(50);
        //}
        Console.ReadKey();
    }

    [Description("排产期初数据,包含了加工方案与工艺")]
    static string InitStock()
    {
        var str = File.ReadAllText(@"D:\\资料\\SomeDemo\\MEAI\\request.json");
        return str;
    }
}

