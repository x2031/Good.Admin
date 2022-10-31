using Hangfire.HttpJob.Agent;
using Hangfire.HttpJob.Agent.Attribute;

namespace Good.Admin.API.Jobs
{
    [TransientJob(RegisterName = "多例job")]//运行期间会存在多个相同任务，不论上次是否执行完毕。类似于多线程重复作业
    public class TransientJob : JobAgent
    {
        public override async Task OnStart(JobContext jobContext)
        {
            jobContext.Console.WriteLine("开始执行多例模式");
            jobContext.Console.WriteLine("循环输出0-100");
            for (int i = 0; i <= 100; i++)
            {
                Console.WriteLine(i);
            }
            jobContext.Console.WriteLine("执行完毕");
        }
    }
}
