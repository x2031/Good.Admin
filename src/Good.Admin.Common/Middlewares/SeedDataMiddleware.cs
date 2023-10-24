using Microsoft.AspNetCore.Builder;

namespace Good.Admin.Common
{
    public static class SeedDataMiddleware
    {
        public static void UseSeedDataMiddle(this IApplicationBuilder app, MyContext myContext, string webRootPath)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (Appsettings.app("AppSettings", "SeedDBEnabled").ObjToBool() || Appsettings.app("AppSettings", "SeedDBDataEnabled").ObjToBool())
                {
                    //myContext.c
                    myContext.CreateEntityByTable();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
