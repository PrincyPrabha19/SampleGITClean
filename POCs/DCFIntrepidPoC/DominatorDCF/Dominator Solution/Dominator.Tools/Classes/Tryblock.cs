// <copyright file="ErrorHanding.cs" company="Dell Inc.">
//      Copyright (c) Dell Inc. All rights reserved.
// </copyright>

using System;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Tools.Classes
{
    public class Tryblock
    {
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        public static bool Run(Action tryAction, Action<Exception> catchAction = null, Action finallyAction = null)
        {
            if (tryAction == null)
                return false;

            try
            {
                tryAction.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                logException(ex);
                catchAction?.Invoke(ex);
                return false;
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        private static void logException(Exception ex)
        {
            try
            {
                logger?.WriteError("Tryblock", null, ex.ToString());
            }
            catch
            {
                // just ignore any exception, it doesn't matter if just lose a debug log.
            }
        }
    }
}
