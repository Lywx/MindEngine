using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindEngine.Core.Util.FSM
{
    using System.Diagnostics;

    public partial class MMStateMachine<TState, TTransition>

        // Provide a partial compiler hint that the both generic parameters are enum
        where TState : struct, IConvertible, IComparable
        where TTransition : struct, IConvertible, IComparable
    {
        public class MMStateRepresentation
        {
            static MMStateRepresentation()
            {
                // Ensure that the both generic parameters are enum
                if (!typeof(TState).IsEnum)
                {
                    throw new ArgumentException("TState must be an enumerated type.");
                }

                if (!typeof(TTransition).IsEnum)
                {
                    throw new ArgumentException("TTransition must be an enumerated type.");
                }

                // Initialize transition count that would be used to initialize 
                // the state transition table
                StateTransitionCount = Enum.GetNames(typeof(TState)).Length
                                       * Enum.GetNames(typeof(TTransition)).Length;
            }

            public MMStateRepresentation(TState state)
            {
                this.State = state;
            }

            public TState State { get; }

            // This property would be different for different generic parameters
            //
            // ReSharper disable once StaticMemberInGenericType
            private static int StateTransitionCount { get; set; }

            /// <summary>
            /// Transition table for the same index element in destination table.
            /// </summary>
            private List<TTransition?> StateTransitionTable { get; } = new List<TTransition?>(new TTransition?[StateTransitionCount]);

            /// <summary>
            /// Destination table.
            /// </summary>
            private List<TState?> StateDestinationTable { get; } = new List<TState?>(new TState?[StateTransitionCount]);

            public void AttachTransition(TState destination, TTransition transition)
            {
                var index = this.StateDestinationIndex(null);

                Debug.Assert(index.HasValue);

                this.StateDestinationTable[index.Value] = destination;
                this.StateTransitionTable[index.Value]  = transition;
            }

            public void RemoveTransition(TState destination)
            {
                var index = this.StateDestinationIndex(destination);

                Debug.Assert(index.HasValue);

                // TODO
                this.StateDestinationTable[index.Value] = null;
                this.StateTransitionTable[index.Value] = null;

                // Tighten the not null value pack
                for (var i = index.Value; i < this.StateDestinationTable.Count; ++i)
                {
                    // Search stops at the edge of empty slots
                    if (!this.StateDestinationTable[i].HasValue)
                    {
                        break;
                    }

                    this.StateDestinationTable[i] = this.StateDestinationTable[i + 1];
                    this.StateTransitionTable[i] = this.StateTransitionTable[i + 1];
                }

                this.StateDestinationTable[i] = null;
                this.StateTransitionTable[i] = null;
            }

            public bool StateTransitionPossible(TTransition transition)
            {
                return this.StateTransitionIndex(transition) != null;
            }

            public TState? StateDestination(TTransition transition)
            {
                var index = this.StateTransitionIndex(transition);
                return index != null ? this.StateDestinationTable[index.Value] : null;
            }

            private int? StateDestinationIndex(TState? destination)
            {
                if (destination.HasValue)
                {
                    return this.StateDestinationIndex(destination.Value);
                }

                // Search for null
                for (var i = 0; i < StateTransitionCount; ++i)
                {
                    if (!this.StateDestinationTable[i].HasValue)
                    {
                        return i;
                    }
                }

                return null;
            }

            private int? StateDestinationIndex(TState destination)
            {
                for (var i = 0; i < StateTransitionCount; ++i)
                {
                    // When hit null before finding the same destination
                    if (!this.StateDestinationTable[i].HasValue)
                    {
                        break;
                    }

                    if (this.StateDestinationTable[i].Value.CompareTo(destination) == 0)
                    {
                        return i;
                    }
                }

                return null;
            }

            private int? StateTransitionIndex(TTransition transition)
            {
                for (var i = 0; i < StateTransitionCount; ++i)
                {
                    if (!this.StateTransitionTable[i].HasValue)
                    {
                        break;
                    }

                    if (this.StateTransitionTable[i].Value.CompareTo(transition) == 0)
                    {
                        return i;
                    }
                }

                return null;
            }
        }
    }

}
