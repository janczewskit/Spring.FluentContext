namespace Spring.FluentContext
{
	public interface ILookupMethodDefinitionBuilder<TObject, in TResult>
	{
		IObjectDefinitionBuilder<TObject> ToReference(string objectId);
		IObjectDefinitionBuilder<TObject> ToDefaultReference();
		IObjectDefinitionBuilder<TObject> ToDefaultReferenceOfType<TReferencedType>() where TReferencedType : TResult;
		IObjectDefinitionBuilder<TObject> ToReference<TRef>(ObjectRef<TRef> reference) where TRef : TResult;
	}
}