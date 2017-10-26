using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MagicCube.Messaging
{
    public class WeakAction
    {
#if SILVERLIGHT
        private Action _action;
#endif
        private Action _staticAction;

        protected MethodInfo Method
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the method that this WeakAction represents.
        /// </summary>
        public virtual string MethodName
        {
            get
            {
                if (_staticAction != null)
                {
#if NETFX_CORE
                    return _staticAction.GetMethodInfo().Name;
#else
                    return _staticAction.Method.Name;
#endif
                }

#if SILVERLIGHT
                if (_action != null)
                {
                    return _action.Method.Name;
                }

                if (Method != null)
                {
                    return Method.Name;
                }

                return string.Empty;
#else
                return Method.Name;
#endif
            }
        }

        protected WeakReference ActionReference
        {
            get;
            set;
        }

        protected WeakReference Reference
        {
            get;
            set;
        }

        public bool IsStatic
        {
            get
            {
#if SILVERLIGHT
                return (_action != null && _action.Target == null)
                    || _staticAction != null;
#else
                return _staticAction != null;
#endif
            }
        }

        protected WeakAction()
        {
        }

        public WeakAction(Action action)
            : this(action == null ? null : action.Target, action)
        {
        }


        [SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "Method should fail with an exception if action is null.")]
        public WeakAction(object target, Action action)
        {
#if NETFX_CORE
            if (action.GetMethodInfo().IsStatic)
#else
            if (action.Method.IsStatic)
#endif
            {
                _staticAction = action;

                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakAction's lifetime.
                    Reference = new WeakReference(target);
                }

                return;
            }

#if SILVERLIGHT
            if (!action.Method.IsPublic
                || (action.Target != null
                    && !action.Target.GetType().IsPublic
                    && !action.Target.GetType().IsNestedPublic))
            {
                _action = action;
            }
            else
            {
                var name = action.Method.Name;

                if (name.Contains("<")
                    && name.Contains(">"))
                {
                    // Anonymous method
                    _action = action;
                }
                else
                {
                    Method = action.Method;
                    ActionReference = new WeakReference(action.Target);
                }
            }
#else
#if NETFX_CORE
            Method = action.GetMethodInfo();
#else
            Method = action.Method;
#endif
            ActionReference = new WeakReference(action.Target);
#endif

            Reference = new WeakReference(target);
        }


        public virtual bool IsAlive
        {
            get
            {
                if (_staticAction == null
                    && Reference == null)
                {
                    return false;
                }

                if (_staticAction != null)
                {
                    if (Reference != null)
                    {
                        return Reference.IsAlive;
                    }

                    return true;
                }

                return Reference.IsAlive;
            }
        }


        public object Target
        {
            get
            {
                if (Reference == null)
                {
                    return null;
                }

                return Reference.Target;
            }
        }

        /// <summary>
        /// The target of the weak reference.
        /// </summary>
        protected object ActionTarget
        {
            get
            {
                if (ActionReference == null)
                {
                    return null;
                }

                return ActionReference.Target;
            }
        }

        public void Execute()
        {
            if (_staticAction != null)
            {
                _staticAction();
                return;
            }

            var actionTarget = ActionTarget;

            if (IsAlive)
            {
                if (Method != null
                    && ActionReference != null
                    && actionTarget != null)
                {
                    Method.Invoke(actionTarget, null);

                    // ReSharper disable RedundantJumpStatement
                    return;
                    // ReSharper restore RedundantJumpStatement
                }

#if SILVERLIGHT
                if (_action != null)
                {
                    _action();
                }
#endif
            }
        }

        public void MarkForDeletion()
        {
            Reference = null;
            ActionReference = null;
            Method = null;
            _staticAction = null;

#if SILVERLIGHT
            _action = null;
#endif
        }
    }


    public class WeakAction<T> : WeakAction, IExecuteWithObject
    {
#if SILVERLIGHT
        private Action<T> _action;
#endif
        private Action<T> _staticAction;

        public override string MethodName
        {
            get
            {
                if (_staticAction != null)
                {
#if NETFX_CORE
                    return _staticAction.GetMethodInfo().Name;
#else
                    return _staticAction.Method.Name;
#endif
                }

#if SILVERLIGHT
                if (_action != null)
                {
                    return _action.Method.Name;
                }

                if (Method != null)
                {
                    return Method.Name;
                }

                return string.Empty;
#else
                return Method.Name;
#endif
            }
        }

        public override bool IsAlive
        {
            get
            {
                if (_staticAction == null
                    && Reference == null)
                {
                    return false;
                }

                if (_staticAction != null)
                {
                    if (Reference != null)
                    {
                        return Reference.IsAlive;
                    }

                    return true;
                }

                return Reference.IsAlive;
            }
        }


        public WeakAction(Action<T> action)
            : this(action == null ? null : action.Target, action)
        {
        }

        [SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "Method should fail with an exception if action is null.")]
        public WeakAction(object target, Action<T> action)
        {
#if NETFX_CORE
            if (action.GetMethodInfo().IsStatic)
#else
            if (action.Method.IsStatic)
#endif
            {
                _staticAction = action;

                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakAction's lifetime.
                    Reference = new WeakReference(target);
                }

                return;
            }

#if SILVERLIGHT
            if (!action.Method.IsPublic
                || (target != null
                    && !target.GetType().IsPublic
                    && !target.GetType().IsNestedPublic))
            {
                _action = action;
            }
            else
            {
                var name = action.Method.Name;

                if (name.Contains("<")
                    && name.Contains(">"))
                {
                    _action = action;
                }
                else
                {
                    Method = action.Method;
                    ActionReference = new WeakReference(action.Target);
                }
            }
#else
#if NETFX_CORE
            Method = action.GetMethodInfo();
#else
            Method = action.Method;
#endif
            ActionReference = new WeakReference(action.Target);
#endif

            Reference = new WeakReference(target);
        }

        public new void Execute()
        {
            Execute(default(T));
        }

        public void Execute(T parameter)
        {
            if (_staticAction != null)
            {
                _staticAction(parameter);
                return;
            }

            var actionTarget = ActionTarget;

            if (IsAlive)
            {
                if (Method != null
                    && ActionReference != null
                    && actionTarget != null)
                {
                    Method.Invoke(
                        actionTarget,
                        new object[]
                        {
                            parameter
                        });
                }

#if SILVERLIGHT
                if (_action != null)
                {
                    _action(parameter);
                }
#endif
            }
        }

        public void ExecuteWithObject(object parameter)
        {
            var parameterCasted = (T)parameter;
            Execute(parameterCasted);
        }

        public new void MarkForDeletion()
        {
#if SILVERLIGHT
            _action = null;
#endif
            _staticAction = null;
            base.MarkForDeletion();
        }
    }
}
