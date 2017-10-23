namespace SimpleMvc.Framework.Contracts.Generic
{
    public interface IRenderable<TModel> : IRenderable
        //where TModel : class
    {
        TModel Model { get; set; }
    }
}
