using Repository.Pattern.Modules;
using Repository.Pattern.DataContext;
using FrameLog.Models;
using FrameLog.Contexts;

namespace FrameLog
{
    public class Logger : ILogging
    {
        public Logger(ContextInfo contextInfo, User user)
        {
            this.user = user;
            this.contextInfo = contextInfo;
            context = new LogContext();
            this.contextInfo.LogContext = context;
        }

        public Logger(ContextInfo contextInfo, int userId)
        {
            this.contextInfo = contextInfo;
            context = new LogContext();
            this.user = context.Users.Find(userId);
        }

        public User user { get; set; }
        public ContextInfo contextInfo { get; set; }
        public LogContext context { get; set; }

        private FrameLog<ChangeSet, User> frameLog;

        public void SaveChangePreCommit(object context)
        {
            frameLog = new FrameLog<ChangeSet, User>(contextInfo, new ChangeSetFactory());
            frameLog.logChanges(user);


        }

        public void SaveChangePostCommit(object context)
        {
            ChangeSet cs = frameLog.getLogs(user);

            if (cs != null)
            {
                this.context.ChangeSets.Add(cs);
                this.context.SaveChanges();
            }
        }
    }
}
