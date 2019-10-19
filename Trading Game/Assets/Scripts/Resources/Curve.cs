/// <summary>
/// Contains basic parameters that can be used to calculate the price of a resource
/// Uses a default straight line curve to calculate prices, should be allowed to be overridden by child classes
/// </summary>

public abstract class Curve
{
	protected curvedetail curveDetails;
	protected float y_displacement;

	public Curve(curvedetail curveDetails)
	{
		this.curveDetails = curveDetails;
		this.y_displacement = this.calculateYDisplacement();
	}

	public abstract int calculateY(int x);

	protected abstract int calculateYDisplacement ();

	public float GetYDisplacement()
	{
		return y_displacement;
	}

	public abstract float GetGradient ();
	public abstract float calculateAreaUnderCurve (int initialX, int finalX);
}
