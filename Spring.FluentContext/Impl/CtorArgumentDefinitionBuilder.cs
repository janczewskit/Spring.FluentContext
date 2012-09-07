using System;
using Spring.FluentContext.Builders;
using Spring.FluentContext.BuildingStages;
using Spring.FluentContext.Utils;
using Spring.Objects.Factory.Config;

namespace Spring.FluentContext.Impl
{
	internal class CtorArgumentDefinitionBuilder<TBuilder, TArgument> : ICtorArgumentDefinitionBuilder<TBuilder, TArgument>
	{
		private readonly IDefinitionHolder _holder;
		private readonly TBuilder _builder;
		private readonly Action<ConstructorArgumentValues, object> _insertCtorArgAction;

		public CtorArgumentDefinitionBuilder(IDefinitionHolder holder, TBuilder builder, int argIndex)
		{
			_holder = holder;
			_builder = builder;
			_insertCtorArgAction = (list, value) => list.AddIndexedArgumentValue(argIndex, value, typeof(TArgument).FullName);
		}

		public CtorArgumentDefinitionBuilder(IDefinitionHolder holder, TBuilder builder)
		{
			_holder = holder;
			_builder = builder;
			_insertCtorArgAction = (list, value) => list.AddGenericArgumentValue(value, typeof(TArgument).FullName);
		}

		public TBuilder ToValue(TArgument value)
		{
			_insertCtorArgAction(_holder.Definition.ConstructorArgumentValues, value);
			return _builder;
		}

		public TBuilder ToRegistered(string objectId)
		{
			_insertCtorArgAction(_holder.Definition.ConstructorArgumentValues, new RuntimeObjectReference(objectId));
			return _builder;
		}

		public TBuilder ToRegistered<TRef>(ObjectRef<TRef> reference) where TRef : TArgument
		{
			return ToRegistered(reference.Id);
		}

		public TBuilder ToRegisteredDefault()
		{
			return ToRegistered(IdGenerator<TArgument>.GetDefaultId());
		}

		public TBuilder ToRegisteredDefaultOfType<TReferencedType>() where TReferencedType : TArgument
		{
			return ToRegistered(IdGenerator<TReferencedType>.GetDefaultId());
		}

		public TBuilder ToInlineDefinition<TInnerObject>() where TInnerObject : TArgument
		{
			_insertCtorArgAction(
				_holder.Definition.ConstructorArgumentValues,
				new ObjectDefinitionBuilder<TInnerObject>(null).Definition);

			return _builder;
		}

		public TBuilder ToInlineDefinition<TInnerObject>(Action<IInstantiationBuildStage<TInnerObject>> innerObjectBuildAction) where TInnerObject : TArgument
		{
			var innerObjectBuilder = new ObjectDefinitionBuilder<TInnerObject>(null);
			innerObjectBuildAction(innerObjectBuilder);
			_insertCtorArgAction(_holder.Definition.ConstructorArgumentValues, innerObjectBuilder.Definition);
			return _builder;
		}
	}
}