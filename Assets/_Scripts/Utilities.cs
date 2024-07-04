using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class MyExtensions
    {
        private static readonly Dictionary<float, WaitForSeconds> _wfsDictionary = new Dictionary<float,
    WaitForSeconds>();
        public static WaitForSeconds WaitForSeconds(this float time)
        {
            if (_wfsDictionary.TryGetValue(time, out WaitForSeconds waitForSeconds))
            {
                return waitForSeconds;
            }
            var newWaitForSeconds = new WaitForSeconds(time);
            _wfsDictionary.Add(time, newWaitForSeconds);
            return newWaitForSeconds;
        }
    }
}
