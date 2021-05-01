using System;

/// <summary>
/// Gradient Constant should always be negative
/// </summary>

public class StraightCurve : Curve 
{
	public StraightCurve (float gradientConstant, float x_displacement, float equilibrium_y, float equilibrium_x) : base (gradientConstant, x_displacement, equilibrium_y, equilibrium_x) {
        InitializeYDisplacement();
    }
		
	public override float calculateY(int x){
		return (gradientConstant * x + y_displacement);
	}

	protected override void InitializeYDisplacement(){
		y_displacement = equilibrium_y - gradientConstant * equilibrium_x;
	}

	public override float GetGradient(){
		return gradientConstant;
	}

	public override float calculateAreaUnderCurve(int initialX, int finalX){	
		return (float)((gradientConstant * Math.Pow (initialX, 2) / 2 + y_displacement * initialX) - (gradientConstant * Math.Pow (finalX, 2) / 2 + y_displacement * finalX));
	}

}
