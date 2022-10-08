namespace Ekstazz.Input.HotKey
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.InputSystem;

    public abstract class DebugAction
    {
        public abstract bool IsTriggered { get; }

        public abstract Action Action { get; }
    }

    public abstract class KeyboardAction : DebugAction
    {
        public abstract Key Key { get; }

        public override bool IsTriggered => Keyboard.current.altKey.isPressed &&
                                            Keyboard.current.ctrlKey.isPressed &&
                                            Keyboard.current[Key].wasPressedThisFrame;

        public override string ToString()
        {
            return $"HotKey \'{Key}\': {GetType().Name}";
        }
    }

    public abstract class MouseClickAction : DebugAction
    {
        protected abstract IEnumerable<Key> Keys { get; }

        public override bool IsTriggered => Mouse.current.press.wasPressedThisFrame &&
                                            Keys.All(k => Keyboard.current[k].isPressed);

        public override string ToString()
        {
            return $"HotKeys {string.Join(", ", Keys.Select(k => $"\'{k}\'"))}: {GetType().Name}";
        }
    }

    public abstract class LeftMouseClickAction : MouseClickAction
    {
        public override bool IsTriggered => Mouse.current.leftButton.wasPressedThisFrame && base.IsTriggered;

        public override string ToString()
        {
            return $"LeftMouse with Keys {string.Join(", ", Keys.Select(k => $"\'{k}\'"))}: {GetType().Name}";
        }
    }
}
