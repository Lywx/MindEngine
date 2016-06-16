namespace MindEngine.Core.Scenes.Node.Action
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class MMActionManager : IMMNodeUpdatable, IDisposable
    {
        #region Logger

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        private bool targetsAvailable;

        private static MMNode[] targetsToUpdate = new MMNode[128];

        private readonly Dictionary<object, HashElement> targets =
            new Dictionary<object, HashElement>();

        private HashElement currentTarget;

        private bool currentTargetSalvaged;

        #region Constructors and Finalizer

        public MMActionManager()
        {
            
        }

        ~MMActionManager()
        {
            this.Dispose(true);
        }

        #endregion Constructors and Finalizer

        #region Nested Type: HashElement

        internal class HashElement
        {
            public int ActionIndex;

            /// <summary>
            /// It stores target' queuing actions' state data. We don't store
            /// actions themselves. Because actions carry not execution information.
            /// </summary>
            public List<MMActionState> ActionStates;

            public bool CurrentActionSalvaged;

            public MMActionState CurrentActionState;

            public bool Paused;

            public object Target;
        }

        #endregion

        #region Action Operations

        public MMActionState AttachAction(
            MMAction action,
            MMNode target,
            bool paused = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            HashElement element;

            // When the target are not queuing any actions
            if (!this.targets.TryGetValue(target, out element))
            {
                element = new HashElement
                {
                    Paused = paused,
                    Target = target
                };

                this.targets.Add(target, element);
                this.targetsAvailable = true;
            }

            this.AllocateHashElement(element);

            Debug.Assert(
                element.ActionStates.All(
                    actionState => actionState.Action != action),
                "Action is already running for this target.");

            var state = action.StartAction(target);
            element.ActionStates.Add(state);

            return state;
        }

        public void RemoveAction(MMActionState actionState)
        {
            if (actionState?.TargetOriginal == null)
            {
                return;
            }

            object target = actionState.TargetOriginal;

            HashElement element;

            if (this.targets.TryGetValue(target, out element))
            {
                var i = element.ActionStates.IndexOf(actionState);

                if (i != -1)
                {
                    this.RemoveActionAtIndex(i, element);
                }
                else
                {
                    logger.Warn("Action not found");
                }
            }
            else
            {
                logger.Warn("Target not found");
            }
        }

        internal void RemoveAction(MMAction action, MMNode target)
        {
            if (action == null
                || target == null)
            {
                return;
            }

            HashElement element;

            if (this.targets.TryGetValue(target, out element))
            {
                var limit = element.ActionStates.Count;
                var actionFound = false;

                for (var i = 0; i < limit; i++)
                {
                    var actionState = element.ActionStates[i];

                    if (actionState.Action == action
                        && actionState.TargetOriginal == target)
                    {
                        this.RemoveActionAtIndex(i, element);
                        actionFound = true;
                        break;
                    }
                }

                if (!actionFound)
                {
                    logger.Warn("Action not found");
                }
            }
            else
            {
                logger.Warn("Target not found");
            }
        }

        public void RemoveAction(int tag, MMNode target)
        {
            Debug.Assert(tag != (int)MMActionTag.Invalid);

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            // Early out if we do not have any targets to search
            if (this.targets.Count == 0)
            {
                return;
            }

            HashElement element;

            if (this.targets.TryGetValue(target, out element))
            {
                var tagLimit = element.ActionStates.Count;
                var tagFound = false;

                for (var i = 0; i < tagLimit; i++)
                {
                    var actionState = element.ActionStates[i];

                    // When both tag and original target match
                    if (actionState.Action.Tag == tag
                        && actionState.TargetOriginal == target)
                    {
                        this.RemoveActionAtIndex(i, element);

                        tagFound = true;

                        break;
                    }
                }

                if (!tagFound)
                {
                    logger.Warn($"Tag {tag} not found");
                }
            }
            else
            {
                logger.Warn("Target not found");
            }
        }

        internal void RemoveActionAtIndex(int index, HashElement element)
        {
            var actionState = element.ActionStates[index];

            // When the a
            if (element.CurrentActionState == actionState
                && !element.CurrentActionSalvaged)
            {
                element.CurrentActionSalvaged = true;
            }

            element.ActionStates.RemoveAt(index);

            // Update actionIndex in case that we are in tick looping over the actions
            if (element.ActionIndex >= index)
            {
                --element.ActionIndex;
            }

            if (element.ActionStates.Count == 0)
            {
                if (this.currentTarget == element)
                {
                    this.currentTargetSalvaged = true;
                }
                else
                {
                    this.DeleteHashElement(element);
                }
            }
        }

        public void RemoveAllActionsFromTarget(MMNode target)
        {
            if (target == null)
            {
                return;
            }

            HashElement element;

            if (this.targets.TryGetValue(target, out element))
            {
                // When current action
                if (element.ActionStates.Contains(element.CurrentActionState) &&
                    !element.CurrentActionSalvaged)
                {
                    element.CurrentActionSalvaged = true;
                }

                element.ActionStates.Clear();

                if (this.currentTarget == element)
                {
                    this.currentTargetSalvaged = true;
                }
                else
                {
                    this.DeleteHashElement(element);
                }
            }
        }

        public void RemoveAllActions()
        {
            if (!this.targetsAvailable)
            {
                return;
            }

            var targetCount = this.targets.Count;

            if (targetsToUpdate.Length < targetCount)
            {
                targetsToUpdate = new MMNode[targetsToUpdate.Length * 2];
            }

            this.targets.Keys.CopyTo(targetsToUpdate, 0);

            for (var i = 0; i < targetCount; i++)
            {
                this.RemoveAllActionsFromTarget(targetsToUpdate[i]);
            }
        }

        public MMAction GetAction(int tag, MMNode target)
        {
            Debug.Assert(tag != (int)MMActionTag.Invalid);

            if (this.targets.Count == 0)
            {
                return null;
            }

            HashElement element;

            if (this.targets.TryGetValue(target, out element))
            {
                if (element.ActionStates != null)
                {
                    for (var i = 0; i < element.ActionStates.Count; i++)
                    {
                        var action = element.ActionStates[i].Action;

                        if (action.Tag == tag)
                        {
                            return action;
                        }
                    }

                    logger.Warn($"Tag {tag} not found");
                }
            }
            else
            {
                logger.Warn("Target not found");
            }

            return null;
        }

        public MMActionState GetActionState(int tag, MMNode target)
        {
            if (tag == (int)MMActionTag.Invalid)
            {
                throw new InvalidOperationException();
            }

            if (this.targets.Count == 0)
            {
                return null;
            }

            HashElement element;

            if (this.targets.TryGetValue(target, out element))
            {
                if (element.ActionStates != null)
                {
                    var limit = element.ActionStates.Count;
                    for (var i = 0; i < limit; i++)
                    {
                        var actionState = element.ActionStates[i];

                        if (actionState.Action.Tag == tag)
                        {
                            return actionState;
                        }
                    }

                    logger.Warn($"Tag {tag} not found");
                }
            }
            else
            {
                logger.Warn("Target not found");
            }

            return null;
        }

        #endregion

        // TODO:
        public int NumberOfRunningActionsInTarget(MMNode target)
        {
            HashElement element;
            if (this.targets.TryGetValue(target, out element))
            {
                return element.ActionStates != null
                           ? element.ActionStates.Count
                           : 0;
            }
            return 0;
        }

        #region HashElement Operations

        internal void AllocateHashElement(HashElement element)
        {
            if (element.ActionStates == null)
            {
                element.ActionStates = new List<MMActionState>();
            }
        }

        internal void DeleteHashElement(HashElement element)
        {
            element.ActionStates.Clear();
            this.targets.Remove(element.Target);

            element.Target = null;
            this.targetsAvailable = this.targets.Count > 0;
        }

        #endregion

        #region Target Operations

        public void PauseTarget(object target)
        {
            HashElement element;

            if (this.targets.TryGetValue(target, out element))
            {
                element.Paused = true;
            }
        }

        public List<object> PauseAllRunningActions()
        {
            var idsWithActions = new List<object>();

            foreach (var element in this.targets.Values)
            {
                if (element.Paused)
                {
                    continue;
                }

                // Pause running actions
                element.Paused = true;
                idsWithActions.Add(element.Target);
            }

            return idsWithActions;
        }

        public void ResumeTarget(object target)
        {
            HashElement element;

            if (this.targets.TryGetValue(target, out element))
            {
                element.Paused = false;
            }
        }

        public void ResumeTargets(List<object> targetsToResume)
        {
            foreach (var t in targetsToResume.ToArray())
            {
                this.ResumeTarget(t);
            }
        }

        #endregion Target Operations

        #region Update

        public void Update(float dt)
        {
            if (!this.targetsAvailable)
            {
                return;
            }

            var targetCount = this.targets.Count;

            while (targetsToUpdate.Length < targetCount)
            {
                targetsToUpdate = new MMNode[targetsToUpdate.Length * 2];
            }

            this.targets.Keys.CopyTo(targetsToUpdate, 0);

            for (var i = 0; i < targetCount; i++)
            {
                HashElement elt;

                if (!this.targets.TryGetValue(targetsToUpdate[i], out elt))
                {
                    continue;
                }

                this.currentTarget = elt;
                this.currentTargetSalvaged = false;

                if (!this.currentTarget.Paused)
                {
                    // The 'ActionsState' may change while inside this loop.
                    for (this.currentTarget.ActionIndex = 0;
                        this.currentTarget.ActionIndex
                        < this.currentTarget.ActionStates.Count;
                        ++this.currentTarget.ActionIndex)
                    {
                        this.currentTarget.CurrentActionState =
                            this.currentTarget.ActionStates[
                                this.currentTarget.ActionIndex];

                        // TODO(Minor): Do I really need this line of code
                        if (this.currentTarget.CurrentActionState == null)
                        {
                            continue;
                        }

                        this.currentTarget.CurrentActionSalvaged = false;

                        // Step through current action state
                        this.currentTarget.CurrentActionState.Step(dt);

                        if (this.currentTarget.CurrentActionSalvaged)
                        {
                            // The currentAction told the node to remove it. To
                            // prevent the action from accidentally deallocating
                            // itself before finishing its step, we retained it.
                            // Now that step is done, it's safe to dispose it.
                        }
                        else if (this.currentTarget.CurrentActionState.IsDone)
                        {
                            this.currentTarget.CurrentActionState.Stop();

                            var actionState =
                                this.currentTarget.CurrentActionState;

                            // Make currentAction nil to prevent removeAction
                            // from salvaging it.
                            this.currentTarget.CurrentActionState = null;
                            this.RemoveAction(actionState);
                        }

                        // Remove reference to the action state
                        this.currentTarget.CurrentActionState = null;
                    }
                }

                // only delete currentTarget if no actions were scheduled during
                // the cycle (issue #481)
                if (this.currentTargetSalvaged
                    && this.currentTarget.ActionStates.Count == 0)
                {
                    this.DeleteHashElement(this.currentTarget);
                }
            }

            // issue #635
            this.currentTarget = null;
        }

        #endregion Update

        #region IDisposal

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveAllActions();
            }
        }

        #endregion IDisposal
    }
}