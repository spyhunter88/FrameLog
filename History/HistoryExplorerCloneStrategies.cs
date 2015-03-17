using System;

namespace FrameLog.History
{
    [Flags]
    public enum HistoryExplorerCloneStrategies
    {
        /// <summary>
        /// No cloning, will trigger an UnableToCloneObjectException
        /// </summary>
        None = 0,

        /// <summary>
        /// Use the ICloneable interface for cloning (only if the model implements ICloneable)
        /// </summary>
        UseCloneable = 0x01,

        /// <summary>
        /// Do a recusive/deep copy of the object using Object.MemberwiseClone()
        /// </summary>
        DeepCopy = 0x02,

        /// <summary>
        /// The default cloning strategy
        /// </summary>
        Default = (UseCloneable | DeepCopy)
    }
}
