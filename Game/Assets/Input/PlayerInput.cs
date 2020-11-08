// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Input
{
    public class @PlayerInput : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerInput()
        {
            this.asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Actions"",
            ""id"": ""31318777-1404-478a-926d-2557c940dbe7"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""d35e711b-4713-44a7-9a73-b71533b22e7f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""eaa44061-bd1f-43b3-ae82-6083ca58b276"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inspect"",
                    ""type"": ""Button"",
                    ""id"": ""c0f3be09-17f1-4d3a-a71f-5289a0cd2822"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keys"",
                    ""id"": ""2c4be5f8-52f7-4252-ba6b-d85c26c19fd6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0fc86b36-f813-410a-85a4-72e731b2a40d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e970f0da-b3f8-4097-90f0-fa89e4eef2c6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ceac21aa-fd2b-4241-bff6-951da8d8b2e6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c7e000f5-c8df-4eea-b9a8-d92364b9312a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""83e05aca-594e-4b05-8c7e-88115e2ee45d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d94e583d-fef5-4485-9cf3-4d5b36ef68ae"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inspect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Actions
            this.m_Actions = this.asset.FindActionMap("Actions", throwIfNotFound: true);
            this.m_Actions_Move = this.m_Actions.FindAction("Move", throwIfNotFound: true);
            this.m_Actions_Interact = this.m_Actions.FindAction("Interact", throwIfNotFound: true);
            this.m_Actions_Inspect = this.m_Actions.FindAction("Inspect", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(this.asset);
        }

        public InputBinding? bindingMask
        {
            get => this.asset.bindingMask;
            set => this.asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => this.asset.devices;
            set => this.asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => this.asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return this.asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return this.asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Enable()
        {
            this.asset.Enable();
        }

        public void Disable()
        {
            this.asset.Disable();
        }

        // Actions
        private readonly InputActionMap m_Actions;
        private IActionsActions m_ActionsActionsCallbackInterface;
        private readonly InputAction m_Actions_Move;
        private readonly InputAction m_Actions_Interact;
        private readonly InputAction m_Actions_Inspect;
        public struct ActionsActions
        {
            private @PlayerInput m_Wrapper;
            public ActionsActions(@PlayerInput wrapper) { this.m_Wrapper = wrapper; }
            public InputAction @Move => this.m_Wrapper.m_Actions_Move;
            public InputAction @Interact => this.m_Wrapper.m_Actions_Interact;
            public InputAction @Inspect => this.m_Wrapper.m_Actions_Inspect;
            public InputActionMap Get() { return this.m_Wrapper.m_Actions; }
            public void Enable() { this.Get().Enable(); }
            public void Disable() { this.Get().Disable(); }
            public bool enabled => this.Get().enabled;
            public static implicit operator InputActionMap(ActionsActions set) { return set.Get(); }
            public void SetCallbacks(IActionsActions instance)
            {
                if (this.m_Wrapper.m_ActionsActionsCallbackInterface != null)
                {
                    this.Move.started -= this.m_Wrapper.m_ActionsActionsCallbackInterface.OnMove;
                    this.Move.performed -= this.m_Wrapper.m_ActionsActionsCallbackInterface.OnMove;
                    this.Move.canceled -= this.m_Wrapper.m_ActionsActionsCallbackInterface.OnMove;
                    this.Interact.started -= this.m_Wrapper.m_ActionsActionsCallbackInterface.OnInteract;
                    this.Interact.performed -= this.m_Wrapper.m_ActionsActionsCallbackInterface.OnInteract;
                    this.Interact.canceled -= this.m_Wrapper.m_ActionsActionsCallbackInterface.OnInteract;
                    this.Inspect.started -= this.m_Wrapper.m_ActionsActionsCallbackInterface.OnInspect;
                    this.Inspect.performed -= this.m_Wrapper.m_ActionsActionsCallbackInterface.OnInspect;
                    this.Inspect.canceled -= this.m_Wrapper.m_ActionsActionsCallbackInterface.OnInspect;
                }
                this.m_Wrapper.m_ActionsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    this.Move.started += instance.OnMove;
                    this.Move.performed += instance.OnMove;
                    this.Move.canceled += instance.OnMove;
                    this.Interact.started += instance.OnInteract;
                    this.Interact.performed += instance.OnInteract;
                    this.Interact.canceled += instance.OnInteract;
                    this.Inspect.started += instance.OnInspect;
                    this.Inspect.performed += instance.OnInspect;
                    this.Inspect.canceled += instance.OnInspect;
                }
            }
        }
        public ActionsActions @Actions => new ActionsActions(this);
        public interface IActionsActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnInspect(InputAction.CallbackContext context);
        }
    }
}
