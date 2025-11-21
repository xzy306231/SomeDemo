// See https://aka.ms/new-console-template for more information
using Azure.AI.OpenAI;
using MEAI.Helper;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

{
    //OpenAIClientOptions clientOptions = new OpenAIClientOptions();
    //clientOptions.Endpoint = new Uri(Keys.AzureOpenAIEndpoint);

    //OpenAIClient aiClient = new(new ApiKeyCredential(Keys.AzureOpenAIApiKey), clientOptions);

    //var chatClient = aiClient.GetChatClient("gpt-4o");

    //var chatService = chatClient.AsIChatClient();

    //var response = await chatService.GetResponseAsync("南京富岛科技");

    //Console.WriteLine(response.ToString());

    //var rep = chatService.GetStreamingResponseAsync("南京富岛科技");

    //await foreach (var item in rep)
    //{
    //    Console.Write(item);
    //    await Task.Delay(100);
    //}

    //var azureEndpoint = new Uri(Keys.AzureOpenAIEndpoint);
    //var azureCredential = new ApiKeyCredential(Keys.AzureOpenAIApiKey);
    //AzureOpenAIClient azureClient = new AzureOpenAIClient(azureEndpoint, azureCredential);

    //    var embeddingDeployment = "text-embedding-3-small";
    //    var documents = new[]
    //    {
    //    "Microsoft.Extensions.AI 为 .NET 提供统一 AI 抽象。",
    //    "向量嵌入可以用来执行语义搜索和相似度匹配。"
    //};

    //    var embeddingsClient = aiClient.GetEmbeddingClient(embeddingDeployment);

    //    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = embeddingsClient.AsIEmbeddingGenerator();

    //    GeneratedEmbeddings<Embedding<float>> generatedEmbeddings = await embeddingGenerator.GenerateAsync(documents);

    //    var singleVector = await embeddingGenerator.GenerateVectorAsync("嵌入生成器适合语义检索");
    //    Console.WriteLine($"向量维度: {singleVector.Length}");


    var chatService = AIClientHelper.GetDefaultChatClient(enableLogging: true);

    //var client = AIClientHelper.GetAzureOpenAIClient(true);
    //var embeddingGenerator = client.GetEmbeddingClient("text-embedding-3-small").AsIEmbeddingGenerator();
    //var embeddings = await embeddingGenerator.GenerateAsync(new[] { "你好，欢迎使用微软认知服务。" });
    //var singleVector = await embeddingGenerator.GenerateVectorAsync("嵌入生成器适合语义检索");

    var option = new ChatClientAgentOptions
    {
        Instructions = "你是一个出行助手",
        Name = "travel",
        //定义输出的Json格式
        // ChatOptions = new ChatOptions { ResponseFormat = ChatResponseFormatJson.ForJsonSchema(schema: null, schemaName: "", schemaDescription: null) },
        ChatMessageStoreFactory = ctx => new InMemoryChatMessageStore(
        serializedStoreState: ctx.SerializedState,
        jsonSerializerOptions: ctx.JsonSerializerOptions)
    };

    //var agentAuto = chatService.CreateAIAgent(
    //options: new ChatClientAgentOptions(instructions: "你是天气助手", tools: [AIFunctionFactory.Create(GetWeather)])
    //{
    //    Name = "WeatherAgent_Auto",
    //    ChatOptions = new ChatOptions
    //    {
    //        ToolMode = ChatToolMode.Auto  // 自动模式
    //    }
    //});

    //// 使用 AIFunctionFactory 创建工具
    //var weatherTool = AIFunctionFactory.Create(GetWeather);

    //var agent = chatService.CreateAIAgent(instructions: "你是一个出行助手", name: "travel", tools: [weatherTool]);

    //var agentThread = agent.GetNewThread();

    //var agentResponse = agent.RunStreamingAsync("深圳天气如何。", agentThread);
    //await foreach (var chunk in agentResponse)
    //{
    //    Console.Write(chunk);
    //    await Task.Delay(50);
    //}

    //Console.WriteLine("\r\n***************************");

    //var r = agent.RunStreamingAsync("我问你什么了", agentThread);
    //await foreach (var chunk in r)
    //{
    //    Console.Write(chunk);
    //    await Task.Delay(50);
    //}
    //Console.WriteLine("\r\n***************************");
    //var jsonElement = agentThread.Serialize();
    //var str = JsonSerializer.Serialize(jsonElement);

    //var oldThread = agent.DeserializeThread(jsonElement);

    //Console.WriteLine(str);
    //Console.ReadKey();

    //var store = agentThread.GetService<ChatMessageStore>();
    //var messageList = (await store.GetMessagesAsync()).ToList();

    //for (int i = 0; i < messageList.Count; i++)
    //{
    //    var message = messageList[i];
    //    var roleIcon = message.Role.ToString() switch
    //    {
    //        "user" => "👤",
    //        "assistant" => "🤖",
    //        "system" => "⚙️",
    //        _ => "📄"
    //    };

    //    Console.WriteLine($"{i + 1}. {roleIcon} [{message.Role}]:");

    //    // 截断过长的内容
    //    var content = message.Text ?? "";
    //    if (content.Length > 100)
    //    {
    //        content = content.Substring(0, 100) + "...";
    //    }
    //    Console.WriteLine($"   {content}\n");
    //}

    // Console.ReadKey();

    //string GetCurrentWeather() => Random.Shared.NextDouble() > 0.5 ? "It's sunny" : "It's raining";
    //string GetCurrentWeather() => "不知道";

    //var tools = AIFunctionFactory.Create(GetCurrentWeather);

   await ScheduleProgram.Main();

    var travelTools = new TravelToolset();

    IList<AITool> batchRegisteredTools =
        typeof(TravelToolset)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(method => AIFunctionFactory.Create(
                method,
                travelTools,
                name: method.Name.ToLowerInvariant(),
                description: method.GetCustomAttribute<DescriptionAttribute>()?.Description))
            .Cast<AITool>()
            .ToList();

    //foreach (var tool in batchRegisteredTools)
    //{
    //    Console.WriteLine($"Registered tool: {tool.Name} - {tool.Description}");
    //}

    //ChatOptions batchOptions = new()
    //{
    //    ToolMode = ChatToolMode.Auto,
    //    Tools = batchRegisteredTools
    //};

   // var datetimeTool = AIFunctionFactory.Create(() => DateTime.Now, "get_current_datetime", "获取当前的日期和时间");

    //var client = chatService.AsBuilder()
    //    .UseFunctionInvocation(configure: options =>
    //    {
    //        options.AdditionalTools = [datetimeTool]; // 注册一些额外的工具，比如时间工具等
    //        options.AllowConcurrentInvocation = true; // 允许模型并发调用多个函数，默认 false
    //        options.IncludeDetailedErrors = true; // 包含详细错误信息，默认 false
    //        options.MaximumConsecutiveErrorsPerRequest = 3; // 每个请求允许的最大连续错误数，防止无限循环，默认 3次
    //        options.MaximumIterationsPerRequest = 5; // 每个请求允许的最大迭代次数，防止无限循环，默认 40次
    //        options.TerminateOnUnknownCalls = false; // 当模型调用了未知的函数时，是否终止对话
    //        options.FunctionInvoker = (context, cancellationToken) =>
    //        {
    //            var functionCall = context.Function;

    //            Console.WriteLine($"Invoking function: {functionCall.Name} with arguments: {functionCall.AdditionalProperties}");

    //            return context.Function.InvokeAsync(context.Arguments, cancellationToken);
    //        };
    //    }).Build();


    //var messages = new List<ChatMessage>
    // {
    //new ChatMessage(ChatRole.System, "你是石化行业的调度排产专家"),
    //new ChatMessage(ChatRole.User, "根据期初库存与工艺，指定的加工方案，进行调度排产")
    //  };

    //ChatOptions options = new()
    //{
    //    ToolMode = ChatToolMode.Auto, // 自动决定是否调用工具，默认值为 Auto
    //    AllowMultipleToolCalls = true, // 允许模型一次调用多个工具，默认 false
    //    Tools = batchRegisteredTools
    //};

    //var weatherResponse = await client.GetResponseAsync(messages, options);
    //Console.WriteLine(weatherResponse.ToString());

    //Console.ReadKey();

    //[Description("查询指定城市的当前天气信息,包括天气状况和温度")]
    //string GetWeather([Description("要查询天气的城市名称,例如: 北京、上海、深圳")] string city)
    //{
    //    // 模拟天气查询 (实际应用中应该调用真实的天气 API)
    //    var weatherData = new Dictionary<string, (string condition, int temperature)>
    //    {
    //        ["北京"] = ("晴天", 15),
    //        ["上海"] = ("多云", 20),
    //        ["深圳"] = ("阴天", 25),
    //        ["广州"] = ("小雨", 22),
    //        ["杭州"] = ("晴天", 18)
    //    };

    //    if (weatherData.TryGetValue(city, out var weather))
    //    {
    //        return $"{city}的天气: {weather.condition}, 温度: {weather.temperature}°C";
    //    }

    //    return $"抱歉,暂时无法获取{city}的天气信息";
    //}
}


