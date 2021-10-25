using System;
using System.Collections.Generic;

namespace MobileClaims.Core.Services
{
    public interface IRehydrationService
    {
        List<Type> BusinessProcess { get; }
        int CurrentRehydrationIndex { get; set; }
        string ProcessEntryPoint { get; set; }
        bool Rehydrating { get; set; }
        bool HackingRehydration { get; set; }
        void Save();
        void Reload();
        void ClearData();
        void ClearFromStartingPoint(Type start);
    }
}