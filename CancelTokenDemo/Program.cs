using System.Threading;

namespace CancelTokenDemo
{
    internal class Program
    {
        /// <summary>
        /// https://mp.weixin.qq.com/s?__biz=MzU2OTY3MTYzOA==&mid=2247492679&idx=1&sn=93b2d7199387ba63e19ae35c3cb27102&chksm=fcf98efecb8e07e83ae970fbd9b5e5572630a1874802fd03764d10351701b40104796bc4031b#rd
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            //CancellationToken cancellationToken = cancellationTokenSource.Token;
            //cancellationToken.Register(() => System.Console.WriteLine("取消了？？？"));
            //cancellationToken.Register(() => System.Console.WriteLine("取消了！！！"));
            //cancellationToken.Register(state => System.Console.WriteLine($"取消了。。。{state}"), "啊啊啊");
            //cancellationTokenSource.CancelAfter(5000);
            // cancellationTokenSource.Cancel();


            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            CancellationToken cancellationToken = tokenSource.Token;
            //打印被取消
            cancellationToken.Register(() => System.Console.WriteLine("被取消了."));
            //模拟传递的场景
            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    System.Console.WriteLine($"一直在执行...{DateTime.Now}");
                    await Task.Delay(1000);
                }
            });
            //5s之后取消
          //  tokenSource.CancelAfter(5000);


            System.Console.WriteLine("做了点别的,然后取消了.");

            //var token = Listen();
            //token.CancelAfter(10000);

            Console.ReadKey();
        }

        public static CancellationTokenSource Listen()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //循环调度执行
            Task.Run(() =>
            {
                while (true)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    //循环执行一些操作
                    Thread.Sleep(1000);
                    Console.WriteLine("Run");
                }
            });

            return cancellationTokenSource;
        }
    }
}
