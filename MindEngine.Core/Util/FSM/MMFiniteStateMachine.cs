using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindEngine.Core.Util.FSM
{
    using System.Diagnostics;

    public partial class MMStateMachine<TState, TTransition>
    {
        public TState State
        {
            get { return _stateAccessor(); }
            private set { _stateMutator(value); }
        }
    }

    public partial class MMStateMachine<TState, TTransition>
    {
        public class MMStateTransition
        {
            public MMStateTransition(TState source, TState destination, TTransition transition)
            {
                this.Source      = source;
                this.Destination = destination;
                this.Transition  = transition;
            }

            public TState Source { get; }

            public TState Destination { get; }

            public TTransition Transition { get; }

            public bool IsReentry => this.Source.Equals(this.Destination);
        }
    }
}