public record WeatherReport(string City, int TemperatureCelsius, bool WillRain);
public class TravelToolset
{
    [Description("查询指定城市的实时天气")]
    public WeatherReport QueryWeather(string city)
    {
        int temperature = Random.Shared.Next(-5, 36);
        bool willRain = Random.Shared.NextDouble() > 0.6;
        return new WeatherReport(city, temperature, willRain);
    }
    [Description("排产期初数据,包含了加工方案与工艺")]
    public string InitStock()
    {
        var str = File.ReadAllText(@"D:\\资料\\SomeDemo\\MEAI\\request.json");
        return str;
    }

    [Description("根据天气提供穿搭建议")]
    public string SuggestOutfit(string city)
    {
        var weather = QueryWeather(city);
        return weather switch
        {
            { WillRain: true } => $"{city} 可能会下雨，建议携带雨具并穿防水外套。",
            { TemperatureCelsius: < 5 } => $"{city} 温度 {weather.TemperatureCelsius}℃，请穿冬装并注意保暖。",
            { TemperatureCelsius: > 28 } => $"{city} 今天很热（{weather.TemperatureCelsius}℃），可以选择短袖和透气面料。",
            _ => $"{city} 气温 {weather.TemperatureCelsius}℃，穿上舒适的日常装束即可。"
        };
    }

}
