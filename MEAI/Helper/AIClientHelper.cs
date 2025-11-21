using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEAI.Helper
{
    public static class AIClientHelper
    {
        /// <summary>
        /// 获取默认的聊天客户端，默认使用 AzureOpenAI 和 gpt-4o 模型
        /// AzureOpenAI: gpt-4o
        /// DeepSeek: deepseek-chat, deepseek-reasoner
        /// Qwen: qwen-max, qwen-plus, qwen-flash
        /// TokenAI: gpt-5-pro, claude-sonnet-4-5-20250929
        /// </summary>
        public static IChatClient GetDefaultChatClient(string provider = "AzureOpenAI", string model = "gpt-4o", bool enableLogging = false)
        {
            var chatClient = provider switch
            {
                "AzureOpenAI" => GetAzureOpenAIClient(enableLogging).GetChatClient(model),
                "DeepSeek" => GetDeepSeekClient(enableLogging).GetChatClient(model),
                "Qwen" => GetQwenClient(enableLogging).GetChatClient(model),
                "TokenAI" => GetTokenAIClient(enableLogging).GetChatClient(model),
                _ => GetAzureOpenAIClient(enableLogging).GetChatClient(model),
            };

            return chatClient.AsIChatClient();
        }

        public static OpenAIClient GetAzureOpenAIClient(bool enableLogging = false)
        {
            var endpoint = Keys.AzureOpenAIEndpoint;
            var apiKey = Keys.AzureOpenAIApiKey;

            var clientOptions = new OpenAIClientOptions();
            clientOptions.Endpoint = new Uri(endpoint);

            if (enableLogging)
            {
                var clientLoggingOptions = NLogHelper.CreateClientLoggingOptions();

                clientOptions.ClientLoggingOptions = clientLoggingOptions;
            }

            var client = new OpenAIClient(new ApiKeyCredential(apiKey), clientOptions);

            return client;
        }

        public static IEmbeddingGenerator<string, Embedding<float>> GetAzureOpenAIEmbeddingGenerator()
        {
            var client = GetAzureOpenAIClient();
            return client.GetEmbeddingClient("text-embedding-3-small").AsIEmbeddingGenerator();
        }


        public static OpenAIClient GetDeepSeekClient(bool enableLogging = false)
            => GetAIClient(Keys.DeepSeekEndpoint, Keys.DeepSeekApiKey, enableLogging);

        public static OpenAIClient GetQwenClient(bool enableLogging = false) =>
            GetAIClient(Keys.QwenEndpoint, Keys.QwenApiKey, enableLogging);

        public static OpenAIClient GetTokenAIClient(bool enableLogging = false) =>
            GetAIClient(Keys.TokenAIEndpoint, Keys.TokenAIApiKey, enableLogging);

        private static OpenAIClient GetAIClient(string endpoint, string apiKey, bool enableLogging = false)
        {
            OpenAIClientOptions clientOptions = new OpenAIClientOptions();
            clientOptions.Endpoint = new Uri(endpoint);

            if (enableLogging)
            {
                var clientLoggingOptions = NLogHelper.CreateClientLoggingOptions();

                clientOptions.ClientLoggingOptions = clientLoggingOptions;
            }

            // 创建自定义的OpenAI客户端
            OpenAIClient aiClient = new(new ApiKeyCredential(apiKey), clientOptions);
            return aiClient;
        }
    }
}
