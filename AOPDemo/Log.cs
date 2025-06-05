using Rougamo;


namespace AOPDemo
{
    public class Log : MoAttribute
    {
        public override void OnEntry(MethodContext context)
        {
            base.OnEntry(context);
            Console.WriteLine($"OnEntry{context.Method.Name}执行时间：{DateTime.Now}");
        }
        public override void OnSuccess(MethodContext context)
        {
            base.OnSuccess(context);
            Console.WriteLine($"OnSuccess{context.Method.Name}执行时间：{DateTime.Now}");
        }
    }
}
