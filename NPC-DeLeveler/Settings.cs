using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace NPC_DeLeveler
{
    /// <summary>
    /// Contains user-modifiable settings used by the patcher process.
    /// </summary>
    internal class Settings
    {
        [MaintainOrder]
        [Tooltip("The level to set all NPC & NPC Leveled-list-entries to.")]
        public short Level = 0;
        public short CalcMinLevel = 0;
        public short CalcMaxLevel = 0;
        [Tooltip("When checked, the player's level values are not modified.")]
        public bool SkipPlayer = true;

        /// <summary>
        /// Apply user settings to the given NPC record.
        /// </summary>
        /// <param name="npc">The Npc record to modify, should be received as a copy of the actual record.</param>
        /// <param name="changeCount">Returns the number of modified values.</param>
        /// <returns>Npc</returns>
        public Npc ApplyTo(Npc npc, out int changes)
        {
            changes = 0;
            // level
            if (!npc.Configuration.Level.Equals(Level))
            {
                npc.Configuration.Level = new NpcLevel()
                {
                    Level = Level
                };
                ++changes;
            }
            // calc minimum level
            if (!npc.Configuration.CalcMinLevel.Equals(CalcMinLevel))
            {
                npc.Configuration.CalcMinLevel = CalcMinLevel;
                ++changes;
            }
            // calc maximum level
            if (!npc.Configuration.CalcMaxLevel.Equals(CalcMaxLevel))
            {
                npc.Configuration.CalcMaxLevel = CalcMaxLevel;
                ++changes;
            }
            return npc;
        }
        /// <summary>
        /// Apply user settings to the given Leveled NPC list entry.
        /// </summary>
        /// <param name="npc">A copy of an entry in an NPC leveled-list.</param>
        /// <param name="changes">Returns the number of modified values.</param>
        /// <returns>LeveledNpcEntry</returns>
        public LeveledNpcEntry ApplyTo(LeveledNpcEntry npc, out int changes)
        {
            changes = 0;
            if (npc.Data == null) // return early if this entry doesn't have valid data
                return npc;
            if (!npc.Data.Level.Equals(Level))
            {
                npc.Data.Level = Level;
                ++changes;
            }
            return npc;
        }
        /// <summary>
        /// Apply user settings to all entries in the given Leveled NPC list.
        /// </summary>
        /// <param name="list">A copy of a NPC leveled-list record.</param>
        /// <param name="changes">Returns the number of modified values.</param>
        /// <returns>LeveledNpc</returns>
        public LeveledNpc ApplyTo(LeveledNpc list, out int changes)
        {
            changes = 0;
            foreach (var npc in list.Entries!) // null check is located in Program.cs loop
            {
                ApplyTo(npc.DeepCopy(), out int subchanges).DeepCopyIn(npc);
                changes += subchanges;
            }
            return list;
        }
    }
}
