using System;
using UnityEngine;

/// <summary>
/// Gradient Constant should always be negative
/// </summary>

public class XSquareCurve : Curve 
{
	public XSquareCurve (curvedetail curveDetails) : base (curveDetails){}

	public override int calculateY(int x)
	{
		return (int)(curveDetails.gradientConstant * Math.Pow (x, 2.0) + y_displacement);
	}

	protected override int calculateYDisplacement()
	{
		return (int)(curveDetails.equilibrium_y - curveDetails.gradientConstant * Math.Pow (curveDetails.equilibrium_x, 2.0));
	}

	public override float GetGradient()
	{
		return curveDetails.gradientConstant;
	}

	public float GetGradient(int x_value)
	{
		return 2 * curveDetails.gradientConstant * x_value;
	}

	public override float calculateAreaUnderCurve(int initialX, int finalX)
	{	
		return CalculateIntegral (initialX) - CalculateIntegral (finalX);
	}

	/// <summary>
	/// Calculates the integral upper or lower bound value of the x-squared function.
	/// </summary>
	/// <returns>The integral's intermediate value for use in calculating the area under the x-squared curve between the upper and lower bound.</returns>
	/// <param name="x">The upper or lower bound of the integral.</param>
	private float CalculateIntegral(int x_value)
	{
		return (float)(curveDetails.gradientConstant * Math.Pow (x_value, 3.0) / 3.0 + y_displacement * x_value);
	}
}
