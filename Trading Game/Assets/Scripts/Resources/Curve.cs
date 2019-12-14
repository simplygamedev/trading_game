using System;

/// <summary>
/// Contains basic parameters that can be used to calculate the price of a resource
/// Uses a default straight line curve to calculate prices, should be allowed to be overridden by child classes
/// </summary>
public abstract class Curve
{
	protected float y_displacement;
    public float x_displacement;

    public float gradientConstant;

    /// <value>Reference Y value used for calculating y displacement</value>
    public float equilibrium_y;
    /// <value>Reference X value used for calculating y displacement</value>
    public float equilibrium_x;

    public Curve(float gradientConstant, float x_displacement, float equilibrium_y, float equilibrium_x)
    {
        this.gradientConstant = gradientConstant;
        this.x_displacement = x_displacement;
        this.equilibrium_y = equilibrium_y;
        this.equilibrium_x = equilibrium_x;
    }

	public abstract float calculateY(int x);

    protected abstract void InitializeYDisplacement();
    public float GetYDisplacement()
    {
        return y_displacement;
    }

	public abstract float GetGradient ();
	public abstract float calculateAreaUnderCurve (int initialX, int finalX);

    public static Curve createCurveByName(string curveName, float gradientConstant, float x_displacement, float equilibrium_y, float equilibrium_x)
    {
        return (Curve)Activator.CreateInstance(Type.GetType(curveName), gradientConstant, x_displacement, equilibrium_y, equilibrium_x);
    }
}
