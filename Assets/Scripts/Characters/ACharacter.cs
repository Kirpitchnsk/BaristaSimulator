using System;
using System.Collections.Generic;
using SibGameJam2026.Characters.Components;
using UnityEngine;
using Zenject;

namespace SibGameJam2026.Characters {
	public abstract class ACharacter : MonoBehaviour {
		private IReadOnlyDictionary<Type, ICharacterComponent> _components = new Dictionary<Type, ICharacterComponent>();
		private readonly List<IUpdatable> _updatableComponents = new();
		
		[field: SerializeField] public CharacterData Data { get; private set; } = new();


		public virtual void Initialize(IReadOnlyDictionary<Type, ICharacterComponent> components) {
			_components = components;
			
			_updatableComponents.Clear();
			foreach (var component in _components.Values) {
				if (component is IUpdatable updatableComponent)
					_updatableComponents.Add(updatableComponent);
			}
		}

		public virtual void Update() {
			foreach (var component in _updatableComponents)
				component.OnUpdate();
		}

		public bool TryGetComponent<TComponent>(out TComponent component) where TComponent : class, ICharacterComponent {
			if (_components.TryGetValue(typeof(TComponent), out var rawComponent)) {
				component = rawComponent as TComponent;
				return component != null;
			}

			component = null;
			return false;
		}

		public TComponent GetComponent<TComponent>() where TComponent : class, ICharacterComponent {
			if (TryGetComponent<TComponent>(out var component))
				return component;

			throw new InvalidOperationException($"Character does not contain component of type {typeof(TComponent).Name}");
		}


		
		public class Factory : PlaceholderFactory<ECharacterType, Vector3, ACharacter> { }
	}
}
