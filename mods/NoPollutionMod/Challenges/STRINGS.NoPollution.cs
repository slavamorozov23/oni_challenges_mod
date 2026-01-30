using STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal partial class STRINGS
    {
        public class NO_POLLUTION_CHALLENGE
        {
            public static LocString NAME = "Challenge";
            public static LocString TOOLTIP = "Select a challenge for this world.";
            public static LocString UNKNOWN_ASTEROID = "Unknown asteroid";
            public static LocString COMPLETION_CYCLE = "Cycle: {0}";
            public static LocString COMPLETION_CYCLE_UNKNOWN = "Cycle: ?";
            public static LocString COMPLETION_FALLBACK = "Challenge completed: {0}";
            public static LocString ALL_ACHIEVEMENTS_REMAINING_CYCLES = "Remaining cycles: {0}";
            public static LocString HUNDRED_DUPLICANTS_PROGRESS = "Duplicants: {0}/{1}";
            public static LocString HUNDRED_DUPLICANTS_FAILED_TOOLTIP = "Challenge failed: A duplicant has died";

            public class NO_TOILETS_STATUS
            {
                public static LocString NAME = "Out of Order (No Toilets Challenge)";
                public static LocString TOOLTIP = "Disabled by the 'No Toilets' challenge.";
            }

            public class LEVELS
            {
                public class NONE
                {
                    public static LocString NAME = "None";
                    public static LocString TOOLTIP = "Challenge disabled.";
                }

                public class NO_POLLUTION
                {
                    public static LocString NAME = "No Pollution";
                    public static LocString TOOLTIP =
                        "No polluting production allowed.\n" +
                        "Includes: coal/gas/petroleum generators, farm stations, fertilizers,\n" +
                        "diamond press, smokehouse, CO2 scrubber, electrolyzer, deoxidizers,\n" +
                        "sublimation station, deodorizer, rocket fabrication, jet suit dock,\n" +
                        "power control station, genetic analysis station.\n" +
                        "In DLC1, Drillcone and biodiesel engine are also forbidden.";
                    public static LocString START =
                        "Challenge active: no polluting production.\n" +
                        "Keep the colony clean and efficient.";
                    public static LocString COMPLETION =
                        "The colony proved it can live cleanly.\n" +
                        "No smoke, no grime, no heavy emissions — only discipline and care.\n" +
                        "Well done, Administrator!";
                }

                public class NO_TOILETS
                {
                    public static LocString NAME = "No Toilets";
                    public static LocString TOOLTIP =
                        "No toilets allowed.\n" +
                        "Find other ways to keep the colony hygienic.";
                    public static LocString START =
                        "Challenge active: no toilets.\n" +
                        "Keep hygiene through ingenuity and careful planning.";
                    public static LocString COMPLETION =
                        "The colony endured without a single toilet.\n" +
                        "Discipline and ingenuity carried the day.";
                }

                public class ONE_DUPLICANT
                {
                    public static LocString NAME = "Only One Duplicant";
                    public static LocString TOOLTIP =
                        "Printing Pod and Mini-Pod will never offer duplicants.\n" +
                        "Frozen Friend cannot be defrosted.\n" +
                        "Bionic duplicants are disabled.";
                    public static LocString START =
                        "Challenge active: only one duplicant.\n" +
                        "Every decision counts when you're alone.";
                    public static LocString COMPLETION =
                        "One duplicant stood alone and still built a thriving colony.";
                }

                public class ALL_ACHIEVEMENTS
                {
                    public static LocString NAME = "All Achievements";
                    public static LocString TOOLTIP =
                        "Earn every colony achievement within 800 cycles.\n" +
                        "Only achievements valid for this asteroid count.";
                    public static LocString START =
                        "Challenge active: all achievements.\n" +
                        "Earn every colony achievement within 800 cycles.";
                    public static LocString COMPLETION =
                        "Every achievement earned.\n" +
                        "The colony's legend is complete.";
                    public static LocString FAILURE =
                        "The cycle limit has passed.\n" +
                        "Not every achievement was secured in time.";
                }

                public class HUNDRED_DUPLICANTS
                {
                    public static LocString NAME = "100 Duplicants";
                    public static LocString TOOLTIP =
                        "Reach 100 duplicants without losing a single one.\n" +
                        "Printing Pod automatically prints duplicants when recharged.\n" +
                        "If any duplicant dies, the challenge fails.";
                    public static LocString START =
                        "Challenge active: 100 duplicants.\n" +
                        "The Printing Pod will automatically print duplicants.\n" +
                        "Keep everyone alive to win.";
                    public static LocString COMPLETION =
                        "100 duplicants reached without a single loss.\n" +
                        "The colony thrives with perfect care and management.";
                    public static LocString FAILURE =
                        "A duplicant has died.\n" +
                        "The challenge cannot continue.";
                }
            }
        }
    }
}
