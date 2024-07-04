using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class MyExtensions
    {
    //    private static readonly Dictionary<float, WaitForSeconds> _wfsDictionary = new Dictionary<float,
    //WaitForSeconds>();
    //    public static WaitForSeconds WaitForSeconds(this float time)
    //    {
    //        if (_wfsDictionary.TryGetValue(time, out WaitForSeconds waitForSeconds))
    //        {
    //            return waitForSeconds;
    //        }
    //        var newWaitForSeconds = new WaitForSeconds(time);
    //        _wfsDictionary.Add(time, newWaitForSeconds);
    //        return newWaitForSeconds;
    //    }

        private static readonly Dictionary<float, WeakReference<WaitForSeconds>> _wfsDictionary = new Dictionary<float, WeakReference<WaitForSeconds>>();
        public static WaitForSeconds WaitForSeconds(this float time)
        {
            if (_wfsDictionary.TryGetValue(time, out WeakReference<WaitForSeconds> weakRef))
            {
                if (weakRef.TryGetTarget(out WaitForSeconds waitForSeconds))
                {
                    return waitForSeconds;
                }
            }

            var newWaitForSeconds = new WaitForSeconds(time);
            _wfsDictionary[time] = new WeakReference<WaitForSeconds>(newWaitForSeconds);
            return newWaitForSeconds;
        }
    }
}
