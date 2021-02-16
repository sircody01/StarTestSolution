using System;
using System.Diagnostics;
using System.Linq;
using log4net;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using Star.Core.Page;
using Star.Pages.Logging;

//Initialize this aspect on assembly level.
[assembly: LogMethodAspect(AttributeTargetTypes = "Star.Pages.*",
    AttributeTargetTypeAttributes = MulticastAttributes.Public,
    AttributeTargetMemberAttributes = MulticastAttributes.Public, AttributePriority = 1)]
[assembly: LogMethodAspect(AttributeTargetMembers = "Dispose", AttributeExclude = true, AttributePriority = 2)]
[assembly: LogMethodAspect(AttributeTargetMembers = ".ctor", AttributeExclude = true, AttributePriority = 2)]
[assembly: LogMethodAspect(AttributeTargetMembers = "set_*", AttributeExclude = true, AttributePriority = 2)]
[assembly: LogMethodAspect(AttributeTargetMembers = "get_*", AttributeExclude = true, AttributePriority = 2)]
[assembly: LogMethodAspect(AttributeTargetMembers = "IsActive", AttributeExclude = true, AttributePriority = 2)]

/*
 * PostSharp Aspect Class for Trace Logging.
 * Class, Method and/or Assembly level attribute.
 * (On Methods' Entry, Exit and Exception)
 * Default: Assembly, because of Free Post Sharp limitations regarding inheritance.
 */

namespace Star.Pages.Logging
{
    [Serializable]
    [DebuggerStepThrough]
    public sealed class LogMethodAspect : OnMethodBoundaryAspect
    {
        private const int IndentSize = 2;

        private static ILog Logger(AdviceArgs args)
        {
            // Retrieve the ILog property from the test currently running.
            if (args.Instance is BaseNavigationProvider)
                return ((BaseNavigationProvider)args.Instance).ActiveTest.PostSharpLogger;
            throw new ArgumentException();
        }

        private static int PostSharpIndent(AdviceArgs args)
        {
            // Retrieve the PostSharpIndent property from the test currently running.
            if (args.Instance is BaseNavigationProvider)
                return ((BaseNavigationProvider)args.Instance).ActiveTest.PostSharpIndent;
            throw new ArgumentException();
        }

        private static void SetPostSharpIndent(AdviceArgs args, int newValue)
        {
            // Retrieve the PostSharpIndent property from the test currently running.
            if (args.Instance is BaseNavigationProvider)
                ((BaseNavigationProvider)args.Instance).ActiveTest.PostSharpIndent = newValue;
            else
                throw new NotSupportedException();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var parameters = "<no args>";
            if (null != args.Arguments && args.Arguments.Any())
            {
                parameters = string.Join("; ",
                    args.Arguments.ToList().ConvertAll(a => (a ?? "null").ToString()).ToArray());
            }

            var entryMessage = string.Format(
                "{2}>>> {0} ( [{1}] )",
                args.Method.Name,
                parameters,
                new string(' ', PostSharpIndent(args)));
            Logger(args).Info(entryMessage);
            SetPostSharpIndent(args, PostSharpIndent(args) + IndentSize);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (PostSharpIndent(args) < IndentSize)
            {
                Logger(args).Error("OnSuccess: Unexpected PostSharpIndent value: " + PostSharpIndent(args));
                SetPostSharpIndent(args, 0);
            }
            else
            {
                SetPostSharpIndent(args, PostSharpIndent(args) - IndentSize);
            }
            var successMessage = string.Format(
                "{2}<<< {0} returned [{1}]",
                args.Method.Name,
                args.ReturnValue ?? "<null>",
                new string(' ', PostSharpIndent(args)));
            Logger(args).Info(successMessage);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            SetPostSharpIndent(args, 0);
            var exceptionMessage =
                $"[{args.Method.DeclaringType.FullName}] !! {args.Exception.GetType().Name} in [{args.Method.Name}]:\n{args.Exception}";
            Logger(args).Error(exceptionMessage);
        }
    }
}
