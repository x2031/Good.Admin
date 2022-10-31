using Hangfire.HttpJob.Agent;
using Hangfire.HttpJob.Agent.Attribute;

namespace Good.Admin.API.Jobs
{
    [SingletonJob(RegisterName="单例Job")] //运行期间只会存在一个任务，如果任务没执行完毕，是不允许再次执行
    public class SingleJob:JobAgent
    {
        public SingleJob()
        { 
        
        }
        public override async Task OnStart(JobContext jobContext)
        {
            jobContext.Console.WriteLine("开始执行单例模式");
            jobContext.Console.WriteLine("循环输出0-100");
            for (int i = 0; i <= 100; i++)
            {
                Console.WriteLine(i);
            }
            jobContext.Console.WriteLine("执行完毕");
        }
    }
}
