using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <remarks>Gradient Constant should always be negative</remarks>
public class XSquareCurve : Curve 
{
	public XSquareCurve (float gradientConstant, float x_displacement, float equilibrium_y, float equilibrium_x) : base (gradientConstant, x_displacement, equilibrium_y, equilibrium_x) {
        InitializeYDisplacement();
    }

	public override float calculateY(int x)
	{
		return (float)(gradientConstant * Math.Pow (x, 2.0) + y_displacement);
	}

	protected override void InitializeYDisplacement()
	{
		y_displacement = (float)(equilibrium_y - gradientConstant * Math.Pow (equilibrium_x, 2.0));
	}

	public override float GetGradient()
	{
		return gradientConstant;
	}

	public float GetGradient(int x_value)
	{
		return 2 * gradientConstant * x_value;
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
		return (float)(gradientConstant * Math.Pow (x_value, 3.0) / 3.0 + y_displacement * x_value);
	}
}
