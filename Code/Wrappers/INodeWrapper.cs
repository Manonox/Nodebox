namespace Nodebox;

public interface INodeWrapper<T> {
	public abstract static T Wrap(Node node);
}
